using Common.Struct;
using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIPanel))]
	public class UIDeckPracticeProductionShipParameterResult : MonoBehaviour
	{
		[SerializeField]
		private UIDeckPracticeUpParameter[] mUIDeckPracticeUpParameters;

		[SerializeField]
		private UIDeckPracticeShipInfo mUIDeckPracticeShipInfo;

		[SerializeField]
		private Transform mTransfrom_ShipOffset;

		[SerializeField]
		private Transform mTransform_TouchControlArea;

		[SerializeField]
		private UITexture mTexture_Ship;

		[SerializeField]
		private UITexture mTexture_Bg;

		private UIPanel mPanelThis;

		private Vector3 mVector3_ShipDefaultLocalPosition;

		private KeyControl mKeyController;

		private DeckPracticeResultModel mDeckPracticeResultModel;

		private Action mOnFinishedProductionListener;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mVector3_ShipDefaultLocalPosition = mTransfrom_ShipOffset.localPosition;
			mTexture_Bg.alpha = 0f;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void SetOnProductionFinishedListener(Action onFinishedProductionListener)
		{
			mOnFinishedProductionListener = onFinishedProductionListener;
		}

		private void OnProductionFinished()
		{
			if (mOnFinishedProductionListener != null)
			{
				mOnFinishedProductionListener();
			}
		}

		public void Initialize(DeckPracticeResultModel deckPracticeResultModel)
		{
			mDeckPracticeResultModel = deckPracticeResultModel;
		}

		public void SetBackGroundAlpha(float alpha)
		{
			mTexture_Bg.alpha = alpha;
		}

		public void StartProduction()
		{
			mPanelThis.alpha = 1f;
			IEnumerator routine = StartProductionCoroutine(mDeckPracticeResultModel, delegate
			{
				OnProductionFinished();
			});
			StartCoroutine(routine);
		}

		private IEnumerator TEST_StartProductionCoroutine(object o, Action onFinished)
		{
			ShipModel[] shipModels = new OrganizeManager(1).GetShipList();
			ShipModel[] array = shipModels;
			foreach (ShipModel ship in array)
			{
				ShipExpModel shipExpInfo = new ShipExpModel(1, ship, 1, new List<int>
				{
					1,
					1,
					1,
					1,
					11,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					11,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					1,
					11
				});
				IEnumerator startProductionShipResult = StartProductionShipResult(ship, shipExpInfo, default(PowUpInfo));
				yield return StartCoroutine(startProductionShipResult);
			}
			if (onFinished != null)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				onFinished();
			}
		}

		private IEnumerator StartProductionCoroutine(DeckPracticeResultModel deckPracticeResultModel, Action onFinished)
		{
			TrophyUtil.Unlock_UserLevel();
			ShipModel[] ships = deckPracticeResultModel.Ships;
			foreach (ShipModel ship in ships)
			{
				ShipExpModel shipExpInfo = deckPracticeResultModel.GetShipExpInfo(ship.MemId);
				PowUpInfo powUpInfo = deckPracticeResultModel.GetShipPowupInfo(ship.MemId);
				IEnumerator startProductionShipResult = StartProductionShipResult(ship, shipExpInfo, powUpInfo);
				yield return StartCoroutine(startProductionShipResult);
			}
			if (onFinished != null)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				onFinished();
			}
		}

		private IEnumerator StartProductionShipResult(ShipModel shipModel, ShipExpModel expModel, PowUpInfo powUpInfo)
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			bool animationNow = true;
			yield return StartCoroutine(InitializeCoroutine(shipModel, powUpInfo));
			mUIDeckPracticeShipInfo.Initialize(shipModel, expModel);
			WaitForKeyOrTouch(mKeyController, delegate
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				DOTween.Kill(this, complete: true);
			});
			Sequence seq = DOTween.Sequence().SetId(this);
			seq.Append(GenerateTweenShipIn());
			seq.Join(mUIDeckPracticeShipInfo.GenerateTweenExpAndLevel());
			seq.Join(GenerateTweenParameterResult());
			ShipUtils.PlayShipVoice(shipModel, local.utils.Utils.GetSpecialVoiceId(shipModel.MstId));
			yield return seq.WaitForStart();
			yield return StartCoroutine(WaitForKeyOrTouch(mKeyController, delegate
			{
                animationNow = false;
			}));
			while (animationNow)
			{
				yield return null;
			}
			yield return null;
		}

		private IEnumerator InitializeCoroutine(ShipModel shipModel, PowUpInfo powUpInfo)
		{
			Texture prevTexture = mTexture_Ship.mainTexture;
			bool aleadyKaryokuMax = shipModel.IsMaxKaryoku() && powUpInfo.Karyoku == 0;
			mUIDeckPracticeUpParameters[0].Initialize(shipModel.Karyoku - powUpInfo.Karyoku, shipModel.Karyoku, aleadyKaryokuMax);
			bool aleadyRaisouMax = shipModel.IsMaxRaisou() && powUpInfo.Raisou == 0;
			mUIDeckPracticeUpParameters[1].Initialize(shipModel.Raisou - powUpInfo.Raisou, shipModel.Raisou, aleadyRaisouMax);
			bool aleadyTaikuMax = shipModel.IsMaxTaiku() && powUpInfo.Taiku == 0;
			mUIDeckPracticeUpParameters[2].Initialize(shipModel.Taiku - powUpInfo.Taiku, shipModel.Taiku, aleadyTaikuMax);
			bool aleadyKaihiMax = shipModel.IsMaxKaihi() && powUpInfo.Kaihi == 0;
			mUIDeckPracticeUpParameters[3].Initialize(shipModel.Kaihi - powUpInfo.Kaihi, shipModel.Kaihi, aleadyKaihiMax);
			bool aleadyLuckyMax = shipModel.IsMaxLucky() && powUpInfo.Lucky == 0;
			mUIDeckPracticeUpParameters[4].Initialize(shipModel.Lucky - powUpInfo.Lucky, shipModel.Lucky, aleadyLuckyMax);
			bool aleadyTaisenMax = shipModel.IsMaxTaisen() && powUpInfo.Taisen == 0;
			mUIDeckPracticeUpParameters[5].Initialize(shipModel.Taisen - powUpInfo.Taisen, shipModel.Taisen, aleadyTaisenMax);
			yield return new WaitForEndOfFrame();
			ResetPositionParameterSlot();
			mUIDeckPracticeShipInfo.Reposition();
			ResetPositionShip();
			mTexture_Ship.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipModel.GetGraphicsMstId(), 9);
			mTexture_Ship.MakePixelPerfect();
			mTexture_Ship.transform.localPosition = Util.Poi2Vec(shipModel.Offsets.GetShipDisplayCenter(shipModel.IsDamaged()));
			if (prevTexture != null)
			{
				yield return new WaitForEndOfFrame();
				Resources.UnloadAsset(prevTexture);
			}
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

		private IEnumerator WaitForKeyOrTouch(KeyControl keyController, Action onKeyAction)
		{
			mKeyController.IsRun = true;
			mTransform_TouchControlArea.SetActive(isActive: true);
			while (keyController != null && !keyController.keyState[1].down && mTransform_TouchControlArea.gameObject.activeSelf)
			{
				yield return null;
			}
			mTransform_TouchControlArea.SetActive(isActive: false);
			onKeyAction?.Invoke();
		}

		private void ResetPositionShip()
		{
			mTransfrom_ShipOffset.localPosition = mVector3_ShipDefaultLocalPosition;
		}

		private void Chain(Action onFinished, Action<Action> chainFrom, Action<Action> chainTo)
		{
			chainFrom(delegate
			{
				chainTo(delegate
				{
					onFinished();
				});
			});
		}

		private void Chain(params Action<Action>[] actions)
		{
			Action<Action> chainFrom = null;
			Action<Action> chainTo = null;
			int num = actions.Length;
			if (2 <= num)
			{
				chainFrom = actions[0];
				chainTo = actions[1];
				Array.Clear(actions, 0, 2);
			}
			else if (1 <= num)
			{
				chainFrom = actions[0];
				chainTo = null;
				Array.Clear(actions, 0, 1);
			}
			actions = (from action in actions
				where action != null
				select action).ToArray();
			Chain((Action)delegate
			{
				Chain(actions);
			}, chainFrom, chainTo);
		}

		private Tween GenerateTweenParameterResult()
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			float num = 0.1f;
			float num2 = (0.6f - num) / (float)mUIDeckPracticeUpParameters.Length;
			for (int i = 0; i < mUIDeckPracticeUpParameters.Length; i++)
			{
				Tween t = mUIDeckPracticeUpParameters[i].GenerateParameterUpAnimation(num).SetId(this);
				t.SetDelay(num2 / 2f);
				sequence.Join(t);
			}
			return sequence;
		}

		private Tween GenerateTweenShipIn()
		{
			return mTransfrom_ShipOffset.DOLocalMove(new Vector3(290f, 70f), 0.6f).SetEase(Ease.OutExpo);
		}

		private Tween GenerateTweenShipOut()
		{
			switch (UnityEngine.Random.Range(0, 2))
			{
			case 0:
				return GenerateTweenShipLeftBottomOutAlphaOut();
			case 1:
				return GenerateTweenShipOutAlphInaScaleIn();
			default:
				return null;
			}
		}

		private Tween GenerateTweenShipLeftBottomOutAlphaOut()
		{
			float defaultHeight = mTexture_Ship.height;
			float slotOutHeight = defaultHeight * 0.5f;
			return DOVirtual.Float(0f, 1f, 0.3f, delegate(float percentage)
			{
				mTexture_Ship.height = (int)(defaultHeight + slotOutHeight * percentage);
				mTexture_Ship.alpha = 1f - percentage;
			}).OnComplete(delegate
			{
				mTexture_Ship.alpha = 1f;
				mTexture_Ship.height = (int)defaultHeight;
				mTransfrom_ShipOffset.localPosition = mVector3_ShipDefaultLocalPosition;
			});
		}

		private Tween GenerateTweenShipOutAlphInaScaleIn()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.Append(DOVirtual.Float(0f, 1f, 0.3f, delegate(float percentage)
			{
				mTexture_Ship.alpha = 1f - percentage;
			}));
			sequence.Join(mTransfrom_ShipOffset.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f));
			sequence.OnComplete(delegate
			{
				mTexture_Ship.alpha = 1f;
				mTransfrom_ShipOffset.localScale = Vector3.one;
				mTransfrom_ShipOffset.localPosition = mVector3_ShipDefaultLocalPosition;
			});
			return sequence;
		}

		private void ResetPositionParameterSlot()
		{
			UIDeckPracticeUpParameter[] array = mUIDeckPracticeUpParameters;
			foreach (UIDeckPracticeUpParameter uIDeckPracticeUpParameter in array)
			{
				uIDeckPracticeUpParameter.Reposition();
			}
		}

		private void OnDestroy()
		{
			mOnFinishedProductionListener = null;
			mUIDeckPracticeUpParameters = null;
			mUIDeckPracticeShipInfo = null;
			mTransfrom_ShipOffset = null;
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Ship);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Bg);
			UserInterfacePortManager.ReleaseUtils.Release(ref mPanelThis);
		}
	}
}
