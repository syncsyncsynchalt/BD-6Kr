using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIDutyOpenDeckRewardGet : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Message;

		[SerializeField]
		private UITexture mTexture_Deck;

		[SerializeField]
		private UITexture mTexture_Yousei;

		[SerializeField]
		private Texture mTexture2d_Yousei_Off;

		[SerializeField]
		private Texture mTexture2d_Yousei_On;

		public void Initialize(Reward_Deck reward)
		{
			Initialize(reward.DeckId);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				Initialize(3);
				PlayAnimation();
			}
		}

		private void Start()
		{
			mTexture_Yousei.alpha = 1E-06f;
			mTexture_Yousei.transform.localPositionY(-90f);
			mTexture_Deck.alpha = 1E-06f;
			mTexture_Deck.transform.localPositionY(-10f);
		}

		public void PlayAnimation()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			Sequence sequence = DOTween.Sequence();
			sequence.SetId(this);
			mTexture_Yousei.alpha = 1E-06f;
			mTexture_Yousei.transform.localPositionY(-90f);
			mTexture_Yousei.mainTexture = mTexture2d_Yousei_Off;
			mTexture_Deck.alpha = 1E-06f;
			mTexture_Deck.transform.localPositionY(-10f);
			Sequence sequence2 = DOTween.Sequence();
			float duration = 1f;
			Tween t = mTexture_Yousei.transform.DOLocalMoveY(-50f, duration).SetEase(Ease.OutBack);
			Tween t2 = DOVirtual.Float(0f, 1f, duration, delegate(float alpha)
			{
				mTexture_Yousei.alpha = alpha;
			});
			sequence2.Append(t);
			sequence2.Join(t2);
			sequence.Append(sequence2);
			DOTween.Sequence();
			TweenCallback action = delegate
			{
				mTexture_Yousei.mainTexture = mTexture2d_Yousei_On;
			};
			Sequence sequence3 = DOTween.Sequence();
			Tween t3 = mTexture_Deck.transform.DOLocalMoveY(20f, duration).SetEase(Ease.OutBack);
			Tween t4 = DOVirtual.Float(0f, 1f, duration, delegate(float alpha)
			{
				mTexture_Deck.alpha = alpha;
			});
			sequence3.OnPlay(action);
			sequence3.Append(t3);
			sequence3.Join(t4);
			sequence.Append(sequence3);
			Sequence sequence4 = DOTween.Sequence();
			Sequence sequence5 = DOTween.Sequence();
			TweenCallback callback = delegate
			{
				mTexture_Yousei.mainTexture = mTexture2d_Yousei_On;
			};
			TweenCallback callback2 = delegate
			{
				mTexture_Yousei.mainTexture = mTexture2d_Yousei_Off;
			};
			sequence5.AppendInterval(0.3f);
			sequence5.AppendCallback(callback2);
			sequence5.AppendInterval(0.3f);
			sequence5.AppendCallback(callback);
			sequence5.SetLoops(int.MaxValue);
			sequence4.Append(sequence5);
			Sequence sequence6 = DOTween.Sequence();
			Tween t5 = mTexture_Deck.transform.DOLocalMoveY(24f, 1.5f).SetEase(Ease.OutQuad);
			Tween t6 = mTexture_Deck.transform.DOLocalMoveY(20f, 1.5f).SetEase(Ease.OutQuad);
			sequence6.Append(t5);
			sequence6.Append(t6);
			sequence6.SetLoops(int.MaxValue);
			sequence4.Join(sequence6);
			sequence.Append(sequence4);
		}

		private void Initialize(int deckId)
		{
			mLabel_Message.text = $"第{deckId}艦隊\nが開放されました！";
			mTexture_Deck.mainTexture = (Resources.Load("Textures/Common/DeckFlag/icon_deck" + deckId) as Texture2D);
			PlayAnimation();
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Message);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Deck);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Yousei);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Yousei_Off);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture2d_Yousei_On);
		}
	}
}
