using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicHangingsFurniture3MillionPeopleMemorial : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Main;

		[SerializeField]
		private Texture mTexture2d_Frame_0;

		[SerializeField]
		private Texture mTexture2d_Frame_1;

		[SerializeField]
		private Texture mTexture2d_Frame_2;

		[SerializeField]
		private AudioClip mAudioClip_Up;

		[SerializeField]
		private AudioClip mAudioClip_Down;

		private bool mIsUnhappiness;

		protected override void OnCalledActionEvent()
		{
			Animation();
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.A))
			{
				Animation();
			}
		}

		private void Animation()
		{
			mIsUnhappiness = !mIsUnhappiness;
			if (mIsUnhappiness)
			{
				Down();
			}
			else
			{
				GenerateUpTween();
			}
		}

		private Tween GenerateUpTween()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			Sequence sequence = DOTween.Sequence();
			sequence.SetId(this);
			TweenCallback action = delegate
			{
				SoundUtils.PlaySE(mAudioClip_Up);
			};
			TweenCallback callback = delegate
			{
				mTexture_Main.mainTexture = mTexture2d_Frame_0;
			};
			TweenCallback callback2 = delegate
			{
				mTexture_Main.mainTexture = mTexture2d_Frame_1;
			};
			TweenCallback callback3 = delegate
			{
				mTexture_Main.mainTexture = mTexture2d_Frame_2;
			};
			sequence.OnPlay(action);
			sequence.AppendCallback(callback3);
			sequence.AppendInterval(0.1f);
			sequence.AppendCallback(callback2);
			sequence.AppendInterval(0.1f);
			sequence.AppendCallback(callback);
			return sequence;
		}

		private void Down()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			SoundUtils.PlaySE(mAudioClip_Down);
			mTexture_Main.mainTexture = mTexture2d_Frame_2;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Main);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Frame_0);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Frame_1);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Frame_2);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_Up);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_Down);
		}
	}
}
