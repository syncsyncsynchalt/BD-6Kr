using DG.Tweening;
using local.models;
using System;
using System.Collections;
using UnityEngine;

public class UISlotItemAlbumListItem : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_SlotItemCard;

	[SerializeField]
	private UILabel mLabel_Number;

	private UIWidget mWidgetThis;

	private IAlbumModel mAlbumModel;

	private Action<UISlotItemAlbumListItem> mOnSelectedListener;

	private IEnumerator mInitializeCoroutine;

	public IAlbumModel GetAlbumModel()
	{
		return mAlbumModel;
	}

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
		mWidgetThis.alpha = 0f;
	}

	public void InitializeDefailt()
	{
		if (mInitializeCoroutine != null)
		{
			StopCoroutine(mInitializeCoroutine);
		}
		mAlbumModel = null;
		mLabel_Number.text = string.Empty;
		mTexture_SlotItemCard.mainTexture = null;
		mWidgetThis.alpha = 0f;
	}

	public void SetOnSelectedListener(Action<UISlotItemAlbumListItem> onSelectedListener)
	{
		mOnSelectedListener = onSelectedListener;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchListItem()
	{
		OnSelectedListItem();
	}

	public void SelectItem()
	{
		OnSelectedListItem();
	}

	private void OnSelectedListItem()
	{
		if (mOnSelectedListener != null)
		{
			mOnSelectedListener(this);
		}
	}

	public IEnumerator GenerateInitializeCoroutine(IAlbumModel albumModel)
	{
		mAlbumModel = albumModel;
		mLabel_Number.text = $"{albumModel.Id:000}";
		if (mTexture_SlotItemCard.mainTexture != null)
		{
			Resources.UnloadAsset(mTexture_SlotItemCard.mainTexture);
			mTexture_SlotItemCard.mainTexture = null;
		}
		if (albumModel is AlbumSlotModel)
		{
			AlbumSlotModel albumSlotModel = (AlbumSlotModel)albumModel;
			Texture slotItemCardTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(albumSlotModel.MstId, 1);
			mTexture_SlotItemCard.mainTexture = slotItemCardTexture;
			mTexture_SlotItemCard.alpha = 1f;
			yield return null;
		}
		else
		{
			mTexture_SlotItemCard.alpha = 0f;
		}
	}

	public void Hide()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		mWidgetThis.alpha = 0f;
	}

	public void Show()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		Sequence sequence = DOTween.Sequence().SetId(this);
		TweenCallback action = delegate
		{
			mWidgetThis.alpha = 0f;
		};
		Tween t = DOVirtual.Float(mWidgetThis.alpha, 1f, 0.15f, delegate(float alpha)
		{
			mWidgetThis.alpha = alpha;
		});
		sequence.OnPlay(action);
		sequence.Append(t);
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		mTexture_SlotItemCard = null;
		mLabel_Number = null;
		mWidgetThis = null;
		mAlbumModel = null;
		mOnSelectedListener = null;
	}
}
