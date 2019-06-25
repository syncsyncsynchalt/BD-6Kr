using Common.Struct;
using live2d;
using local.models;
using System;
using UnityEngine;

namespace KCV
{
	public class UIShipCharacter : MonoBehaviour
	{
		public enum Live2DPosition
		{
			Strategy,
			Port,
			Natural
		}

		[SerializeField]
		protected UITexture Render;

		[SerializeField]
		protected TweenPosition tweenPosition;

		public AnimationCurve ForwardCurve;

		public AnimationCurve ReverseCurve;

		public bool isEnter;

		public Vector3 L2dBias;

		public Vector3 ShipIn;

		public Vector3 ShipIn2;

		public Vector3 ShipOut;

		public ShipModel shipModel;

		public UITexture render => Render;

		private void Awake()
		{
			if (GetComponent<UITexture>() != null)
			{
				Render = GetComponent<UITexture>();
			}
			if (GetComponent<TweenPosition>() != null)
			{
				tweenPosition = GetComponent<TweenPosition>();
			}
		}

		public void moveCharacterX(float targetX, float time, Action act)
		{
			Vector3 localPosition = base.transform.localPosition;
			Vector3 enterPosition = getEnterPosition();
			float y = enterPosition.y;
			Vector3 localPosition2 = base.transform.localPosition;
			PlayMove(localPosition, new Vector3(targetX, y, localPosition2.z), time, act, isActive: true);
		}

		public void moveAddCharacterX(float addX, float time, Action act)
		{
			PlayMove(base.transform.localPosition, base.transform.localPosition + new Vector3(addX, 0f, 0f), time, act, isActive: true);
		}

		public void Enter(Action act)
		{
			if (!isEnter)
			{
				this.SetActive(isActive: true);
				SingletonMonoBehaviour<Live2DModel>.Instance.Enable();
				PlayMove(getExitPosition(), getEnterPosition(), 0.4f, act, isActive: true);
				isEnter = true;
			}
			else
			{
				act?.Invoke();
			}
		}

		public void Exit(Action act, bool isActive = false)
		{
			if (isEnter)
			{
				PlayMove(base.transform.localPosition, getExitPosition(), 0.4f, act, isActive, isReverse: true);
				isEnter = false;
			}
			else
			{
				act?.Invoke();
			}
		}

		private TweenPosition PlayMove(Vector3 from, Vector3 to, float time, Action Onfinished, bool isActive, bool isReverse = false)
		{
			TweenPosition tweenPosition = this.tweenPosition;
			tweenPosition.enabled = true;
			tweenPosition.ResetToBeginning();
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.duration = time;
			tweenPosition.onFinished.Clear();
			tweenPosition.SetOnFinished(delegate
			{
				if (Onfinished != null)
				{
					Onfinished();
				}
				this.SetActive(isActive);
			});
			tweenPosition.animationCurve = ((!isReverse) ? ForwardCurve : ReverseCurve);
			tweenPosition.PlayForward();
			return tweenPosition;
		}

		public Vector3 getEnterPosition()
		{
			return (!SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel) ? ShipIn : (ShipIn + L2dBias);
		}

		public Vector3 getEnterPosition2()
		{
			return (!SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel) ? ShipIn2 : (ShipIn2 + L2dBias);
		}

		public Vector3 getExitPosition()
		{
			return (!SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel) ? ShipOut : (ShipOut + L2dBias);
		}

		public void ChangeCharacter()
		{
			ChangeCharacter(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip());
		}

		public void ChangeCharacter(ShipModelMst model)
		{
			ChangeCharacter(model, -1, isDamaged: false);
		}

		public virtual void ChangeCharacter(ShipModel model)
		{
			ChangeCharacter(model, -1);
		}

		public void ChangeCharacter(Live2DModelUnity Live2D, ShipModel model)
		{
			Point live2dBias = model.Offsets.GetLive2dBias();
			float x = live2dBias.x;
			Point live2dBias2 = model.Offsets.GetLive2dBias();
			L2dBias = new Vector3(x, live2dBias2.y, 0f);
			Point boko = model.Offsets.GetBoko(damaged: false);
			float x2 = boko.x;
			Point boko2 = model.Offsets.GetBoko(damaged: false);
			ShipIn = new Vector3(x2, boko2.y, 0f);
			Point cutinSp1_InBattle = model.Offsets.GetCutinSp1_InBattle(damaged: false);
			float x3 = cutinSp1_InBattle.x;
			Point cutinSp1_InBattle2 = model.Offsets.GetCutinSp1_InBattle(damaged: false);
			ShipIn2 = new Vector3(x3, cutinSp1_InBattle2.y, 0f);
			Point boko3 = model.Offsets.GetBoko(damaged: false);
			ShipOut = new Vector3(1300f, boko3.y, 0f);
			Render.mainTexture = SingletonMonoBehaviour<Live2DModel>.Instance.ChangeCharacter(Live2D, model);
			UITexture render = Render;
			Point live2dSize = model.Offsets.GetLive2dSize();
			render.width = (int)((float)live2dSize.x * 1.25f);
			UITexture render2 = Render;
			Point live2dSize2 = model.Offsets.GetLive2dSize();
			render2.height = live2dSize2.y;
			Transform transform = base.transform;
			Point boko4 = model.Offsets.GetBoko(damaged: false);
			int y = boko4.y;
			Point live2dBias3 = model.Offsets.GetLive2dBias();
			transform.localPositionY(y + live2dBias3.y);
		}

		public void ChangeCharacter(ShipModel model, int DeckID)
		{
			shipModel = model;
			ShipModelMst model2 = (model == null) ? null : model;
			bool isDamaged = model?.IsDamaged() ?? false;
			ChangeCharacter(model2, DeckID, isDamaged);
		}

		public void ChangeCharacter(ShipModelMst model, int DeckID, bool isDamaged)
		{
			if (CheckMstIDEnable(model))
			{
				if (tweenPosition != null)
				{
					tweenPosition.enabled = false;
				}
				int graphicsMstId = model.GetGraphicsMstId();
				if (DeckID == -1)
				{
					Render.mainTexture = SingletonMonoBehaviour<Live2DModel>.Instance.ChangeCharacter(graphicsMstId, isDamaged);
				}
				else
				{
					Render.mainTexture = SingletonMonoBehaviour<Live2DModel>.Instance.ChangeCharacter(graphicsMstId, isDamaged, DeckID);
				}
				Point live2dBias = model.Offsets.GetLive2dBias();
				float x = live2dBias.x;
				Point live2dBias2 = model.Offsets.GetLive2dBias();
				L2dBias = new Vector3(x, live2dBias2.y, 0f);
				Point boko = model.Offsets.GetBoko(isDamaged);
				float x2 = boko.x;
				Point boko2 = model.Offsets.GetBoko(isDamaged);
				ShipIn = new Vector3(x2, boko2.y, 0f);
				Point cutinSp1_InBattle = model.Offsets.GetCutinSp1_InBattle(isDamaged);
				float x3 = cutinSp1_InBattle.x;
				Point cutinSp1_InBattle2 = model.Offsets.GetCutinSp1_InBattle(isDamaged);
				ShipIn2 = new Vector3(x3, cutinSp1_InBattle2.y, 0f);
				Point boko3 = model.Offsets.GetBoko(isDamaged);
				ShipOut = new Vector3(1300f, boko3.y, 0f);
				if (SingletonMonoBehaviour<Live2DModel>.Instance.isLive2DModel)
				{
					UITexture render = Render;
					Point live2dSize = model.Offsets.GetLive2dSize();
					render.width = (int)((float)live2dSize.x * 1380f / 1024f);
					UITexture render2 = Render;
					Point live2dSize2 = model.Offsets.GetLive2dSize();
					render2.height = live2dSize2.y;
					Transform transform = base.transform;
					Point boko4 = model.Offsets.GetBoko(isDamaged);
					int y = boko4.y;
					Point live2dBias3 = model.Offsets.GetLive2dBias();
					transform.localPositionY(y + live2dBias3.y);
				}
				else
				{
					L2dBias = Vector3.zero;
					Transform transform2 = base.transform;
					Point boko5 = model.Offsets.GetBoko(isDamaged);
					transform2.localPositionY(boko5.y);
					Render.MakePixelPerfect();
				}
			}
		}

		protected virtual bool CheckMstIDEnable(ShipModelMst model)
		{
			if (model == null)
			{
				Render.mainTexture = null;
				return false;
			}
			return true;
		}

		public float getModelDefaultPosX()
		{
			ShipModel flagShip = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip();
			if (flagShip == null)
			{
				return 0f;
			}
			Point boko = flagShip.Offsets.GetBoko(flagShip.IsDamaged());
			int x = boko.x;
			Point live2dBias = flagShip.Offsets.GetLive2dBias();
			return x + live2dBias.x;
		}

		public int GetWidth()
		{
			return Render.width;
		}

		public int GetHeight()
		{
			return Render.height;
		}
	}
}
