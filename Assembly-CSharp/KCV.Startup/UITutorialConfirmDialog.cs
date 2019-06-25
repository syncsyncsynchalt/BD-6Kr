using KCV.Utils;
using Librarys.Object;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	public class UITutorialConfirmDialog : AbsDialog<int, UIDialogButton>
	{
		[Serializable]
		private struct Params : IDisposable
		{
			public float showDialogOpenTime;

			public float showOverlayAlphaTime;

			public void Dispose()
			{
			}
		}

		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private Transform _traDialogBackground;

		[SerializeField]
		private Params _strParams;

		public static UITutorialConfirmDialog Instantiate(UITutorialConfirmDialog prefab, Transform parent)
		{
			UITutorialConfirmDialog uITutorialConfirmDialog = UnityEngine.Object.Instantiate(prefab);
			uITutorialConfirmDialog.transform.parent = parent;
			uITutorialConfirmDialog.transform.localPositionZero();
			uITutorialConfirmDialog.transform.localScaleOne();
			uITutorialConfirmDialog.Setup();
			return uITutorialConfirmDialog;
		}

		private bool Setup()
		{
			_uiOverlay.alpha = 0f;
			_traDialogBackground.localScale = Vector3.one * 0.5f;
			int cnt = 0;
			_listButtons.ForEach(delegate(UIDialogButton x)
			{
				int nIndex = cnt;
				List<EventDelegate> onActive = Util.CreateEventDelegateList(this, "OnActive", cnt);
				UITutorialConfirmDialog uITutorialConfirmDialog = this;
				x.Init(nIndex, isValid: true, isColliderEnabled: true, 11, onActive, ((AbsDialog<int, UIDialogButton>)uITutorialConfirmDialog).OnDecide);
				cnt++;
			});
			return true;
		}

		public override bool Init(Action onCancel, Action<int> onDecide)
		{
			_actOnCancel = onCancel;
			_actOnDecide = onDecide;
			return true;
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _uiOverlay);
			Mem.Del(ref _traDialogBackground);
		}

		protected override void PreparaNext(bool isFoward)
		{
			int currentIndex = base.currentIndex;
			base.currentIndex = Mathe.NextElement(base.currentIndex, 0, 1, isFoward);
			if (currentIndex != base.currentIndex)
			{
				ChangeFocus(base.currentIndex);
			}
		}

		protected override void OpenAnimation(Action onFinished)
		{
			_traDialogBackground.LTScale(Vector3.one, _strParams.showDialogOpenTime).setEase(LeanTweenType.linear).setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
			});
			_uiOverlay.transform.LTValue(_uiOverlay.alpha, 0.5f, _strParams.showOverlayAlphaTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiOverlay.alpha = x;
			});
		}

		protected override void CloseAimation(Action onFinished)
		{
		}

		protected override void OnChangeFocus()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
		}

		protected override void OnActive(int nIndex)
		{
			base.currentIndex = nIndex;
			OnDecide();
		}

		public override void OnCancel()
		{
			_listButtons.ForEach(delegate(UIDialogButton x)
			{
				x.toggle.enabled = false;
			});
			base.OnCancel();
		}

		public override void OnDecide()
		{
			_listButtons.ForEach(delegate(UIDialogButton x)
			{
				x.toggle.enabled = false;
			});
			Dlg.Call(ref _actOnDecide, base.currentIndex);
		}
	}
}
