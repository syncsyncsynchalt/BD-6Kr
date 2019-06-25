using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurnitureHanabi : UIDynamicWindowFurniture
	{
		private const int TRIGGER_HOUR = 20;

		private const int TRIGGER_MINUTE = 0;

		private AudioSource mAudioSource_Playing;

		[SerializeField]
		private AudioClip mAudioClip_Hanabi_0;

		[SerializeField]
		private UITexture mTexture_Fire;

		[SerializeField]
		private Texture mTexture2d_Red;

		[SerializeField]
		private Texture mTexture2d_Yellow;

		[SerializeField]
		private Texture mTexture2d_Blue;

		[SerializeField]
		private Texture mTexture2d_White;

		[SerializeField]
		private bool mAnimated;

		protected override void OnUpdate()
		{
			UpdateWindow();
			if (mFurnitureModel != null && mFurnitureModel.GetDateTime().Hour == 20 && mFurnitureModel.GetDateTime().Minute == 0 && !DOTween.IsTweening(this) && !mAnimated)
			{
				mAnimated = true;
				Animation();
			}
		}

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
		}

		private void Animation()
		{
			Sequence s = DOTween.Sequence().SetId(this);
			Tween t = GenerateFirstFireWorks();
			s.Append(t);
			s.Append(GenerateCommonFireWorks());
			s.Append(GenerateCommonFireWorks());
			s.Append(GenerateCommonFireWorks());
		}

		private Tween GenerateFirstFireWorks()
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			TweenCallback callback = delegate
			{
				mTexture_Fire.mainTexture = mTexture2d_Red;
			};
			TweenCallback callback2 = delegate
			{
				mTexture_Fire.mainTexture = mTexture2d_Yellow;
			};
			TweenCallback callback3 = delegate
			{
				mTexture_Fire.mainTexture = mTexture2d_Blue;
			};
			TweenCallback callback4 = delegate
			{
				mTexture_Fire.mainTexture = mTexture2d_White;
			};
			Tween t = DOVirtual.Float(0f, 1f, 0.15f, delegate(float alpha)
			{
				mTexture_Fire.alpha = alpha;
			});
			Tween t2 = DOVirtual.Float(1f, 0f, 0.8f, delegate(float alpha)
			{
				mTexture_Fire.alpha = alpha;
			});
			Tween t3 = DOVirtual.Float(1f, 0f, 1.6f, delegate(float alpha)
			{
				mTexture_Fire.alpha = alpha;
			});
			TweenCallback action = delegate
			{
				mTexture_Fire.alpha = 0f;
				mAudioSource_Playing = SoundUtils.PlaySE(mAudioClip_Hanabi_0);
			};
			Tween t4 = DOVirtual.Float(0f, 1f, 2f, delegate(float alpha)
			{
				mTexture_Fire.alpha = alpha;
			});
			sequence.OnPlay(action);
			sequence.Append(t4);
			sequence.AppendCallback(callback);
			sequence.AppendInterval(0.15f);
			sequence.AppendCallback(callback2);
			sequence.AppendInterval(0.15f);
			sequence.AppendCallback(callback3);
			sequence.AppendInterval(0f);
			sequence.Append(t2);
			sequence.AppendCallback(callback4);
			sequence.Append(t);
			sequence.Append(t3);
			return sequence;
		}

		private Tween GenerateCommonFireWorks()
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			TweenCallback callback = delegate
			{
				mTexture_Fire.mainTexture = mTexture2d_Red;
			};
			TweenCallback callback2 = delegate
			{
				mTexture_Fire.mainTexture = mTexture2d_Yellow;
			};
			TweenCallback callback3 = delegate
			{
				mTexture_Fire.mainTexture = mTexture2d_Blue;
			};
			TweenCallback callback4 = delegate
			{
				mTexture_Fire.mainTexture = mTexture2d_White;
			};
			Tween t = DOVirtual.Float(0f, 1f, 0.15f, delegate(float alpha)
			{
				mTexture_Fire.alpha = alpha;
			});
			Tween t2 = DOVirtual.Float(1f, 0f, 0.8f, delegate(float alpha)
			{
				mTexture_Fire.alpha = alpha;
			});
			Tween t3 = DOVirtual.Float(1f, 0f, 1.6f, delegate(float alpha)
			{
				mTexture_Fire.alpha = alpha;
			});
			TweenCallback action = delegate
			{
				mTexture_Fire.alpha = 0f;
			};
			Tween t4 = DOVirtual.Float(0f, 1f, 1.5f, delegate(float alpha)
			{
				mTexture_Fire.alpha = alpha;
			});
			sequence.OnPlay(action);
			sequence.Append(t4);
			sequence.AppendCallback(callback);
			sequence.AppendInterval(0.15f);
			sequence.AppendCallback(callback2);
			sequence.AppendInterval(0.15f);
			sequence.AppendCallback(callback3);
			sequence.AppendInterval(0f);
			sequence.Append(t2);
			sequence.AppendCallback(callback4);
			sequence.Append(t);
			sequence.Append(t3);
			return sequence;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if ((Object)mAudioSource_Playing != null)
			{
				mAudioSource_Playing.Stop();
			}
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_Hanabi_0);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Fire);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Red);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Yellow);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Blue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_White);
			mAudioSource_Playing = null;
		}
	}
}
