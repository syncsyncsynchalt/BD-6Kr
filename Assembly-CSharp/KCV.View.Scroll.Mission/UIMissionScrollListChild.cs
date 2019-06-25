using Common.Enum;
using Common.Struct;
using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.View.Scroll.Mission
{
	public class UIMissionScrollListChild : UIScrollListChild<MissionModel>
	{
		public enum ActionType
		{
			Back,
			Action,
			Description
		}

		public delegate void UIMissionScrollListChildAction(ActionType actionType, UIMissionScrollListChild calledObject);

		[SerializeField]
		private UILabel mLabel_Title;

		[SerializeField]
		private UISprite mSprite_Status;

		[SerializeField]
		private UISprite mSprite_Reward_00;

		[SerializeField]
		private UISprite mSprite_Reward_01;

		[SerializeField]
		private UISprite mSprite_Reward_02;

		[SerializeField]
		private UISprite mSprite_Reward_03;

		[SerializeField]
		private UITexture mTexture_BackgroundDesign;

		private UIMissionScrollListChildAction mUIMissionScrollListChildActionCallBack;

		private UIButton mButton_Focus;

		private KeyControl mKeyController;

		public void SetOnUIMissionScrollListChildAction(UIMissionScrollListChildAction action)
		{
			mUIMissionScrollListChildActionCallBack = action;
		}

		protected override void InitializeChildContents(MissionModel model, bool clickable)
		{
			mLabel_Title.text = model.Name;
			switch (model.State)
			{
			case MissionClearKinds.CLEARED:
				mSprite_Status.spriteName = "icon_check";
				mSprite_Status.MakePixelPerfect();
				break;
			case MissionClearKinds.NEW:
				mSprite_Status.spriteName = "icon_new";
				mSprite_Status.MakePixelPerfect();
				break;
			default:
				mSprite_Status.spriteName = string.Empty;
				break;
			}
			InitializeRewardIcon(model.GetRewardMaterials());
			mTexture_BackgroundDesign.mainTexture = Resources.Load<Texture>($"Textures/Mission/sea/{AreaIdToSeaSpriteName(model.AreaId)}");
		}

		private string AreaIdToSeaSpriteName(int areaId)
		{
			switch (areaId)
			{
			case 1:
			case 8:
			case 9:
			case 11:
			case 12:
				return "list_sea" + 1;
			case 3:
			case 13:
				return "list_sea" + 3;
			case 2:
			case 4:
			case 5:
			case 6:
			case 7:
			case 10:
			case 14:
				return "list_sea" + 2;
			case 15:
			case 16:
			case 17:
				return "list_sea" + 4;
			default:
				return string.Empty;
			}
		}

		private void InitializeRewardIcon(MaterialInfo materialInfo)
		{
			if (0 < materialInfo.Ammo)
			{
				mSprite_Reward_00.spriteName = "item_" + enumMaterialCategoryToId(enumMaterialCategory.Bull);
			}
			else
			{
				mSprite_Reward_00.spriteName = string.Empty;
			}
			if (0 < materialInfo.Baux)
			{
				mSprite_Reward_01.spriteName = "item_" + enumMaterialCategoryToId(enumMaterialCategory.Bauxite);
			}
			else
			{
				mSprite_Reward_01.spriteName = string.Empty;
			}
			if (0 < materialInfo.Fuel)
			{
				mSprite_Reward_02.spriteName = "item_" + enumMaterialCategoryToId(enumMaterialCategory.Fuel);
			}
			else
			{
				mSprite_Reward_02.spriteName = string.Empty;
			}
			if (0 < materialInfo.Steel)
			{
				mSprite_Reward_03.spriteName = "item_" + enumMaterialCategoryToId(enumMaterialCategory.Steel);
			}
			else
			{
				mSprite_Reward_03.spriteName = string.Empty;
			}
		}

		private int enumMaterialCategoryToId(enumMaterialCategory type)
		{
			switch (type)
			{
			case enumMaterialCategory.Fuel:
				return 31;
			case enumMaterialCategory.Bull:
				return 32;
			case enumMaterialCategory.Steel:
				return 33;
			case enumMaterialCategory.Bauxite:
				return 34;
			default:
				return 0;
			}
		}

		public override void Hover()
		{
			base.Hover();
		}

		public override void RemoveHover()
		{
			base.RemoveHover();
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[1].down)
			{
				if (mButton_Focus.Equals(mButton_Action))
				{
					CallBackAction(ActionType.Action, this);
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				CallBackAction(ActionType.Back, this);
			}
		}

		public KeyControl GetKeyController()
		{
			mKeyController = new KeyControl();
			return mKeyController;
		}

		public void RemoveKeyFocus()
		{
			mKeyController = null;
			Hover();
		}

		private void CallBackAction(ActionType actionType, UIMissionScrollListChild calledObject)
		{
			if (mUIMissionScrollListChildActionCallBack != null)
			{
				mUIMissionScrollListChildActionCallBack(actionType, calledObject);
			}
		}

		protected override void OnCallDestroy()
		{
			base.OnCallDestroy();
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Title);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Status);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Reward_00);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Reward_01);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Reward_02);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Reward_03);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_BackgroundDesign);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Focus);
			mUIMissionScrollListChildActionCallBack = null;
			mKeyController = null;
		}
	}
}
