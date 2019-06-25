using Common.Enum;
using local.models;
using Server_Controllers;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHexTile : MonoBehaviour
	{
		[SerializeField]
		private UISprite HexTile;

		[SerializeField]
		private UIButton TileButton;

		[SerializeField]
		private StrategyTileDockIcons DockIcons;

		private UISprite ShadowTile;

		private UISprite GrowTile;

		private TweenScale ScaleAnimation;

		private MapAreaModel areaModel;

		public int DebugRP = 100;

		public bool isFocus;

		public bool isMovable;

		public bool isColorChanged;

		private static readonly Color32 AttentionColor = new Color32(byte.MaxValue, 185, 11, 110);

		private static readonly Color32 CautionColor = new Color32(byte.MaxValue, 167, 107, 200);

		private static readonly Color32 WarningColor = new Color32(byte.MaxValue, 107, 107, 200);

		private static readonly Color32 AlertColor = new Color32(byte.MaxValue, 40, 40, 200);

		private static readonly Color32 InvationColor = new Color32(byte.MaxValue, 0, 0, 200);

		[Button("TileColorRefreshdebug", "TileColorRefreshdebug", new object[]
		{

		})]
		public int button1;

		[Button("SetRebellionPoint", "SetRebellionPoint", new object[]
		{

		})]
		public int button2;

		public int areaID => areaModel.Id;

		public bool isOpen => areaModel.IsOpen();

		public bool isRebellionTile
		{
			get;
			private set;
		}

		public bool isNatural => !isFocus && !isMovable;

		public UIButton getTileButton()
		{
			return TileButton;
		}

		public MapAreaModel GetAreaModel()
		{
			return areaModel;
		}

		private void Awake()
		{
			ScaleAnimation = GetComponent<TweenScale>();
			base.transform.localScaleX(0f);
			base.transform.localScaleY(0f);
			GrowTile = ((Component)base.transform.FindChild("Grow")).GetComponent<UISprite>();
			ShadowTile = ((Component)GrowTile.transform.FindChild("Shadow")).GetComponent<UISprite>();
			TileButton.tweenTarget = GrowTile.gameObject;
			ShadowTile.enabled = true;
			GrowTile.enabled = true;
			TileButton.duration = 0.5f;
			EventDelegate.Add(TileButton.onClick, PlayBoundAnimation);
			EventDelegate.Add(TileButton.onClick, OnPushFocusArea);
			EventDelegate.Add(TileButton.onClick, OnPushFocusChange);
		}

		public void PlayPopUpAnimation(Action<bool> callBack, float delay)
		{
			ScaleAnimation.delay = delay;
			ScaleAnimation.PlayForward();
			if (callBack != null)
			{
				ScaleAnimation.SetOnFinished(delegate
				{
					callBack(obj: true);
				});
			}
		}

		public void setAreaModel(MapAreaModel model)
		{
			areaModel = model;
		}

		public UISprite getSprite()
		{
			return HexTile;
		}

		public void PlayBoundAnimation()
		{
			if (StrategyTopTaskManager.GetSailSelect().isRun || StrategyTopTaskManager.GetShipMove().isRun)
			{
				ScaleAnimation.delay = 0f;
				ScaleAnimation.from = new Vector3(0.9f, 0.9f, 0.9f);
				ScaleAnimation.ResetToBeginning();
				ScaleAnimation.PlayForward();
			}
		}

		private void OnPushFocusChange()
		{
			if (StrategyTopTaskManager.GetSailSelect().isRun || StrategyTopTaskManager.GetShipMove().isRun)
			{
				StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(areaID);
			}
		}

		private void OnPushFocusArea()
		{
			if ((StrategyTopTaskManager.GetSailSelect().isRun || StrategyTopTaskManager.GetShipMove().isRun) && StrategyTopTaskManager.Instance.TileManager.FocusTile == this)
			{
				if (StrategyTopTaskManager.GetSailSelect().isRun)
				{
					StrategyTopTaskManager.GetSailSelect().OpenCommandMenu();
				}
				else if (areaID == SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID)
				{
					StrategyTopTaskManager.GetShipMove().OnMoveCancel();
				}
				else
				{
					StrategyTopTaskManager.GetShipMove().OnMoveDeside();
				}
			}
		}

		public void setTileColor()
		{
			if (isNatural || isFocus)
			{
				Color color = getRebellionColor();
				TileButton.hover = color;
				TileButton.pressed = color;
				TileButton.defaultColor = color;
				isColorChanged = ((color != Color.clear) ? true : false);
			}
			else if (isMovable)
			{
				TileButton.hover = Color.blue;
				TileButton.pressed = Color.blue;
				TileButton.defaultColor = Color.blue;
			}
			else
			{
				Debug.Log("Tile" + areaID);
			}
		}

		public void setRebellionTile()
		{
			isRebellionTile = true;
			Color color = InvationColor;
			TileButton.hover = color;
			TileButton.pressed = color;
			TileButton.defaultColor = color;
		}

		private Color32 getRebellionColor()
		{
			switch (areaModel.RState)
			{
			case RebellionState.Safety:
				return Color.clear;
			case RebellionState.Attention:
				return AttentionColor;
			case RebellionState.Caution:
				return CautionColor;
			case RebellionState.Warning:
				return WarningColor;
			case RebellionState.Alert:
				return AlertColor;
			case RebellionState.Invation:
				return InvationColor;
			default:
				return Color.white;
			}
		}

		public void TileColorRefreshdebug()
		{
			TileColorRefresh(null);
		}

		public void TileColorRefresh(Action Onfinished)
		{
			TileButton.enabled = false;
			TweenAlpha tweenAlpha = TweenAlpha.Begin(GrowTile.gameObject, 3f, 0f);
			tweenAlpha.onFinished.Clear();
			tweenAlpha.SetOnFinished(delegate
			{
				TileButton.enabled = true;
				TileButton.defaultColor = Color.clear;
				TileButton.hover = Color.clear;
				TileButton.pressed = Color.clear;
				if (Onfinished != null)
				{
					Onfinished();
				}
			});
		}

		public void ChangeMoveTileColor()
		{
			TweenColor.Begin(HexTile.gameObject, 0.2f, Color.cyan);
			ShadowTile.depth = 1;
			GrowTile.depth = 2;
		}

		public void ClearTileColor()
		{
			TweenColor.Begin(HexTile.gameObject, 0.2f, Color.white);
			ShadowTile.depth = 4;
			GrowTile.depth = 5;
		}

		public Vector3 getDefaultPosition()
		{
			return base.transform.localPosition + base.transform.parent.localPosition;
		}

		public void PlayAreaOpenAnimation(GameObject AnimationPrefab, Action Onfinished)
		{
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			this.SetActive(isActive: true);
			base.transform.localScale = Vector3.zero;
			ScaleAnimation.from = Vector3.zero;
			ScaleAnimation.delay = 0f;
			ScaleAnimation.ResetToBeginning();
			ScaleAnimation.PlayForward();
			ScaleAnimation.duration = 2f;
			this.SafeGetTweenRotation(Vector3.zero, new Vector3(0f, 0f, 720f), 1.5f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
			Keyframe[] array = new Keyframe[2]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			};
			ScaleAnimation.animationCurve = new AnimationCurve(array);
			GameObject gameObject = Util.Instantiate(AnimationPrefab, base.transform.parent.gameObject);
			gameObject.transform.transform.position = HexTile.transform.position;
			GrowTile.color = Color.clear;
			StartCoroutine(waitAnimationFinish(gameObject, Onfinished, 2f));
		}

		public void PlayAreaCloseAnimation(Action Onfinished)
		{
			StartTileBreakAnimation(Onfinished);
		}

		private IEnumerator waitAnimationFinish(GameObject Animation, Action Onfinished, float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			Onfinished();
			if (Animation != null)
			{
				UnityEngine.Object.Destroy(Animation.gameObject);
			}
			yield return null;
		}

		public void setActivePositionAnimation(bool isActive)
		{
		}

		public void StartTileBreakAnimation(Action Onfinished)
		{
			StrategyHexBreak strategyHexBreak = StrategyHexBreak.Instantiate(Resources.Load<StrategyHexBreak>("Prefabs/StrategyPrefab/StrategyTop/TileBreak"), base.transform);
			this.DelayActionFrame(3, delegate
			{
				strategyHexBreak.Play(delegate
				{
					Onfinished();
					base.transform.localScale = Vector3.zero;
					UnityEngine.Object.Destroy(strategyHexBreak.gameObject);
				});
				HexTile.alpha = 0f;
				GrowTile.alpha = 0f;
			});
		}

		public void StartTileLightAnimation(Action Onfinished)
		{
			StrategyHexLight strategyHexLight = StrategyHexLight.Instantiate(Resources.Load<StrategyHexLight>("Prefabs/StrategyPrefab/StrategyTop/TileLight"), base.transform);
			this.DelayActionFrame(3, delegate
			{
				strategyHexLight.Play(delegate
				{
					if (Onfinished != null)
					{
						Onfinished();
					}
					UnityEngine.Object.Destroy(strategyHexLight.gameObject);
				});
			});
			TileButton.enabled = true;
			TileButton.defaultColor = Color.clear;
			TileButton.hover = Color.clear;
			TileButton.pressed = Color.clear;
		}

		private void SetRebellionPoint()
		{
			Debug_Mod.SetRebellionPoint(areaID, DebugRP);
			StrategyTopTaskManager.CreateLogicManager();
			setTileColor();
		}

		public void UpdateDockIcons()
		{
			if (DockIcons != null)
			{
				DockIcons.SetDockIcon(areaModel);
			}
		}

		public void SetVisibleDockIcons(bool isVisible)
		{
			if (DockIcons != null)
			{
				DockIcons.SetActive(isVisible);
			}
		}
	}
}
