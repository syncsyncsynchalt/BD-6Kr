using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV
{
	public class CommonDeckSwitchManager : MonoBehaviour
	{
		private const string SPRITE_ON = "pin_on";

		private const string SPRITE_OFF = "pin_off";

		private const string SPRITE_NONE = "pin_none";

		private const string OTHER_SPRITE_ON = "other_on";

		private const string OTHER_SPRITE_OFF = "other_off";

		private const string OTHER_SPRITE_NONE = "other_none";

		private CommonDeckSwitchHandler handler;

		[SerializeField]
		private UISprite templateSprite;

		private List<UISprite> switchableIconSprites = new List<UISprite>();

		private int horizontalIconMargin;

		private bool otherEnabled;

		private KeyControl keyController;

		private int currentIdx;

		private DeckModel[] decks;

		private int switchableIconCount => (!otherEnabled) ? validDeckCount : (validDeckCount + 1);

		public DeckModel currentDeck => (!otherEnabled || currentIdx != switchableIconCount - 1) ? decks[currentIdx] : null;

		public bool keyControlEnable
		{
			get;
			set;
		}

		private int validDeckCount => decks.Length;

		public bool isChangeRight => currentIdx < switchableIconCount - 1;

		public bool isChangeLeft => 0 < currentIdx;

		public virtual void Init(ManagerBase manager, DeckModel[] decks, CommonDeckSwitchHandler handler, KeyControl keyController, bool otherEnabled)
		{
			Init(manager, decks, handler, keyController, otherEnabled, 0, 50);
		}

		public virtual void Init(ManagerBase manager, DeckModel[] decks, CommonDeckSwitchHandler handler, KeyControl keyController, bool otherEnabled, DeckModel currentDeck, int horizontalIconMargin = 50)
		{
			DeckModel[] array = (from e in decks
				where e.MissionState == MissionStates.NONE
				select e).ToArray();
			int num = 0;
			int num2 = 0;
			DeckModel[] array2 = array;
			foreach (DeckModel deckModel in array2)
			{
				if (deckModel.Id == currentDeck.Id)
				{
					num2 = num;
					break;
				}
				num++;
			}
			Init(manager, array, handler, keyController, otherEnabled, num2, horizontalIconMargin);
		}

		protected void Init(ManagerBase manager, DeckModel[] srcDecks, CommonDeckSwitchHandler handler, KeyControl keyController, bool otherEnabled, int currentIdx, int horizontalIconMargin)
		{
			decks = srcDecks;
			this.handler = handler;
			this.keyController = keyController;
			this.currentIdx = currentIdx;
			this.otherEnabled = otherEnabled;
			this.horizontalIconMargin = horizontalIconMargin;
			int deckCount = manager.UserInfo.DeckCount;
			keyControlEnable = true;
			int num = deckCount + (otherEnabled ? 1 : 0);
			int num2 = -(num - 1) * horizontalIconMargin / 2;
			HashSet<int> validIndices = new HashSet<int>();
			decks.ForEach(delegate(DeckModel e)
			{
				validIndices.Add(e.Id - 1);
			});
			if (otherEnabled)
			{
				validIndices.Add(num - 1);
			}
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Util.Instantiate(templateSprite.gameObject, base.gameObject);
				gameObject.transform.localPosition(new Vector3(num2 + horizontalIconMargin * i, 0f, 0f));
				UISprite component = gameObject.GetComponent<UISprite>();
				if (validIndices.Contains(i))
				{
					switchableIconSprites.Add(component);
				}
				else
				{
					component.spriteName = "pin_none";
				}
			}
			templateSprite.SetActive(isActive: false);
			if (!handler.IsDeckSelectable(currentIdx, currentDeck))
			{
				ProcessNext(0);
			}
			handler.OnDeckChange(currentDeck);
			RefleshIcons();
		}

		private void Update()
		{
			if (keyControlEnable && keyController != null)
			{
				if (keyController.IsRSLeftDown())
				{
					ChangePrevDeck();
				}
				else if (keyController.keyState[18].down)
				{
					ChangeNextDeck();
				}
			}
		}

		public void ChangePrevDeck()
		{
			if (ProcessPrev())
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				RefleshIcons();
				handler.OnDeckChange(currentDeck);
			}
		}

		public void ChangeNextDeck()
		{
			if (ProcessNext())
			{
				SoundUtils.PlayOneShotSE(SEFIleInfos.CommonEnter1);
				RefleshIcons();
				handler.OnDeckChange(currentDeck);
			}
		}

		private bool ProcessNext()
		{
			return ProcessNext(currentIdx + 1);
		}

		private bool ProcessNext(int startIdx)
		{
			for (int i = startIdx; i < switchableIconCount; i++)
			{
				if (handler.IsDeckSelectable(i, (i < decks.Length) ? decks[i] : null))
				{
					currentIdx = i;
					return true;
				}
			}
			return false;
		}

		private bool ProcessPrev()
		{
			for (int num = currentIdx - 1; num >= 0; num--)
			{
				if (handler.IsDeckSelectable(num, decks[num]))
				{
					currentIdx = num;
					return true;
				}
			}
			return false;
		}

		public void RefleshIcons()
		{
			for (int i = 0; i < switchableIconCount; i++)
			{
				bool flag = otherEnabled && i == switchableIconCount - 1;
				string spriteName = (i != currentIdx) ? ((!handler.IsDeckSelectable(i, (i < decks.Length) ? decks[i] : null)) ? ((!flag) ? "pin_none" : "other_none") : ((!flag) ? "pin_off" : "other_off")) : ((!flag) ? "pin_on" : "other_on");
				switchableIconSprites[i].spriteName = spriteName;
			}
		}

		protected virtual void OnCallDestroy()
		{
		}

		private void OnDestroy()
		{
			OnCallDestroy();
			handler = null;
			templateSprite = null;
			switchableIconSprites = null;
			keyController = null;
			decks = null;
		}
	}
}
