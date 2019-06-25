using Common.Enum;
using KCV.Scene.Port;
using KCV.Utils;
using KCV.View;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyDetail : BoardDialog
	{
		public enum SelectType
		{
			Positive,
			Negative
		}

		public delegate void UIDutyDetailAction(SelectType type);

		[SerializeField]
		private UILabel mLabelTitle;

		[SerializeField]
		private UILabel mLabelDescription;

		[SerializeField]
		private UILabel mLabelFuelValue;

		[SerializeField]
		private UILabel mLabelSteelValue;

		[SerializeField]
		private UILabel mLabelAmmoValue;

		[SerializeField]
		private UILabel mLabelBauxiteValue;

		[SerializeField]
		private UIDutyStartButton mDutyStartButton;

		[SerializeField]
		private UISprite[] mSprites_RewardMaterials;

		private UIDutyDetailAction mDutyDetailActionCallBack;

		private DutyModel mDutyModel;

		private KeyControl mKeyController;

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[14].down)
				{
					mDutyStartButton.FocusNegative(seFlag: true);
				}
				else if (mKeyController.keyState[10].down)
				{
					mDutyStartButton.FocusPositive();
				}
				else if (mKeyController.keyState[0].down)
				{
					mKeyController = null;
					mDutyDetailActionCallBack(SelectType.Negative);
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				}
				else if (mKeyController.keyState[1].down)
				{
					mKeyController = null;
					mDutyStartButton.ClickFocusButton();
				}
			}
		}

		public new KeyControl Show()
		{
			base.Show();
			mKeyController = new KeyControl();
			if (mDutyModel.Category == 2)
			{
				ShipUtils.PlayPortVoice(2);
			}
			else if (mDutyModel.Category == 5)
			{
				ShipUtils.PlayPortVoice(3);
			}
			return mKeyController;
		}

		public void SetDutyDetailCallBack(UIDutyDetailAction action)
		{
			mDutyDetailActionCallBack = action;
		}

		public void Initialize(DutyModel dutyModel)
		{
			mDutyModel = dutyModel;
			StartCoroutine(InitializeCoroutine(mDutyModel));
		}

		private IEnumerator InitializeCoroutine(DutyModel dutyModel)
		{
			mLabelTitle.text = dutyModel.Title;
			mLabelDescription.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(24, 1, dutyModel.Description);
			mLabelFuelValue.text = dutyModel.Fuel.ToString();
			mLabelSteelValue.text = dutyModel.Steel.ToString();
			mLabelAmmoValue.text = dutyModel.Ammo.ToString();
			mLabelBauxiteValue.text = dutyModel.Baux.ToString();
			mDutyStartButton.SetOnPositiveSelectedCallBack(delegate
			{
				if (this.mDutyDetailActionCallBack != null)
				{
					this.mDutyDetailActionCallBack(SelectType.Positive);
				}
			});
			mDutyStartButton.SetOnNegativeSelectedCallBack(delegate
			{
				if (this.mDutyDetailActionCallBack != null)
				{
					this.mDutyDetailActionCallBack(SelectType.Negative);
				}
			});
			mDutyStartButton.SetOnSelectedCallBack(delegate
			{
				this.mKeyController = null;
			});
			int materialIconIndex = 0;
			enumMaterialCategory[] showRewards = new enumMaterialCategory[4]
			{
				enumMaterialCategory.Build_Kit,
				enumMaterialCategory.Dev_Kit,
				enumMaterialCategory.Repair_Kit,
				enumMaterialCategory.Revamp_Kit
			};
			foreach (KeyValuePair<enumMaterialCategory, int> material in mDutyModel.RewardMaterials)
			{
				if (showRewards.Contains(material.Key) && 0 < material.Value && materialIconIndex < mSprites_RewardMaterials.Length)
				{
					mSprites_RewardMaterials[materialIconIndex].spriteName = $"item_{MaterialEnumToMasterId(material.Key)}";
					materialIconIndex++;
				}
			}
			for (int index = materialIconIndex; index < mSprites_RewardMaterials.Length; index++)
			{
				mSprites_RewardMaterials[index].spriteName = "none";
			}
			yield return null;
		}

		public new void Hide(Action action)
		{
			base.Hide(action);
		}

		private int MaterialEnumToMasterId(enumMaterialCategory category)
		{
			int result = 0;
			switch (category)
			{
			case enumMaterialCategory.Bauxite:
				result = 34;
				break;
			case enumMaterialCategory.Build_Kit:
				result = 2;
				break;
			case enumMaterialCategory.Bull:
				result = 32;
				break;
			case enumMaterialCategory.Dev_Kit:
				result = 3;
				break;
			case enumMaterialCategory.Fuel:
				result = 31;
				break;
			case enumMaterialCategory.Repair_Kit:
				result = 1;
				break;
			case enumMaterialCategory.Revamp_Kit:
				result = 4;
				break;
			case enumMaterialCategory.Steel:
				result = 33;
				break;
			}
			return result;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref mSprites_RewardMaterials);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelDescription);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelFuelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelSteelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelAmmoValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelBauxiteValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelTitle);
			mDutyStartButton = null;
			mDutyDetailActionCallBack = null;
			mDutyModel = null;
			mKeyController = null;
		}
	}
}
