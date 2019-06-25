using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class UIPressAnyKey : MonoBehaviour
	{
		[Serializable]
		private struct Param
		{
			[SerializeField]
			private float _fInterval;

			[SerializeField]
			private Vector3 _vPos;

			[SerializeField]
			private AnimationCurve _acCurve;

			public float interval => _fInterval;

			public Vector3 pos => _vPos;

			public AnimationCurve curve => _acCurve;

			public Param(float interval, Vector3 pos, AnimationCurve curve)
			{
				_fInterval = interval;
				_vPos = pos;
				_acCurve = curve;
			}
		}

		[SerializeField]
		private UISprite _uiPressAnyKey;

		[SerializeField]
		private Param _strParam;

		[SerializeField]
		private UIInvisibleCollider _uiInvisivleCollider;

		private bool _isPress;

		private Action _actOnPress;

		public static UIPressAnyKey Instantiate(UIPressAnyKey prefab, Transform parent, Action onPress)
		{
			UIPressAnyKey uIPressAnyKey = UnityEngine.Object.Instantiate(prefab);
			uIPressAnyKey.transform.parent = parent;
			uIPressAnyKey.transform.localScaleOne();
			uIPressAnyKey.transform.localPosition = uIPressAnyKey._strParam.pos;
			uIPressAnyKey.Init(onPress);
			return uIPressAnyKey;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiPressAnyKey);
			Mem.Del(ref _strParam);
			Mem.Del(ref _uiInvisivleCollider);
			Mem.Del(ref _actOnPress);
		}

		private bool Init(Action onPress)
		{
			_isPress = false;
			_actOnPress = onPress;
			_uiInvisivleCollider.SetOnTouch(OnPress);
			return true;
		}

		public bool Run()
		{
			KeyControl keyControl = TitleTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.MARU) || keyControl.GetDown(KeyControl.KeyName.BATU) || keyControl.GetDown(KeyControl.KeyName.SHIKAKU) || keyControl.GetDown(KeyControl.KeyName.SANKAKU) || keyControl.GetDown(KeyControl.KeyName.START))
			{
				OnPress();
				return true;
			}
			_uiPressAnyKey.alpha = (float)Math.Abs(Math.Sin(Time.time * _strParam.interval));
			return _isPress;
		}

		private void OnPress()
		{
			_isPress = true;
			_uiInvisivleCollider.collider2D.enabled = false;
			PlayPressAnyKey().setOnComplete(_actOnPress);
		}

		private LTDescr PlayPressAnyKey()
		{
			_uiPressAnyKey.alpha = 1f;
			return _uiPressAnyKey.transform.LTValue(0f, 1f, 1.25f).setOnUpdate(delegate(float x)
			{
				_uiPressAnyKey.alpha = x;
			}).setEase(_strParam.curve);
		}
	}
}
