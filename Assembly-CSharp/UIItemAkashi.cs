using DG.Tweening;
using KCV;
using KCV.Scene.Port;
using System;
using UnityEngine;

public class UIItemAkashi : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_Akashi;

	[SerializeField]
	private Texture[] mTextureAkashi;

	[SerializeField]
	private UIButton mButton_Akashi;

	[SerializeField]
	private Vector3 mVector3_Akashi_ShowLocalPosition;

	[SerializeField]
	private Vector3 mVector3_Akashi_GoBackLocalPosition;

	[SerializeField]
	private Vector3 mVector3_Akashi_WaitingLocalPosition;

	private bool mShown;

	private Action mOnHidenCallBack;

	private KeyControl mKeyController;

	private void Start()
	{
		mTexture_Akashi.transform.localPosition = mVector3_Akashi_WaitingLocalPosition;
		mTexture_Akashi.mainTexture = mTextureAkashi[0];
		mShown = false;
	}

	private void Update()
	{
		if (mKeyController != null && mShown && mKeyController.IsAnyKey)
		{
			Hide(0.5f);
		}
	}

	public void Show()
	{
		mTexture_Akashi.transform.DOLocalMove(mVector3_Akashi_ShowLocalPosition, 0.4f).OnComplete(delegate
		{
			mShown = true;
		}).SetEase(Ease.OutSine);
	}

	public void SetClickable(bool clickable)
	{
		mButton_Akashi.isEnabled = clickable;
	}

	public void Hide()
	{
		Hide(0f);
	}

	private void Hide(float delay)
	{
		mTexture_Akashi.mainTexture = mTextureAkashi[1];
		mTexture_Akashi.transform.DOLocalMove(mVector3_Akashi_GoBackLocalPosition, 0.4f).SetDelay(delay).OnComplete(delegate
		{
			mShown = false;
			OnHidden();
		})
			.SetEase(Ease.OutSine);
	}

	public void SetOnHiddenCallBack(Action onHidenCallBack)
	{
		mOnHidenCallBack = onHidenCallBack;
	}

	private void OnHidden()
	{
		if (mOnHidenCallBack != null)
		{
			mOnHidenCallBack();
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public void OnTouchAkashi()
	{
		if (mShown)
		{
			Hide();
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Releases(ref mTextureAkashi);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Akashi);
		UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Akashi);
	}
}
