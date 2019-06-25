using Common.Enum;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	public class UICompassManager : MonoBehaviour
	{
		[Serializable]
		private class CompassGirlMessage
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UISprite _uiBackground;

			[SerializeField]
			private UILabel _uiMessage;

			private UIWidget _uiWidget;

			public Transform transform => _tra;

			public UIWidget widget
			{
				get
				{
					if (_uiWidget == null)
					{
						_uiWidget = ((Component)transform).GetComponent<UIWidget>();
					}
					return _uiWidget;
				}
			}

			private CompassGirlMessage()
			{
			}

			public bool Init()
			{
				widget.alpha = 0f;
				_uiMessage.text = string.Empty;
				return true;
			}

			public bool UnInit()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiBackground);
				Mem.Del(ref _uiMessage);
				Mem.Del(ref _uiWidget);
				return true;
			}

			public void ClearMessage()
			{
				_uiMessage.text = string.Empty;
			}

			public void UpdateMessage(string message)
			{
				_uiMessage.text = message;
			}

			public LTDescr Show()
			{
				return transform.LTValue(widget.alpha, 1f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
				{
					widget.alpha = x;
				});
			}

			public LTDescr Hide()
			{
				return transform.LTValue(widget.alpha, 0f, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
				{
					widget.alpha = x;
				});
			}
		}

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UICompass _uiCompass;

		[SerializeField]
		private UICompassGirl _uiCompassGirl;

		[SerializeField]
		private CompassGirlMessage _clsCompassGirlMessage;

		[SerializeField]
		private UIInvisibleCollider _uiInvisibleCollider;

		private Action _actOnFinished;

		public static UICompassManager Instantiate(UICompassManager prefab, Transform parent, CompassType iCompassType, Transform ship, Transform nextCell)
		{
			UICompassManager uICompassManager = UnityEngine.Object.Instantiate(prefab);
			uICompassManager.transform.parent = parent;
			uICompassManager.transform.localPositionZero();
			uICompassManager.transform.localScaleOne();
			uICompassManager.Init(iCompassType, ship, nextCell);
			return uICompassManager;
		}

		private bool Init(CompassType iCompassType, Transform ship, Transform nextCell)
		{
			_uiBackground.alpha = 0f;
			_uiCompassGirl.Init(OnUpdateMessage, OnStopRollCompass, iCompassType);
			_clsCompassGirlMessage.Init();
			_uiCompass.Init(OnStoppedCompass, ship, nextCell);
			_uiInvisibleCollider.SetOnTouch(delegate
			{
			});
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiCompassGirl);
			Mem.Del(ref _uiCompass);
			Mem.Del(ref _uiBackground);
			_clsCompassGirlMessage.UnInit();
			Mem.Del(ref _clsCompassGirlMessage);
			Mem.Del(ref _actOnFinished);
		}

		public void Play(Action onFinished)
		{
			_actOnFinished = onFinished;
			_uiBackground.transform.LTValue(0f, 0.5f, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				_uiBackground.alpha = x;
			});
			_uiCompass.Show().setOnComplete(OnShowCompass);
		}

		private void OnShowCompass()
		{
			_clsCompassGirlMessage.Show();
			_uiCompassGirl.Play();
		}

		private void OnStoppedCompass()
		{
			_uiBackground.transform.LTValue(_uiBackground.alpha, 0f, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				_uiBackground.alpha = x;
			});
			_clsCompassGirlMessage.Hide();
			_uiCompassGirl.Hide();
			_uiCompass.Hide().setOnComplete(OnFinished);
		}

		private void OnUpdateMessage(string message)
		{
			_clsCompassGirlMessage.UpdateMessage(message);
		}

		private void OnStopRollCompass(UICompass.Power power)
		{
			_uiCompass.StopRoll(power);
		}

		private void OnFinished()
		{
			Dlg.Call(ref _actOnFinished);
		}
	}
}
