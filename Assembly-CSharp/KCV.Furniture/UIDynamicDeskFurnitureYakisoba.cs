using DG.Tweening;
using KCV.Scene.Port;
using UnityEngine;

namespace KCV.Furniture
{
	public class UIDynamicDeskFurnitureYakisoba : UIDynamicFurniture
	{
		[SerializeField]
		private UITexture mTexture_Smoke_0;

		[SerializeField]
		private UITexture mTexture_Smoke_1;

		[SerializeField]
		private UITexture mTexture_Smoke_2;

		protected override void OnAwake()
		{
			mTexture_Smoke_0.alpha = 0f;
			mTexture_Smoke_1.alpha = 0f;
			mTexture_Smoke_2.alpha = 0f;
		}

		protected override void OnInitialize(UIFurnitureModel uiFurnitureModel)
		{
			base.OnInitialize(uiFurnitureModel);
			Animation();
		}

		private void Animation()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			Sequence sequence = DOTween.Sequence();
			sequence.SetId(this);
			Tween t = GenerateTweenSmoke(mTexture_Smoke_0, 1f);
			Tween t2 = GenerateTweenSmoke(mTexture_Smoke_1, 1f);
			Tween t3 = GenerateTweenSmoke(mTexture_Smoke_2, 1f);
			sequence.Append(t3);
			sequence.Append(t);
			sequence.Append(t2);
			sequence.AppendInterval(1f);
			sequence.SetLoops(int.MaxValue, LoopType.Restart);
		}

		private Tween GenerateTweenSmoke(UITexture smokeTexture, float duration)
		{
			if (DOTween.IsTweening(smokeTexture))
			{
				DOTween.Kill(this, complete: true);
			}
			Sequence sequence = DOTween.Sequence().SetId(this);
			sequence.SetId(sequence);
			Sequence sequence2 = DOTween.Sequence().SetId(this);
			Tween t = DOVirtual.Float(0f, 1f, duration * 0.5f, delegate(float alpha)
			{
				smokeTexture.alpha = alpha;
			});
			Tween t2 = DOVirtual.Float(1f, 0f, duration * 0.5f, delegate(float alpha)
			{
				smokeTexture.alpha = alpha;
			});
			sequence2.Append(t);
			sequence2.Append(t2);
			Transform transform = smokeTexture.transform;
			Vector3 localPosition = smokeTexture.transform.localPosition;
			Tween t3 = transform.DOLocalMoveY(localPosition.y + 10f, duration);
			sequence.Append(sequence2);
			sequence.Join(t3);
			return sequence;
		}

		protected override void OnDestroyEvent()
		{
			base.OnDestroyEvent();
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Smoke_0);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Smoke_1);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Smoke_2);
		}
	}
}
