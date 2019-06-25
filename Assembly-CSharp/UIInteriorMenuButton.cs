using Common.Enum;
using DG.Tweening;
using KCV.Scene.Port;
using System;
using UnityEngine;

public class UIInteriorMenuButton : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_Area;

	[SerializeField]
	private UISprite mSprite_Menu;

	[SerializeField]
	private UIButton mButton_Menu;

	[SerializeField]
	private UISprite mSprite_Yousei;

	private Action mOnClickListener;

	public FurnitureKinds mFurnitureKind
	{
		get;
		private set;
	}

	private void Awake()
	{
		mSprite_Yousei.spriteName = "mini_08_a_02";
		mSprite_Yousei.transform.localPositionY(-25f);
		mSprite_Yousei.fillAmount = 0.5f;
	}

	public void Initialize(FurnitureKinds furnitureKind)
	{
		mFurnitureKind = furnitureKind;
	}

	public void RemoveFocus()
	{
		DOTween.Kill(this);
		mSprite_Yousei.spriteName = "mini_08_a_02";
		mButton_Menu.SetState(UIButtonColor.State.Normal, immediate: true);
		Sequence sequence = DOTween.Sequence();
		Tween t = mSprite_Yousei.transform.DOLocalMoveY(-25f, 0.3f);
		Tween t2 = DOVirtual.Float(mSprite_Yousei.fillAmount, 0.5f, 0.3f, delegate(float percentage)
		{
			mSprite_Yousei.fillAmount = percentage;
		});
		Tween t3 = DOVirtual.Float(mTexture_Area.alpha, 0f, 0.5f, delegate(float percentage)
		{
			mTexture_Area.alpha = percentage;
		});
		sequence.Append(t);
		sequence.Join(t2);
		sequence.Join(t3);
		sequence.SetId(this);
	}

	public void SetEnableButton(bool enable)
	{
		mButton_Menu.isEnabled = enable;
	}

	public void Focus()
	{
		DOTween.Kill(this);
		mSprite_Yousei.spriteName = "mini_08_a_01";
		mButton_Menu.SetState(UIButtonColor.State.Hover, immediate: true);
		Sequence sequence = DOTween.Sequence();
		Tween t = mSprite_Yousei.transform.DOLocalMoveY(50f, 0.3f);
		Tween t2 = DOVirtual.Float(mSprite_Yousei.fillAmount, 1f, 0.3f, delegate(float percentage)
		{
			mSprite_Yousei.fillAmount = percentage;
		});
		Tween t3 = DOVirtual.Float(0.2f, 1f, 0.3f, delegate(float percentage)
		{
			mTexture_Area.alpha = percentage;
		});
		mTexture_Area.transform.localScale = new Vector3(0.1f, 0.1f);
		Tween t4 = mTexture_Area.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutExpo);
		sequence.Append(t);
		sequence.Join(t2);
		sequence.Join(t3);
		sequence.Join(t4);
		sequence.OnComplete(delegate
		{
			DOVirtual.Float(1f, 0.5f, 1.5f, delegate(float percentage)
			{
				mTexture_Area.alpha = percentage;
			}).SetId(this).SetLoops(int.MaxValue, LoopType.Yoyo)
				.SetEase(Ease.OutCirc);
		});
		sequence.SetId(this);
	}

	public void Click()
	{
		mSprite_Yousei.spriteName = "mini_08_a_03";
		DOVirtual.DelayedCall(0.1f, delegate
		{
			mSprite_Yousei.spriteName = "mini_08_a_04";
		}).SetId(this).OnComplete(delegate
		{
			if (mOnClickListener != null)
			{
				mOnClickListener();
			}
		});
	}

	public void SetOnClickListener(Action onClickListener)
	{
		mOnClickListener = onClickListener;
	}

	[Obsolete("Inspector上でイベントを設定する為に使用するのでスクリプトないでは使用しないでください")]
	public void OnTouchYousei()
	{
		Click();
	}

	private void OnDestroy()
	{
		DOTween.Kill(this);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Area);
		UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Menu);
		UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Yousei);
		UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Menu);
		mOnClickListener = null;
	}
}
