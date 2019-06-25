using DG.Tweening;
using local.models;
using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(UIWidget))]
public class UIShipAlbumListItem : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_ShipCard;

	[SerializeField]
	private UILabel mLabel_Number;

	[SerializeField]
	private Animation mAnimation_MarriagedRing;

	private UIWidget mWidgetThis;

	private IAlbumModel mAlbumModel;

	private Action<UIShipAlbumListItem> mOnSelectedListener;

	public IAlbumModel GetAlbumModel()
	{
		return mAlbumModel;
	}

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
	}

	public IEnumerator GenerateInitializeCoroutine(IAlbumModel albumModel)
	{
		mAlbumModel = albumModel;
		mLabel_Number.text = $"{albumModel.Id:000}";
		if (mTexture_ShipCard.mainTexture != null)
		{
			Resources.UnloadAsset(mTexture_ShipCard.mainTexture);
			mTexture_ShipCard.mainTexture = null;
		}
		if (albumModel is AlbumShipModel)
		{
			AlbumShipModel albumShipModel = (AlbumShipModel)albumModel;
			Texture shipCardTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(albumShipModel.MstId, 3);
			if (UserInterfaceAlbumManager.CheckMarriaged(albumShipModel))
			{
				((Component)mAnimation_MarriagedRing).gameObject.SetActive(true);
				mAnimation_MarriagedRing.Play();
			}
			else
			{
				((Component)mAnimation_MarriagedRing).gameObject.SetActive(false);
			}
			yield return null;
			mTexture_ShipCard.mainTexture = shipCardTexture;
			mTexture_ShipCard.alpha = 1f;
		}
		else
		{
			mTexture_ShipCard.alpha = 0f;
			((Component)mAnimation_MarriagedRing).gameObject.SetActive(false);
		}
	}

	public void SetOnSelectedListener(Action<UIShipAlbumListItem> onSelectedListener)
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
		mTexture_ShipCard = null;
		mLabel_Number = null;
		mWidgetThis = null;
		mAlbumModel = null;
		mOnSelectedListener = null;
		mAnimation_MarriagedRing = null;
	}
}
