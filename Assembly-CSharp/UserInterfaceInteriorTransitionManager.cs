using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UserInterfaceInteriorTransitionManager : MonoBehaviour
{
	[SerializeField]
	private UIFurnitureYousei mUIFurnitureYousei;

	[SerializeField]
	private UITexture mTexture_Background;

	private UIPanel mPanelThis;

	private void Awake()
	{
		mPanelThis = GetComponent<UIPanel>();
		mPanelThis.alpha = 1E-06f;
	}

	public void SwitchToStore(Action onFinishedAnimation)
	{
		DOTween.Kill(this);
		DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
		{
			mPanelThis.alpha = alpha;
		}).SetId(this);
		mUIFurnitureYousei.Initialize(UIFurnitureYousei.YouseiType.Store);
		mUIFurnitureYousei.StartWalk();
		mUIFurnitureYousei.transform.localPosition = Vector3.zero;
		mUIFurnitureYousei.transform.localScale = Vector3.zero;
		mUIFurnitureYousei.transform.DOLocalMoveY(12.5f, 0.2f).SetLoops(30, LoopType.Yoyo).SetId(this);
		mUIFurnitureYousei.transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutQuint).OnComplete(delegate
		{
			mUIFurnitureYousei.transform.DOScale(new Vector3(2f, 2f, 2f), 1f).SetDelay(0.5f).SetId(this);
			DOVirtual.Float(mUIFurnitureYousei.alpha, 0f, 0.6f, delegate(float alpha)
			{
				mUIFurnitureYousei.alpha = alpha;
			}).SetDelay(0.5f).SetEase(Ease.OutExpo)
				.OnComplete(delegate
				{
					DOVirtual.Float(1f, 0f, 0.3f, delegate(float alpha)
					{
						mPanelThis.alpha = alpha;
					}).SetId(this);
					if (onFinishedAnimation != null)
					{
						onFinishedAnimation();
					}
				})
				.SetId(this);
		})
			.SetId(this);
	}

	public void SwitchToHome(Action onFinishedAnimation)
	{
		DOTween.Kill(this);
		DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
		{
			mPanelThis.alpha = alpha;
		}).SetId(this);
		mUIFurnitureYousei.Initialize(UIFurnitureYousei.YouseiType.Room);
		mUIFurnitureYousei.StartWalk();
		mUIFurnitureYousei.transform.localPosition = Vector3.zero;
		mUIFurnitureYousei.transform.localScale = Vector3.zero;
		mUIFurnitureYousei.transform.DOLocalMoveY(12.5f, 0.2f).SetLoops(30, LoopType.Yoyo).SetId(this);
		mUIFurnitureYousei.transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutQuint).OnComplete(delegate
		{
			mUIFurnitureYousei.transform.DOScale(new Vector3(2f, 2f, 2f), 1f).SetDelay(0.5f).SetId(this);
			DOVirtual.Float(mUIFurnitureYousei.alpha, 0f, 0.6f, delegate(float alpha)
			{
				mUIFurnitureYousei.alpha = alpha;
			}).SetDelay(0.5f).SetEase(Ease.OutExpo)
				.OnComplete(delegate
				{
					DOVirtual.Float(1f, 0f, 0.3f, delegate(float alpha)
					{
						mPanelThis.alpha = alpha;
					}).SetId(this);
					if (onFinishedAnimation != null)
					{
						onFinishedAnimation();
					}
				})
				.SetId(this);
		})
			.SetId(this);
	}

	public void Release()
	{
		DOTween.Kill(this);
		mUIFurnitureYousei.Release();
		mTexture_Background.mainTexture = null;
		mTexture_Background = null;
		mUIFurnitureYousei = null;
		mPanelThis = null;
	}
}
