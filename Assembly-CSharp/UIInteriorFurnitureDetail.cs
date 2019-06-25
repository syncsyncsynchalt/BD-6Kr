using DG.Tweening;
using KCV;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIButtonManager))]
public class UIInteriorFurnitureDetail : MonoBehaviour
{
	private enum TweenAnimationType
	{
		ShowHide,
		Background
	}

	[SerializeField]
	private UITexture mTexture_Thumbnail;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UILabel mLabel_Description;

	[SerializeField]
	private UIButton mButton_Change;

	[SerializeField]
	private UIButton mButton_Preview;

	[SerializeField]
	private UITexture mTexture_TouchBackArea;

	[SerializeField]
	private OnClickEventSender mOnClickEventSender_TouchBackArea;

	[SerializeField]
	private Transform mEquipMark;

	private UIButton[] mFocasableButtons;

	private UIButton mFocusButton;

	private UIButtonManager mButtonManager;

	private int mDeckId;

	private FurnitureModel mFurnitureModel;

	private KeyControl mKeyController;

	private Action mOnSelectBackListener;

	private Action mOnSelectChangeListener;

	private Action mOnSelectPreviewListener;

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public void Initialize(int deckId, FurnitureModel furnitureModel)
	{
		mDeckId = deckId;
		mFurnitureModel = furnitureModel;
		mTexture_Thumbnail.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(furnitureModel.Type, furnitureModel.MstId);
		mLabel_Name.text = furnitureModel.Name;
		mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(14, 1, furnitureModel.Description);
		if (mFurnitureModel.GetSettingFlg(mDeckId))
		{
			mEquipMark.SetActive(isActive: true);
		}
		else
		{
			mEquipMark.SetActive(isActive: false);
		}
		mOnClickEventSender_TouchBackArea.SetClickable(clickable: false);
	}

	public void SetOnSelectBackListener(Action onSelectBackListener)
	{
		mOnSelectBackListener = onSelectBackListener;
	}

	public void SetOnSelectChangeListener(Action onSelectChangeListener)
	{
		mOnSelectChangeListener = onSelectChangeListener;
	}

	public void SetOnSelectPreviewListener(Action onSelectPreviewListener)
	{
		mOnSelectPreviewListener = onSelectPreviewListener;
	}

	private void Start()
	{
		List<UIButton> list = new List<UIButton>();
		list.Add(mButton_Change);
		list.Add(mButton_Preview);
		mFocasableButtons = list.ToArray();
	}

	private void Awake()
	{
		mButtonManager = GetComponent<UIButtonManager>();
		mButtonManager.IndexChangeAct = delegate
		{
			ChangeFocus(mButtonManager.nowForcusButton);
		};
	}

	public void StartState()
	{
		if (DOTween.IsTweening(TweenAnimationType.Background))
		{
			DOTween.Kill(TweenAnimationType.Background);
		}
		DOVirtual.Float(mTexture_TouchBackArea.alpha, 0.5f, 0.3f, delegate(float alpha)
		{
			mTexture_TouchBackArea.alpha = alpha;
		}).SetId(TweenAnimationType.Background);
		UIButton[] array = mFocasableButtons;
		foreach (UIButton uIButton in array)
		{
			uIButton.enabled = true;
		}
		ChangeFocus(mFocasableButtons[0]);
		mOnClickEventSender_TouchBackArea.SetClickable(clickable: true);
	}

	public void Show()
	{
		if (DOTween.IsTweening(TweenAnimationType.ShowHide))
		{
			DOTween.Kill(TweenAnimationType.ShowHide);
		}
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.gameObject, 0.3f);
		tweenPosition.from = base.transform.localPosition;
		TweenPosition tweenPosition2 = tweenPosition;
		Vector3 localPosition = base.transform.localPosition;
		float y = localPosition.y;
		Vector3 localPosition2 = base.transform.localPosition;
		tweenPosition2.to = new Vector3(0f, y, localPosition2.z);
		tweenPosition.ignoreTimeScale = true;
	}

	public void Hide()
	{
		if (DOTween.IsTweening(TweenAnimationType.ShowHide))
		{
			DOTween.Kill(TweenAnimationType.ShowHide);
		}
		base.transform.DOLocalMoveX(960f, 0.3f).SetId(TweenAnimationType.ShowHide);
	}

	public void QuitState()
	{
		mKeyController = null;
		ChangeFocus(mFocasableButtons[0]);
		UIButton[] array = mFocasableButtons;
		foreach (UIButton uIButton in array)
		{
			uIButton.enabled = false;
		}
		if (DOTween.IsTweening(TweenAnimationType.Background))
		{
			DOTween.Kill(TweenAnimationType.Background);
		}
		DOVirtual.Float(mTexture_TouchBackArea.alpha, 0.0001f, 0.15f, delegate(float alpha)
		{
			mTexture_TouchBackArea.alpha = alpha;
		}).SetId(TweenAnimationType.Background);
		mOnClickEventSender_TouchBackArea.SetClickable(clickable: false);
	}

	private void Update()
	{
		if (mKeyController == null || !(mFocusButton != null))
		{
			return;
		}
		if (mKeyController.keyState[14].down)
		{
			int num = Array.IndexOf(mFocasableButtons, mFocusButton);
			int num2 = num - 1;
			if (0 <= num2)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				ChangeFocus(mFocasableButtons[num2]);
			}
		}
		else if (mKeyController.keyState[10].down)
		{
			int num3 = Array.IndexOf(mFocasableButtons, mFocusButton);
			int num4 = num3 + 1;
			if (num4 < mFocasableButtons.Length)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				ChangeFocus(mFocasableButtons[num4]);
			}
		}
		else if (mKeyController.keyState[1].down)
		{
			if (mFocusButton != null)
			{
				if (mFocusButton.Equals(mButton_Change))
				{
					OnSelectChange();
				}
				else if (mFocusButton.Equals(mButton_Preview))
				{
					OnSelectPreview();
				}
			}
		}
		else if (mKeyController.keyState[0].down)
		{
			OnSelectBack();
		}
	}

	[Obsolete("Inspector上で使用するメソッドです")]
	public void OnTouchSelectChange()
	{
		if (mKeyController != null)
		{
			OnSelectChange();
		}
	}

	[Obsolete("Inspector上で使用するメソッドです")]
	public void OnTouchSelectPreview()
	{
		if (mKeyController != null)
		{
			OnSelectPreview();
		}
	}

	[Obsolete("Inspector上で使用するメソッドです")]
	public void OnTouchBack()
	{
		OnSelectBack();
	}

	private void OnSelectBack()
	{
		if (mOnSelectBackListener != null)
		{
			mOnSelectBackListener();
		}
	}

	private void OnSelectChange()
	{
		if (mOnSelectChangeListener != null)
		{
			mOnSelectChangeListener();
		}
	}

	private void OnSelectPreview()
	{
		if (mOnSelectPreviewListener != null)
		{
			mOnSelectPreviewListener();
		}
	}

	private void ChangeFocus(UIButton targetButton)
	{
		if (mFocusButton != null)
		{
			mFocusButton.SetState(UIButtonColor.State.Normal, immediate: true);
		}
		mFocusButton = targetButton;
		if (mFocusButton != null)
		{
			mFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
		}
	}

	public void ResumeState()
	{
		UIButton[] array = mFocasableButtons;
		foreach (UIButton uIButton in array)
		{
			uIButton.isEnabled = true;
		}
		ChangeFocus(mFocusButton);
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Releases(ref mFocasableButtons);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Thumbnail);
		UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Name);
		UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Description);
		UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Change);
		UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Preview);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_TouchBackArea);
		UserInterfacePortManager.ReleaseUtils.Release(ref mFocusButton);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Thumbnail);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Thumbnail);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Thumbnail);
		mOnClickEventSender_TouchBackArea = null;
		mEquipMark = null;
		mFurnitureModel = null;
		mKeyController = null;
		mOnSelectBackListener = null;
		mOnSelectChangeListener = null;
		mOnSelectPreviewListener = null;
	}
}
