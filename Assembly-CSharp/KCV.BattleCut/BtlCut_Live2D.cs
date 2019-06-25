using KCV.Utils;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class BtlCut_Live2D : MonoBehaviour
	{
		[SerializeField]
		private UIShipCharacter _uiShipCharacter;

		private UIPanel _uiPanel;

		public UIShipCharacter shipCharacter => _uiShipCharacter;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static BtlCut_Live2D Instantiate(BtlCut_Live2D prefab, Transform parent, Vector3 pos)
		{
			BtlCut_Live2D btlCut_Live2D = UnityEngine.Object.Instantiate(prefab);
			btlCut_Live2D.transform.parent = parent;
			btlCut_Live2D.transform.localPositionZero();
			btlCut_Live2D.transform.localScaleOne();
			return btlCut_Live2D;
		}

		private void OnDestroy()
		{
			_uiShipCharacter = null;
			_uiPanel = null;
		}

		public BtlCut_Live2D ChangeMotion(Live2DModel.MotionType iType)
		{
			Live2DModel instance = SingletonMonoBehaviour<Live2DModel>.Instance;
			instance.ChangeMotion(iType);
			return this;
		}

		public BtlCut_Live2D Play()
		{
			Live2DModel instance = SingletonMonoBehaviour<Live2DModel>.Instance;
			instance.PlayOnce(Live2DModel.MotionType.Battle, null);
			return this;
		}

		public BtlCut_Live2D PlayShipVoice(int nVoiceNum)
		{
			Live2DModel instance = SingletonMonoBehaviour<Live2DModel>.Instance;
			ShipUtils.PlayShipVoice(BattleCutManager.GetMapManager().Deck.GetFlagShip(), nVoiceNum);
			return this;
		}

		public BtlCut_Live2D Hide(Action callback)
		{
			shipCharacter.transform.LTValue(1f, 0f, Defines.FORMATION_FORMATIONLABEL_ALPHA_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				shipCharacter.render.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					if (callback != null)
					{
						callback();
					}
				});
			return this;
		}

		public LTDescr Show()
		{
			panel.transform.LTValue(0f, 1f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
			Transform transform = shipCharacter.transform;
			Vector3 localPosition = shipCharacter.transform.localPosition;
			return transform.LTMoveLocal(new Vector3(-200f, localPosition.y, 0f), 0.2f).setEase(LeanTweenType.easeOutQuint);
		}

		public LTDescr ShowLive2D()
		{
			panel.transform.LTValue(0f, 1f, 0.2f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
			Transform transform = shipCharacter.transform;
			Vector3 localPosition = shipCharacter.transform.localPosition;
			return transform.LTMoveLocal(new Vector3(-200f, localPosition.y, 0f), 0.5f).setEase(LeanTweenType.easeOutQuint);
		}
	}
}
