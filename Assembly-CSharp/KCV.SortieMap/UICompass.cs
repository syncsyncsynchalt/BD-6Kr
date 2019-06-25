using KCV.Utils;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIWidget))]
	public class UICompass : MonoBehaviour
	{
		public enum Power
		{
			Low,
			High
		}

		[SerializeField]
		private UISprite _uiBase;

		private UIWidget _uiWidget;

		private Action _actOnStopCompass;

		private float _fCompassPoint;

		private UIWidget widget => this.GetComponentThis(ref _uiWidget);

		private float targetCompassPoint => Mathe.Euler2Deg(_fCompassPoint);

		private float nowCompassPoint
		{
			get
			{
				Vector3 eulerAngles = _uiBase.transform.localRotation.eulerAngles;
				return Mathe.Euler2Deg(eulerAngles.z);
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiWidget);
			Mem.Del(ref _actOnStopCompass);
			Mem.Del(ref _uiBase);
		}

		public bool Init(Action onStop, Transform from, Transform to)
		{
			_actOnStopCompass = onStop;
			widget.alpha = 0f;
			base.transform.localScale = Vector3.one;
			_uiBase.transform.rotation = Quaternion.identity;
			_fCompassPoint = CalcTargetCompassPoint(from.position, to.position);
			return true;
		}

		private float CalcTargetCompassPoint(Vector3 from, Vector3 to)
		{
			return Mathe.Rad2Deg(Mathf.Atan2(to.y - from.y, to.x - from.x));
		}

		public LTDescr Show()
		{
			base.transform.localScale = Vector3.one * 1.1f;
			base.transform.localPositionY(50f);
			widget.transform.LTValue(widget.alpha, 1f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				widget.alpha = x;
			});
			widget.transform.LTMoveLocalY(0f, 0.5f).setEase(LeanTweenType.easeInCubic);
			return widget.transform.LTScale(Vector3.one, 0.5f);
		}

		public LTDescr Hide()
		{
			widget.transform.LTValue(widget.alpha, 0f, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				widget.alpha = x;
			});
			widget.transform.LTMoveLocalY(50f, 0.5f).setEase(LeanTweenType.easeOutCubic);
			return widget.transform.LTScale(Vector3.one * 1.1f, 0.5f).setEase(LeanTweenType.easeOutQuad);
		}

		public void StopRoll(Power iPower)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_031);
			switch (iPower)
			{
			case Power.Low:
				_uiBase.transform.LTRotateAroundLocal(Vector3.back, targetCompassPoint - nowCompassPoint + (float)(360 * XorRandom.GetILim(2, 3)) - 90f, 2f).setEase(LeanTweenType.easeOutQuad).setOnComplete((Action)delegate
				{
					Dlg.Call(ref _actOnStopCompass);
				});
				break;
			case Power.High:
				_uiBase.transform.LTRotateAroundLocal(Vector3.back, targetCompassPoint - nowCompassPoint + (float)(360 * XorRandom.GetILim(3, 4)) - 90f, 2f).setEase(LeanTweenType.easeOutElastic).setOnComplete((Action)delegate
				{
					Dlg.Call(ref _actOnStopCompass);
				});
				break;
			}
		}
	}
}
