using Common.Enum;
using KCV.Scene.Port;
using KCV.View;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIDutyDetailCheck : BoardDialog
	{
		public enum SelectType
		{
			Positive,
			Negative
		}

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
		private UISprite[] mSprites_RewardMaterials;

		private Action mClosedCallBack;

		private DutyModel mDutyModel;

		private KeyControl mKeyController;

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[0].down)
				{
					mKeyController = null;
					mClosedCallBack();
				}
				else if (mKeyController.keyState[1].down)
				{
					mKeyController = null;
					mClosedCallBack();
				}
			}
		}

		public new KeyControl Show()
		{
			base.Show();
			mKeyController = new KeyControl();
			return mKeyController;
		}

		public void SetDutyDetailCheckClosedCallBack(Action action)
		{
			mClosedCallBack = action;
		}

		public void Initialize(DutyModel dutyModel)
		{
			mDutyModel = dutyModel;
			mLabelTitle.text = dutyModel.Title;
			mLabelDescription.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(24, 1, dutyModel.Description);
			mLabelFuelValue.text = dutyModel.Fuel.ToString();
			mLabelSteelValue.text = dutyModel.Steel.ToString();
			mLabelAmmoValue.text = dutyModel.Ammo.ToString();
			mLabelBauxiteValue.text = dutyModel.Baux.ToString();
			int num = 0;
			enumMaterialCategory[] source = new enumMaterialCategory[4]
			{
				enumMaterialCategory.Build_Kit,
				enumMaterialCategory.Dev_Kit,
				enumMaterialCategory.Repair_Kit,
				enumMaterialCategory.Revamp_Kit
			};
			foreach (KeyValuePair<enumMaterialCategory, int> rewardMaterial in mDutyModel.RewardMaterials)
			{
				if (source.Contains(rewardMaterial.Key) && 0 < rewardMaterial.Value && num < mSprites_RewardMaterials.Length)
				{
					mSprites_RewardMaterials[num].spriteName = $"item_{MaterialEnumToMasterId(rewardMaterial.Key)}";
					num++;
				}
			}
			for (int i = num; i < mSprites_RewardMaterials.Length; i++)
			{
				mSprites_RewardMaterials[i].spriteName = "none";
			}
		}

		public new void Hide(Action action)
		{
			base.Hide(action);
		}

		public void OnClickClose()
		{
			mKeyController = null;
			mClosedCallBack();
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
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelTitle);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelDescription);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelFuelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelSteelValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelAmmoValue);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabelBauxiteValue);
			UserInterfacePortManager.ReleaseUtils.Releases(ref mSprites_RewardMaterials);
			mClosedCallBack = null;
			mDutyModel = null;
			mKeyController = null;
		}
	}
}
