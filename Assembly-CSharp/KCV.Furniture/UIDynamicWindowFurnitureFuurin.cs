using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicWindowFurnitureFuurin : UIDynamicWindowFurniture
	{
		private const int MAX_FUURIN_RANDOM_CALL = 3;

		[SerializeField]
		private Transform mTransform_Fuurin;

		[SerializeField]
		private Transform mTransform_Wing;

		[SerializeField]
		private UITexture mTexture_Wing;

		[SerializeField]
		private Texture mTexture2d_Frame_0;

		[SerializeField]
		private Texture mTexture2d_Frame_1;

		[SerializeField]
		private Texture mTexture2d_Frame_2;

		[SerializeField]
		private AudioClip mAudioClip_Fuurin;

		private int mFuurinCounter;

		private List<int> TimeTable = new List<int>
		{
			5,
			10,
			20,
			25,
			35,
			40,
			50,
			55
		};

		private Stack<int> mCheckTable = new Stack<int>();

		protected override void OnAwake()
		{
			mCheckTable.Clear();
		}

		protected override void OnCalledActionEvent()
		{
			Animation();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			DoTimmingTriggerAnimation();
		}

		private bool DoTimmingTriggerAnimation()
		{
			int minute = mFurnitureModel.GetDateTime().Minute;
			if (0 == minute)
			{
				if (0 < mCheckTable.Count)
				{
					mFuurinCounter = 0;
					mCheckTable.Clear();
				}
				return false;
			}
			if (mCheckTable.Contains(minute))
			{
				return false;
			}
			if (DOTween.IsTweening(this))
			{
				return false;
			}
			if (3 <= mFuurinCounter)
			{
				return false;
			}
			if (!TimeTable.Contains(minute))
			{
				return false;
			}
			mCheckTable.Push(minute);
			if (Random.Range(0, 100) < 50)
			{
				Animation();
				mFuurinCounter++;
				return true;
			}
			return false;
		}

		private void Animation()
		{
			if (!DOTween.IsTweening(this))
			{
				Sequence sequence = DOTween.Sequence();
				sequence.SetId(this);
				Sequence sequence2 = DOTween.Sequence();
				sequence2.SetId(this);
				mTransform_Fuurin.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -10f));
				Tween t = mTransform_Fuurin.transform.DOLocalRotate(Vector3.zero, 0.6f);
				Tween t2 = mTransform_Fuurin.transform.DOLocalRotate(new Vector3(0f, 0f, -5f), 0.6f);
				Tween t3 = mTransform_Fuurin.transform.DOLocalRotate(Vector3.zero, 0.3f);
				sequence2.Append(t);
				sequence2.Append(t2);
				sequence2.Append(t3);
				Sequence sequence3 = DOTween.Sequence();
				sequence3.SetId(this);
				TweenCallback action = delegate
				{
					mTexture_Wing.mainTexture = mTexture2d_Frame_2;
				};
				sequence3.OnPlay(action);
				sequence3.AppendInterval(0.1f);
				TweenCallback tweenCallback = (TweenCallback)delegate
				{
					mTexture_Wing.mainTexture = mTexture2d_Frame_0;
				};
				Tween t4 = mTransform_Wing.transform.DOLocalRotate(new Vector3(0f, 0f, -4f), 0.2f);
				Tween t5 = mTransform_Wing.transform.DOLocalRotate(new Vector3(0f, 0f, 25f), 0.8f);
				Tween t6 = mTransform_Wing.transform.DOLocalRotate(new Vector3(0f, 0f, -12.5f), 0.8f);
				Tween t7 = mTransform_Wing.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.8f);
				sequence3.Append(t4);
				sequence3.Append(t5);
				sequence3.Append(t6);
				sequence3.Append(t7);
				sequence3.AppendCallback(delegate
				{
					mTexture_Wing.mainTexture = mTexture2d_Frame_1;
				});
				sequence3.AppendInterval(0.1f);
				sequence3.AppendCallback(delegate
				{
					mTexture_Wing.mainTexture = mTexture2d_Frame_0;
				});
				sequence.Append(sequence2);
				sequence.Join(sequence3);
				SoundUtils.PlaySE(mAudioClip_Fuurin);
			}
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Wing);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Frame_0);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Frame_1);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Frame_2);
			mTransform_Fuurin = null;
			mTransform_Wing = null;
		}
	}
}
