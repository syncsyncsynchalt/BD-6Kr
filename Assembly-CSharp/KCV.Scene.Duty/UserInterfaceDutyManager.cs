using AnimationOrTween;
using Common.Enum;
using DG.Tweening;
using KCV.Battle.Production;
using KCV.Dialog;
using KCV.Production;
using KCV.Scene.Duty.Reward;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Duty
{
	public class UserInterfaceDutyManager : MonoBehaviour
	{
		private const BGMFileInfos SCENE_BGM = BGMFileInfos.PortTools;

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private Transform mTransform_AllClearText;

		[SerializeField]
		private UIDutyDetail mPrefabDutyDetail;

		[SerializeField]
		private UIDutyDetailCheck mPrefabDutyDetailCheck;

		[SerializeField]
		private UIGetRewardDialog mPrefabUIDutyRewardMaterialsDialog;

		[SerializeField]
		private UIDutyRewardExchangeItem mPrefabUIDutyRewardExchangeItem;

		[SerializeField]
		private ProdRewardGet mPrefabRewardShip;

		[SerializeField]
		private ModalCamera mModalCamera;

		[SerializeField]
		private UIDutyOhyodo mPrefab_DutyOhyodo;

		[SerializeField]
		private UIDutyGrid mDutyGrid;

		[SerializeField]
		private UILabel mLabel_DutyCount;

		[SerializeField]
		private Font mUseFont;

		[SerializeField]
		private UITexture mTexture_LeftArrow;

		[SerializeField]
		private UITexture mTexture_RightArrow;

		[SerializeField]
		private UITexture mTexture_LeftArrowShadow;

		[SerializeField]
		private UITexture mTexture_RightArrowShadow;

		private DutyManager mDutyManager;

		private KeyControl mFocusKeyController;

		public bool _DeteilMode;

		[SerializeField]
		private int FIND_MISSION_ID = 120;

		[SerializeField]
		private float arrowDuration = 0.3f;

		[SerializeField]
		private float arrowAlpha = 0.6f;

		[SerializeField]
		private int arrowMove = 20;

		private IEnumerator Start()
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			yield return new WaitForEndOfFrame();
			mDutyManager = new DutyManager();
			SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(mDutyManager);
			mDutyGrid.SetOnSummarySelectedCallBack(UIDutySummaryEventCallBack);
			mDutyGrid.SetOnChangePageListener(OnChangePageDutyGrid);
			DutyModel[] duties = GetDuties();
			mDutyGrid.Initialize(duties);
			SoundFile.LoadBGM(BGMFileInfos.PortTools);
			_DeteilMode = false;
			if (duties.Length <= 0)
			{
				mDutyGrid.GoToPage(0, focus: false);
				mTransform_AllClearText.SetActive(isActive: true);
				UpdateOrderPossibleDutyCount(0, animate: false);
				KeyControl nextKeyController = mDutyGrid.GetKeyController();
				mDutyGrid.FirstFocus();
				ChangeKeyController(nextKeyController);
				stopWatch.Stop();
			}
			else
			{
				int orderDutyCount = mDutyManager.MaxExecuteCount - mDutyManager.GetExecutedDutyList().Count;
				UpdateOrderPossibleDutyCount(orderDutyCount, animate: false);
				KeyControl greetingOhyodoKeyController = new KeyControl();
				ChangeKeyController(greetingOhyodoKeyController);
				mDutyGrid.GoToPage(0, focus: false);
				stopWatch.Stop();
				for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
				{
					yield return new WaitForEndOfFrame();
				}
				yield return new WaitForEndOfFrame();
				StartCoroutine(GreetingOhyodo(greetingOhyodoKeyController, delegate
				{
					KeyControl keyController = this.mDutyGrid.GetKeyController();
					this.ChangeKeyController(keyController);
				}));
				mDutyGrid.FirstFocus();
			}
			SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
			{
                var preloadBGM = SoundFile.LoadBGM(BGMFileInfos.PortTools);
                SoundUtils.SwitchBGM(preloadBGM);
			});
		}

		private void OnChangePageDutyGrid()
		{
			int num = mDutyGrid.GetCurrentPageIndex() + 1;
			int pageSize = mDutyGrid.GetPageSize();
			if (DOTween.IsTweening(mTexture_LeftArrow))
			{
				DOTween.Kill(mTexture_LeftArrow);
			}
			if (DOTween.IsTweening(mTexture_RightArrow))
			{
				DOTween.Kill(mTexture_RightArrow);
			}
			if (DOTween.IsTweening(mTexture_LeftArrowShadow))
			{
				DOTween.Kill(mTexture_LeftArrowShadow);
			}
			if (DOTween.IsTweening(mTexture_RightArrowShadow))
			{
				DOTween.Kill(mTexture_RightArrowShadow);
			}
			if (pageSize == 0 || pageSize == 1)
			{
				mTexture_RightArrow.SetActive(isActive: false);
				mTexture_LeftArrow.SetActive(isActive: false);
				mTexture_RightArrowShadow.SetActive(isActive: false);
				mTexture_LeftArrowShadow.SetActive(isActive: false);
				return;
			}
			if (1 < num)
			{
				mTexture_LeftArrow.SetActive(isActive: true);
				mTexture_LeftArrowShadow.SetActive(isActive: true);
				mTexture_LeftArrowShadow.alpha = 1f;
				mTexture_LeftArrowShadow.transform.localPosition = mTexture_LeftArrow.transform.localPosition;
				GenerateTweenArrow(mTexture_LeftArrowShadow, Direction.Forward).SetLoops(int.MaxValue).SetId(mTexture_LeftArrowShadow);
			}
			else
			{
				mTexture_LeftArrow.SetActive(isActive: false);
				mTexture_LeftArrowShadow.SetActive(isActive: false);
			}
			if (num < pageSize)
			{
				mTexture_RightArrow.SetActive(isActive: true);
				mTexture_RightArrowShadow.SetActive(isActive: true);
				mTexture_RightArrowShadow.alpha = 1f;
				mTexture_RightArrowShadow.transform.localPosition = mTexture_RightArrow.transform.localPosition;
				GenerateTweenArrow(mTexture_RightArrowShadow, Direction.Reverse).SetLoops(int.MaxValue).SetId(mTexture_RightArrowShadow);
			}
			else
			{
				mTexture_RightArrow.SetActive(isActive: false);
				mTexture_RightArrowShadow.SetActive(isActive: false);
			}
		}

		private void UpdateOrderPossibleDutyCount(int displayValue, bool animate)
		{
			if (DOTween.IsTweening(mLabel_DutyCount))
			{
				DOTween.Kill(mLabel_DutyCount, complete: true);
			}
			if (animate)
			{
				Sequence sequence = DOTween.Sequence();
				Tween t = mLabel_DutyCount.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.1f);
				TweenCallback callback = delegate
				{
					mLabel_DutyCount.text = displayValue.ToString();
				};
				Tween t2 = mLabel_DutyCount.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
				sequence.SetEase(Ease.OutQuint);
				sequence.AppendInterval(0.25f);
				sequence.Append(t);
				sequence.AppendCallback(callback);
				sequence.Append(t2);
				sequence.SetId(mLabel_DutyCount);
			}
			else
			{
				mLabel_DutyCount.text = displayValue.ToString();
			}
		}

		private IEnumerator GreetingOhyodo(KeyControl keyController, Action onFinishedGreeting)
		{
			UIDutyOhyodo ohyodo = Util.Instantiate(mPrefab_DutyOhyodo.gameObject, mModalCamera.gameObject).GetComponent<UIDutyOhyodo>();
			bool show = false;
			ohyodo.Show(delegate
			{
                show = true;
			});
			while (!show)
			{
				yield return null;
			}
			while (!Input.anyKey)
			{
				yield return null;
			}
			ohyodo.EnableTouchBackArea(enabled: true);
			ohyodo.Hide(delegate
			{
				onFinishedGreeting();
				UnityEngine.Object.Destroy(ohyodo.gameObject);
			});
			yield return null;
		}

		private void Update()
		{
			if (mFocusKeyController != null)
			{
				mFocusKeyController.Update();
				if (mFocusKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
				{
					mFocusKeyController.ClearKeyAll();
					mFocusKeyController.firstUpdate = true;
					mFocusKeyController = null;
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
			}
		}

		private void ChangeKeyController(KeyControl keyController)
		{
			if (mFocusKeyController != null)
			{
				mFocusKeyController.firstUpdate = true;
				mFocusKeyController.ClearKeyAll();
			}
			mFocusKeyController = keyController;
			if (mFocusKeyController != null)
			{
				mFocusKeyController.firstUpdate = true;
				mFocusKeyController.ClearKeyAll();
			}
		}

		private DutyModel[] GetDuties()
		{
			return mDutyManager.GetDuties(is_sorted: true);
		}

		private KeyControl ShowUIDutyDetail(UIDutySummary summary)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			UIDutyDetail dutyDetail = null;
			_DeteilMode = true;
			dutyDetail = Util.Instantiate(mPrefabDutyDetail.gameObject, mModalCamera.gameObject).GetComponent<UIDutyDetail>();
			dutyDetail.Initialize(summary.GetModel());
			dutyDetail.SetDutyDetailCallBack(delegate(UIDutyDetail.SelectType selectedType)
			{
				if (selectedType == UIDutyDetail.SelectType.Positive)
				{
					mDutyManager.StartDuty(summary.GetModel().No);
					UpdateOrderPossibleDutyCount(mDutyManager.MaxExecuteCount - mDutyManager.GetExecutedDutyList().Count, animate: true);
					DutyModel duty = mDutyManager.GetDuty(summary.GetModel().No);
					summary.Initialize(summary.GetIndex(), duty);
					TutorialModel tutorial = mDutyManager.UserInfo.Tutorial;
					if (tutorial.GetStep() == 0 && !tutorial.GetStepTutorialFlg(1))
					{
						tutorial.SetStepTutorialFlg(1);
						CommonPopupDialog.Instance.StartPopup("「はじめての任務！」 達成");
						SoundUtils.PlaySE(SEFIleInfos.SE_012);
					}
				}
				dutyDetail.Hide(delegate
				{
					_DeteilMode = false;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
					KeyControl keyController = mDutyGrid.GetKeyController();
					UnityEngine.Object.Destroy(dutyDetail.gameObject);
					mModalCamera.Close();
					ChangeKeyController(keyController);
				});
			});
			return dutyDetail.Show();
		}

		private KeyControl ShowUIDetailCheck(UIDutySummary summary)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			UIDutyDetailCheck dutyDetail = null;
			dutyDetail = Util.Instantiate(mPrefabDutyDetailCheck.gameObject, mModalCamera.gameObject).GetComponent<UIDutyDetailCheck>();
			dutyDetail.Initialize(summary.GetModel());
			dutyDetail.SetDutyDetailCheckClosedCallBack(delegate
			{
				dutyDetail.Hide(delegate
				{
					_DeteilMode = false;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
					KeyControl keyController = mDutyGrid.GetKeyController();
					UnityEngine.Object.Destroy(dutyDetail.gameObject);
					mModalCamera.Close();
					ChangeKeyController(keyController);
				});
			});
			return dutyDetail.Show();
		}

		private void UIDutySummaryEventCallBack(UIDutySummary.SelectType type, UIDutySummary summary)
		{
			switch (type)
			{
			case UIDutySummary.SelectType.Back:
				summary.Hover();
				ChangeKeyController(mDutyGrid.GetKeyController());
				break;
			case UIDutySummary.SelectType.Action:
			case UIDutySummary.SelectType.Hover:
				mDutyGrid.SetKeyController(null);
				switch (summary.GetModel().State)
				{
				case QuestState.WAITING_START:
					PlaySe(SEFIleInfos.CommonEnter2);
					if (mDutyManager.GetExecutedDutyList().Count < mDutyManager.MaxExecuteCount)
					{
						mModalCamera.Show();
						KeyControl keyController = ShowUIDutyDetail(summary);
						ChangeKeyController(keyController);
					}
					else
					{
						mModalCamera.Show();
						KeyControl keyController = ShowUIDetailCheck(summary);
						ChangeKeyController(keyController);
					}
					break;
				case QuestState.COMPLETE:
				{
					List<DutyModel.InvalidType> invalidTypes = summary.GetModel().GetInvalidTypes();
					if (invalidTypes.Count == 0)
					{
						PlaySe(SEFIleInfos.SE_012);
						mModalCamera.Show();
						IReward[] rewards = mDutyManager.RecieveRewards(summary.GetModel().No).ToArray();
						IEnumerator routine = ReciveReward(rewards);
						StartCoroutine(routine);
						TutorialModel tutorial = mDutyManager.UserInfo.Tutorial;
						if (tutorial.GetStep() == 6 && !tutorial.GetStepTutorialFlg(7))
						{
							tutorial.SetStepTutorialFlg(7);
							CommonPopupDialog.Instance.StartPopup("「任務完了！」 達成");
							SoundUtils.PlaySE(SEFIleInfos.SE_012);
						}
						break;
					}
					switch (invalidTypes[0])
					{
					case DutyModel.InvalidType.LOCK_TARGET_SLOT:
						CommonPopupDialog.Instance.StartPopup("該当装備がロックされています");
						break;
					case DutyModel.InvalidType.MAX_SHIP:
						CommonPopupDialog.Instance.StartPopup("艦が保有上限に達しています");
						break;
					case DutyModel.InvalidType.MAX_SLOT:
						CommonPopupDialog.Instance.StartPopup("装備が保有上限に達しています");
						break;
					}
					ChangeKeyController(mDutyGrid.GetKeyController());
					break;
				}
				case QuestState.RUNNING:
				{
					PlaySe(SEFIleInfos.SE_028);
					mDutyManager.Cancel(summary.GetModel().No);
					UpdateOrderPossibleDutyCount(mDutyManager.MaxExecuteCount - mDutyManager.GetExecutedDutyList().Count, animate: true);
					DutyModel duty = mDutyManager.GetDuty(summary.GetModel().No);
					summary.Initialize(summary.GetIndex(), duty);
					ChangeKeyController(mDutyGrid.GetKeyController());
					break;
				}
				}
				break;
			case UIDutySummary.SelectType.CallDetail:
			{
				_DeteilMode = true;
				mDutyGrid.SetKeyController(null);
				mModalCamera.Show();
				KeyControl keyController = ShowUIDetailCheck(summary);
				ChangeKeyController(keyController);
				break;
			}
			}
		}

		private IEnumerator OnReciveSpoint(Reward_SPoint spoint)
		{
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			new List<IReward>();
			rewardMateralsDialog.Initialize(new IReward[1]
			{
				spoint
			});
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			TutorialModel model = mDutyManager.UserInfo.Tutorial;
			if (SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckFirstTutorial(model, TutorialGuideManager.TutorialID.StrategyPoint))
			{
				rewardDialogKeyController.IsRun = false;
				yield return new WaitForSeconds(1f);
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(model, TutorialGuideManager.TutorialID.StrategyPoint, delegate
				{
                    rewardDialogKeyController.IsRun = true;
				});
			}
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardMaterials(IReward_Material[] materials)
		{
			TrophyUtil.Unlock_Material();
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			List<IReward> rewardMaterials = new List<IReward>();
			foreach (IReward_Material material in materials)
			{
				rewardMaterials.Add(material);
			}
			IReward[] rewards = rewardMaterials.ToArray();
			rewardMateralsDialog.Initialize(rewards);
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardItems(Reward_Useitems useItems)
		{
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			List<Reward_Useitem> rewardItems = new List<Reward_Useitem>();
			IReward_Useitem[] rewards = useItems.Rewards;
			for (int i = 0; i < rewards.Length; i++)
			{
				Reward_Useitem iMaterial = (Reward_Useitem)rewards[i];
				Reward_Useitem rewardItem = iMaterial;
				rewardItems.Add(rewardItem);
			}
			rewardMateralsDialog.Initialize(rewardItems.ToArray());
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardSlotItem(IReward_Slotitem reward_Slotitem)
		{
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			rewardMateralsDialog.Initialize(reward_Slotitem);
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardOpenDeckPractice(Reward_DeckPracitce reward)
		{
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			rewardMateralsDialog.Initialize(reward);
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardTransportCraft(Reward_TransportCraft reward)
		{
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			rewardMateralsDialog.Initialize(reward);
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
                recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardItem(Reward_Useitem reward)
		{
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			rewardMateralsDialog.Initialize(reward);
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardShip(IReward_Ship reward)
		{
			bool recived = false;
			KeyControl rewardKeyController = new KeyControl();
			ProdReceiveShip reciveShipAnimation = ProdReceiveShip.Instantiate(PrefabFile.Load<ProdReceiveShip>(PrefabFileInfos.CommonProdReceiveShip), mModalCamera.gameObject.transform, (Reward_Ship)reward, 1, rewardKeyController, needBGM: false);
			ChangeKeyController(rewardKeyController);
			reciveShipAnimation.Play(delegate
			{
				recived = true;
			});
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardDeck(Reward_Deck reward)
		{
			TrophyUtil.Unlock_DeckNum();
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			rewardMateralsDialog.Initialize(reward);
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardLargeBuild(Reward_LargeBuild reward)
		{
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			rewardMateralsDialog.Initialize(reward);
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardExchangeSlotItem(IReward_Exchange_Slotitem reward)
		{
			bool recived = false;
			Reward_Exchange_Slotitem exchangeSlotItem = (Reward_Exchange_Slotitem)reward;
			KeyControl rewardKeyController = new KeyControl();
			UIDutyRewardExchangeItem uiDutyRewardExchangeItem = Util.Instantiate(mPrefabUIDutyRewardExchangeItem.gameObject, mModalCamera.gameObject).GetComponent<UIDutyRewardExchangeItem>();
			uiDutyRewardExchangeItem.Initialize(exchangeSlotItem, delegate
			{
				recived = true;
			});
			ChangeKeyController(rewardKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator OnReciveRewardFurniture(Reward_Furniture reward)
		{
			bool recived = false;
			UIGetRewardDialog rewardMateralsDialog = Util.Instantiate(mPrefabUIDutyRewardMaterialsDialog.gameObject, mModalCamera.gameObject).GetComponent<UIGetRewardDialog>();
			rewardMateralsDialog.Initialize(reward);
			rewardMateralsDialog.SetOnDialogClosedCallBack(delegate
			{
				recived = true;
				rewardMateralsDialog.Close();
				UnityEngine.Object.Destroy(rewardMateralsDialog.gameObject);
			});
			KeyControl rewardDialogKeyController = rewardMateralsDialog.Show();
			ChangeKeyController(rewardDialogKeyController);
			while (!recived)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}

		private IEnumerator ReciveReward(IReward[] rewards)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			mModalCamera.Show();
			foreach (IReward reward in rewards)
			{
				if (reward is IReward_Materials)
				{
					enumMaterialCategory[] fourMaterialsFilter = new enumMaterialCategory[5]
					{
						enumMaterialCategory.Bauxite,
						enumMaterialCategory.Bull,
						enumMaterialCategory.Fuel,
						enumMaterialCategory.Steel,
						enumMaterialCategory.Bauxite
					};
					enumMaterialCategory[] kitMaterialsFilter = new enumMaterialCategory[5]
					{
						enumMaterialCategory.Build_Kit,
						enumMaterialCategory.Dev_Kit,
						enumMaterialCategory.Repair_Kit,
						enumMaterialCategory.Revamp_Kit,
						enumMaterialCategory.Bauxite
					};
					List<IReward_Material> fourMaterials = new List<IReward_Material>();
					List<IReward_Material> kitMaterials = new List<IReward_Material>();
					IReward_Materials materials = (IReward_Materials)reward;
					IReward_Material[] rewards2 = materials.Rewards;
					foreach (IReward_Material material in rewards2)
					{
						if (fourMaterialsFilter.Contains(material.Type))
						{
							fourMaterials.Add(material);
						}
						else if (kitMaterialsFilter.Contains(material.Type))
						{
							kitMaterials.Add(material);
						}
					}
					if (0 < fourMaterials.Count)
					{
						yield return StartCoroutine(OnReciveRewardMaterials(fourMaterials.ToArray()));
					}
					if (0 < kitMaterials.Count)
					{
						foreach (IReward_Material kitMat in kitMaterials)
						{
							yield return StartCoroutine(OnReciveRewardMaterials(new IReward_Material[1]
							{
								kitMat
							}));
						}
					}
				}
				else if (reward is IReward_Ship)
				{
					yield return StartCoroutine(OnReciveRewardShip((IReward_Ship)reward));
				}
				else if (reward is Reward_Useitems)
				{
					TrophyUtil.Unlock_AlbumSlotNum();
					yield return StartCoroutine(OnReciveRewardItems((Reward_Useitems)reward));
				}
				else if (reward is Reward_Deck)
				{
					yield return StartCoroutine(OnReciveRewardDeck((Reward_Deck)reward));
				}
				else if (reward is Reward_LargeBuild)
				{
					yield return StartCoroutine(OnReciveRewardLargeBuild((Reward_LargeBuild)reward));
				}
				else if (reward is IReward_Exchange_Slotitem)
				{
					yield return StartCoroutine(OnReciveRewardExchangeSlotItem((IReward_Exchange_Slotitem)reward));
				}
				else if (reward is Reward_Furniture)
				{
					yield return StartCoroutine(OnReciveRewardFurniture((Reward_Furniture)reward));
				}
				else if (reward is Reward_SPoint)
				{
					yield return StartCoroutine(OnReciveSpoint((Reward_SPoint)reward));
				}
				else if (reward is Reward_Useitem)
				{
					TrophyUtil.Unlock_AlbumSlotNum();
					yield return StartCoroutine(OnReciveRewardItem((Reward_Useitem)reward));
				}
				else if (reward is IReward_Slotitem)
				{
					TrophyUtil.Unlock_AlbumSlotNum();
					yield return StartCoroutine(OnReciveRewardSlotItem((IReward_Slotitem)reward));
				}
				else if (reward is Reward_DeckPracitce)
				{
					yield return StartCoroutine(OnReciveRewardOpenDeckPractice((Reward_DeckPracitce)reward));
				}
				else if (reward is Reward_TransportCraft)
				{
					yield return StartCoroutine(OnReciveRewardTransportCraft((Reward_TransportCraft)reward));
				}
			}
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mDutyManager);
			DutyModel[] duties = GetDuties();
			mDutyGrid.Initialize(duties);
			mDutyGrid.GoToPage(0);
			mModalCamera.Close();
			ChangeKeyController(mDutyGrid.GetKeyController());
			mDutyGrid.FirstFocus();
			if (duties.Length <= 0)
			{
				mTransform_AllClearText.SetActive(isActive: true);
			}
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			UpdateOrderPossibleDutyCount(mDutyManager.MaxExecuteCount - mDutyManager.GetExecutedDutyList().Count, animate: true);
		}

		private bool CanStartDuty(DutyModel dutyModel)
		{
			if (dutyModel.State == QuestState.RUNNING || dutyModel.State == QuestState.COMPLETE)
			{
				return false;
			}
			int count = mDutyManager.GetRunningDutyList().Count;
			if (count + 1 <= mDutyManager.MaxExecuteCount)
			{
				return true;
			}
			return false;
		}

		private void PlaySe(SEFIleInfos seType)
		{
			if ((SingletonMonoBehaviour<SoundManager>.Instance != null) ? true : false)
			{
				SoundUtils.PlaySE(seType);
			}
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(mLabel_DutyCount))
			{
				DOTween.Kill(mLabel_DutyCount);
			}
			if (DOTween.IsTweening(mTexture_LeftArrow))
			{
				DOTween.Kill(mTexture_LeftArrow);
			}
			if (DOTween.IsTweening(mTexture_RightArrow))
			{
				DOTween.Kill(mTexture_RightArrow);
			}
			if (DOTween.IsTweening(mTexture_LeftArrowShadow))
			{
				DOTween.Kill(mTexture_LeftArrowShadow);
			}
			if (DOTween.IsTweening(mTexture_RightArrowShadow))
			{
				DOTween.Kill(mTexture_RightArrowShadow);
			}
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTextures_Preload);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_DutyCount);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_LeftArrow);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_RightArrow);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_LeftArrowShadow);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_RightArrowShadow);
			mTransform_AllClearText = null;
			mPrefabDutyDetail = null;
			mPrefabDutyDetailCheck = null;
			mPrefabUIDutyRewardMaterialsDialog = null;
			mPrefabRewardShip = null;
			mModalCamera = null;
			mPrefab_DutyOhyodo = null;
			mDutyGrid = null;
			mDutyManager = null;
			mFocusKeyController = null;
		}

		private Tween GenerateTweenArrow(UITexture arrow, Direction direction)
		{
			int num = 0;
			switch (direction)
			{
			case Direction.Forward:
				num = -arrowMove;
				break;
			case Direction.Reverse:
				num = arrowMove;
				break;
			}
			if (DOTween.IsTweening(arrow))
			{
				DOTween.Kill(arrow);
			}
			Sequence sequence = DOTween.Sequence().SetId(arrow);
			Transform transform = arrow.transform;
			Vector3 localPosition = arrow.transform.localPosition;
			Tween t = transform.DOLocalMoveX(localPosition.x + (float)num, arrowDuration);
			Tween t2 = DOVirtual.Float(arrow.alpha, 0f, arrowAlpha, delegate(float alpha)
			{
				arrow.alpha = alpha;
			});
			sequence.Append(t);
			sequence.Join(t2);
			return sequence;
		}
	}
}
