using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodeModernizationShipTargetListChild : AbstractUIRemodelListChild<RemodeModernizationShipTargetListChild>
	{
		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UISprite mSprite_Karyoku;

		[SerializeField]
		private UISprite mSprite_Raisou;

		[SerializeField]
		private UISprite mSprite_Soukou;

		[SerializeField]
		private UISprite mSprite_Taikuu;

		[SerializeField]
		private UISprite mSprite_Luck;

		[SerializeField]
		private Transform mLEVEL;

		[SerializeField]
		private Transform mUNSET;

		[SerializeField]
		private UISprite mListBar;

		protected override void InitializeChildContents(RemodeModernizationShipTargetListChild childData, bool clickable)
		{
			switch (childData.mOption)
			{
			case RemodeModernizationShipTargetListChild.ListItemOption.Model:
				InitializeModel(childData.mShipModel);
				break;
			case RemodeModernizationShipTargetListChild.ListItemOption.UnSet:
				InitializeUnset();
				break;
			}
		}

		private void InitializeUnset()
		{
			mCommonShipBanner.SetActive(isActive: false);
			mSprite_Karyoku.alpha = 0f;
			mSprite_Raisou.alpha = 0f;
			mSprite_Soukou.alpha = 0f;
			mSprite_Taikuu.alpha = 0f;
			mSprite_Luck.alpha = 0f;
			mLabel_Name.alpha = 0f;
			mLabel_Level.alpha = 0f;
			mLabel_Level.text = "はずす";
			mLabel_Name.text = string.Empty;
			mLEVEL.localScale = Vector3.zero;
			mUNSET.localScale = Vector3.one;
			mListBar.color = Color.white;
		}

		private void InitializeModel(ShipModel shipModel)
		{
			mCommonShipBanner.SetActive(isActive: true);
			mSprite_Karyoku.alpha = 1f;
			mSprite_Raisou.alpha = 1f;
			mSprite_Soukou.alpha = 1f;
			mSprite_Taikuu.alpha = 1f;
			mSprite_Luck.alpha = 1f;
			mLabel_Name.alpha = 1f;
			mLabel_Level.alpha = 1f;
			mLEVEL.localScale = Vector3.one;
			mUNSET.localScale = Vector3.zero;
			mListBar.color = Color.white;
			if (0 < shipModel.PowUpKaryoku)
			{
				mSprite_Karyoku.spriteName = "icon_1_on";
			}
			else
			{
				mSprite_Karyoku.spriteName = "icon_1";
			}
			if (0 < shipModel.PowUpRaisou)
			{
				mSprite_Raisou.spriteName = "icon_2_on";
			}
			else
			{
				mSprite_Raisou.spriteName = "icon_2";
			}
			if (0 < shipModel.PowUpSoukou)
			{
				mSprite_Soukou.spriteName = "icon_3_on";
			}
			else
			{
				mSprite_Soukou.spriteName = "icon_3";
			}
			if (0 < shipModel.PowUpTaikuu)
			{
				mSprite_Taikuu.spriteName = "icon_4_on";
			}
			else
			{
				mSprite_Taikuu.spriteName = "icon_4";
			}
			if (0 < shipModel.PowUpLucky)
			{
				mSprite_Luck.spriteName = "icon_5_on";
			}
			else
			{
				mSprite_Luck.spriteName = "icon_5";
			}
			mCommonShipBanner.SetShipDataWithDisableParticle(shipModel);
			mLabel_Level.text = shipModel.Level.ToString();
			mLabel_Name.text = shipModel.Name;
		}

		public override void OnTouchScrollListChild()
		{
			if (base.IsShown)
			{
				base.OnTouchScrollListChild();
			}
		}
	}
}
