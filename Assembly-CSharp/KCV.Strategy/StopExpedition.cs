using DG.Tweening;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StopExpedition : MonoBehaviour
	{
		public enum ActionType
		{
			StartMission,
			NotStartMission,
			Shown,
			Hiden
		}

		public enum CheckType
		{
			CallTankerCountUp,
			CallTankerCountDown,
			CanStartCheck
		}

		public delegate void UIMissionWithTankerConfirmPopUpAction(ActionType actionType, UIMissionWithTankerConfirmPopUp calledObject);

		public delegate bool UIMissionWithTankerConfirmPopUpCheck(CheckType actionType, UIMissionWithTankerConfirmPopUp calledObject);

		[SerializeField]
		private UILabel mLabel_RequireDay;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIMissionShipBanner[] mUIMissionShipBanners;

		[SerializeField]
		private UILabel mLabel_Message;

		[SerializeField]
		private UILabel mLabel_MissionName;

		[SerializeField]
		private DialogAnimation dialogAnim;

		[SerializeField]
		private UIPanel RightPanel;

		private UIButton mFocusButton;

		private KeyControl mKeyController;

		private UIMissionWithTankerConfirmPopUpAction mUIMissionWithTankerConfirmPopUpAction;

		private UIMissionWithTankerConfirmPopUpCheck mUIMissionWithTankerConfirmPopUpTankerCheck;

		private Coroutine mInitializeCoroutine;

		private int mHasTankerCount;

		private MissionManager missionMng;

		public int SettingTankerCount
		{
			get;
			private set;
		}

		public DeckModel MissionStartDeckModel
		{
			get;
			private set;
		}

		public void StartPanel(MissionManager missionMng)
		{
			this.missionMng = missionMng;
			base.transform.localScale = Vector3.zero;
			Transform child = base.transform.FindChild("RightPanel/RequireDay").GetChild(1);
			UnityEngine.Object.Destroy(((Component)child).GetComponent<UISprite>());
			UILabel uILabel = child.AddComponent<UILabel>();
			child.localPositionX(-25f);
			uILabel.fontSize = 22;
			uILabel.text = "残り遠征日数";
			uILabel.overflowMethod = UILabel.Overflow.ResizeFreely;
			uILabel.font = mLabel_RequireDay.font;
			uILabel.color = new Color32(75, 75, 75, byte.MaxValue);
			dialogAnim.OpenAction = delegate
			{
				Initialize(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
				RightPanel.SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, null, string.Empty);
				GetKeyController();
			};
			dialogAnim.StartAnim(DialogAnimation.AnimType.POPUP, isOpen: true);
		}

		public void Initialize(DeckModel deckModel)
		{
			mLabel_Message.alpha = 0.01f;
			MissionStartDeckModel = deckModel;
			ChangeFocusButton(mButton_Negative);
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			int missionId = currentDeck.MissionId;
			MissionModel mission = missionMng.GetMission(missionId);
			mLabel_RequireDay.text = currentDeck.MissionRemainingTurns.ToString();
			mLabel_MissionName.text = mission.Name;
			if (mInitializeCoroutine != null)
			{
				StopCoroutine(mInitializeCoroutine);
				mInitializeCoroutine = null;
			}
			mInitializeCoroutine = StartCoroutine(InitailizeCoroutine(deckModel, delegate
			{
				mInitializeCoroutine = null;
			}));
		}

		private IEnumerator InitailizeCoroutine(DeckModel deckModel, Action callBack)
		{
			float delayStart = Time.deltaTime;
			ShipModel[] ships = deckModel.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				mUIMissionShipBanners[i].Initialize(i + 1, ships[i]);
				mUIMissionShipBanners[i].Show();
				UIUtil.AnimationOnFocus(mUIMissionShipBanners[i].transform, null);
				yield return new WaitForSeconds(0.05f);
			}
			yield return null;
			Vector3 localPosition = mLabel_Message.transform.localPosition;
			float x = localPosition.x - 50f;
			Vector3 localPosition2 = mLabel_Message.transform.localPosition;
			float y = localPosition2.y;
			Vector3 localPosition3 = mLabel_Message.transform.localPosition;
			Vector3 from = new Vector3(x, y, localPosition3.z);
			Vector3 to = mLabel_Message.transform.localPosition;
			mLabel_Message.transform.localPosition = from;
			mLabel_Message.transform.DOLocalMove(to, 0.3f);
			DOVirtual.DelayedCall(Time.deltaTime - delayStart, delegate
			{
				this.mLabel_Message.transform.DOLocalMove(to, 0.3f);
			});
			mLabel_Message.alpha = 1f;
			callBack?.Invoke();
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
			if (mKeyController != null)
			{
				mKeyController.Update();
				if (mKeyController.keyState[14].down)
				{
					ChangeFocusButton(mButton_Negative);
				}
				else if (mKeyController.keyState[10].down)
				{
					ChangeFocusButton(mButton_Positive);
				}
				else if (mKeyController.keyState[1].down)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					mFocusButton.SendMessage("OnClick");
				}
				else if (mKeyController.keyState[0].down)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
					mButton_Negative.SendMessage("OnClick");
				}
			}
		}

		public void OnClickPositiveButton()
		{
			DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			missionMng.MissionStop(currentDeck.Id);
			mKeyController = null;
			dialogAnim.CloseAction = DestroyThis;
			dialogAnim.StartAnim(DialogAnimation.AnimType.FEAD, isOpen: false);
			StrategyTopTaskManager.GetCommandMenu().DeckEnableCheck();
		}

		public void OnClickNegativeButton()
		{
			mKeyController = null;
			dialogAnim.CloseAction = DestroyThis;
			dialogAnim.StartAnim(DialogAnimation.AnimType.FEAD, isOpen: false);
		}

		public void SetOnUIMissionWithTankerConfirmPopUpAction(UIMissionWithTankerConfirmPopUpAction action)
		{
		}

		public void SetOnUIMissionWithTankerConfirmPopUpTankerCheckDelegate(UIMissionWithTankerConfirmPopUpCheck method)
		{
		}

		private void UpdateSettingTankerCountLabel(int value, bool isPoor)
		{
		}

		private void ChangeFocusButton(UIButton target)
		{
			if (target == null)
			{
				mButton_Negative.SetState(UIButtonColor.State.Normal, immediate: true);
				mButton_Positive.SetState(UIButtonColor.State.Hover, immediate: true);
				return;
			}
			mFocusButton = target;
			if (target.Equals(mButton_Negative))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				mButton_Negative.SetState(UIButtonColor.State.Hover, immediate: true);
				mButton_Positive.SetState(UIButtonColor.State.Normal, immediate: true);
			}
			else if (target.Equals(mButton_Positive))
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				mButton_Negative.SetState(UIButtonColor.State.Normal, immediate: true);
				mButton_Positive.SetState(UIButtonColor.State.Hover, immediate: true);
			}
		}

		private void DestroyThis()
		{
			KeyControlManager.Instance.KeyController = StrategyTopTaskManager.GetCommandMenu().keyController;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
