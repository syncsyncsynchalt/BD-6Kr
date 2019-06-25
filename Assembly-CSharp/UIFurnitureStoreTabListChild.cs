using KCV;
using KCV.View.ScrollView;
using local.models;
using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget))]
public class UIFurnitureStoreTabListChild : MonoBehaviour, UIScrollListItem<FurnitureModel, UIFurnitureStoreTabListChild>
{
	private UIWidget mWidgetThis;

	private int mRealIndex;

	private FurnitureModel mFurnitureModel;

	private Action<UIFurnitureStoreTabListChild> mOnTouchListener;

	private Transform mCachedTransform;

	[SerializeField]
	private GameObject mBackground;

	[SerializeField]
	private UILabel CoinValue;

	[SerializeField]
	private UILabel Name;

	[SerializeField]
	private UILabel Detail;

	[SerializeField]
	private UISprite[] Stars;

	[SerializeField]
	private UILabel SoldOut;

	[SerializeField]
	private UITexture texture;

	private Action<Texture> mReleaseRequestFurnitureTexture;

	public UITexture GetFurnitureTextureView()
	{
		return texture;
	}

	private void Awake()
	{
		mWidgetThis = GetComponent<UIWidget>();
		mWidgetThis.alpha = 1E-07f;
	}

	public void Initialize(int realIndex, FurnitureModel model)
	{
		mRealIndex = realIndex;
		mFurnitureModel = model;
		CoinValue.textInt = model.Price;
		Name.text = model.Name;
		Detail.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(36, 1, model.Description);
		SetStars(model.Rarity);
		SoldOut.enabled = model.IsPossession();
		if (texture.mainTexture != null)
		{
			ReleaseRequestFurnitureTexture(texture.mainTexture);
		}
		texture.mainTexture = null;
		texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.Furniture.LoadInteriorStoreFurniture(model.Type, model.MstId);
		mWidgetThis.alpha = 1f;
	}

	private void SetStars(int num)
	{
		for (int i = 0; i < Stars.Length; i++)
		{
			Stars[i].SetActive(num > i);
		}
	}

	public void InitializeDefault(int realIndex)
	{
		mRealIndex = realIndex;
		mFurnitureModel = null;
		if (texture.mainTexture != null)
		{
			ReleaseRequestFurnitureTexture(texture.mainTexture);
		}
		texture.mainTexture = null;
		mWidgetThis.alpha = 1E-07f;
	}

	public int GetRealIndex()
	{
		return mRealIndex;
	}

	public FurnitureModel GetModel()
	{
		return mFurnitureModel;
	}

	public int GetHeight()
	{
		return 154;
	}

	public void SetOnTouchListener(Action<UIFurnitureStoreTabListChild> onTouchListener)
	{
		mOnTouchListener = onTouchListener;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void Touch()
	{
		if (mOnTouchListener != null)
		{
			mOnTouchListener(this);
		}
	}

	public void Hover()
	{
		UISelectedObject.SelectedOneObjectBlink(mBackground, value: true);
	}

	public void RemoveHover()
	{
		UISelectedObject.SelectedOneObjectBlink(mBackground, value: false);
	}

	public Transform GetTransform()
	{
		if (mCachedTransform == null)
		{
			mCachedTransform = base.transform;
		}
		return mCachedTransform;
	}

	public void SetOnReleaseRequestFurnitureTextureListener(Action<Texture> releaseRequestFurnitureTexture)
	{
		mReleaseRequestFurnitureTexture = releaseRequestFurnitureTexture;
	}

	private void ReleaseRequestFurnitureTexture(Texture requestTexture)
	{
		if (mReleaseRequestFurnitureTexture != null)
		{
			mReleaseRequestFurnitureTexture(requestTexture);
		}
	}

	private void OnDestroy()
	{
		mReleaseRequestFurnitureTexture = null;
		mWidgetThis = null;
		mFurnitureModel = null;
		mOnTouchListener = null;
		mCachedTransform = null;
		mBackground = null;
		CoinValue = null;
		Name = null;
		Detail = null;
		Stars = null;
		SoldOut = null;
		texture = null;
	}
}
