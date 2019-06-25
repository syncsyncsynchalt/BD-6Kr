using DG.Tweening;
using KCV.Scene.Port;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIDutyGetRewardOpenCreateLargeTanker : MonoBehaviour
	{
		[SerializeField]
		private UISprite mSprite_Yousei_Spanner;

		[SerializeField]
		private UISprite mSprite_Yousei_Osage;

		[SerializeField]
		private UISprite mSprite_Yousei_Kinpatsu;

		[SerializeField]
		private UISprite mSprite_Yousei_Hammer;

		[SerializeField]
		private Transform mTransform_TankerBuilders;

		private void Start()
		{
			IEnumerator routine = YouseiAnimation();
			StartCoroutine(routine);
		}

		private IEnumerator YouseiAnimation()
		{
			while (true)
			{
				mSprite_Yousei_Spanner.spriteName = "mini_02_c_01";
				mSprite_Yousei_Osage.spriteName = "mini_04_c_01";
				mSprite_Yousei_Kinpatsu.spriteName = "mini_01_c_01";
				mSprite_Yousei_Hammer.spriteName = "mini_03_c_01";
				bool jump = true;
				mTransform_TankerBuilders.DOLocalMoveY(-40f, 1f).OnComplete(delegate
				{
                    jump = false;
				});
				while (jump)
				{
					yield return null;
				}
				mSprite_Yousei_Spanner.spriteName = "mini_02_c_02";
				mSprite_Yousei_Osage.spriteName = "mini_04_c_02";
				mSprite_Yousei_Kinpatsu.spriteName = "mini_01_c_02";
				mSprite_Yousei_Hammer.spriteName = "mini_03_c_02";
				mTransform_TankerBuilders.DOLocalMoveY(-80f, 0.8f).OnComplete(delegate
				{
                    jump = true;
				}).SetEase(Ease.OutBounce);
				while (!jump)
				{
					yield return null;
				}
			}
		}

		private void OnDestroy()
		{
			StopCoroutine(YouseiAnimation());
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Yousei_Spanner);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Yousei_Osage);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Yousei_Kinpatsu);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Yousei_Hammer);
			mTransform_TankerBuilders = null;
		}
	}
}
