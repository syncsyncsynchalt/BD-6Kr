using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionHeader : MonoBehaviour
	{
		[SerializeField]
		private UISprite _uiBackground;

		[SerializeField]
		private List<Animation> _listGearAnimation;

		[SerializeField]
		private UILabel _uiLabel;

		[Header("[Animation Properties]")]
		[SerializeField]
		private Vector3 _vShowPos = new Vector3(171f, 233f, 0f);

		[SerializeField]
		private Vector3 _vHidePos = new Vector3(171f, 315f, 0f);

		private UIPanel _uiPanel;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static UIRebellionHeader Instantiate(UIRebellionHeader prefab, Transform parent)
		{
			UIRebellionHeader uIRebellionHeader = UnityEngine.Object.Instantiate(prefab);
			uIRebellionHeader.transform.parent = parent;
			uIRebellionHeader.transform.localPositionZero();
			uIRebellionHeader.transform.localScaleOne();
			uIRebellionHeader.Init();
			return uIRebellionHeader;
		}

		private bool Init()
		{
			_listGearAnimation.ForEach(delegate(Animation x)
			{
				x.Stop();
			});
			base.transform.localPosition = _vHidePos;
			panel.alpha = 0f;
			panel.widgetsAreStatic = true;
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.DelListSafe(ref _listGearAnimation);
			Mem.Del(ref _uiLabel);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _vShowPos);
			Mem.Del(ref _vHidePos);
		}

		public void Show(Action onFinished)
		{
			panel.widgetsAreStatic = false;
			_listGearAnimation.ForEach(delegate(Animation x)
			{
				x.Play();
			});
			panel.transform.LTValue(panel.alpha, 1f, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
			base.transform.LTMoveLocal(_vShowPos, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public void Hide(Action onFinished)
		{
			panel.transform.LTValue(panel.alpha, 0f, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
			base.transform.LTMoveLocal(_vHidePos, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
				_listGearAnimation.ForEach(delegate(Animation x)
				{
					x.Stop();
				});
				panel.widgetsAreStatic = true;
			});
		}
	}
}
