using Common.Enum;
using DG.Tweening;
using KCV;
using KCV.Utils;
using local.models;
using local.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UIButtonManager))]
[RequireComponent(typeof(UIPanel))]
[SelectionBase]
public class UISlotItemAlbumDetail : MonoBehaviour
{
	[Serializable]
	private class Parameter
	{
		[SerializeField]
		private Transform mTransformParent;

		[SerializeField]
		private UILabel mLabel_Param;

		[SerializeField]
		private UITexture mTexture_Param;

		public Transform GetTransform()
		{
			return mTransformParent;
		}

		public UILabel GetLabel()
		{
			return mLabel_Param;
		}

		public UITexture GetTexture()
		{
			return mTexture_Param;
		}
	}

	private class SlotItemAlbumDetailTextureInfo
	{
		public enum GraphicType
		{
			First,
			Second,
			Third,
			Fourth
		}

		private GraphicType mGraphicType;

		private int mGraphicSlotItemMstId;

		private Texture mTexture2d_Cache;

		private int mWidth;

		private int mHeight;

		private Vector3 mVector3_Offset;

		private Vector3 mVector3_Scale;

		private bool mNeedPixelPerfect;

		public SlotItemAlbumDetailTextureInfo(GraphicType graphicType, int graphicSlotItemId, Vector3 scale, Vector3 offset, int width, int height)
		{
			mGraphicSlotItemMstId = graphicSlotItemId;
			mGraphicType = graphicType;
			mWidth = width;
			mHeight = height;
			mVector3_Offset = offset;
			mVector3_Scale = scale;
			mNeedPixelPerfect = false;
		}

		public SlotItemAlbumDetailTextureInfo(GraphicType graphicType, int graphicSlotItemId, Vector3 scale, Vector3 offset, bool needPixelPerfect)
		{
			mGraphicSlotItemMstId = graphicSlotItemId;
			mGraphicType = graphicType;
			mWidth = -1;
			mHeight = -1;
			mVector3_Offset = offset;
			mVector3_Scale = scale;
			mNeedPixelPerfect = needPixelPerfect;
		}

		public bool NeedPixelPerfect()
		{
			return mNeedPixelPerfect;
		}

		public Vector3 GetOffset()
		{
			return mVector3_Offset;
		}

		public int GetWidth()
		{
			return mWidth;
		}

		public int GetHeight()
		{
			return mHeight;
		}

		public Vector3 GetScale()
		{
			return mVector3_Scale;
		}

		public Texture RequestTexture()
		{
			if (mTexture2d_Cache == null)
			{
				mTexture2d_Cache = LoadTexture(mGraphicType, mGraphicSlotItemMstId);
			}
			return mTexture2d_Cache;
		}

		private Texture LoadTexture(GraphicType graphicType, int graphicShipId)
		{
			switch (graphicType)
			{
			case GraphicType.First:
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(mGraphicSlotItemMstId, 1);
			case GraphicType.Second:
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(mGraphicSlotItemMstId, 2);
			case GraphicType.Third:
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(mGraphicSlotItemMstId, 3);
			case GraphicType.Fourth:
				return SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(mGraphicSlotItemMstId, 4);
			default:
				return null;
			}
		}

		public void ReleaseTexture()
		{
			if (mTexture2d_Cache != null && mGraphicType != 0)
			{
				Resources.UnloadAsset(mTexture2d_Cache);
				mTexture2d_Cache = null;
			}
		}

		public static SlotItemAlbumDetailTextureInfo[] GenerateSlotItemGraphicsInfo(AlbumSlotModel albumSlotItemModel)
		{
			List<SlotItemAlbumDetailTextureInfo> list = new List<SlotItemAlbumDetailTextureInfo>();
			list.Add(new SlotItemAlbumDetailTextureInfo(GraphicType.Second, albumSlotItemModel.MstId, Vector3.one, Vector3.zero, 287, 430));
			list.Add(new SlotItemAlbumDetailTextureInfo(GraphicType.First, albumSlotItemModel.MstId, Vector3.one, Vector3.zero, 260, 260));
			list.Add(new SlotItemAlbumDetailTextureInfo(GraphicType.Third, albumSlotItemModel.MstId, Vector3.one, Vector3.zero, 287, 430));
			list.Add(new SlotItemAlbumDetailTextureInfo(GraphicType.Fourth, albumSlotItemModel.MstId, Vector3.one, Vector3.zero, 287, 430));
			return list.ToArray();
		}

		private static Vector3 GenerateDefaultScaleBySlotId(int ShipId)
		{
			switch (ShipId)
			{
			case 319:
				return Vector3.one * 0.5f;
			case 192:
				return Vector3.one * 0.36f;
			case 193:
				return Vector3.one * 0.4f;
			case 416:
				return Vector3.one * 0.5f;
			default:
				return new Vector3(0.5f, 0.5f);
			}
		}

		private static Vector3 GenerateOffsetByShipId(int ShipId)
		{
			switch (ShipId)
			{
			case 319:
				return Vector3.up * -10f;
			case 192:
				return Vector3.up * -10f;
			case 193:
				return Vector3.up * -20f;
			case 416:
				return Vector3.up * -20f;
			default:
				return Vector3.zero;
			}
		}
	}

	private UIPanel mPanelThis;

	private UIButtonManager mButtonManager;

	[SerializeField]
	private Transform mTransform_OffsetForTexture;

	[SerializeField]
	private UITexture mTexture_SlotItem;

	[SerializeField]
	private UITexture mTexture_TypeIcon;

	[SerializeField]
	private UITexture mTexture_SlotItemTypeBackground;

	[SerializeField]
	protected UIButton mButton_Next;

	[SerializeField]
	protected UIButton mButton_Prev;

	[SerializeField]
	private UILabel mLabel_No;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UILabel mLabel_Description;

	[SerializeField]
	private UILabel mLabel_TypeName;

	[SerializeField]
	private Parameter[] mParameters;

	[SerializeField]
	private UITexture[] mTextures_EquipmentShipIcon;

	private UIButton[] mButtons_Focasable;

	private SlotItemAlbumDetailTextureInfo[] mSlotItemAlbumDetailTextureInfos;

	private SlotItemAlbumDetailTextureInfo mCurrentShipAlbumDetailTextureInfo;

	protected UIButton mCurrentFocusButton;

	protected KeyControl mKeyController;

	private AlbumSlotModel mAlbumSlotModel;

	public bool _Stc_R;

	private Action<Tween> mOnBackListener;

	private void Awake()
	{
		mPanelThis = GetComponent<UIPanel>();
		mPanelThis.alpha = 0f;
		mButtonManager = GetComponent<UIButtonManager>();
		mButtonManager.IndexChangeAct = delegate
		{
			int num = Array.IndexOf(mButtons_Focasable, mButtonManager.nowForcusButton);
			if (-1 < num)
			{
				ChangeFocusButton(mButtonManager.nowForcusButton, needSe: false);
			}
		};
	}

	public void Initialize(AlbumSlotModel albumSlotModel)
	{
		_Stc_R = false;
		mAlbumSlotModel = albumSlotModel;
		int maxLineInFullWidthChar = 22;
		int fullWidthCharBuffer = 1;
		if (mSlotItemAlbumDetailTextureInfos != null)
		{
			SlotItemAlbumDetailTextureInfo[] array = mSlotItemAlbumDetailTextureInfos;
			foreach (SlotItemAlbumDetailTextureInfo slotItemAlbumDetailTextureInfo in array)
			{
				slotItemAlbumDetailTextureInfo.ReleaseTexture();
			}
		}
		mSlotItemAlbumDetailTextureInfos = SlotItemAlbumDetailTextureInfo.GenerateSlotItemGraphicsInfo(albumSlotModel);
		if (mTexture_SlotItemTypeBackground.mainTexture != null)
		{
			Resources.UnloadAsset(mTexture_SlotItemTypeBackground.mainTexture);
			mTexture_SlotItemTypeBackground.mainTexture = null;
		}
		mTexture_SlotItemTypeBackground.mainTexture = RequestTextureShipTypeBackGround(mAlbumSlotModel.Type2);
		mLabel_TypeName.text = Utils.GetSlotitemType3Name(mAlbumSlotModel.Type3);
		mLabel_No.text = $"{mAlbumSlotModel.Id:000}";
		mLabel_Name.text = mAlbumSlotModel.Name;
		InitializeEquipmentShipIcons(mAlbumSlotModel);
		InitializeParameters(mAlbumSlotModel);
		if (mTexture_TypeIcon.mainTexture != null)
		{
			Resources.UnloadAsset(mTexture_TypeIcon.mainTexture);
		}
		mTexture_TypeIcon.mainTexture = null;
		mTexture_TypeIcon.mainTexture = RequestTextureTypeIcon(mAlbumSlotModel.Type4);
		mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(maxLineInFullWidthChar, fullWidthCharBuffer, mAlbumSlotModel.Detail);
		mButtons_Focasable = GetFocasableButtons();
		ChangeFocusTexture(mSlotItemAlbumDetailTextureInfos[0]);
	}

	private void InitializeParameters(AlbumSlotModel albumSlotModel)
	{
		string[] array = new string[5]
		{
			"無",
			"短",
			"中",
			"長",
			"超長"
		};
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		if (0 < albumSlotModel.Soukou)
		{
			dictionary.Add(2, albumSlotModel.Soukou.ToString());
		}
		if (0 < albumSlotModel.Hougeki)
		{
			dictionary.Add(7, albumSlotModel.Hougeki.ToString());
		}
		if (0 < albumSlotModel.Raigeki)
		{
			dictionary.Add(8, albumSlotModel.Raigeki.ToString());
		}
		if (0 < albumSlotModel.Bakugeki)
		{
			dictionary.Add(14, albumSlotModel.Bakugeki.ToString());
		}
		if (0 < albumSlotModel.Taikuu)
		{
			dictionary.Add(9, albumSlotModel.Taikuu.ToString());
		}
		if (0 < albumSlotModel.Taisen)
		{
			dictionary.Add(10, albumSlotModel.Taisen.ToString());
		}
		if (0 < albumSlotModel.HouMeityu)
		{
			dictionary.Add(13, albumSlotModel.HouMeityu.ToString());
		}
		if (0 < albumSlotModel.Kaihi)
		{
			dictionary.Add(3, albumSlotModel.Kaihi.ToString());
		}
		if (0 < albumSlotModel.Sakuteki)
		{
			dictionary.Add(11, albumSlotModel.Sakuteki.ToString());
		}
		if (0 < albumSlotModel.Syatei)
		{
			dictionary.Add(6, array[albumSlotModel.Syatei]);
		}
		int[] array2 = dictionary.Keys.ToArray();
		for (int i = 0; i < mParameters.Length; i++)
		{
			Parameter parameter = mParameters[i];
			if (parameter.GetTexture().mainTexture != null)
			{
				Resources.UnloadAsset(parameter.GetTexture().mainTexture);
				parameter.GetTexture().mainTexture = null;
			}
		}
		for (int j = 0; j < mParameters.Length; j++)
		{
			Parameter parameter2 = mParameters[j];
			if (j < array2.Length)
			{
				int num = array2[j];
				parameter2.GetTransform().SetActive(isActive: true);
				parameter2.GetLabel().text = dictionary[num];
				parameter2.GetTexture().mainTexture = (Resources.Load($"Textures/Album/status_icon/status_{num}") as Texture);
			}
			else
			{
				parameter2.GetTransform().SetActive(isActive: false);
			}
		}
	}

	private void InitializeEquipmentShipIcons(AlbumSlotModel albumSlotModel)
	{
		List<string> list = new List<string>();
		if (albumSlotModel.CanEquip(SType.Destroyter))
		{
			list.Add("ship1");
		}
		if (albumSlotModel.CanEquip(SType.LightCruiser))
		{
			list.Add("ship2");
		}
		if (albumSlotModel.CanEquip(SType.HeavyCruiser))
		{
			list.Add("ship3");
		}
		if (albumSlotModel.CanEquip(SType.BattleShip))
		{
			list.Add("ship6");
		}
		if (albumSlotModel.CanEquip(SType.LightAircraftCarrier))
		{
			list.Add("ship8");
		}
		if (albumSlotModel.CanEquip(SType.AircraftCarrier))
		{
			list.Add("ship9");
		}
		if (albumSlotModel.CanEquip(SType.SeaplaneTender))
		{
			list.Add("ship12");
		}
		if (albumSlotModel.CanEquip(SType.AviationBattleShip))
		{
			list.Add("ship7");
		}
		for (int i = 0; i < mTextures_EquipmentShipIcon.Length; i++)
		{
			if (i < list.Count)
			{
				mTextures_EquipmentShipIcon[i].mainTexture = RequestTextureShipTypeIcon(list[i]);
			}
			else
			{
				mTextures_EquipmentShipIcon[i].mainTexture = null;
			}
		}
	}

	protected virtual UIButton[] GetFocasableButtons()
	{
		List<UIButton> list = new List<UIButton>();
		list.Add(mButton_Prev);
		list.Add(mButton_Next);
		return list.ToArray();
	}

	private Texture RequestTextureShipTypeIcon(string resourceName)
	{
		return Resources.Load<Texture2D>($"Textures/Album/ship_type_icon/{resourceName}");
	}

	private Texture RequestTextureShipTypeBackGround(int shipTypeId)
	{
		int num = -1;
		switch (shipTypeId)
		{
		case 1:
			num = 1;
			break;
		case 2:
			num = 2;
			break;
		case 3:
			num = 3;
			break;
		case 4:
			num = 31;
			break;
		case 5:
			num = 4;
			break;
		case 6:
			num = 8;
			break;
		case 7:
			num = 5;
			break;
		case 8:
			num = 6;
			break;
		case 9:
			num = 9;
			break;
		case 10:
			num = 28;
			break;
		case 14:
			num = 10;
			break;
		case 15:
			num = 11;
			break;
		case 16:
			num = 12;
			break;
		case 17:
			num = 13;
			break;
		case 18:
			num = 14;
			break;
		case 19:
			num = 15;
			break;
		case 20:
			num = 16;
			break;
		case 21:
			num = 17;
			break;
		case 22:
			num = 18;
			break;
		case 23:
			num = 19;
			break;
		case 24:
			num = 20;
			break;
		case 25:
			num = 21;
			break;
		case 26:
			num = 22;
			break;
		case 27:
			num = 23;
			break;
		case 28:
			num = 24;
			break;
		case 29:
			num = 25;
			break;
		case 30:
			num = 26;
			break;
		case 31:
			num = 9;
			break;
		case 32:
			num = 27;
			break;
		case 33:
			num = 29;
			break;
		case 34:
			num = 30;
			break;
		case 35:
			num = 15;
			break;
		}
		return Resources.Load<Texture2D>($"Textures/Album/weapon_bg/weapon_{num}");
	}

	private Texture RequestTextureTypeIcon(int typeIcon)
	{
		return Resources.Load<Texture2D>($"Textures/Album/Emblem/icon_1_weapon{typeIcon}");
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchNextTexture()
	{
		ChangeFocusButton(mButton_Next, needSe: false);
		NextImage();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchPrevTexture()
	{
		ChangeFocusButton(mButton_Prev, needSe: false);
		PrevImage();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnToucBack()
	{
		OnBack();
	}

	private void Update()
	{
		if (mKeyController == null)
		{
			return;
		}
		if (mKeyController.IsRDown())
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}
		else if (mKeyController.keyState[14].down)
		{
			int num = Array.IndexOf(mButtons_Focasable, mButtonManager.nowForcusButton);
			int num2 = num - 1;
			if (0 <= num2)
			{
				ChangeFocusButton(mButtons_Focasable[num2], needSe: true);
			}
		}
		else if (mKeyController.keyState[10].down)
		{
			int num3 = Array.IndexOf(mButtons_Focasable, mCurrentFocusButton);
			int num4 = num3 + 1;
			if (num4 < mButtons_Focasable.Length)
			{
				ChangeFocusButton(mButtons_Focasable[num4], needSe: true);
			}
		}
		else if (mKeyController.keyState[1].down)
		{
			OnSelectCircleButton();
		}
		else if (mKeyController.keyState[0].down)
		{
			OnBack();
		}
	}

	private void OnDestroy()
	{
		mPanelThis = null;
		mButtonManager = null;
		mTransform_OffsetForTexture = null;
		mTexture_SlotItem = null;
		mTexture_TypeIcon = null;
		mTexture_SlotItemTypeBackground = null;
		mButton_Next = null;
		mButton_Prev = null;
		mLabel_No = null;
		mLabel_Name = null;
		mLabel_Description = null;
		mLabel_TypeName = null;
		mParameters = null;
		mTextures_EquipmentShipIcon = null;
		mButtons_Focasable = null;
		mSlotItemAlbumDetailTextureInfos = null;
		mCurrentShipAlbumDetailTextureInfo = null;
		mCurrentFocusButton = null;
		mKeyController = null;
		mAlbumSlotModel = null;
	}

	protected virtual void OnSelectCircleButton()
	{
		if (mButton_Prev.Equals(mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			PrevImage();
		}
		else if (mButton_Next.Equals(mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			NextImage();
		}
	}

	public void SetOnBackListener(Action<Tween> onBackListener)
	{
		mOnBackListener = onBackListener;
	}

	private void OnBack()
	{
		_Stc_R = true;
		Tween obj = GenerateTweenClose();
		if (mOnBackListener != null)
		{
			mOnBackListener(obj);
		}
	}

	public void StartState()
	{
		ChangeFocusButton(mButtons_Focasable[0], needSe: false);
		ChangeFocusTexture(mSlotItemAlbumDetailTextureInfos[0]);
	}

	protected void NextImage()
	{
		int num = Array.IndexOf(mSlotItemAlbumDetailTextureInfos, mCurrentShipAlbumDetailTextureInfo);
		int num2 = num + 1;
		SlotItemAlbumDetailTextureInfo slotItemAlbumDetailTextureInfo = (num2 >= mSlotItemAlbumDetailTextureInfos.Length) ? mSlotItemAlbumDetailTextureInfos[0] : mSlotItemAlbumDetailTextureInfos[num2];
		ChangeFocusTexture(slotItemAlbumDetailTextureInfo);
	}

	protected void PrevImage()
	{
		int num = Array.IndexOf(mSlotItemAlbumDetailTextureInfos, mCurrentShipAlbumDetailTextureInfo);
		int num2 = num - 1;
		SlotItemAlbumDetailTextureInfo slotItemAlbumDetailTextureInfo = (0 > num2) ? mSlotItemAlbumDetailTextureInfos[mSlotItemAlbumDetailTextureInfos.Length - 1] : mSlotItemAlbumDetailTextureInfos[num2];
		ChangeFocusTexture(slotItemAlbumDetailTextureInfo);
	}

	private void ChangeFocusTexture(SlotItemAlbumDetailTextureInfo slotItemAlbumDetailTextureInfo)
	{
		mCurrentShipAlbumDetailTextureInfo = slotItemAlbumDetailTextureInfo;
		mTexture_SlotItem.mainTexture = mCurrentShipAlbumDetailTextureInfo.RequestTexture();
		if (mCurrentShipAlbumDetailTextureInfo.NeedPixelPerfect())
		{
			mTexture_SlotItem.MakePixelPerfect();
		}
		else
		{
			int width = mCurrentShipAlbumDetailTextureInfo.GetWidth();
			int height = mCurrentShipAlbumDetailTextureInfo.GetHeight();
			mTexture_SlotItem.SetDimensions(width, height);
		}
		mTransform_OffsetForTexture.transform.localScale = mCurrentShipAlbumDetailTextureInfo.GetScale();
		mTransform_OffsetForTexture.transform.localPosition = mCurrentShipAlbumDetailTextureInfo.GetOffset();
	}

	private void ChangeFocusButton(UIButton targetButton, bool needSe)
	{
		if (mCurrentFocusButton != null)
		{
			if (!mCurrentFocusButton.Equals(targetButton) && needSe)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			mCurrentFocusButton.SetState(UIButtonColor.State.Normal, immediate: true);
		}
		mCurrentFocusButton = targetButton;
		if (mCurrentFocusButton != null)
		{
			mCurrentFocusButton.SetState(UIButtonColor.State.Hover, immediate: true);
		}
	}

	public void Show()
	{
		_Stc_R = false;
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this, complete: true);
		}
		DOVirtual.Float(mPanelThis.alpha, 1f, 0.3f, delegate(float alpha)
		{
			mPanelThis.alpha = alpha;
		}).SetId(this);
	}

	private Tween GenerateTweenClose()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this, complete: true);
		}
		SingletonMonoBehaviour<SoundManager>.Instance.StopVoice();
		return DOVirtual.Float(mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
		{
			mPanelThis.alpha = alpha;
		}).SetId(this);
	}
}
