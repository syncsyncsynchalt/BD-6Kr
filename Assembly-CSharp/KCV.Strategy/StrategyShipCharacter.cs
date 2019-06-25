using Common.Enum;
using Common.Struct;
using KCV.Utils;
using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyShipCharacter : UIShipCharacter
	{
		[SerializeField]
		private UISprite DeckStateTag;

		[SerializeField]
		private StrategyCharacterCollision collision;

		public static ShipModel nowShipModel;

		[SerializeField]
		private UITexture HeadArea;

		[Button("DebugChange", "DebugChange", new object[]
		{
			1
		})]
		public int Button01;

		[Button("DebugChange", "DebugChangePrev", new object[]
		{
			-1
		})]
		public int Button02;

		public int DebugMstID = 1;

		public int GraphicID;

		private void Start()
		{
			if (nowShipModel == null && SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck != null)
			{
				nowShipModel = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip();
			}
		}

		public void setState(DeckModel deck)
		{
			string text = string.Empty;
			if (isEnsei(deck))
			{
				text = "ensei";
			}
			if (isTettai(deck))
			{
				text = "withdraw";
			}
			DeckStateTag.spriteName = "shipicon_" + text;
			if (text != string.Empty)
			{
				base.transform.localPosition = getExitPosition();
			}
			else if (isEnter)
			{
				base.transform.localPosition = getEnterPosition();
			}
		}

		protected override bool CheckMstIDEnable(ShipModelMst model)
		{
			if (model == null)
			{
				Render.mainTexture = null;
				if (collision != null)
				{
					collision.SetActive(isActive: false);
				}
				return false;
			}
			if (collision != null)
			{
				collision.SetActive(isActive: true);
			}
			return true;
		}

		private bool isEnsei(DeckModel deck)
		{
			return deck.MissionState != MissionStates.NONE;
		}

		private bool isTettai(DeckModel deck)
		{
			return deck.GetFlagShip() != null && deck.GetFlagShip().IsEscaped();
		}

		public void PlayVoice(DeckModel deck)
		{
			if (!isEnsei(deck) && !isTettai(deck) && deck.GetFlagShip() != null)
			{
				ShipUtils.PlayShipVoice(deck.GetFlagShip(), 2);
			}
		}

		public void PlayVoice(EscortDeckModel deck)
		{
			if (deck.GetFlagShip() != null)
			{
				ShipUtils.PlayShipVoice(deck.GetFlagShip(), 2);
			}
		}

		public void ChangeCharacter(DeckModel deck)
		{
			ShipModel shipModel = deck.GetFlagShip();
			if (deck != null && deck.MissionState != 0)
			{
				shipModel = null;
			}
			if (SingletonMonoBehaviour<Live2DModel>.Instance != null && !SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel && nowShipModel != null && shipModel != null && (nowShipModel.GetGraphicsMstId() != shipModel.GetGraphicsMstId() || !shipModel.IsDamaged()) && Render != null && Render.mainTexture != null)
			{
				Resources.UnloadAsset(Render.mainTexture);
				Render.mainTexture = null;
			}
			ChangeCharacter(shipModel, deck.Id);
			nowShipModel = shipModel;
			if (collision != null)
			{
				collision.ResetTouchCount();
				if (Render != null)
				{
					collision.SetCollisionHight(Render.height);
				}
			}
		}

		public void ResetPosition()
		{
			base.transform.localPositionX(ShipIn.x + L2dBias.x);
		}

		private DeckModel getDeck(ShipModel model)
		{
			if (model == null)
			{
				return null;
			}
			int num = model.IsInDeck();
			switch (num)
			{
			case -1:
				return null;
			case 0:
				num = 1;
				break;
			}
			return StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(num);
		}

		public void DebugChange(int direction)
		{
			ShipModelMst shipModelMst = null;
			for (int i = 0; i < 200; i++)
			{
				DebugMstID = (int)Util.RangeValue(DebugMstID + direction, 1f, 500f);
				try
				{
					shipModelMst = new ShipModelMst(DebugMstID);
					int buildStep = shipModelMst.BuildStep;
				}
				catch (KeyNotFoundException)
				{
					continue;
				}
				if (shipModelMst != null && shipModelMst.MstId != 0)
				{
					break;
				}
			}
			GraphicID = shipModelMst.GetGraphicsMstId();
			ChangeCharacter(shipModelMst, -1, isDamaged: false);
			base.transform.localPositionX(ShipIn.x + L2dBias.x);
			if (shipModelMst == null)
			{
				DebugMstID = 0;
			}
			if (HeadArea != null && shipModel != null)
			{
				Transform transform = HeadArea.transform;
				Point face = shipModel.Offsets.GetFace(damaged: false);
				transform.localPositionX((float)face.x - L2dBias.x);
				Transform transform2 = HeadArea.transform;
				Point face2 = shipModel.Offsets.GetFace(damaged: false);
				transform2.localPositionY((float)(-face2.y) - L2dBias.y);
				HeadArea.depth = 15;
			}
		}

		private void OnDestroy()
		{
			nowShipModel = null;
			collision = null;
			DeckStateTag = null;
		}

		public void ResetTouchCount()
		{
			collision.ResetTouchCount();
		}

		public void SetCollisionEnable(bool isEnable)
		{
			collision.SetActive(isEnable);
		}

		public void SetEnableBackTouch(bool isEnable)
		{
			collision.SetEnableBackTouch(isEnable);
		}
	}
}
