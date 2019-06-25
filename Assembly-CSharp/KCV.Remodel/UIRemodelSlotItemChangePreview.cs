using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelSlotItemChangePreview : MonoBehaviour, UIRemodelView
	{
		private Vector3 showPos = new Vector3(6f, 277f);

		private Vector3 hidePos = new Vector3(550f, 277f);

		[SerializeField]
		private UIRemodelSlotItemChangePreviewChildPane srcItemPane;

		[SerializeField]
		private UIRemodelSlotItemChangePreviewChildPane dstItemPane;

		[SerializeField]
		private UIButton mButton_TouchBackArea;

		private UIButton mButtonFocus;

		private int[] voiceTypes = new int[3]
		{
			9,
			10,
			26
		};

		private int voiceTypeIdx;

		private KeyControl mKeyController;

		private bool isShown;

		public ShipModel mTargetShipModel
		{
			get;
			private set;
		}

		public SlotitemModel dstSlotItemModel
		{
			get;
			private set;
		}

		public int mSlotIndex
		{
			get;
			private set;
		}

		public void Initialize(KeyControl keyController, ShipModel targetShipModel, SlotitemModel srcSlotItemModel, SlotitemModel dstSlotItemModel, int slotIndex)
		{
			mKeyController = keyController;
			mTargetShipModel = targetShipModel;
			this.dstSlotItemModel = dstSlotItemModel;
			mSlotIndex = slotIndex;
			Texture slotItemTexture = srcItemPane.GetSlotItemTexture();
			Texture slotItemTexture2 = dstItemPane.GetSlotItemTexture();
			if (slotItemTexture != null && slotItemTexture2 != null && slotItemTexture.Equals(slotItemTexture2))
			{
				srcItemPane.UnloadSlotItemTexture(unloadTexture: true);
				dstItemPane.UnloadSlotItemTexture();
			}
			else
			{
				srcItemPane.UnloadSlotItemTexture(unloadTexture: true);
				dstItemPane.UnloadSlotItemTexture(unloadTexture: true);
			}
			srcItemPane.Init4Upper(srcSlotItemModel);
			dstItemPane.Init4Lower(dstSlotItemModel, srcSlotItemModel);
			if (srcSlotItemModel != null)
			{
				srcItemPane.BackGround.mainTexture = Resources.Load<Texture2D>("Textures/remodel/PlaneSkillTex/weapon_info_lv" + srcSlotItemModel.SkillLevel);
			}
			if (dstSlotItemModel != null)
			{
				dstItemPane.BackGround.mainTexture = Resources.Load<Texture2D>("Textures/remodel/PlaneSkillTex/weapon_info_lv" + dstSlotItemModel.SkillLevel);
			}
		}

		private void Awake()
		{
			base.transform.localPosition = hidePos;
		}

		public void Start()
		{
			base.transform.localPosition = hidePos;
			mButton_TouchBackArea.SetActive(isActive: false);
		}

		public void ReleaseSlotItemInfo()
		{
			mTargetShipModel = null;
			dstSlotItemModel = null;
		}

		private void ProcessChange()
		{
			if (isShown)
			{
				Hide();
				bool isExSlot = mTargetShipModel.SlotCount <= mSlotIndex && mTargetShipModel.HasExSlot();
				UserInterfaceRemodelManager.instance.ProcessChangeSlotItem(mSlotIndex, dstSlotItemModel, getNextVoiceType(), isExSlot);
			}
		}

		private void Back()
		{
			if (isShown)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				UserInterfaceRemodelManager.instance.Back2SoubiHenkouItemSelect();
				Hide();
			}
		}

		public void OnTouchForward()
		{
			if (base.enabled)
			{
				ProcessChange();
			}
		}

		public void OnTouchBack()
		{
			if (isShown)
			{
				Back();
			}
		}

		private void SetKeyController(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			mKeyController = keyController;
		}

		private void Update()
		{
			if (mKeyController != null && base.enabled && isShown)
			{
				if (mKeyController.IsMaruDown())
				{
					ProcessChange();
				}
				else if (mKeyController.IsBatuDown())
				{
					Back();
				}
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
			base.enabled = true;
			mButton_TouchBackArea.SetActive(isActive: true);
			RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.2f, delegate
			{
				isShown = true;
			});
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			base.enabled = false;
			isShown = false;
			mButton_TouchBackArea.SetActive(isActive: false);
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.2f, delegate
				{
					base.gameObject.SetActive(false);
				});
				return;
			}
			base.transform.localPosition = hidePos;
			base.gameObject.SetActive(false);
		}

		private int getNextVoiceType()
		{
			voiceTypeIdx++;
			if (voiceTypes.Length <= voiceTypeIdx)
			{
				voiceTypeIdx = 0;
			}
			return voiceTypes[voiceTypeIdx];
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_TouchBackArea);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButtonFocus);
			Texture slotItemTexture = srcItemPane.GetSlotItemTexture();
			Texture slotItemTexture2 = dstItemPane.GetSlotItemTexture();
			if (slotItemTexture != null && slotItemTexture2 != null && slotItemTexture.Equals(slotItemTexture2))
			{
				srcItemPane.UnloadSlotItemTexture(unloadTexture: true);
				dstItemPane.UnloadSlotItemTexture();
			}
			else
			{
				srcItemPane.UnloadSlotItemTexture(unloadTexture: true);
				dstItemPane.UnloadSlotItemTexture(unloadTexture: true);
			}
			srcItemPane = null;
			dstItemPane = null;
			mTargetShipModel = null;
			dstSlotItemModel = null;
		}
	}
}
