using DG.Tweening;
using KCV.Scene.Mission;
using KCV.Scene.Port;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Strategy
{
	[RequireComponent(typeof(UIPanel))]
	public class UserInterfaceStrategyResult : MonoBehaviour
	{
		private UIPanel mPanelThis;

		[SerializeField]
		private UILabel mLabel_MissionName;

		[SerializeField]
		private UILabel mLabel_AdmiralName;

		[SerializeField]
		private UILabel mLabel_AdmiralLevel;

		[SerializeField]
		private UILabel mLabel_DeckName;

		[SerializeField]
		private UIMissionResultBonus mMissionResultBonus;

		[SerializeField]
		private UIMissionResultStatus mMissionResultStatus;

		[SerializeField]
		private UIMissionJudgeCutIn mMissionJudgeCutIn;

		[SerializeField]
		private UITexture mTexture_FlagShipGraphic;

		[SerializeField]
		private Transform mTransform_TouchControlArea;

		private StrategyMapManager mStrategyMapManager;

		private KeyControl mKeyController;

		private MissionResultModel mMissionResultModel;

		private Action mOnSelectNextAction;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				mKeyController.Update();
			}
		}

		public void Initialize(StrategyMapManager manager, MissionResultModel missionResultModel, KeyControl keyController, Action onSelectNextAction)
		{
			mMissionResultModel = missionResultModel;
			mTransform_TouchControlArea.SetActive(isActive: false);
			mStrategyMapManager = manager;
			mMissionJudgeCutIn.Initialize(mMissionResultModel.result);
			mMissionResultBonus.Inititalize(mMissionResultModel);
			mMissionResultStatus.Inititalize(mMissionResultModel);
			ShipModel shipModel = mMissionResultModel.Ships[0];
			mTexture_FlagShipGraphic.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.GetGraphicsMstId(), (!shipModel.IsDamaged()) ? 9 : 10);
			mTexture_FlagShipGraphic.MakePixelPerfect();
			mTexture_FlagShipGraphic.transform.localPosition = Util.Poi2Vec(shipModel.Offsets.GetShipDisplayCenter(shipModel.IsDamaged()));
			mLabel_AdmiralLevel.text = mStrategyMapManager.UserInfo.Level.ToString();
			mLabel_AdmiralName.text = mStrategyMapManager.UserInfo.Name;
			mLabel_AdmiralName.supportEncoding = false;
			mLabel_DeckName.text = mStrategyMapManager.UserInfo.GetDeck(mMissionResultModel.DeckID).Name;
			mLabel_DeckName.supportEncoding = false;
			mLabel_MissionName.text = mMissionResultModel.MissionName;
			mKeyController = keyController;
			mOnSelectNextAction = onSelectNextAction;
		}

		private void Start()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		}

		public void Play()
		{
			mMissionResultStatus.PlayShowBanners(delegate
			{
				mMissionJudgeCutIn.SetOnFinishedAnimationListener(delegate
				{
					TrophyUtil.Unlock_UserLevel();
					TrophyUtil.Unlock_Material();
					mKeyController.IsRun = true;
					StartCoroutine(WaitForKeyOrTouch(mKeyController, delegate
					{
						mKeyController.ClearKeyAll();
						mMissionJudgeCutIn.SetActive(isActive: false);
						mMissionResultStatus.PlayShowBannersExp(null);
						mMissionResultBonus.Play(delegate
						{
							mTransform_TouchControlArea.SetActive(isActive: true);
							mKeyController.IsRun = true;
							StartCoroutine(WaitForKeyOrTouch(mKeyController, delegate
							{
								mKeyController.IsRun = true;
								if (mOnSelectNextAction != null)
								{
									mOnSelectNextAction();
								}
							}));
						});
					}));
				});
				mMissionJudgeCutIn.Play();
			});
		}

		private IEnumerator WaitForKeyOrTouch(KeyControl keyController, Action onKeyAction)
		{
			mTransform_TouchControlArea.SetActive(isActive: true);
			while (keyController != null && !keyController.keyState[1].down && mTransform_TouchControlArea.gameObject.activeSelf)
			{
				yield return null;
			}
			mTransform_TouchControlArea.SetActive(isActive: false);
			onKeyAction?.Invoke();
		}

		[Obsolete("インスペクタ上のUIButtonに紐付けて使用します。")]
		public void OnTouchNext()
		{
			OnCallNext();
		}

		private void OnCallNext()
		{
			if (mKeyController != null && mKeyController.IsRun)
			{
				mTransform_TouchControlArea.SetActive(isActive: false);
				mKeyController.IsRun = false;
			}
		}

		public void FadeOut(Action onFinished)
		{
			DOVirtual.Float(mPanelThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				mPanelThis.alpha = alpha;
			}).OnComplete(delegate
			{
				if (onFinished != null)
				{
					onFinished();
				}
			});
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mPanelThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_MissionName);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_AdmiralName);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_AdmiralLevel);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_DeckName);
			if (mMissionResultModel.Ships[0].IsDamaged())
			{
				UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_FlagShipGraphic);
			}
			else
			{
				UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_FlagShipGraphic, unloadUnUsedAsset: true);
			}
			mMissionResultModel = null;
			mMissionResultBonus = null;
			mMissionResultStatus = null;
			mMissionJudgeCutIn = null;
			mTransform_TouchControlArea = null;
			mStrategyMapManager = null;
			mKeyController = null;
		}
	}
}
