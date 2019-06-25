using Common.Enum;
using KCV.Scene.Duty;
using KCV.Scene.Port;
using KCV.UseItem;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemListManager : MonoBehaviour
	{
		private enum State
		{
			ItemSelect,
			UseCheck,
			GetReward,
			ExchangeSelect,
			UseProd,
			NONE
		}

		private const int ITEM_LIST_MODELS_SPLIT = 7;

		[SerializeField]
		private Transform mTransform_DialogArea;

		[SerializeField]
		private UIItemList mItemList;

		[SerializeField]
		private UIItemListChildDetail mItemListChildDetai;

		[SerializeField]
		private UIButton mButton_ChangeItemStore;

		[SerializeField]
		private UIItemExchangeMedalConfirm mItemExchangeMedalConfirm;

		[SerializeField]
		private UIItemUseLimitOverConfirm mItemUseLimitOverConfirm;

		[SerializeField]
		private UIItemUseConfirm mItemUseConfirm;

		[SerializeField]
		private UIGetRewardDialog mPrefab_UIGetRewardDialog;

		[SerializeField]
		private UIUseItemReceiveFurnitureBox mPrefab_UIUseItemReceiveFurnitureBox;

		private Stack<State> mStateStack = new Stack<State>();

		private KeyControl mKeyController;

		private ItemlistManager mItemListManager;

		private Action mOnBack;

		private Action mOnSwitchItemStore;

		private State CurrentState
		{
			get
			{
				if (0 < mStateStack.Count)
				{
					return mStateStack.Peek();
				}
				return State.NONE;
			}
		}

		private void Start()
		{
			mItemList.SetOnFocusChangeListener(OnItemListFocusChangeListener);
			mItemList.SetOnSelectListener(OnItemListInItemSelectedListener);
			mItemListChildDetai.SetOnUseCallBack(OnSelectUseItemCallBack);
			mItemListChildDetai.SetOnCancelCallBack(OnCancelUseItem);
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
				{
					OnBack();
				}
				else if (mKeyController.IsRSRightDown())
				{
					SwitchItemStore();
				}
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_ChangeItemStore);
			mTransform_DialogArea = null;
			mItemList = null;
			mItemListChildDetai = null;
			mItemExchangeMedalConfirm = null;
			mItemUseLimitOverConfirm = null;
			mItemUseConfirm = null;
			mPrefab_UIGetRewardDialog = null;
			mPrefab_UIUseItemReceiveFurnitureBox = null;
		}

		public void OnTouchSwitchItemStore()
		{
			SwitchItemStore();
		}

		private void SwitchItemStore()
		{
			if (CurrentState == State.ItemSelect)
			{
				OnSwitchItemStore();
			}
		}

		public void Initialize(ItemlistManager manager)
		{
			mItemListManager = manager;
			ItemlistModel[] models = mItemListManager.HaveItems.ToArray();
			mItemList.Initialize(models);
		}

		public void StartState()
		{
			mStateStack.Clear();
			mItemList.FirstFocus();
			mItemList.SetKeyController(mKeyController);
			PushState(State.ItemSelect);
		}

		public void Clean()
		{
			mItemListManager = null;
			mKeyController = null;
			mItemList.Clean();
			mItemListChildDetai.Clean();
			mStateStack.Clear();
		}

		private void OnSelectUseItemCallBack(ItemlistModel model)
		{
			bool activeState = CurrentState == State.ItemSelect || CurrentState == State.UseCheck;
			if (!activeState)
			{
				return;
			}
			if (CurrentState == State.ItemSelect)
			{
				mItemList.SetKeyController(null);
				PushState(State.UseCheck);
			}
			mItemListChildDetai.SetKeyController(null);
			if (IsExchangeItemlistModel(model))
			{
				int mstId = model.MstId;
				if (mstId == 57)
				{
					mItemExchangeMedalConfirm.Initialize(model, mItemListManager);
					mItemExchangeMedalConfirm.SetKeyController(mKeyController);
					mItemExchangeMedalConfirm.SetOnExchangeItemSelectedCallBack(delegate(ItemExchangeKinds selectedItemExchangeKind)
					{
						mItemExchangeMedalConfirm.SetKeyController(null);
						ItemlistManager.Result result3 = mItemListManager.UseItem(model.MstId, is_force: false, selectedItemExchangeKind);
						if (result3.IsLimitOver())
						{
							mItemExchangeMedalConfirm.Close(null);
							mItemUseLimitOverConfirm.SetOnNegativeCallBack(delegate
							{
								if (CurrentState == State.ExchangeSelect)
								{
									PopState();
									mItemUseLimitOverConfirm.Close(null);
									mItemUseLimitOverConfirm.SetKeyController(null);
									ResumeState();
								}
							});
							mItemUseLimitOverConfirm.SetOnPositiveCallBack(delegate
							{
								if (CurrentState == State.ExchangeSelect)
								{
									PopState();
									mItemUseLimitOverConfirm.Close(null);
									mItemUseLimitOverConfirm.SetKeyController(null);
									ItemlistManager.Result result4 = mItemListManager.UseItem(model.MstId, is_force: true, selectedItemExchangeKind);
									StartCoroutine(OnGetRewards(model, result4.Rewards, delegate
									{
										mItemList.Refresh(mItemListManager.HaveItems.ToArray());
										ResumeState();
									}));
								}
							});
							mItemUseLimitOverConfirm.Initialize();
							mItemUseLimitOverConfirm.SetKeyController(mKeyController);
							mItemUseLimitOverConfirm.Show(null);
							ReplaceState(State.ExchangeSelect);
						}
						else
						{
							mItemExchangeMedalConfirm.Close(null);
							ReplaceState(State.GetReward);
							StartCoroutine(OnGetRewards(model, result3.Rewards, delegate
							{
								mItemList.Refresh(mItemListManager.HaveItems.ToArray());
								PopState();
								ResumeState();
							}));
						}
					});
					mItemExchangeMedalConfirm.SetOnCancelCallBack(delegate
					{
						mItemExchangeMedalConfirm.Close(null);
						mItemExchangeMedalConfirm.SetKeyController(null);
						PopState();
						ResumeState();
					});
					mItemExchangeMedalConfirm.Show(null);
					ReplaceState(State.ExchangeSelect);
				}
			}
			else
			{
				mItemUseConfirm.SetOnNegativeCallBack(delegate
				{
					bool flag = CurrentState == State.ExchangeSelect;
					if (activeState)
					{
						PopState();
						mItemUseConfirm.SetKeyController(null);
						mItemUseConfirm.Close(null);
						ResumeState();
					}
				});
				mItemUseConfirm.SetOnPositiveCallBack(delegate
				{
					bool flag2 = CurrentState == State.ExchangeSelect;
					if (activeState)
					{
						mItemUseConfirm.SetKeyController(null);
						mItemUseConfirm.Close(null);
						ItemlistManager.Result result = mItemListManager.UseItem(model.MstId, is_force: false, ItemExchangeKinds.NONE);
						if (result == null)
						{
							if (model.MstId == 53)
							{
								CommonPopupDialog.Instance.StartPopup("これ以上拡張できません");
							}
							PopState();
							ResumeState();
						}
						else if (result.IsLimitOver())
						{
							ReplaceState(State.ExchangeSelect);
							mItemUseLimitOverConfirm.SetOnNegativeCallBack(delegate
							{
								PopState();
								mItemUseLimitOverConfirm.SetKeyController(null);
								mItemUseLimitOverConfirm.Close(null);
								ResumeState();
							});
							mItemUseLimitOverConfirm.SetOnPositiveCallBack(delegate
							{
								mItemUseLimitOverConfirm.SetKeyController(null);
								mItemUseLimitOverConfirm.Close(null);
								ItemlistManager.Result result2 = mItemListManager.UseItem(model.MstId, is_force: true, ItemExchangeKinds.NONE);
								StartCoroutine(OnGetRewards(model, result2.Rewards, delegate
								{
									mItemList.Refresh(mItemListManager.HaveItems.ToArray());
									PopState();
									ResumeState();
								}));
							});
							mItemUseLimitOverConfirm.Initialize();
							mItemUseLimitOverConfirm.SetKeyController(mKeyController);
							mItemUseLimitOverConfirm.Show(null);
						}
						else
						{
							ReplaceState(State.GetReward);
							IReward[] rewards = result.Rewards;
							StartCoroutine(OnGetRewards(model, rewards, delegate
							{
								mItemList.Refresh(mItemListManager.HaveItems.ToArray());
								PopState();
								ResumeState();
							}));
						}
					}
				});
				mItemListChildDetai.SetKeyController(null);
				ReplaceState(State.ExchangeSelect);
				mItemUseConfirm.Initialize();
				mItemUseConfirm.Show(null);
				mItemUseConfirm.SetKeyController(mKeyController);
			}
		}

		private bool IsExchangeItemlistModel(ItemlistModel model)
		{
			int mstId = model.MstId;
			if (mstId == 57)
			{
				return true;
			}
			return false;
		}

		private IEnumerator OnGetUseItemReward(ItemlistModel usedModel, IReward_Useitem reward)
		{
			switch (usedModel.MstId)
			{
			case 10:
			case 11:
			case 12:
			{
				UIUseItemReceiveFurnitureBox furnitureBox = Util.Instantiate(mPrefab_UIUseItemReceiveFurnitureBox.gameObject, mTransform_DialogArea.gameObject).GetComponent<UIUseItemReceiveFurnitureBox>();
				mItemListChildDetai.SetKeyController(null);
				furnitureBox.SetKeyController(mKeyController);
				bool showFlag = true;
				StartCoroutine(furnitureBox.Show(usedModel.MstId, reward.Count, delegate
				{
                    furnitureBox.SetKeyController(null);
					UnityEngine.Object.Destroy(furnitureBox.gameObject);
                    showFlag = false;
				}));
				while (showFlag)
				{
					yield return null;
				}
				break;
			}
			case 57:
			{
				UIGetRewardDialog dialog = Util.Instantiate(mPrefab_UIGetRewardDialog.gameObject, mTransform_DialogArea.gameObject).GetComponent<UIGetRewardDialog>();
				bool showDialogFlag = true;
				dialog.Initialize(new Reward_Useitem[1]
				{
					(Reward_Useitem)reward
				});
				dialog.Show(mKeyController);
				dialog.SetOnDialogClosedCallBack(delegate
				{
                    showDialogFlag = false;
					UnityEngine.Object.Destroy(dialog.gameObject);
				});
				while (showDialogFlag)
				{
					yield return null;
				}
				break;
			}
			}
		}

		private IEnumerator OnGetMaterialsReward(ItemlistModel usedModel, IReward_Materials reward)
		{
			UIGetRewardDialog dialog = Util.Instantiate(mPrefab_UIGetRewardDialog.gameObject, mTransform_DialogArea.gameObject).GetComponent<UIGetRewardDialog>();
			List<Reward_Material> rewardMaterials = new List<Reward_Material>();
			IReward_Material[] rewards = reward.Rewards;
			foreach (IReward_Material material in rewards)
			{
				rewardMaterials.Add((Reward_Material)material);
			}
			bool showFlag = true;
			dialog.Initialize(rewardMaterials.ToArray());
			dialog.Show(mKeyController);
			dialog.SetOnDialogClosedCallBack(delegate
			{
                showFlag = false;
				UnityEngine.Object.Destroy(dialog.gameObject);
			});
			while (showFlag)
			{
				yield return null;
			}
			yield return null;
		}

		private IEnumerator OnGetRewards(ItemlistModel usedModel, IReward[] rewards, Action onAllReceived)
		{
			foreach (IReward reward in rewards)
			{
				if (reward is IReward_Useitem)
				{
					yield return StartCoroutine(OnGetUseItemReward(usedModel, (IReward_Useitem)reward));
				}
				else if (reward is IReward_Materials)
				{
					TrophyUtil.Unlock_Material();
					yield return StartCoroutine(OnGetMaterialsReward(usedModel, (IReward_Materials)reward));
				}
			}
			onAllReceived?.Invoke();
		}

		private void OnCancelUseItem()
		{
			if (CurrentState == State.UseCheck)
			{
				PopState();
				ResumeState();
			}
		}

		private void OnItemListFocusChangeListener(ItemlistModel model)
		{
			mItemListChildDetai.UpdateInfo(model);
		}

		private void OnItemListInItemSelectedListener(ItemlistModel model)
		{
			mItemList.SetKeyController(null);
			PushState(State.UseCheck);
		}

		private void OnSelectListInChild(UIItemListChild itemListChild)
		{
			if (itemListChild != null && mItemListChildDetai.Usable())
			{
				mItemList.SetKeyController(null);
				PushState(State.UseCheck);
			}
		}

		public void SetKeyController(KeyControl keyControl)
		{
			mKeyController = keyControl;
			mItemList.SetKeyController(mKeyController);
		}

		public void SetOnBackListener(Action onBack)
		{
			mOnBack = onBack;
		}

		private void OnBack()
		{
			if (mOnBack != null)
			{
				mOnBack();
			}
		}

		public void SetOnSwitchItemStoreListener(Action onSwitchItemStore)
		{
			mOnSwitchItemStore = onSwitchItemStore;
		}

		private void OnSwitchItemStore()
		{
			if (mOnSwitchItemStore != null)
			{
				mOnSwitchItemStore();
			}
		}

		private void PushState(State state)
		{
			mStateStack.Push(state);
			OnPushState(mStateStack.Peek());
		}

		private void ReplaceState(State state)
		{
			if (0 < mStateStack.Count)
			{
				PopState();
			}
			mStateStack.Push(state);
			OnPushState(mStateStack.Peek());
		}

		private void PopState()
		{
			if (0 < mStateStack.Count)
			{
				State state = mStateStack.Pop();
				OnPopState(state);
			}
		}

		private void ResumeState()
		{
			if (0 < mStateStack.Count)
			{
				OnResumeState(mStateStack.Peek());
			}
		}

		private void OnPushState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.GetReward:
				break;
			case State.ItemSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			case State.UseCheck:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnPushStateUseCheck();
				break;
			case State.ExchangeSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			}
		}

		private void OnPopState(State state)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			switch (state)
			{
			case State.UseCheck:
				mItemListChildDetai.SetKeyController(null);
				break;
			case State.ItemSelect:
				OnPopStateItemSelect();
				break;
			}
		}

		private void OnResumeState(State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case State.GetReward:
				break;
			case State.ItemSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				OnResumeStateItemSelect();
				break;
			case State.UseCheck:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			case State.ExchangeSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			}
		}

		private void OnPushStateUseCheck()
		{
			mItemListChildDetai.SetKeyController(mKeyController);
		}

		private void OnPopStateItemSelect()
		{
			mItemList.SetKeyController(null);
		}

		private void OnResumeStateItemSelect()
		{
			mItemList.SetKeyController(mKeyController);
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mItemListManager);
		}

		public string StateToString()
		{
			mStateStack.ToArray();
			string text = string.Empty;
			foreach (State item in mStateStack)
			{
				text = item + " > " + text;
			}
			return text;
		}
	}
}
