using System;
using Common.Enum;
using DG.Tweening;
using KCV.Dialog;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	public class UserInterfaceRevampManager : MonoBehaviour
	{
		public class RevampContext
		{
			public class SlotItemInfo
			{
				public int MstId
				{
					get;
					private set;
				}

				public int MemId
				{
					get;
					private set;
				}

				public int Level
				{
					get;
					private set;
				}

				public string Name
				{
					get;
					private set;
				}

				public SlotItemInfo(SlotitemModel slotItemModel)
				{
					if (slotItemModel != null)
					{
						MstId = slotItemModel.MstId;
						Level = slotItemModel.Level;
						MemId = slotItemModel.MemId;
						Name = slotItemModel.Name;
					}
					else
					{
						MstId = -1;
						Level = -1;
					}
				}
			}

			public const int UNSET = -1;

			private SlotItemInfo mBeforeSlotItemInfo;

			private SlotItemInfo mAfterSlotItemInfo;

			private RevampRecipeModel mRevampRecipe;

			private ShipModel mConsortShip;

			private bool mIsDetermined;

			public RevampRecipeModel RevampRecipe => mRevampRecipe;

			public ShipModel ConsortShip => mConsortShip;

			public bool IsDetermined => mIsDetermined;

			public bool Success
			{
				get;
				private set;
			}

			public void SetBeforeSlotItemInfo(SlotitemModel model)
			{
				mBeforeSlotItemInfo = new SlotItemInfo(model);
			}

			public void SetAfterSlotItemInfo(SlotitemModel model)
			{
				mAfterSlotItemInfo = new SlotItemInfo(model);
			}

			public void SetRevampRecipe(RevampRecipeModel revampRecipeModel)
			{
				mRevampRecipe = revampRecipeModel;
			}

			public void SetDetermined(bool isDetermined)
			{
				mIsDetermined = isDetermined;
			}

			public void SetConsortShip(ShipModel consortShipModel)
			{
				mConsortShip = consortShipModel;
			}

			public void SetSuccess(bool success)
			{
				Success = success;
			}

			public SlotItemInfo GetAfterSlotItemInfo()
			{
				return mAfterSlotItemInfo;
			}

			public bool IsModelChange()
			{
				return mBeforeSlotItemInfo.MstId != mAfterSlotItemInfo.MstId;
			}

			public SlotItemInfo GetBeforeSlotItemInfo()
			{
				return mBeforeSlotItemInfo;
			}
		}

		public class LocalUtils
		{
			public static string GenerateRevampSettingMessage(RevampValidationResult iResult, RevampRecipeDetailModel model)
			{
				if (model == null)
				{
					return null;
				}
				string str = "[000000]";
				switch (iResult)
				{
				case RevampValidationResult.Max_Level:
					str += $"[FF0000]現在、選択された装備[-]\n";
					str += $"[329ad6]{model.Slotitem.Name}[-]\n";
					str += $"[FF0000]は、これ以上の改修ができません。[-]";
					break;
				case RevampValidationResult.Lock:
					str += $"[FF0000]この装備を改修するには\n\u3000同装備のロック解除が必要です。[-]";
					break;
				case RevampValidationResult.Less_Slotitem_No_Lock:
					str += $"[FF0000]この改修に必要となる\n(無改修)\n[-]";
					str = ((0 >= model.RequiredSlotitemId) ? (str + $"[329ad6]{model.Slotitem.Name}x{model.RequiredSlotitemCount}[-]") : (str + $"[329ad6]{new SlotitemModel_Mst(model.RequiredSlotitemId).Name}x{model.RequiredSlotitemCount}[-]"));
					str += $"[FF0000]が足りません。[-]";
					break;
				case RevampValidationResult.Less_Slotitem:
					str += "[FF0000]この改修に必要となる\n(無改修)\n[-]";
					str = ((0 >= model.RequiredSlotitemId) ? (str + $"[329ad6]{model.Slotitem.Name}×{model.RequiredSlotitemCount}[-]") : (str + $"[329ad6]{new SlotitemModel_Mst(model.RequiredSlotitemId).Name}×{model.RequiredSlotitemCount}[-]"));
					str += "\n";
					str += "[FF0000]が足りません。[-]";
					break;
				case RevampValidationResult.OK:
				{
					if (model.RequiredSlotitemId == 0)
					{
						str += $"[329ad6]{model.Slotitem.Name}[-]\n";
						str += $"を改修しますね！[-]";
						break;
					}
					SlotitemModel_Mst slotitemModel_Mst = new SlotitemModel_Mst(model.RequiredSlotitemId);
					str += $"[329ad6]{model.Slotitem.Name}[-]\n";
					str += $"を改修しますね！[-]";
					if (0 < model.RequiredSlotitemCount)
					{
						str += "\n";
						str += "[000000]この改修には、無改修の\n";
						str += $"[329ad6]{slotitemModel_Mst.Name}×{model.RequiredSlotitemCount}[-]";
						str += "\n\nが必要です。[-]";
						str += "\n[666666](※改修で消費します)[-]";
					}
					break;
				}
				case RevampValidationResult.Less_Fuel:
				case RevampValidationResult.Less_Ammo:
				case RevampValidationResult.Less_Steel:
				case RevampValidationResult.Less_Baux:
				case RevampValidationResult.Less_Devkit:
				case RevampValidationResult.Less_Revkit:
					str += $"[FF0000]\u3000資材が足りません。";
					break;
				}
				return str + "[-]";
			}
		}

		private const int MASTER_ID_AKASHI = 182;

		private const int MASTER_ID_AKASHI_KAI = 187;

		private const int NON_ASSISTANT_SHIP = 0;

		private RevampManager mRevampManager;

		private RevampContext mRevampContext;

		[SerializeField]
		private Camera mCameraTouchEventCatch;

		[SerializeField]
		private Camera mCameraProduction;

		[SerializeField]
		private UIRevampSlotItemScrollListParentNew mUIRevampSlotItemScrollListParentNew;

		[SerializeField]
		private UIRevampRecipeScrollParentNew mRevampRecipeScrollParentNew;

		[SerializeField]
		private UIRevampSetting mPrefab_RevampSetting;

		[SerializeField]
		private UIRevampIcon mPrefab_RevampIcon;

		[SerializeField]
		private UIRevampMaterialsInfo mRevampMaterialsInfo;

		[SerializeField]
		private UIRevampBalloon mRevampInfoBalloon;

		[SerializeField]
		private UIRevampAkashi mRevampAkashi;

		[SerializeField]
		private ModalCamera mModalCamera;

		[SerializeField]
		private UITexture mTexture_AssistantShip;

		[SerializeField]
		private ParticleSystem mParticleSystem_SuccessStars;

		[SerializeField]
		private Transform mTransform_AssistantShipParent;

		[SerializeField]
		private Vector3 mVector3_AssistantShipShowLocalPosition;

		[SerializeField]
		private Vector3 mVector3_AssistantShipHideLocalPosition;

		private KeyControl mFocusKeyController;

		private int mDeckId;

		private int mAreaId;

		private UIButton _uiOverlayButton2;

		public bool _isTop;

		public bool _isAnimation;

		public bool _isSettingMode;

		private AudioClip mAudioClip_BGM;

		private AudioClip mAudioClip_SE_020;

		private AudioClip mAudioClip_SE_022;

		private AudioClip mAudioClip_SE_017;

		private AudioClip mAudioClip_SE_002;

		private AudioClip mAudioClip_SE_023;

		private AudioClip mAudioClip_SE_021;

		private AudioClip mAudioClip_303;

		private AudioClip mAudioClip_304;

		private AudioClip mAudioClip_305;

		private AudioClip mAudioClip_308;

		private AudioClip mAudioClip_309;

		private AudioClip mAudioClip_313;

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			_isTop = true;
			_isAnimation = false;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			mAreaId = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			mDeckId = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			mRevampManager = new RevampManager(mAreaId, mDeckId);
			mRevampManager.GetRecipes();
			InitializeAkashi();
			mRevampMaterialsInfo.Initialize(mRevampManager);
			mAudioClip_BGM = SoundFile.LoadBGM((BGMFileInfos)206);
			mAudioClip_303 = SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, 303);
			mAudioClip_304 = SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, 304);
			mAudioClip_305 = SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, 305);
			mAudioClip_308 = SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, 308);
			mAudioClip_309 = SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, 309);
			mAudioClip_313 = SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, 313);
			mAudioClip_SE_021 = SoundFile.LoadSE(SEFIleInfos.SE_021);
			mAudioClip_SE_022 = SoundFile.LoadSE(SEFIleInfos.SE_022);
			mAudioClip_SE_017 = SoundFile.LoadSE(SEFIleInfos.SE_017);
			mAudioClip_SE_020 = SoundFile.LoadSE(SEFIleInfos.SE_020);
			mAudioClip_SE_002 = SoundFile.LoadSE(SEFIleInfos.SE_002);
			mAudioClip_SE_023 = SoundFile.LoadSE(SEFIleInfos.SE_023);
			int random = UnityEngine.Random.Range(0, 100);
			if (70 < random)
			{
				SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, 301);
			}
			else
			{
				SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(0, 302);
			}
			SingletonMonoBehaviour<PortObjectManager>.Instance.OverwriteSceneObject(base.transform.gameObject);
			stopWatch.Stop();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			KeyControl nextKeyController = ShowUIRevampRecipeList(0);
			SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
			{
				SingletonMonoBehaviour<SoundManager>.Instance.SwitchBGM(this.mAudioClip_BGM);
                this.PlayAkashiVoice(mAudioClip_303);
			});
			ChangeFocusKeyController(nextKeyController);
			mRevampMaterialsInfo.Show();
			UpdateInfo(mRevampManager);
		}

		private void InitializeAkashi()
		{
			if (mRevampManager.Deck.GetFlagShip().MstId == 187)
			{
				mRevampAkashi.Initialize(UIRevampAkashi.CharacterType.AkashiKai);
			}
			else
			{
				mRevampAkashi.Initialize(UIRevampAkashi.CharacterType.Akashi);
			}
			string message = mRevampInfoBalloon.GetMessageBuilder().AddMessage("提督、明石の工廠へようこそ！", lineBreak: true).AddMessage("どの装備の改修を試みますか？")
				.Build();
			mRevampInfoBalloon.SayMessage(message);
		}

		private void Update()
		{
			if (mFocusKeyController != null)
			{
				mFocusKeyController.Update();
				if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable && mFocusKeyController.IsRDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
					mFocusKeyController = null;
				}
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mParticleSystem_SuccessStars);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_BGM);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE_020);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE_022);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE_017);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE_002);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE_023);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE_021);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_303);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_304);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_305);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_308);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_309);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_313);
			mRevampManager = null;
			mRevampContext = null;
			mCameraTouchEventCatch = null;
			mCameraProduction = null;
			mUIRevampSlotItemScrollListParentNew = null;
			mRevampRecipeScrollParentNew = null;
			mPrefab_RevampSetting = null;
			mPrefab_RevampIcon = null;
			mRevampMaterialsInfo = null;
			mRevampInfoBalloon = null;
			mRevampAkashi = null;
			mModalCamera = null;
			mTexture_AssistantShip = null;
			mTransform_AssistantShipParent = null;
			mFocusKeyController = null;
		}

		private KeyControl ShowUIRevampRecipeList(int firstFocusIndex)
		{
			mRevampRecipeScrollParentNew.SetActive(isActive: false);
			mRevampRecipeScrollParentNew.SetActive(isActive: true);
			_isTop = true;
			mRevampManager.GetRecipes();
			mRevampContext = new RevampContext();
			mRevampRecipeScrollParentNew.Initialize(mRevampManager);
			mRevampRecipeScrollParentNew.SetOnSelectedListener(OnSelectedRecipeListener);
			mRevampRecipeScrollParentNew.SetCamera(mCameraTouchEventCatch);
			KeyControl keyController = mRevampRecipeScrollParentNew.GetKeyController();
			mRevampRecipeScrollParentNew.PlaySlotInAnimation();
			mRevampRecipeScrollParentNew.SetOnFinishedSlotInAnimationListener(delegate
			{
				mRevampRecipeScrollParentNew.StartControl();
			});
			return keyController;
		}

		private void OnSelectedRecipeListener(UIRevampRecipeScrollChildNew child)
		{
			mRevampRecipeScrollParentNew.LockControl();
			int num = UnityEngine.Random.Range(0, 100);
			if (30 < num)
			{
				PlayAkashiVoice(mAudioClip_303);
			}
			else
			{
				PlayAkashiVoice(mAudioClip_304);
			}
			RevampRecipeModel model = child.GetModel().Model;
			mRevampContext.SetRevampRecipe(model);
			SoundUtils.PlaySE(mAudioClip_SE_002);
			mRevampRecipeScrollParentNew.SetActive(isActive: false);
			KeyControl keyController = ShowUIRevampSlotItemGrid(mRevampContext);
			ChangeFocusKeyController(keyController);
		}

		private void PlayAkashiVoice(AudioClip audioClip)
		{
			SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(audioClip);
		}

		private KeyControl ShowUIRevampSlotItemGrid(RevampContext revampContext)
		{
			_isTop = false;
			SlotitemModel[] slotitemList = mRevampManager.GetSlotitemList(mRevampContext.RevampRecipe.RecipeId);
			ChangeFocusKeyController(null);
			mUIRevampSlotItemScrollListParentNew.SetActive(isActive: false);
			mUIRevampSlotItemScrollListParentNew.SetActive(isActive: true);
			_uiOverlayButton2 = mUIRevampSlotItemScrollListParentNew.GetOverlayBtn2();
			EventDelegate.Add(_uiOverlayButton2.onClick, _onClickOverlayButton2);
			mRevampInfoBalloon.alpha = 1E-10f;
			mUIRevampSlotItemScrollListParentNew.Initialize(slotitemList);
			mUIRevampSlotItemScrollListParentNew.SetCamera(mCameraTouchEventCatch);
			mUIRevampSlotItemScrollListParentNew.SetOnSelectedSlotItemListener(OnSelectedSlotItemListener);
			mUIRevampSlotItemScrollListParentNew.SetOnBackListener(OnBackSlotItemList);
			mUIRevampSlotItemScrollListParentNew.StartControl();
			return mUIRevampSlotItemScrollListParentNew.GetKeyController();
		}

		private void OnBackSlotItemList()
		{
			mRevampManager.GetRecipes();
			SoundUtils.PlaySE(mAudioClip_SE_017);
			mRevampInfoBalloon.alpha = 1f;
			mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
			KeyControl keyController = ShowUIRevampRecipeList(0);
			ChangeFocusKeyController(keyController);
			mUIRevampSlotItemScrollListParentNew.SetActive(isActive: false);
		}

		private void _onClickOverlayButton2()
		{
			_isTop = false;
			RevampRecipeModel[] recipes = mRevampManager.GetRecipes();
			int num = 0;
			mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
			KeyControl keyController = (num > recipes.Length) ? ShowUIRevampRecipeList(0) : ShowUIRevampRecipeList(num);
			ChangeFocusKeyController(keyController);
			mUIRevampSlotItemScrollListParentNew.SetActive(isActive: false);
		}

		private void OnSelectedSlotItemListener(UIRevampSlotItemScrollListChildNew selectedSlotItemView)
		{
			mRevampInfoBalloon.alpha = 1f;
			mRevampContext.SetBeforeSlotItemInfo(selectedSlotItemView.GetModel());
			mRevampManager.GetDetail(mRevampContext.RevampRecipe.RecipeId, mRevampContext.GetBeforeSlotItemInfo().MemId);
			UIRevampSetting revampSetting = ShowUIRevampSetting(mRevampContext);
			int num = UnityEngine.Random.Range(0, 100);
			if (40 < num)
			{
				PlayAkashiVoice(mAudioClip_305);
			}
			else
			{
				PlayAkashiVoice(mAudioClip_313);
			}
			mUIRevampSlotItemScrollListParentNew.SetActive(isActive: false);
			revampSetting.Show(delegate
			{
				_isSettingMode = true;
				KeyControl keyController = revampSetting.GetKeyController();
				ChangeFocusKeyController(keyController);
			});
		}

		private UIRevampSetting ShowUIRevampSetting(RevampContext revampContext)
		{
			RevampRecipeDetailModel detail = mRevampManager.GetDetail(revampContext.RevampRecipe.RecipeId, revampContext.GetBeforeSlotItemInfo().MemId);
			mRevampManager.IsValidRevamp(detail);
			UIRevampSetting component = Util.Instantiate(mPrefab_RevampSetting.gameObject, base.gameObject).GetComponent<UIRevampSetting>();
			component.SetOnRevampSettingActionCallBack(UIRevampSettingActionCallBack);
			component.Initialize(detail, UIRevampRecipeSettingCheckDelegate, mCameraProduction);
			return component;
		}

		private RevampValidationResult UIRevampRecipeSettingCheckDelegate(RevampRecipeDetailModel targetModel)
		{
			RevampValidationResult revampValidationResult = mRevampManager.IsValidRevamp(targetModel);
			mRevampInfoBalloon.SayMessage(LocalUtils.GenerateRevampSettingMessage(revampValidationResult, targetModel));
			return revampValidationResult;
		}

		private void UIRevampSettingActionCallBack(UIRevampSetting.ActionType actionType, UIRevampSetting calledObject)
		{
			_isSettingMode = false;
			switch (actionType)
			{
			case UIRevampSetting.ActionType.CancelRevamp:
				OnCanelRevampSetting(calledObject);
				break;
			case UIRevampSetting.ActionType.StartRevamp:
				OnStartRevamp(calledObject);
				break;
			}
		}

		private void OnStartRevamp(UIRevampSetting calledObject)
		{
			StartCoroutine(OnStartRevampCoroutine(calledObject));
		}

		private IEnumerator OnStartRevampCoroutine(UIRevampSetting calledObject)
		{
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = false;
			}
			_isAnimation = true;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			SoundUtils.PlaySE(mAudioClip_SE_020);
			RevampRecipeDetailModel revampRecipeDetail = mRevampManager.GetDetail(mRevampContext.RevampRecipe.RecipeId, mRevampContext.GetBeforeSlotItemInfo().MemId);
			revampRecipeDetail.Determined = calledObject.IsDetermined();
			SlotitemModel revampedSlotItemModel = mRevampManager.Revamp(revampRecipeDetail);
			List<int> ids = new List<int>();
			ids.AddRange(TrophyUtil.Unlock_At_Revamp());
			ids.AddRange(TrophyUtil.Unlock_AlbumSlotNum());
			SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(ids);
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(mRevampManager);
			}
			if (revampedSlotItemModel != null)
			{
				mRevampContext.SetSuccess(success: true);
				mRevampContext.SetAfterSlotItemInfo(revampedSlotItemModel);
			}
			else
			{
				mRevampContext.SetSuccess(success: false);
			}
			UpdateInfo(mRevampManager);
			int consortShipResourceId = -1;
			int consortShipVoiceId = -1;
			ShipModel consortShipModel = mRevampManager.GetConsortShip(revampRecipeDetail, out consortShipResourceId, out consortShipVoiceId);
			if (consortShipModel != null)
			{
				int mstId = mRevampManager.UserInfo.GetDeck(mDeckId).GetShip(1).MstId;
				mTexture_AssistantShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(consortShipModel.GetGraphicsMstId(), (!consortShipModel.IsDamaged()) ? 9 : 10);
				mTexture_AssistantShip.MakePixelPerfect();
			}
			yield return new WaitForEndOfFrame();
			calledObject.Hide(delegate
			{
                throw new NotImplementedException("なにこれ");
                //if (base._003CexistConsortShip_003E__6)
				//{
				//	this.mTransform_AssistantShipParent.DOLocalMove(this.mVector3_AssistantShipShowLocalPosition, 0.3f).OnComplete(delegate
				//	{
				//		ShipUtils.PlayShipVoice(consortShipModel, consortShipVoiceId);
				//	});
				//}

				this.ChangeFocusKeyController(null);
                UnityEngine.Object.Destroy(calledObject.gameObject);
				this.mRevampAkashi.ChangeBodyTo(UIRevampAkashi.BodyType.Making);
				this.mRevampInfoBalloon.SayMessage("[000000]改修中・・・[-]");
				this.OnStartRevampAnimation(this.mRevampContext);
			});
		}

		private void OnStartRevampAnimation(RevampContext mRevampContext)
		{
			if (mRevampContext.Success)
			{
				OnStartSuccessRevampAnimation(mRevampContext);
			}
			else
			{
				OnStartFailRevampAnimation(mRevampContext);
			}
		}

		private void OnStartSuccessRevampAnimation(RevampContext context)
		{
			_isTop = true;
			UIRevampIcon revampIcon = Util.Instantiate(mPrefab_RevampIcon.gameObject, base.gameObject).GetComponent<UIRevampIcon>();
			revampIcon.Initialize(context.GetBeforeSlotItemInfo().MstId, context.GetBeforeSlotItemInfo().Level, mCameraProduction);
			revampIcon.StartRevamp(context.GetAfterSlotItemInfo().MstId, context.GetAfterSlotItemInfo().Level, context.GetAfterSlotItemInfo().Name, delegate
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				mRevampAkashi.ChangeBodyTo(UIRevampAkashi.BodyType.Normal);
				_isAnimation = false;
				string empty = string.Empty;
				PlayAkashiVoice(mAudioClip_308);
				empty += "[000000]改修成功しました。";
				empty += "\n";
				empty += $"[329ad6]{mRevampContext.GetAfterSlotItemInfo().Name}[-]";
				mParticleSystem_SuccessStars.Play(false);
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
				}
				TrophyUtil.Unlock_AlbumSlotNum();
				if (mRevampContext.IsModelChange())
				{
					SoundUtils.PlaySE(mAudioClip_SE_023);
				}
				else
				{
					SoundUtils.PlaySE(mAudioClip_SE_021);
				}
				KeyControl keyController = mRevampInfoBalloon.SayMessage(empty, delegate
				{
					if (mRevampContext.ConsortShip != null)
					{
						mTransform_AssistantShipParent.DOLocalMove(mVector3_AssistantShipHideLocalPosition, 0.6f);
					}
					mRevampInfoBalloon.alpha = 1f;
                    UnityEngine.Object.Destroy(revampIcon.gameObject);
					mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
					mRevampManager.GetDetail(mRevampContext.RevampRecipe.RecipeId, mRevampContext.GetBeforeSlotItemInfo().MemId);
					RevampRecipeModel[] recipes = mRevampManager.GetRecipes();
					int num = 0;
					RevampRecipeModel[] array = recipes;
					foreach (RevampRecipeModel revampRecipeModel in array)
					{
						if (revampRecipeModel.RecipeId == mRevampContext.RevampRecipe.RecipeId)
						{
							break;
						}
						num++;
					}
					KeyControl keyController2 = (num > recipes.Length) ? ShowUIRevampRecipeList(0) : ShowUIRevampRecipeList(num);
					ChangeFocusKeyController(keyController2);
				});
				ChangeFocusKeyController(keyController);
				mTransform_AssistantShipParent.DOLocalMove(mVector3_AssistantShipHideLocalPosition, 0.6f);
			});
		}

		private void OnStartFailRevampAnimation(RevampContext context)
		{
			_isTop = true;
			UIRevampIcon revampIcon = Util.Instantiate(mPrefab_RevampIcon.gameObject, base.gameObject).GetComponent<UIRevampIcon>();
			revampIcon.Initialize(context.GetBeforeSlotItemInfo().MstId, context.GetBeforeSlotItemInfo().Level, mCameraProduction);
			revampIcon.StartRevamp(context.GetBeforeSlotItemInfo().MstId, context.GetBeforeSlotItemInfo().Level, context.GetBeforeSlotItemInfo().Name, delegate
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				mRevampAkashi.ChangeBodyTo(UIRevampAkashi.BodyType.Normal);
				string empty = string.Empty;
				_isAnimation = false;
				PlayAkashiVoice(mAudioClip_309);
				SoundUtils.PlaySE(mAudioClip_SE_022);
				empty += "[000000]改修失敗しました。";
				if (SingletonMonoBehaviour<UIPortFrame>.exist())
				{
					SingletonMonoBehaviour<UIPortFrame>.Instance.isColliderEnabled = true;
				}
				KeyControl keyController = mRevampInfoBalloon.SayMessage(empty, delegate
				{
					if (mRevampContext.ConsortShip != null)
					{
						mTransform_AssistantShipParent.DOLocalMove(mVector3_AssistantShipHideLocalPosition, 0.6f);
					}
                    UnityEngine.Object.Destroy(revampIcon.gameObject);
					mRevampInfoBalloon.alpha = 1f;
					mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
					mRevampManager.GetDetail(mRevampContext.RevampRecipe.RecipeId, mRevampContext.GetBeforeSlotItemInfo().MemId);
					RevampRecipeModel[] recipes = mRevampManager.GetRecipes();
					int num = 0;
					RevampRecipeModel[] array = recipes;
					foreach (RevampRecipeModel revampRecipeModel in array)
					{
						if (revampRecipeModel.RecipeId == mRevampContext.RevampRecipe.RecipeId)
						{
							break;
						}
						num++;
					}
					KeyControl keyController2 = (num > recipes.Length) ? ShowUIRevampRecipeList(0) : ShowUIRevampRecipeList(num);
					ChangeFocusKeyController(keyController2);
				});
				ChangeFocusKeyController(keyController);
				mTransform_AssistantShipParent.DOLocalMove(mVector3_AssistantShipHideLocalPosition, 0.6f);
			});
		}

		private void OnCanelRevampSetting(UIRevampSetting calledObject)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			SoundUtils.PlaySE(mAudioClip_SE_017);
			KeyControl nextFocusKeyController = null;
			calledObject.Hide(delegate
			{
				RevampRecipeModel[] recipes = mRevampManager.GetRecipes();
				RevampRecipeDetailModel detail = mRevampManager.GetDetail(mRevampContext.RevampRecipe.RecipeId, mRevampContext.GetBeforeSlotItemInfo().MemId);
				int num = 0;
				RevampRecipeModel[] array = recipes;
				foreach (RevampRecipeModel revampRecipeModel in array)
				{
					if (revampRecipeModel.RecipeId == detail.RecipeId)
					{
						break;
					}
					num++;
				}
				mRevampInfoBalloon.alpha = 1f;
				mRevampInfoBalloon.SayMessage("[000000]どの装備の改修を試みますか？");
				if (num <= recipes.Length)
				{
					nextFocusKeyController = ShowUIRevampRecipeList(num);
				}
				else
				{
					nextFocusKeyController = ShowUIRevampRecipeList(0);
				}
				ChangeFocusKeyController(nextFocusKeyController);
                UnityEngine.Object.Destroy(calledObject.gameObject);
			});
		}

		private void ChangeFocusKeyController(KeyControl keyController)
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

		private void UpdateInfo(ManagerBase manager)
		{
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(manager);
			}
			mRevampMaterialsInfo.UpdateInfo(manager);
		}
	}
}
