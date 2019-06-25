using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeShutter : MonoBehaviour
	{
		[SerializeField]
		private Animation mAnimation_Shutter;

		[SerializeField]
		private Transform mTransform_ShutterTop;

		[SerializeField]
		private Transform mTransform_ShutterBotton;

		private Vector3 mVector3_LocalPositionCloseShutterTop;

		private Vector3 mVector3_LocalPositionCloseShutterBottom;

		private Action mOnFinishedOpenShutterAnimationListener;

		private Action mOnFinishedCloseShutterAnimationListener;

		private void Awake()
		{
			mVector3_LocalPositionCloseShutterTop = mTransform_ShutterTop.localPosition;
			mVector3_LocalPositionCloseShutterBottom = mTransform_ShutterBotton.localPosition;
			OpenWithNonAnimation();
		}

		public void SetOnFinishedOpenShutterAnimationListener(Action onFinished)
		{
			mOnFinishedOpenShutterAnimationListener = onFinished;
		}

		public void SetOnFinishedCloseShutterAnimationListener(Action onFinished)
		{
			mOnFinishedCloseShutterAnimationListener = onFinished;
		}

		public void OpenWithNonAnimation()
		{
			mTransform_ShutterTop.localPositionY(230f);
			mTransform_ShutterBotton.localPositionY(-230f);
		}

		public void CloseWithNonAnimation()
		{
			mTransform_ShutterTop.localPositionY(mVector3_LocalPositionCloseShutterTop.y);
			mTransform_ShutterBotton.localPositionY(mVector3_LocalPositionCloseShutterBottom.y);
		}

		public void OpenShutter()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_034);
			mAnimation_Shutter.Play("Anim_OpenShutter");
		}

		public void CloseShutter()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_034);
			mAnimation_Shutter.Play("Anim_CloseShutter");
		}

		public void OnFinishedOpenShutterAnimation()
		{
			if (mOnFinishedOpenShutterAnimationListener != null)
			{
				mOnFinishedOpenShutterAnimationListener();
			}
		}

		public void OnFinishedCloseShutterAnimation()
		{
			if (mOnFinishedCloseShutterAnimationListener != null)
			{
				mOnFinishedCloseShutterAnimationListener();
			}
		}

		private void OnDestroy()
		{
			mOnFinishedOpenShutterAnimationListener = null;
			mOnFinishedCloseShutterAnimationListener = null;
			if ((UnityEngine.Object)mAnimation_Shutter != null)
			{
				mAnimation_Shutter.Stop();
			}
			mAnimation_Shutter = null;
			mTransform_ShutterTop = null;
			mTransform_ShutterBotton = null;
		}
	}
}
