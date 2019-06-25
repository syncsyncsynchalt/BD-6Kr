using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyShip : MonoBehaviour
	{
		[SerializeField]
		private UISprite shipSprite;

		[SerializeField]
		private UITexture shipStateIcon;

		[SerializeField]
		private UISprite shipNoIcon;

		[SerializeField]
		private Texture2D EnseiIcon;

		[SerializeField]
		private Texture2D TettaiIcon;

		[SerializeField]
		private TweenScale popupAnimation;

		private UIButtonMessage ButtonMes;

		[SerializeField]
		private AnimationCurve curve;

		public DeckModel deck
		{
			get;
			private set;
		}

		private void Awake()
		{
			ButtonMes = shipSprite.GetComponent<UIButtonMessage>();
			ButtonMes.target = base.gameObject;
			ButtonMes.functionName = "OnIconTouch";
			setColliderEnable(isEnable: false);
		}

		public void setDeckModel(DeckModel deck)
		{
			this.deck = deck;
		}

		public void setShipGraph()
		{
			int num = 2;
			if (deck.GetFlagShip() != null)
			{
				num = deck.GetFlagShip().ShipType;
			}
			shipSprite.spriteName = "shipicon_" + num;
			shipSprite.MakePixelPerfect();
			shipSprite.transform.localScaleX(0.8f);
			shipNoIcon = ((Component)base.transform.FindChild("Number")).GetComponent<UISprite>();
			shipStateIcon = ((Component)base.transform.FindChild("stateIcon")).GetComponent<UITexture>();
		}

		public void setShipState()
		{
			if (isActionEndOrEnseiOrTettai())
			{
				setShipColor(Color.gray);
			}
			else
			{
				setShipColor(Color.white);
			}
			if (deck.HasBling())
			{
				shipStateIcon.mainTexture = TettaiIcon;
				shipStateIcon.width = 60;
				shipStateIcon.height = 80;
			}
			else if (deck.MissionState == MissionStates.RUNNING || deck.MissionState == MissionStates.STOP)
			{
				shipStateIcon.mainTexture = EnseiIcon;
				shipStateIcon.width = 60;
				shipStateIcon.height = 80;
			}
			else
			{
				shipStateIcon.mainTexture = null;
			}
		}

		public void unsetShipStateIcon()
		{
			shipStateIcon.mainTexture = null;
		}

		public void setShipAreaPosition(Vector3 pos)
		{
			base.transform.position = pos;
		}

		public void popUpShipIcon()
		{
			popupAnimation.ResetToBeginning();
			popupAnimation.PlayForward();
			if (popupAnimation.animationCurve != curve)
			{
				popupAnimation.SetOnFinished(delegate
				{
					ChangePopUpAnimation();
					setColliderEnable(isEnable: true);
				});
			}
		}

		private void ChangePopUpAnimation()
		{
			popupAnimation.animationCurve = curve;
			popupAnimation.duration = 0.3f;
			popupAnimation.onFinished.Clear();
		}

		private void setShipColor(Color color)
		{
			shipSprite.color = color;
			shipNoIcon.color = color;
		}

		private bool isActionEndOrEnseiOrTettai()
		{
			return deck.IsActionEnd() || deck.MissionState == MissionStates.RUNNING || deck.MissionState == MissionStates.STOP;
		}

		private void OnIconTouch()
		{
			if (!(StrategyTopTaskManager.Instance != null) || StrategyTopTaskManager.GetSailSelect().isRun)
			{
				popUpShipIcon();
				if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == deck && StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID == deck.AreaId)
				{
					StrategyTopTaskManager.GetSailSelect().OpenCommandMenu();
				}
				else
				{
					ChangeCurrentDeck();
				}
			}
		}

		private void ChangeCurrentDeck()
		{
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = deck;
			StrategyTopTaskManager.GetSailSelect().changeDeck(deck.Id);
			StrategyTopTaskManager.Instance.GetAreaMng().ChangeFocusTile(deck.AreaId);
			StrategyTopTaskManager.Instance.ShipIconManager.changeFocus();
			StrategyTopTaskManager.Instance.UIModel.Character.PlayVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck);
		}

		public void setColliderEnable(bool isEnable)
		{
			if (ButtonMes != null)
			{
				ButtonMes.enabled = isEnable;
			}
		}
	}
}
