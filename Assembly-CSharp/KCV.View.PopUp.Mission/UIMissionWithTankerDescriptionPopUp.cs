using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.View.PopUp.Mission
{
	[RequireComponent(typeof(UIPanel))]
	public class UIMissionWithTankerDescriptionPopUp : MonoBehaviour
	{
		public enum ActionType
		{
			Shown,
			Hiden
		}

		public delegate void UIMissionWithTankerDescriptionPopUpAction(ActionType actionType, UIMissionWithTankerDescriptionPopUp calledObject);

		private UIMissionWithTankerDescriptionPopUpAction mUIMissionWithTankerDescriptionPopUpAction;

		[SerializeField]
		private UILabel mLabel_Title;

		[SerializeField]
		private UILabel mLabel_Description;

		[SerializeField]
		private UILabel mLabel_RequireDay;

		[SerializeField]
		private UILabel mLabel_RequireTransportCraft;

		[SerializeField]
		private UILabel mLabel_RequireFuel;

		[SerializeField]
		private UILabel mLabel_RequireAmmo;

		[SerializeField]
		private UITexture mTexture_BackgroundDesign;

		[SerializeField]
		private UISprite mSprite_Reward_00;

		[SerializeField]
		private UISprite mSprite_Reward_01;

		private UIPanel mPanelThis;

		private MissionModel mModel;

		private KeyControl mKeyController;

		private Coroutine mInitializeCoroutine;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0.01f;
		}

		public KeyControl GetKeyController()
		{
			if (mKeyController == null)
			{
				mKeyController = new KeyControl();
			}
			return mKeyController;
		}

		private void Update()
		{
			if (mKeyController != null && (mKeyController.keyState[0].down || mKeyController.keyState[1].down))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				Hide();
			}
		}

		public void Show()
		{
			mPanelThis.alpha = 1f;
			CallBackAction(ActionType.Shown, this);
		}

		public void Hide()
		{
			mPanelThis.alpha = 0.01f;
			CallBackAction(ActionType.Hiden, this);
		}

		public void Initialize(MissionModel model)
		{
			mModel = model;
			mInitializeCoroutine = StartCoroutine(InitializeCoroutine(model));
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
				return "popup_sea" + 1;
			case 3:
			case 13:
				return "popup_sea" + 3;
			case 2:
			case 4:
			case 5:
			case 6:
			case 7:
			case 10:
			case 14:
				return "popup_sea" + 2;
			case 15:
			case 16:
			case 17:
				return "popup_sea" + 4;
			default:
				return string.Empty;
			}
		}

		private IEnumerator InitializeCoroutine(MissionModel model)
		{
			mLabel_Title.alpha = 0.01f;
			mLabel_Title.text = model.Name;
			mLabel_Description.text = UserInterfaceAlbumManager.Utils.NormalizeDescription(25, 1, model.Description);
			mLabel_RequireDay.text = model.Turn.ToString();
			mLabel_RequireTransportCraft.text = model.TankerCount.ToString();
			mLabel_RequireAmmo.text = getDisplayText(model.UseAmmo);
			mLabel_RequireFuel.text = getDisplayText(model.UseFuel);
			mTexture_BackgroundDesign.mainTexture = Resources.Load<Texture>($"Textures/Mission/sea_description/{AreaIdToSeaSpriteName(model.AreaId)}");
			List<Reward_Useitem> rewards = model.GetRewardUseitems();
			switch (rewards.Count)
			{
			case 0:
				mSprite_Reward_00.spriteName = string.Empty;
				mSprite_Reward_01.spriteName = string.Empty;
				break;
			case 1:
				mSprite_Reward_00.spriteName = "item_" + rewards[0].Id.ToString();
				mSprite_Reward_01.spriteName = string.Empty;
				break;
			case 2:
				mSprite_Reward_00.spriteName = "item_" + rewards[0].Id.ToString();
				mSprite_Reward_01.spriteName = "item_" + rewards[1].Id.ToString();
				break;
			default:
				mSprite_Reward_00.spriteName = "item_" + rewards[0].Id.ToString();
				mSprite_Reward_01.spriteName = "item_" + rewards[1].Id.ToString();
				break;
			}
			yield return null;
			Vector3 localPosition = mLabel_Title.transform.localPosition;
			float x = localPosition.x - 50f;
			Vector3 localPosition2 = mLabel_Title.transform.localPosition;
			float y = localPosition2.y;
			Vector3 localPosition3 = mLabel_Title.transform.localPosition;
			Vector3 from = new Vector3(x, y, localPosition3.z);
			Vector3 to = mLabel_Title.transform.localPosition;
			mLabel_Title.transform.localPosition = from;
			mLabel_Title.transform.DOLocalMove(to, 0.3f);
			mLabel_Title.alpha = 1f;
		}

		public void SetOnUIMissionWithTankerDescriptionPopUpAction(UIMissionWithTankerDescriptionPopUpAction action)
		{
			mUIMissionWithTankerDescriptionPopUpAction = action;
		}

		private void CallBackAction(ActionType actionType, UIMissionWithTankerDescriptionPopUp calledObject)
		{
			if (mUIMissionWithTankerDescriptionPopUpAction != null)
			{
				mUIMissionWithTankerDescriptionPopUpAction(actionType, calledObject);
			}
		}

		private string getDisplayText(double rate)
		{
			if (rate == 0.0)
			{
				return "なし";
			}
			if (rate <= 0.30000001192092896)
			{
				return "少量";
			}
			return "普通";
		}

		private void OnDestroy()
		{
			mUIMissionWithTankerDescriptionPopUpAction = null;
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Title);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Description);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_RequireDay);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_RequireTransportCraft);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_RequireFuel);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_RequireAmmo);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_BackgroundDesign);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Reward_00);
			UserInterfacePortManager.ReleaseUtils.Release(ref mSprite_Reward_01);
			UserInterfacePortManager.ReleaseUtils.Release(ref mPanelThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Description);
			mModel = null;
			mKeyController = null;
			mInitializeCoroutine = null;
		}
	}
}
