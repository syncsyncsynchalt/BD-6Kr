using DG.Tweening;
using KCV.Scene.Duty.Reward;
using KCV.Utils;
using KCV.View;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UIGetRewardDialog : BoardDialog
	{
		[Button("PlayKiraAnimation", "キラキラ", new object[]
		{

		})]
		public int button;

		[SerializeField]
		private Transform mTransform_Contents;

		[SerializeField]
		private UIDutyOpenDeckRewardGet mPrefab_UIDutyOpenDeckRewardGet;

		[SerializeField]
		private UIDutyGetRewardUseItem mPrefab_UIDutyGetRewardUseItem;

		[SerializeField]
		private UIDutyGetRewardOpenCreateLargeTanker mPrefab_UIDutyGetRewardOpenCreateLargeTanker;

		[SerializeField]
		private UIDutyGetRewardSlotItem mPrefab_UIDutyGetRewardSlotItem;

		[SerializeField]
		private UIRewardMaterialsGrid mDutyRewardMaterialsGrid;

		[SerializeField]
		private UIRewardUseItemsGrid mDutyRewardUseItemsGrid;

		[SerializeField]
		private UIDutyRewardExchangeItem mPrefab_UIDutyRewardExchangeItem;

		[SerializeField]
		private UIDutyGetRewardFurniture mPrefab_UIDutyGetRewardFurniture;

		[SerializeField]
		private UIDutyOpenDeckPracticeRewardGet mPrefab_UIDutyOpenDeckPracticeRewardGet;

		[SerializeField]
		private UIDutyGetTransportCraftRewardGet mPrefab_UIDutyGetTransportCraftRewardGet;

		[SerializeField]
		private UIPanel KiraPanel;

		[SerializeField]
		private UIPanel mPanel_RewardArea;

		private KeyControl mKeyController;

		private Action mClosedCallBack;

		public void Initialize(IReward[] materials)
		{
			if (materials.Length == 1)
			{
				mDutyRewardMaterialsGrid.transform.localScale = new Vector3(1.25f, 1.25f);
			}
			else
			{
				mDutyRewardMaterialsGrid.transform.localScale = Vector3.one;
			}
			mDutyRewardMaterialsGrid.Initialize(materials);
			mDutyRewardMaterialsGrid.GoToPage(0);
		}

		public void Initialize(Reward_LargeBuild largeBuildObject)
		{
			Util.Instantiate(mPrefab_UIDutyGetRewardOpenCreateLargeTanker.gameObject, mPanel_RewardArea.gameObject);
		}

		public void Initialize(Reward_Deck rewardDeck)
		{
			Util.Instantiate(mPrefab_UIDutyOpenDeckRewardGet.gameObject, mPanel_RewardArea.gameObject).GetComponent<UIDutyOpenDeckRewardGet>().Initialize(rewardDeck);
		}

		public void Initialize(Reward_Furniture rewardFurniture)
		{
			Util.Instantiate(mPrefab_UIDutyGetRewardFurniture.gameObject, mPanel_RewardArea.gameObject).GetComponent<UIDutyGetRewardFurniture>().Initialize(rewardFurniture);
		}

		public void Initialize(Reward_Useitem reward)
		{
			Util.Instantiate(mPrefab_UIDutyGetRewardUseItem.gameObject, mPanel_RewardArea.gameObject).GetComponent<UIDutyGetRewardUseItem>().Initialize(reward);
		}

		public void Initialize(IReward_Slotitem reward_Slotitem)
		{
			Util.Instantiate(mPrefab_UIDutyGetRewardSlotItem.gameObject, mPanel_RewardArea.gameObject).GetComponent<UIDutyGetRewardSlotItem>().Initialize((Reward_Slotitem)reward_Slotitem);
		}

		public void Initialize(Reward_DeckPracitce reward)
		{
			Util.Instantiate(mPrefab_UIDutyOpenDeckPracticeRewardGet.gameObject, mPanel_RewardArea.gameObject).GetComponent<UIDutyOpenDeckPracticeRewardGet>().Initialize(reward);
		}

		public void Initialize(Reward_TransportCraft reward)
		{
			Util.Instantiate(mPrefab_UIDutyGetTransportCraftRewardGet.gameObject, mPanel_RewardArea.gameObject).GetComponent<UIDutyGetTransportCraftRewardGet>().Initialize(reward);
		}

		public void SetOnDialogClosedCallBack(Action callBack)
		{
			mClosedCallBack = callBack;
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[0].down)
				{
					Close();
				}
				else if (mKeyController.keyState[1].down)
				{
					Close();
				}
			}
		}

		public new KeyControl Show()
		{
			base.Show();
			mTransform_Contents.transform.localScale = new Vector3(0.5f, 0.5f);
			mTransform_Contents.transform.DOScale(Vector3.one, 0.3f);
			mKeyController = new KeyControl();
			PlayKiraAnimation();
			return mKeyController;
		}

		public void Show(KeyControl keyController)
		{
			base.Show();
			mTransform_Contents.transform.localScale = new Vector3(0.5f, 0.5f);
			mTransform_Contents.transform.DOScale(Vector3.one, 0.3f);
			mKeyController = keyController;
			PlayKiraAnimation();
		}

		public void Close()
		{
			if (mKeyController != null && mKeyController.IsRun)
			{
				Hide(mClosedCallBack);
				mKeyController = null;
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void PlayKiraAnimation()
		{
			iTween.ValueTo(base.gameObject, iTween.Hash("from", -970, "to", 1000, "time", 2f, "onupdate", "UpdateHandler", "looptype", iTween.LoopType.loop));
		}

		private void UpdateHandler(float value)
		{
			KiraPanel.clipOffset = new Vector2(value, 0f);
		}

		private void OnDestroy()
		{
			mTransform_Contents = null;
			mPrefab_UIDutyOpenDeckRewardGet = null;
			mPrefab_UIDutyGetRewardUseItem = null;
			mPrefab_UIDutyGetRewardOpenCreateLargeTanker = null;
			mPrefab_UIDutyGetRewardSlotItem = null;
			mDutyRewardMaterialsGrid = null;
			mDutyRewardUseItemsGrid = null;
			mPrefab_UIDutyRewardExchangeItem = null;
			mPrefab_UIDutyGetRewardFurniture = null;
			mPrefab_UIDutyOpenDeckPracticeRewardGet = null;
			mPrefab_UIDutyGetTransportCraftRewardGet = null;
			KiraPanel = null;
			mPanel_RewardArea = null;
			mKeyController = null;
			mClosedCallBack = null;
		}
	}
}
