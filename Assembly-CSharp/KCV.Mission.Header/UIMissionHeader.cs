using Common.Struct;
using local.managers;
using System;
using UnityEngine;

namespace KCV.Mission.Header
{
	public class UIMissionHeader : MonoBehaviour
	{
		private const int CIRCLEMODE_ROT_SPD = 10;

		private Action mOnClickCircleButtonCallBack;

		[SerializeField]
		private Transform mHeaderCircle;

		[SerializeField]
		private UILabel mLabelDay;

		[SerializeField]
		private UILabel mLabelFuelValue;

		[SerializeField]
		private UILabel mLabelAmmoValue;

		[SerializeField]
		private UILabel mLabelSteelValue;

		[SerializeField]
		private UILabel mLabelBauxiteValue;

		[SerializeField]
		private UILabel mLabelBuildKitValue;

		[SerializeField]
		private UILabel mLabelRepairKitValue;

		[SerializeField]
		private UILabel mLabelTransportCraftValue;

		[SerializeField]
		private UISprite mSpriteAreaName;

		private void Update()
		{
			mHeaderCircle.transform.Rotate(new Vector3(0f, 0f, 10f) * (0f - Time.deltaTime));
		}

		public void Initialize(MissionManager manager, Action OnClickCircleButtonCallBack)
		{
			mOnClickCircleButtonCallBack = OnClickCircleButtonCallBack;
			Refresh(manager);
		}

		public void Refresh(MissionManager manager)
		{
			UILabel uILabel = mLabelDay;
			string[] array = new string[6];
			TurnString datetimeString = manager.DatetimeString;
			array[0] = datetimeString.Year;
			array[1] = "の年\u3000";
			TurnString datetimeString2 = manager.DatetimeString;
			array[2] = datetimeString2.Month;
			array[3] = " ";
			TurnString datetimeString3 = manager.DatetimeString;
			array[4] = datetimeString3.Day;
			array[5] = " 日";
			uILabel.text = string.Concat(array);
			if (manager.UserInfo.GetMaterialMaxNum() <= manager.Material.Fuel)
			{
				mLabelFuelValue.color = Color.yellow;
			}
			else
			{
				mLabelFuelValue.color = Color.white;
			}
			mLabelFuelValue.text = manager.Material.Fuel.ToString();
			if (manager.UserInfo.GetMaterialMaxNum() <= manager.Material.Ammo)
			{
				mLabelAmmoValue.color = Color.yellow;
			}
			else
			{
				mLabelAmmoValue.color = Color.white;
			}
			mLabelAmmoValue.text = manager.Material.Ammo.ToString();
			if (manager.UserInfo.GetMaterialMaxNum() <= manager.Material.Steel)
			{
				mLabelSteelValue.color = Color.yellow;
			}
			else
			{
				mLabelSteelValue.color = Color.white;
			}
			mLabelSteelValue.text = manager.Material.Steel.ToString();
			if (manager.UserInfo.GetMaterialMaxNum() <= manager.Material.Baux)
			{
				mLabelBauxiteValue.color = Color.yellow;
			}
			else
			{
				mLabelBauxiteValue.color = Color.white;
			}
			mLabelBauxiteValue.text = manager.Material.Baux.ToString();
			mLabelBuildKitValue.text = manager.Material.Devkit.ToString();
			mLabelRepairKitValue.text = manager.Material.RepairKit.ToString();
			mLabelTransportCraftValue.text = manager.TankerCount.ToString() + " 隻";
			mSpriteAreaName.spriteName = $"map_txt{manager.AreaId:00}_on";
			mSpriteAreaName.MakePixelPerfect();
		}

		public void OnClickCircleButton()
		{
			if (mOnClickCircleButtonCallBack != null)
			{
				mOnClickCircleButtonCallBack();
			}
		}
	}
}
