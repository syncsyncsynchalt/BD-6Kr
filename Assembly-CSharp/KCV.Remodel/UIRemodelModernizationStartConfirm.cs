using Common.Struct;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelModernizationStartConfirm : MonoBehaviour, UIRemodelView, IBannerResourceManage
	{
		private const string MAX_POWER_UP_BBCODE = "[e3904d]";

		private const string POWER_UP_BBCODE = "[00a4ff]";

		private const string DEFAULT_COLOR_BBCODE = "[202020]";

		private const string BBCODE_CLOSE = "[-]";

		[SerializeField]
		private UIRemodelModernizationStartConfirmSlot[] mUIRemodelModernizationStartConfirmSlots;

		[SerializeField]
		private UILabel[] mLabel_PrevParams;

		[SerializeField]
		private UILabel[] mLabel_NextParams;

		[SerializeField]
		private Transform[] mTransform_MaxTags;

		[SerializeField]
		private Transform[] mTransform_Arrows;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UITexture mTexture_Ship;

		[SerializeField]
		private UITexture mTexture_background;

		[SerializeField]
		public new Camera camera;

		private UIPanel popupString;

		[SerializeField]
		private UITexture BehindTexture_Ship;

		private UIWidget mUIWidgetThis;

		private Vector3 showScale = Vector3.one;

		private Vector3 hideScale = Vector3.one * 0.5f;

		private ShipModel mEatShipModel;

		private List<ShipModel> mBaitShipmodels;

		private UIButton mButtonCurrentFocus;

		private KeyControl mKeyController;

		private UIButton[] mButtonFocasable;

		private UIButton _BeforeFocus;

		private bool animating;

		private UIPanel PopupString
		{
			get
			{
				if (popupString == null)
				{
					popupString = GameObject.Find("PopupMessage").GetComponent<UIPanel>();
				}
				return popupString;
			}
		}

		private void Awake()
		{
			mUIWidgetThis = GetComponent<UIWidget>();
			mUIWidgetThis.alpha = 0.001f;
			if (BehindTexture_Ship == null)
			{
				BehindTexture_Ship = GameObject.Find("UIComponents/UIRemodelShipStatus/Texture_Ship").GetComponent<UITexture>();
			}
			Hide(animation: false);
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (mKeyController == null || !base.enabled || animating)
			{
				return;
			}
			if (mKeyController.IsLeftDown())
			{
				ChangeFocusButton(mButton_Negative);
			}
			else if (mKeyController.IsRightDown())
			{
				ChangeFocusButton(mButton_Positive);
			}
			else if (mKeyController.IsMaruDown())
			{
				if (mButtonCurrentFocus.Equals(mButton_Negative))
				{
					Back();
				}
				else if (mButtonCurrentFocus.Equals(mButton_Positive))
				{
					Forward();
				}
			}
			else if (mKeyController.IsBatuDown())
			{
				Back();
			}
		}

		public void DrawShip(ShipModel eatShipModel)
		{
			mEatShipModel = eatShipModel;
			Point face = mEatShipModel.Offsets.GetFace(eatShipModel.IsDamaged());
			mTexture_Ship.transform.localPosition = new Vector3(face.x, face.y);
			mTexture_Ship.mainTexture = ShipUtils.LoadTexture(mEatShipModel, (!mEatShipModel.IsDamaged()) ? 9 : 10);
			mTexture_Ship.MakePixelPerfect();
			mTexture_Ship.transform.localPosition = Util.Poi2Vec(mEatShipModel.Offsets.GetShipDisplayCenter(mEatShipModel.IsDamaged()));
		}

		public void Initialize(KeyControl keyController, ShipModel eatShipModel, List<ShipModel> baitShipModels, PowUpInfo powerUpInfo)
		{
			base.enabled = true;
			camera.gameObject.SetActive(true);
			mKeyController = keyController;
			mEatShipModel = eatShipModel;
			mBaitShipmodels = baitShipModels;
			_BeforeFocus = mButton_Positive;
			mButtonCurrentFocus = mButton_Negative;
			UIRemodelModernizationStartConfirmSlot[] array = mUIRemodelModernizationStartConfirmSlots;
			foreach (UIRemodelModernizationStartConfirmSlot uIRemodelModernizationStartConfirmSlot in array)
			{
				uIRemodelModernizationStartConfirmSlot.StopKira();
			}
			for (int j = 0; j < mUIRemodelModernizationStartConfirmSlots.Length; j++)
			{
				if (j < baitShipModels.Count)
				{
					mUIRemodelModernizationStartConfirmSlots[j].Initialize(baitShipModels[j]);
				}
				else
				{
					mUIRemodelModernizationStartConfirmSlots[j].Initialize(null);
				}
			}
			int num = eatShipModel.Karyoku + powerUpInfo.Karyoku;
			if (eatShipModel.KaryokuMax <= eatShipModel.Karyoku)
			{
				mTransform_MaxTags[0].localScale = Vector3.one;
				mTransform_Arrows[0].SetActive(isActive: false);
				mLabel_PrevParams[0].text = "[202020]" + eatShipModel.Karyoku.ToString() + "[-]";
				mLabel_NextParams[0].text = string.Empty;
			}
			else if (0 < powerUpInfo.Karyoku)
			{
				if (eatShipModel.KaryokuMax <= num)
				{
					mLabel_PrevParams[0].text = "[e3904d]" + eatShipModel.Karyoku.ToString() + "[-]";
					mLabel_NextParams[0].text = string.Empty;
					mTransform_Arrows[0].SetActive(isActive: false);
					mTransform_MaxTags[0].localScale = Vector3.one;
				}
				else
				{
					mLabel_PrevParams[0].text = "[202020]" + eatShipModel.Karyoku.ToString() + "[-]";
					mLabel_NextParams[0].text = "[00a4ff]" + num.ToString() + "[-]";
					mTransform_Arrows[0].SetActive(isActive: true);
					mTransform_MaxTags[0].localScale = Vector3.zero;
				}
			}
			else
			{
				mLabel_PrevParams[0].text = "[202020]" + eatShipModel.Karyoku.ToString() + "[-]";
				mLabel_NextParams[0].text = "[202020]" + eatShipModel.Karyoku.ToString() + "[-]";
				mTransform_MaxTags[0].localScale = Vector3.zero;
				mTransform_Arrows[0].SetActive(isActive: false);
			}
			int num2 = eatShipModel.Raisou + powerUpInfo.Raisou;
			if (eatShipModel.RaisouMax <= eatShipModel.Raisou)
			{
				mTransform_MaxTags[1].localScale = Vector3.one;
				mTransform_Arrows[1].SetActive(isActive: false);
				mLabel_PrevParams[1].text = "[202020]" + eatShipModel.Raisou.ToString() + "[-]";
				mLabel_NextParams[1].text = string.Empty;
			}
			else if (0 < powerUpInfo.Raisou)
			{
				if (eatShipModel.RaisouMax <= num2)
				{
					mLabel_PrevParams[1].text = "[e3904d]" + eatShipModel.Raisou.ToString() + "[-]";
					mLabel_NextParams[1].text = string.Empty;
					mTransform_Arrows[1].SetActive(isActive: false);
					mTransform_MaxTags[1].localScale = Vector3.one;
				}
				else
				{
					mLabel_PrevParams[1].text = "[202020]" + eatShipModel.Raisou.ToString() + "[-]";
					mLabel_NextParams[1].text = "[00a4ff]" + num2.ToString() + "[-]";
					mTransform_Arrows[1].SetActive(isActive: true);
					mTransform_MaxTags[1].localScale = Vector3.zero;
				}
			}
			else
			{
				mLabel_PrevParams[1].text = "[202020]" + eatShipModel.Raisou.ToString() + "[-]";
				mLabel_NextParams[1].text = "[202020]" + eatShipModel.Raisou.ToString() + "[-]";
				mTransform_MaxTags[1].localScale = Vector3.zero;
				mTransform_Arrows[1].SetActive(isActive: false);
			}
			int num3 = eatShipModel.Taiku + powerUpInfo.Taiku;
			if (eatShipModel.TaikuMax <= eatShipModel.Taiku)
			{
				mTransform_MaxTags[2].localScale = Vector3.one;
				mTransform_Arrows[2].SetActive(isActive: false);
				mLabel_PrevParams[2].text = "[202020]" + eatShipModel.Taiku.ToString() + "[-]";
				mLabel_NextParams[2].text = string.Empty;
			}
			else if (0 < powerUpInfo.Taiku)
			{
				if (eatShipModel.TaikuMax <= num3)
				{
					mLabel_PrevParams[2].text = "[e3904d]" + eatShipModel.Taiku.ToString() + "[-]";
					mLabel_NextParams[2].text = string.Empty;
					mTransform_Arrows[2].SetActive(isActive: false);
					mTransform_MaxTags[2].localScale = Vector3.one;
				}
				else
				{
					mLabel_PrevParams[2].text = "[202020]" + eatShipModel.Taiku.ToString() + "[-]";
					mLabel_NextParams[2].text = "[00a4ff]" + num3.ToString() + "[-]";
					mTransform_Arrows[2].SetActive(isActive: true);
					mTransform_MaxTags[2].localScale = Vector3.zero;
				}
			}
			else
			{
				mLabel_PrevParams[2].text = "[202020]" + eatShipModel.Taiku.ToString() + "[-]";
				mLabel_NextParams[2].text = "[202020]" + eatShipModel.Taiku.ToString() + "[-]";
				mTransform_MaxTags[2].localScale = Vector3.zero;
				mTransform_Arrows[2].SetActive(isActive: false);
			}
			int num4 = eatShipModel.Soukou + powerUpInfo.Soukou;
			if (eatShipModel.SoukouMax <= eatShipModel.Soukou)
			{
				mTransform_MaxTags[3].localScale = Vector3.one;
				mTransform_Arrows[3].SetActive(isActive: false);
				mLabel_PrevParams[3].text = "[202020]" + eatShipModel.Soukou.ToString() + "[-]";
				mLabel_NextParams[3].text = string.Empty;
			}
			else if (0 < powerUpInfo.Soukou)
			{
				if (eatShipModel.SoukouMax <= num4)
				{
					mLabel_PrevParams[3].text = "[e3904d]" + eatShipModel.Soukou.ToString() + "[-]";
					mLabel_NextParams[3].text = string.Empty;
					mTransform_Arrows[3].SetActive(isActive: false);
					mTransform_MaxTags[3].localScale = Vector3.one;
				}
				else
				{
					mLabel_PrevParams[3].text = "[202020]" + eatShipModel.Soukou.ToString() + "[-]";
					mLabel_NextParams[3].text = "[00a4ff]" + num4.ToString() + "[-]";
					mTransform_Arrows[3].SetActive(isActive: true);
					mTransform_MaxTags[3].localScale = Vector3.zero;
				}
			}
			else
			{
				mLabel_PrevParams[3].text = "[202020]" + eatShipModel.Soukou.ToString() + "[-]";
				mLabel_NextParams[3].text = "[202020]" + eatShipModel.Soukou.ToString() + "[-]";
				mTransform_MaxTags[3].localScale = Vector3.zero;
				mTransform_Arrows[3].SetActive(isActive: false);
			}
			int num5 = eatShipModel.Lucky + powerUpInfo.Lucky;
			if (eatShipModel.LuckyMax <= eatShipModel.Lucky)
			{
				mTransform_MaxTags[4].localScale = Vector3.one;
				mTransform_Arrows[4].SetActive(isActive: false);
				mLabel_PrevParams[4].text = "[202020]" + eatShipModel.Lucky.ToString() + "[-]";
				mLabel_NextParams[4].text = string.Empty;
			}
			else if (0 < powerUpInfo.Lucky)
			{
				if (eatShipModel.LuckyMax <= num5)
				{
					mLabel_PrevParams[4].text = "[e3904d]" + eatShipModel.Lucky.ToString() + "[-]";
					mLabel_NextParams[4].text = string.Empty;
					mTransform_Arrows[4].SetActive(isActive: false);
					mTransform_MaxTags[4].localScale = Vector3.one;
				}
				else
				{
					mLabel_PrevParams[4].text = "[202020]" + eatShipModel.Lucky.ToString() + "[-]";
					mLabel_NextParams[4].text = "[00a4ff]" + num5.ToString() + "[-]";
					mTransform_Arrows[4].SetActive(isActive: true);
					mTransform_MaxTags[4].localScale = Vector3.zero;
				}
			}
			else
			{
				mLabel_PrevParams[4].text = "[202020]" + eatShipModel.Lucky.ToString() + "[-]";
				mLabel_NextParams[4].text = "[202020]" + eatShipModel.Lucky.ToString() + "[-]";
				mTransform_MaxTags[4].localScale = Vector3.zero;
				mTransform_Arrows[4].SetActive(isActive: false);
			}
			List<UIButton> list = new List<UIButton>();
			list.Add(mButton_Negative);
			list.Add(mButton_Positive);
			mButtonFocasable = list.ToArray();
			mButtonFocasable[0].GetComponent<UISprite>().spriteName = "btn_cancel_on";
		}

		private void Forward()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			if (!animating)
			{
				StartCoroutine(HideAnimation());
			}
		}

		private void Back()
		{
			if (!animating)
			{
				Hide();
				UserInterfaceRemodelManager.instance.Back2KindaikaKaishu();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			mKeyController = keyController;
		}

		public void Show()
		{
			base.enabled = true;
			animating = true;
			PopupString.enabled = false;
			base.transform.localScale = hideScale;
			camera.SetActive(isActive: true);
			ChangeFocusButton(mButton_Positive, isSirent: true);
			ChangeFocusButton(mButton_Negative, isSirent: true);
			ChangeFocusButton(mButton_Positive, isSirent: true);
			ChangeFocusButton(mButton_Negative, isSirent: true);
			TweenScale tweenScale = TweenScale.Begin(base.gameObject, 0.5f, showScale);
			tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
			TweenAlpha.Begin(mTexture_background.gameObject, 0.1f, 0.2f);
			TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, 0.55f, 1f);
			EventDelegate.Set(tweenAlpha.onFinished, delegate
			{
				base.gameObject.transform.localScale = showScale;
				animating = false;
			});
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			base.enabled = false;
			mTransform_Arrows[0].SetActive(isActive: false);
			mTransform_Arrows[1].SetActive(isActive: false);
			mTransform_Arrows[2].SetActive(isActive: false);
			mTransform_Arrows[3].SetActive(isActive: false);
			mTransform_Arrows[4].SetActive(isActive: false);
			camera.SetActive(isActive: false);
			if (animation)
			{
				animating = true;
				TweenScale.Begin(base.gameObject, 0.3f, hideScale);
				TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, 0.3f, 0f);
				EventDelegate.Set(tweenAlpha.onFinished, delegate
				{
					animating = false;
				});
			}
			else
			{
				mUIWidgetThis.alpha = 0.001f;
			}
			PopupString.enabled = true;
		}

		public IEnumerator HideAnimation()
		{
			UserInterfaceRemodelManager.instance.Wait2AnimationFromKindaikaKakunin();
			base.enabled = false;
			BehindTexture_Ship.color = new Color(1f, 1f, 1f, 0.001f);
			TweenAlpha.Begin(base.gameObject, 0.5f, 0f);
			yield return new WaitForSeconds(0.5f);
			UserInterfaceRemodelManager.instance.Resume2WaitKindaikaKakunin();
			mTransform_Arrows[0].SetActive(isActive: false);
			mTransform_Arrows[1].SetActive(isActive: false);
			mTransform_Arrows[2].SetActive(isActive: false);
			mTransform_Arrows[3].SetActive(isActive: false);
			mTransform_Arrows[4].SetActive(isActive: false);
			camera.SetActive(isActive: false);
			UIRemodelModernizationStartConfirmSlot[] array = mUIRemodelModernizationStartConfirmSlots;
			foreach (UIRemodelModernizationStartConfirmSlot slot in array)
			{
				slot.StopKira();
			}
			BehindTexture_Ship.color = new Color(1f, 1f, 1f, 1f);
			UserInterfaceRemodelManager.instance.Forward2KindaikaKaishuAnimation(mBaitShipmodels, mEatShipModel);
			PopupString.enabled = true;
		}

		private void ChangeFocusButton(UIButton targetButton)
		{
			ChangeFocusButton(targetButton, isSirent: false);
		}

		private void ChangeFocusButton(UIButton targetButton, bool isSirent)
		{
			if (mButtonCurrentFocus != null)
			{
				mButtonCurrentFocus.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			mButtonCurrentFocus = targetButton;
			if (mButtonCurrentFocus != null)
			{
				mButtonCurrentFocus.SetState(UIButtonColor.State.Hover, immediate: true);
			}
			if (_BeforeFocus != targetButton)
			{
				if (!isSirent)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				_BeforeFocus = targetButton;
			}
		}

		public void OnTouchPositive()
		{
			ChangeFocusButton(mButton_Positive);
			Forward();
		}

		public void OnTouchNegative()
		{
			ChangeFocusButton(mButton_Negative);
			Back();
		}

		public void OnTouchBack()
		{
			if (UserInterfaceRemodelManager.instance.status == ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN)
			{
				Back();
			}
		}

		private void OnDestroy()
		{
			if (mUIRemodelModernizationStartConfirmSlots != null)
			{
				for (int i = 0; i < mUIRemodelModernizationStartConfirmSlots.Length; i++)
				{
					mUIRemodelModernizationStartConfirmSlots[i] = null;
				}
			}
			mUIRemodelModernizationStartConfirmSlots = null;
			if (mLabel_PrevParams != null)
			{
				for (int j = 0; j < mLabel_PrevParams.Length; j++)
				{
					if (mLabel_PrevParams[j] != null)
					{
						UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_PrevParams[j]);
					}
				}
			}
			mLabel_PrevParams = null;
			if (mLabel_NextParams != null)
			{
				for (int k = 0; k < mLabel_NextParams.Length; k++)
				{
					UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_NextParams[k]);
				}
			}
			mLabel_NextParams = null;
			if (mTransform_MaxTags != null)
			{
				for (int l = 0; l < mTransform_MaxTags.Length; l++)
				{
					mTransform_MaxTags[l] = null;
				}
			}
			mTransform_MaxTags = null;
			if (mTransform_Arrows != null)
			{
				for (int m = 0; m < mTransform_Arrows.Length; m++)
				{
					mTransform_Arrows[m] = null;
				}
				mTransform_Arrows = null;
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Positive);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Negative);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Ship);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_background);
			UserInterfacePortManager.ReleaseUtils.Release(ref popupString);
			UserInterfacePortManager.ReleaseUtils.Release(ref BehindTexture_Ship);
			UserInterfacePortManager.ReleaseUtils.Release(ref mUIWidgetThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButtonCurrentFocus);
			UserInterfacePortManager.ReleaseUtils.Releases(ref mButtonFocasable);
			UserInterfacePortManager.ReleaseUtils.Release(ref _BeforeFocus);
			camera = null;
			mEatShipModel = null;
			mBaitShipmodels.Clear();
			mBaitShipmodels = null;
			mKeyController = null;
		}

		public CommonShipBanner[] GetBanner()
		{
			List<CommonShipBanner> list = new List<CommonShipBanner>();
			UIRemodelModernizationStartConfirmSlot[] array = mUIRemodelModernizationStartConfirmSlots;
			foreach (UIRemodelModernizationStartConfirmSlot uIRemodelModernizationStartConfirmSlot in array)
			{
				CommonShipBanner shipBanner = uIRemodelModernizationStartConfirmSlot.GetShipBanner();
				list.Add(shipBanner);
			}
			return list.ToArray();
		}
	}
}
