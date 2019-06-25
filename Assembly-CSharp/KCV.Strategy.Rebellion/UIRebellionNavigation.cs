using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionNavigation : MonoBehaviour
	{
		[SerializeField]
		private UIPanel _uiPanel;

		[SerializeField]
		private UIHowTo _uiHowTo;

		[Header("[Animation Properties]")]
		[SerializeField]
		private Vector3 _vShowPos = new Vector3(480f, -238f, 0f);

		[SerializeField]
		private Vector3 _vHidePos = new Vector3(1000f, -238f, 0f);

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static UIRebellionNavigation Instantiate(UIRebellionNavigation prefab, Transform parent, CtrlRebellionOrganize.RebellionOrganizeMode iMode)
		{
			UIRebellionNavigation uIRebellionNavigation = UnityEngine.Object.Instantiate(prefab);
			uIRebellionNavigation.transform.parent = parent;
			uIRebellionNavigation.transform.localPosition = uIRebellionNavigation._vHidePos;
			uIRebellionNavigation.transform.localScaleOne();
			uIRebellionNavigation.SetNavigation(iMode);
			return uIRebellionNavigation;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _uiHowTo);
			Mem.Del(ref _vShowPos);
			Mem.Del(ref _vShowPos);
		}

		public void SetNavigation(CtrlRebellionOrganize.RebellionOrganizeMode iMode)
		{
			DebugUtils.Log("反抗編成ナビ設定:" + iMode);
			List<UIHowToDetail> list = new List<UIHowToDetail>();
			panel.widgetsAreStatic = false;
			switch (iMode)
			{
			case CtrlRebellionOrganize.RebellionOrganizeMode.Main:
				list.Add(new UIHowToDetail(HowToKey.btn_sankaku, "詳細をみる"));
				list.Add(new UIHowToDetail(HowToKey.btn_batsu, "出撃中止"));
				list.Add(new UIHowToDetail(HowToKey.btn_shikaku, "はずす"));
				list.Add(new UIHowToDetail(HowToKey.btn_maru, "決定"));
				break;
			case CtrlRebellionOrganize.RebellionOrganizeMode.Detail:
				list.Add(new UIHowToDetail(HowToKey.btn_sankaku, "戻る"));
				list.Add(new UIHowToDetail(HowToKey.btn_maru, "決定"));
				break;
			}
			_uiHowTo.Refresh(list.ToArray());
			panel.widgetsAreStatic = true;
		}

		public void Show(Action onFinished)
		{
			panel.transform.LTCancel();
			panel.widgetsAreStatic = false;
			panel.transform.LTValue(panel.alpha, 1f, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
			panel.transform.LTMoveLocal(_vShowPos, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		public void Hide(Action onFinished)
		{
			panel.transform.LTCancel();
			panel.transform.LTValue(panel.alpha, 0f, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
			panel.transform.LTMoveLocal(_vShowPos, 0.2f).setEase(CtrlRebellionOrganize.STATE_CHANGE_EASING).setOnComplete((Action)delegate
			{
				panel.widgetsAreStatic = true;
				Dlg.Call(ref onFinished);
			});
		}
	}
}
