using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyOhyodo : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Ohyodo;

		[SerializeField]
		private Texture[] mTextureOhyodo;

		[SerializeField]
		private Vector3 mVector3_Ohyodo_ShowLocalPosition;

		[SerializeField]
		private Vector3 mVector3_Ohyodo_GoBackLocalPosition;

		[SerializeField]
		private Vector3 mVector3_Ohyodo_WaitingLocalPosition;

		[SerializeField]
		private UIButton mButton_TouchBackArea;

		private void Start()
		{
			mTexture_Ohyodo.transform.localPosition = mVector3_Ohyodo_WaitingLocalPosition;
			mTexture_Ohyodo.mainTexture = mTextureOhyodo[0];
			EnableTouchBackArea(enabled: false);
		}

		public void Show(Action onFinishedAnimation)
		{
			ShipUtils.PlayPortVoice(1);
			mTexture_Ohyodo.transform.DOLocalMove(mVector3_Ohyodo_ShowLocalPosition, 0.4f).OnComplete(delegate
			{
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation();
				}
			}).SetEase(Ease.OutSine);
		}

		public void Hide(Action onFinishedAnimation)
		{
			mTexture_Ohyodo.mainTexture = mTextureOhyodo[1];
			mTexture_Ohyodo.transform.DOLocalMove(mVector3_Ohyodo_GoBackLocalPosition, 0.4f).SetDelay(0.5f).OnComplete(delegate
			{
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation();
				}
			})
				.SetEase(Ease.OutSine);
		}

		public void EnableTouchBackArea(bool enabled)
		{
			mButton_TouchBackArea.enabled = enabled;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTextureOhyodo);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_TouchBackArea);
		}
	}
}
