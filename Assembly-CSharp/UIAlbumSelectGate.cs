using DG.Tweening;
using KCV;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIButtonManager))]
public class UIAlbumSelectGate : MonoBehaviour
{
	private UIButtonManager mUIButtonManager;

	[SerializeField]
	private UITexture mTexture_Focus;

	[SerializeField]
	private UIButton mButton_ShipAlbum;

	[SerializeField]
	private UIButton mButton_SlotItemAlbum;

	[SerializeField]
	private UITexture mTexture_FlagShip;

	private UIButton mButton_CurrentFocus;

	private KeyControl mKeyController;

	private Action mOnSelectedBackListener;

	private Action mOnSelectedSlotItemAlbumListener;

	private Action mOnSelectedShipAlbumListener;

	private void Awake()
	{
		mUIButtonManager = GetComponent<UIButtonManager>();
		mUIButtonManager.IndexChangeAct = delegate
		{
			if (mKeyController != null)
			{
				ChangeFocusButton(mUIButtonManager.nowForcusButton);
			}
		};
		mButton_CurrentFocus = mButton_ShipAlbum;
		mTexture_Focus.transform.localPosition = new Vector3(-240f, 0f, 0f);
	}

	private void Update()
	{
		if (mKeyController != null)
		{
			if (mKeyController.keyState[14].down)
			{
				ChangeFocusButton(mButton_ShipAlbum);
			}
			else if (mKeyController.keyState[10].down)
			{
				ChangeFocusButton(mButton_SlotItemAlbum);
			}
			else if (mKeyController.keyState[1].down)
			{
				OnSelectCurrentFocus();
			}
			else if (mKeyController.IsRDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
			}
			else if (mKeyController.IsBatuDown())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			}
		}
	}

	private void ChangeFocusButton(UIButton button)
	{
		if (mButton_CurrentFocus != null && mButton_CurrentFocus.Equals(button))
		{
			return;
		}
		mButton_CurrentFocus = button;
		if (mButton_CurrentFocus != null)
		{
			if (mButton_ShipAlbum.Equals(mButton_CurrentFocus))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				mTexture_Focus.transform.DOLocalMoveX(-240f, 0.3f).SetId(mTexture_Focus);
			}
			else if (mButton_SlotItemAlbum.Equals(mButton_CurrentFocus))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				mTexture_Focus.transform.DOLocalMoveX(240f, 0.3f).SetId(mTexture_Focus);
			}
		}
	}

	private void OnSelectedShipAlbum()
	{
		if (mOnSelectedShipAlbumListener != null)
		{
			mOnSelectedShipAlbumListener();
		}
	}

	private void OnSelectedSlotItemAlbum()
	{
		if (mOnSelectedSlotItemAlbumListener != null)
		{
			mOnSelectedSlotItemAlbumListener();
		}
	}

	private void OnSelectedBack()
	{
		if (mOnSelectedBackListener != null)
		{
			mOnSelectedBackListener();
		}
	}

	private void OnSelectCurrentFocus()
	{
		if (mButton_CurrentFocus != null)
		{
			SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
			if (mButton_CurrentFocus.Equals(mButton_ShipAlbum))
			{
				OnSelectedShipAlbum();
			}
			else if (mButton_CurrentFocus.Equals(mButton_SlotItemAlbum))
			{
				OnSelectedSlotItemAlbum();
			}
		}
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchShipAlbum()
	{
		ChangeFocusButton(mButton_ShipAlbum);
		OnSelectCurrentFocus();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchSlotItemAlbum()
	{
		ChangeFocusButton(mButton_SlotItemAlbum);
		OnSelectCurrentFocus();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchBack()
	{
		OnSelectedBack();
	}

	public void SetOnSelectedShipAlbumListener(Action onSelectedShipAlbumListener)
	{
		mOnSelectedShipAlbumListener = onSelectedShipAlbumListener;
	}

	public void SetOnSelectedSlotItemAlbumListener(Action onSelectedSlotItemAlbumListener)
	{
		mOnSelectedSlotItemAlbumListener = onSelectedSlotItemAlbumListener;
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public void SetOnSelectedBackListener(Action onSelectedBackListener)
	{
		mOnSelectedBackListener = onSelectedBackListener;
	}

	public void Initialize(ShipModel shipModel)
	{
		mTexture_FlagShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.GetGraphicsMstId(), (!shipModel.IsDamaged()) ? 9 : 10);
		mTexture_FlagShip.transform.localPosition = Util.Poi2Vec(shipModel.Offsets.GetFace(shipModel.IsDamaged()));
		mTexture_FlagShip.MakePixelPerfect();
	}
}
