using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelKaizou : MonoBehaviour, UIRemodelView
	{
		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UILabel mLabel_Ammo;

		[SerializeField]
		private UILabel mLabel_Steel;

		[SerializeField]
		private Transform mTransform_DevKit;

		[SerializeField]
		private Transform mTransform_BluePrint;

		[SerializeField]
		private UISprite mSprite_Background;

		[SerializeField]
		private UIButton mButton_GradeUp;

		[SerializeField]
		private UIButton mButton_TouchBackArea;

		private Vector3 showPos = new Vector3(240f, 0f);

		private Vector3 hidePos = new Vector3(1000f, 0f);

		private KeyControl mKeyController;

		private ShipModel mTargetShipModel;

		private int needBP;

		private bool isShown;

		public void Initialize(ShipModel shipModel, int needBluePrint)
		{
			mTargetShipModel = shipModel;
			mLabel_Level.text = mTargetShipModel.Level.ToString();
			mLabel_Ammo.text = mTargetShipModel.AfterAmmo.ToString();
			mLabel_Steel.text = mTargetShipModel.AfterSteel.ToString();
			mLabel_Name.text = mTargetShipModel.Name;
			needBP = needBluePrint;
			if (0 < mTargetShipModel.AfterDevkit)
			{
				mSprite_Background.height = 315;
				mTransform_DevKit.SetActive(isActive: true);
			}
			else
			{
				mSprite_Background.height = 245;
				mTransform_DevKit.SetActive(isActive: false);
			}
			if (needBluePrint != 0)
			{
				mTransform_BluePrint.SetActive(isActive: true);
			}
			else
			{
				mTransform_BluePrint.SetActive(isActive: false);
			}
		}

		private void Awake()
		{
			mButton_TouchBackArea.SetActive(isActive: false);
			base.transform.localPosition = hidePos;
		}

		private void Update()
		{
			if (mKeyController != null && base.enabled && isShown)
			{
				if (mKeyController.IsMaruDown())
				{
					Forward();
				}
				else if (mKeyController.IsBatuDown())
				{
					Back();
				}
			}
		}

		private void Forward()
		{
			if (!UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.GradeupBtnEnabled)
			{
				if (UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.DesignSpecificationsForGradeup > UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.DesignSpecifications)
				{
					CommonPopupDialog.Instance.StartPopup("この改装には「改装設計図」が" + UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.DesignSpecificationsForGradeup + "枚必要です");
				}
				else if (mTargetShipModel.AfterAmmo > UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.Material.Ammo || mTargetShipModel.AfterSteel > UserInterfaceRemodelManager.instance.mRemodelGradeUpManager.Material.Steel)
				{
					CommonPopupDialog.Instance.StartPopup("資材が不足しています");
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup("現在、改装が出来ません");
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			}
			else
			{
				Hide();
				UserInterfaceRemodelManager.instance.Forward2KaizoAnimation(mTargetShipModel);
			}
		}

		private void Back()
		{
			RemoveFocus();
			UserInterfaceRemodelManager.instance.Back2ModeSelect();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		public void Show()
		{
			Show(animation: true);
		}

		public void Show(bool animation)
		{
			base.gameObject.SetActive(true);
			base.enabled = true;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.3f, delegate
				{
					isShown = true;
				});
			}
			else
			{
				isShown = true;
				base.transform.localPosition = showPos;
			}
			mButton_TouchBackArea.SetActive(isActive: true);
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			base.enabled = true;
			isShown = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.3f, delegate
				{
					base.gameObject.SetActive(false);
				});
			}
			else
			{
				base.transform.localPosition = hidePos;
				base.gameObject.SetActive(false);
			}
			mButton_TouchBackArea.SetActive(isActive: false);
		}

		public void OnTouchStart()
		{
			Forward();
		}

		public void OnTouchBackArea()
		{
			Back();
		}

		public void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			mKeyController = keyController;
			if (mKeyController != null)
			{
				mButton_TouchBackArea.SetActive(isActive: true);
				mButton_GradeUp.SetState(UIButtonColor.State.Hover, immediate: true);
			}
			else
			{
				mButton_TouchBackArea.SetActive(isActive: false);
				mButton_GradeUp.SetState(UIButtonColor.State.Normal, immediate: true);
			}
		}

		public void RemoveFocus()
		{
			mButton_GradeUp.SetState(UIButtonColor.State.Normal, immediate: true);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Name);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Level);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Ammo);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Steel);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Background);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_GradeUp);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_TouchBackArea);
			mTransform_DevKit = null;
			mTransform_BluePrint = null;
			mKeyController = null;
			mTargetShipModel = null;
		}
	}
}
