using DG.Tweening;
using KCV;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(UIPanel))]
[RequireComponent(typeof(UIButtonManager))]
public class UIShipAlbumDetail : MonoBehaviour
{
	public class ShipAlbumDetailTextureInfo
	{
		public enum GraphicType
		{
			Card,
			ShipNormal,
			ShipLive2d,
			ShipTaiha
		}

		private GraphicType mGraphicType;

		private int mGraphicShipId;

		private Texture mTexture2d_Cache;

		private int mWidth;

		private int mHeight;

		private Vector3 mVector3_Offset;

		private Vector3 mVector3_Scale;

		private bool mNeedPixelPerfect;

		public ShipAlbumDetailTextureInfo(GraphicType graphicType, int graphicShipId, Vector3 scale, Vector3 offset, int width, int height)
		{
			mGraphicShipId = graphicShipId;
			mGraphicType = graphicType;
			mWidth = width;
			mHeight = height;
			mVector3_Offset = offset;
			mVector3_Scale = scale;
			mNeedPixelPerfect = false;
		}

		public ShipAlbumDetailTextureInfo(GraphicType graphicType, int graphicShipId, Vector3 scale, Vector3 offset, bool needPixelPerfect)
		{
			mGraphicShipId = graphicShipId;
			mGraphicType = graphicType;
			mWidth = -1;
			mHeight = -1;
			mVector3_Offset = offset;
			mVector3_Scale = scale;
			mNeedPixelPerfect = needPixelPerfect;
		}

		public int GetGraphicShipId()
		{
			return mGraphicShipId;
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
				mTexture2d_Cache = LoadTexture(mGraphicType, mGraphicShipId);
			}
			return mTexture2d_Cache;
		}

		private Texture LoadTexture(GraphicType graphicType, int graphicShipId)
		{
			switch (graphicType)
			{
			case GraphicType.Card:
				return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mGraphicShipId, 3);
			case GraphicType.ShipNormal:
				return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mGraphicShipId, 9);
			case GraphicType.ShipTaiha:
				return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(mGraphicShipId, 10);
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

		public static ShipAlbumDetailTextureInfo[] GenerateShipGraphicsInfo(AlbumShipModel albumShipModel)
		{
			List<ShipAlbumDetailTextureInfo> list = new List<ShipAlbumDetailTextureInfo>();
			int[] mstIDs = albumShipModel.MstIDs;
			foreach (int num in mstIDs)
			{
				list.Add(new ShipAlbumDetailTextureInfo(GraphicType.ShipNormal, num, GenerateDefaultScaleByShipId(num), GenerateOffsetByShipId(num), needPixelPerfect: true));
				list.Add(new ShipAlbumDetailTextureInfo(GraphicType.Card, num, Vector3.one, Vector3.zero, 218, 300));
				if (albumShipModel.IsDamaged(num))
				{
					list.Add(new ShipAlbumDetailTextureInfo(GraphicType.ShipTaiha, num, GenerateDefaultScaleByShipId(num), GenerateOffsetByShipId(num), needPixelPerfect: true));
				}
			}
			return list.ToArray();
		}

		private static Vector3 GenerateDefaultScaleByShipId(int ShipId)
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
	private UITexture mTexture_Ship;

	[SerializeField]
	private UITexture mTexture_ShipTypeBackground;

	[SerializeField]
	private UISprite mSprite_ShipTypeIcon;

	[SerializeField]
	protected UIButton mButton_Next;

	[SerializeField]
	protected UIButton mButton_Prev;

	[SerializeField]
	protected UIButton mButton_Voice;

	[SerializeField]
	private UILabel mLabel_No;

	[SerializeField]
	private UILabel mLabel_Name;

	[SerializeField]
	private UILabel mLabel_Description;

	[SerializeField]
	private UILabel mLabel_karyoku;

	[SerializeField]
	private UILabel mLabel_Taikyu;

	[SerializeField]
	private UILabel mLabel_Raisou;

	[SerializeField]
	private UILabel mLabel_Kaihi;

	[SerializeField]
	private UILabel mLabel_Taiku;

	[SerializeField]
	private UILabel mLabel_ShipTypeText;

	[SerializeField]
	private UIPentagonChart mPentagonChart;

	private UIButton[] mButtons_Focasable;

	private ShipAlbumDetailTextureInfo[] mShipAlbumDetailTextureInfos;

	private ShipAlbumDetailTextureInfo mCurrentShipAlbumDetailTextureInfo;

	protected UIButton mCurrentFocusButton;

	protected KeyControl mKeyController;

	private AlbumShipModel mAlbumShipModel;

	private AudioSource mVoiceAudioSource;

	public bool _Stc_R;

	private Action<Tween> mOnBackListener;

	private IEnumerator mChangeFocusTextureCoroutine;

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

	public void Initialize(AlbumShipModel albumShipModel)
	{
		_Stc_R = false;
		mAlbumShipModel = albumShipModel;
		int maxLineInFullWidthChar = 22;
		int fullWidthCharBuffer = 1;
		if (mShipAlbumDetailTextureInfos != null)
		{
			ShipAlbumDetailTextureInfo[] array = mShipAlbumDetailTextureInfos;
			foreach (ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo in array)
			{
				shipAlbumDetailTextureInfo.ReleaseTexture();
			}
		}
		if (mTexture_ShipTypeBackground.mainTexture != null)
		{
			Resources.UnloadAsset(mTexture_ShipTypeBackground.mainTexture);
			mTexture_ShipTypeBackground.mainTexture = null;
		}
		mLabel_No.text = $"{mAlbumShipModel.Id:000}";
		mLabel_Name.text = mAlbumShipModel.Name;
		mLabel_Taikyu.text = mAlbumShipModel.Taikyu.ToString();
		mLabel_Taiku.text = mAlbumShipModel.Taiku.ToString();
		mLabel_Raisou.text = mAlbumShipModel.Raisou.ToString();
		mLabel_karyoku.text = mAlbumShipModel.Karyoku.ToString();
		mLabel_Kaihi.text = mAlbumShipModel.Kaihi.ToString();
		mPentagonChart.Initialize(mAlbumShipModel.Karyoku, mAlbumShipModel.Raisou, mAlbumShipModel.Taiku, mAlbumShipModel.Kaihi, mAlbumShipModel.Taikyu);
		mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(maxLineInFullWidthChar, fullWidthCharBuffer, mAlbumShipModel.Detail);
		mShipAlbumDetailTextureInfos = ShipAlbumDetailTextureInfo.GenerateShipGraphicsInfo(mAlbumShipModel);
		mTexture_ShipTypeBackground.mainTexture = RequestShipTypeBackGround(mAlbumShipModel.ShipType);
		mSprite_ShipTypeIcon.spriteName = $"ship{albumShipModel.ShipType}";
		mLabel_ShipTypeText.text = $"{albumShipModel.ClassTypeName} {albumShipModel.ClassNum}番艦";
		mButtons_Focasable = GetFocasableButtons();
		ChangeFocusTexture(mShipAlbumDetailTextureInfos[0]);
	}

	public ShipAlbumDetailTextureInfo FocusTextureInfo()
	{
		return mCurrentShipAlbumDetailTextureInfo;
	}

	protected virtual UIButton[] GetFocasableButtons()
	{
		List<UIButton> list = new List<UIButton>();
		list.Add(mButton_Prev);
		list.Add(mButton_Voice);
		list.Add(mButton_Next);
		return list.ToArray();
	}

	private Texture RequestShipTypeBackGround(int shipTypeId)
	{
		int num = -1;
		switch (shipTypeId)
		{
		case 21:
			num = 19;
			break;
		case 20:
			num = 13;
			break;
		case 19:
			num = 15;
			break;
		case 11:
		case 18:
			num = 9;
			break;
		case 17:
			num = 16;
			break;
		case 16:
			num = 12;
			break;
		case 14:
			num = 11;
			break;
		case 13:
			num = 10;
			break;
		case 10:
			num = 7;
			break;
		case 8:
		case 9:
			num = 6;
			break;
		case 7:
			num = 8;
			break;
		case 6:
			num = 5;
			break;
		case 5:
			num = 4;
			break;
		case 4:
			num = 3;
			break;
		case 3:
			num = 2;
			break;
		case 2:
			num = 1;
			break;
		}
		return Resources.Load<Texture2D>($"Textures/Album/ship_bg/ship_{num}");
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
	public void OnTouchVoice()
	{
		ChangeFocusButton(mButton_Voice, needSe: false);
		PlayVoice();
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnToucBack()
	{
		OnBack();
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	private void Update()
	{
		if (mKeyController == null)
		{
			return;
		}
		if (mKeyController.keyState[14].down)
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
		else if (mKeyController.IsRDown())
		{
			if ((UnityEngine.Object)mVoiceAudioSource != null)
			{
				mVoiceAudioSource.Stop();
			}
			mVoiceAudioSource = null;
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}
	}

	protected virtual void OnSelectCircleButton()
	{
		if (mButton_Prev.Equals(mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			PrevImage();
		}
		else if (mButton_Voice.Equals(mCurrentFocusButton))
		{
			PlayVoice();
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
		mPentagonChart.PlayHide();
		if (mOnBackListener != null)
		{
			mOnBackListener(obj);
		}
	}

	public void StartState()
	{
		ChangeFocusButton(mButtons_Focasable[0], needSe: false);
		ChangeFocusTexture(mShipAlbumDetailTextureInfos[0]);
	}

	protected void PlayVoice()
	{
		if ((UnityEngine.Object)mVoiceAudioSource != null)
		{
			mVoiceAudioSource.Stop();
			AudioClip clip = mVoiceAudioSource.clip;
			Resources.UnloadAsset((UnityEngine.Object)clip);
		}
		int voiceMstId = mAlbumShipModel.GetVoiceMstId(25);
		AudioClip clip2 = SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(voiceMstId, 25);
		mVoiceAudioSource = SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(clip2);
	}

	protected void NextImage()
	{
		int num = Array.IndexOf(mShipAlbumDetailTextureInfos, mCurrentShipAlbumDetailTextureInfo);
		int num2 = num + 1;
		ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo = (num2 >= mShipAlbumDetailTextureInfos.Length) ? mShipAlbumDetailTextureInfos[0] : mShipAlbumDetailTextureInfos[num2];
		ChangeFocusTexture(shipAlbumDetailTextureInfo);
	}

	protected void PrevImage()
	{
		int num = Array.IndexOf(mShipAlbumDetailTextureInfos, mCurrentShipAlbumDetailTextureInfo);
		int num2 = num - 1;
		ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo = (0 > num2) ? mShipAlbumDetailTextureInfos[mShipAlbumDetailTextureInfos.Length - 1] : mShipAlbumDetailTextureInfos[num2];
		ChangeFocusTexture(shipAlbumDetailTextureInfo);
	}

	private void ChangeFocusTexture(ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo)
	{
		if (mChangeFocusTextureCoroutine != null)
		{
			StopCoroutine(mChangeFocusTextureCoroutine);
		}
		mChangeFocusTextureCoroutine = ChangeFocusTextureCoroutine(shipAlbumDetailTextureInfo);
		StartCoroutine(mChangeFocusTextureCoroutine);
	}

	private IEnumerator ChangeFocusTextureCoroutine(ShipAlbumDetailTextureInfo shipAlbumDetailTextureInfo)
	{
		mCurrentShipAlbumDetailTextureInfo = shipAlbumDetailTextureInfo;
		Texture shipDetailTexture = mCurrentShipAlbumDetailTextureInfo.RequestTexture();
		yield return null;
		mTexture_Ship.mainTexture = shipDetailTexture;
		if (mCurrentShipAlbumDetailTextureInfo.NeedPixelPerfect())
		{
			mTexture_Ship.MakePixelPerfect();
		}
		else
		{
			int width = mCurrentShipAlbumDetailTextureInfo.GetWidth();
			int height = mCurrentShipAlbumDetailTextureInfo.GetHeight();
			mTexture_Ship.SetDimensions(width, height);
		}
		mTransform_OffsetForTexture.transform.localScale = mCurrentShipAlbumDetailTextureInfo.GetScale();
		mTransform_OffsetForTexture.transform.localPosition = mCurrentShipAlbumDetailTextureInfo.GetOffset();
	}

	private void ChangeFocusButton(UIButton targetButton, bool needSe)
	{
		if (mCurrentFocusButton != null)
		{
			if (!mCurrentFocusButton.Equals(targetButton))
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
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this, complete: true);
		}
		mPentagonChart.Play();
		PlayVoice();
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
		if ((UnityEngine.Object)mVoiceAudioSource != null)
		{
			mVoiceAudioSource.Stop();
		}
		return DOVirtual.Float(mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
		{
			mPanelThis.alpha = alpha;
		}).SetId(this);
	}
}
