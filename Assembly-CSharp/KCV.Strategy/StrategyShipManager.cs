using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyShipManager : MonoBehaviour
	{
		[Button("moveArea", "moveAreaShip", new object[]
		{

		})]
		public int button1;

		public int area;

		[SerializeField]
		private StrategyShip[] allShipIcons;

		private List<StrategyShip> existShipIcons;

		private int[] areaDeckCount;

		private List<float[,]> shipNoPosList;

		public GameObject DeckSelectCursol;

		private UISprite CursolSprite;

		[SerializeField]
		private StrategyOrganizeGuideMessage OrganizeMessage;

		public bool isShipMoving;

		private int nowFocusDisableDeckNo;

		public float Base = 1f;

		public float Far = 1.7f;

		public float Middle = 0.7f;

		public float near = 0.5f;

		[Button("DebugPosListChange", "DebugPosListChange", new object[]
		{

		})]
		public int button2;

		private Action CharacterMove;

		private void moveArea()
		{
			StartCoroutine(moveAreaAllShip(area, isWait: false));
		}

		private void Awake()
		{
			CursolSprite = ((Component)DeckSelectCursol.transform.FindChild("Cursol")).GetComponent<UISprite>();
			nowFocusDisableDeckNo = -1;
			shipNoPosList = new List<float[,]>();
			shipNoPosList.Add(new float[1, 2]);
			shipNoPosList.Add(new float[2, 2]
			{
				{
					0f - Base,
					0f
				},
				{
					Base,
					0f
				}
			});
			shipNoPosList.Add(new float[3, 2]
			{
				{
					0f,
					Middle
				},
				{
					0f - Base,
					0f - Middle
				},
				{
					Base,
					0f - Middle
				}
			});
			shipNoPosList.Add(new float[4, 2]
			{
				{
					0f - Base,
					near
				},
				{
					Base,
					near
				},
				{
					0f - Base,
					0f - near
				},
				{
					Base,
					0f - near
				}
			});
			shipNoPosList.Add(new float[5, 2]
			{
				{
					0f - Base,
					Base
				},
				{
					Base,
					Base
				},
				{
					0f - Base,
					0f - Base
				},
				{
					Base,
					0f - Base
				},
				{
					0f,
					0f
				}
			});
			shipNoPosList.Add(new float[6, 2]
			{
				{
					0f - Base,
					Base
				},
				{
					Base,
					Base
				},
				{
					0f - Far,
					0f
				},
				{
					Far,
					0f
				},
				{
					0f - Base,
					0f - Base
				},
				{
					Base,
					0f - Base
				}
			});
			shipNoPosList.Add(new float[7, 2]
			{
				{
					0f - Base,
					Base
				},
				{
					Base,
					Base
				},
				{
					0f - Far,
					0f
				},
				{
					Far,
					0f
				},
				{
					0f - Base,
					0f - Base
				},
				{
					Base,
					0f - Base
				},
				{
					0f,
					0f
				}
			});
			shipNoPosList.Add(new float[8, 2]
			{
				{
					0f - Base,
					Base
				},
				{
					Base,
					Base
				},
				{
					0f - Far,
					0f
				},
				{
					Far,
					0f
				},
				{
					0f - Base,
					0f - Base
				},
				{
					Base,
					0f - Base
				},
				{
					0f,
					near
				},
				{
					0f,
					0f - near
				}
			});
		}

		private void DebugPosListChange()
		{
			shipNoPosList = new List<float[,]>();
			shipNoPosList.Add(new float[1, 2]);
			shipNoPosList.Add(new float[2, 2]
			{
				{
					0f - Base,
					0f
				},
				{
					Base,
					0f
				}
			});
			shipNoPosList.Add(new float[3, 2]
			{
				{
					0f,
					Middle
				},
				{
					0f - Base,
					0f - Middle
				},
				{
					Base,
					0f - Middle
				}
			});
			shipNoPosList.Add(new float[4, 2]
			{
				{
					0f - Base,
					near
				},
				{
					Base,
					near
				},
				{
					0f - Base,
					0f - near
				},
				{
					Base,
					0f - near
				}
			});
			shipNoPosList.Add(new float[5, 2]
			{
				{
					0f - Base,
					Base
				},
				{
					Base,
					Base
				},
				{
					0f - Base,
					0f - Base
				},
				{
					Base,
					0f - Base
				},
				{
					0f,
					0f
				}
			});
			shipNoPosList.Add(new float[6, 2]
			{
				{
					0f - Base,
					Base
				},
				{
					Base,
					Base
				},
				{
					0f - Far,
					0f
				},
				{
					Far,
					0f
				},
				{
					0f - Base,
					0f - Base
				},
				{
					Base,
					0f - Base
				}
			});
			shipNoPosList.Add(new float[7, 2]
			{
				{
					0f - Base,
					Base
				},
				{
					Base,
					Base
				},
				{
					0f - Far,
					0f
				},
				{
					Far,
					0f
				},
				{
					0f - Base,
					0f - Base
				},
				{
					Base,
					0f - Base
				},
				{
					0f,
					0f
				}
			});
			shipNoPosList.Add(new float[8, 2]
			{
				{
					0f - Base,
					Base
				},
				{
					Base,
					Base
				},
				{
					0f - Far,
					0f
				},
				{
					Far,
					0f
				},
				{
					0f - Base,
					0f - Base
				},
				{
					Base,
					0f - Base
				},
				{
					0f,
					near
				},
				{
					0f,
					0f - near
				}
			});
		}

		public static DeckModel[] getEnableDecks(DeckModel[] decks)
		{
			List<DeckModel> list = new List<DeckModel>();
			for (int i = 0; i < decks.Length; i++)
			{
				if (decks[i].Count > 0)
				{
					list.Add(decks[i]);
				}
			}
			return list.ToArray();
		}

		public static DeckModel[] getDisableDecks(DeckModel[] decks)
		{
			List<DeckModel> list = new List<DeckModel>();
			for (int i = 0; i < decks.Length; i++)
			{
				if (decks[i].Count <= 0)
				{
					list.Add(decks[i]);
				}
			}
			return list.ToArray();
		}

		public void setShipIcons(DeckModel[] decks, bool isScaleZero = true)
		{
			areaDeckCount = new int[18];
			existShipIcons = new List<StrategyShip>();
			makeDeckExsistList(decks);
			setActiveIcons(decks, isScaleZero);
			setShipIconsGraph();
			setShipIconsState();
			setShipIconsPosition();
		}

		private void makeDeckExsistList(DeckModel[] decks)
		{
			for (int i = 0; i < decks.Length; i++)
			{
				if (decks[i].Count > 0)
				{
					allShipIcons[decks[i].Id - 1].setDeckModel(decks[i]);
					existShipIcons.Add(allShipIcons[decks[i].Id - 1]);
				}
			}
		}

		private void setActiveIcons(DeckModel[] decks, bool isScaleZero)
		{
			for (int i = 1; i < allShipIcons.Length + 1; i++)
			{
				bool isActive = false;
				for (int j = 0; j < decks.Length; j++)
				{
					if (decks[j].Id == i && decks[j].Count != 0)
					{
						isActive = true;
					}
				}
				if (isScaleZero)
				{
					allShipIcons[i - 1].transform.localScaleZero();
				}
				else
				{
					allShipIcons[i - 1].transform.localScaleOne();
				}
				allShipIcons[i - 1].SetActive(isActive);
			}
		}

		public void popUpShipIcon()
		{
			for (int i = 0; i < existShipIcons.Count; i++)
			{
				existShipIcons[i].popUpShipIcon();
			}
			OrganizeMessage.UpdateVisible();
		}

		private void setShipIconsGraph()
		{
			for (int i = 0; i < existShipIcons.Count; i++)
			{
				existShipIcons[i].setShipGraph();
			}
		}

		private void setShipIconsPosition()
		{
			for (int i = 0; i < existShipIcons.Count; i++)
			{
				int areaId = existShipIcons[i].deck.AreaId;
				int no = areaDeckCount[areaId];
				areaDeckCount[areaId]++;
				Vector3 shipIconPosition = GetShipIconPosition(areaId, no);
				existShipIcons[i].setShipAreaPosition(shipIconPosition);
			}
		}

		public void setShipIconsState()
		{
			for (int i = 0; i < existShipIcons.Count; i++)
			{
				existShipIcons[i].setShipState();
			}
		}

		public void unsetShipIconsStateForSupportMission()
		{
			for (int i = 0; i < existShipIcons.Count; i++)
			{
				if (existShipIcons[i].deck.IsInSupportMission())
				{
					existShipIcons[i].unsetShipStateIcon();
				}
			}
		}

		public void changeFocus()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck == null)
			{
				App.Initialize();
				StrategyTopTaskManager.CreateLogicManager();
				SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(1);
			}
			int id = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			if (isDisableDeck(id))
			{
				DeckSelectCursol.transform.parent = OrganizeMessage.transform;
			}
			else
			{
				DeckSelectCursol.transform.parent = allShipIcons[id - 1].transform;
			}
			DeckSelectCursol.transform.localScale = Vector3.one;
			DeckSelectCursol.transform.localPosition = Vector3.zero;
			CursolSprite.ParentHasChanged();
		}

		public Vector3 GetShipIconPosition(int AreaID, int No)
		{
			Vector3 position = StrategyTopTaskManager.Instance.TileManager.Tiles[AreaID].transform.position;
			int num = getEnableDecks(StrategyTopTaskManager.GetLogicManager().Area[AreaID].GetDecks()).Length;
			float x = 0.25f * shipNoPosList[num - 1][No, 0];
			float y = 0.25f * shipNoPosList[num - 1][No, 1];
			return position + new Vector3(x, y, 0f);
		}

		public void sortAreaShipIcon(int targetAreaID, bool isMoveCharacter, bool isUpdateOrganizeMessage)
		{
			DeckModel[] enableDecks = getEnableDecks(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecksFromArea(targetAreaID));
			for (int i = 0; i < enableDecks.Length; i++)
			{
				int num = enableDecks[i].Id - 1;
				Hashtable hashtable = new Hashtable();
				hashtable.Add("position", GetShipIconPosition(targetAreaID, i));
				hashtable.Add("time", 2f);
				hashtable.Add("easetype", iTween.EaseType.linear);
				if (i == enableDecks.Length - 1)
				{
					hashtable.Add("oncomplete", "OnCompleteMove");
					hashtable.Add("oncompletetarget", base.gameObject);
					int id = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
					hashtable.Add("oncompleteparams", id);
					if (isMoveCharacter)
					{
						CharacterMove = delegate
						{
							StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(isEnter: true, null);
						};
					}
				}
				iTween.MoveTo(allShipIcons[num].gameObject, hashtable);
				allShipIcons[num].setColliderEnable(isEnable: false);
			}
			if (isUpdateOrganizeMessage)
			{
				OrganizeMessage.UpdateVisible();
			}
		}

		public IEnumerator moveAreaAllShip(int areaID, bool isWait)
		{
			getEnableDecks(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecksFromArea(areaID));
			sortAreaShipIcon(1, isMoveCharacter: false, isUpdateOrganizeMessage: false);
			sortAreaShipIcon(areaID, isMoveCharacter: false, isUpdateOrganizeMessage: true);
			if (isWait)
			{
				yield return new WaitForSeconds(2f);
			}
		}

		private void OnCompleteMove(int deckID)
		{
			setShipIconsState();
			StrategyTopTaskManager.Instance.GetInfoMng().updateFooterInfo(isUpdateMaterial: false);
			if (CharacterMove != null)
			{
				CharacterMove();
			}
			CharacterMove = null;
			StrategyShip[] array = allShipIcons;
			foreach (StrategyShip strategyShip in array)
			{
				strategyShip.setColliderEnable(isEnable: true);
			}
			isShipMoving = false;
		}

		public void SetVisible(bool isVisible)
		{
			float alpha = isVisible ? 1 : 0;
			for (int i = 0; i < existShipIcons.Count; i++)
			{
				TweenAlpha.Begin(existShipIcons[i].gameObject, 0.2f, alpha);
			}
			if (isVisible)
			{
				OrganizeMessage.UpdateVisible();
			}
			else
			{
				OrganizeMessage.setVisible(isVisible: false);
			}
		}

		public void SetColliderEnable(bool isEnable)
		{
			StrategyShip[] array = allShipIcons;
			foreach (StrategyShip strategyShip in array)
			{
				strategyShip.setColliderEnable(isEnable);
			}
		}

		public DeckModel getNextDeck(int nowDeckID, bool isSeachLocalArea)
		{
			return getDeck(nowDeckID, isNext: true, isSeachLocalArea);
		}

		public DeckModel getPrevDeck(int nowDeckID, bool isSeachLocalArea)
		{
			return getDeck(nowDeckID, isNext: false, isSeachLocalArea);
		}

		private DeckModel getDeck(int nowDeckID, bool isNext, bool isSeachLocalArea)
		{
			int num = 0;
			List<StrategyShip> list = existShipIcons;
			if (isSeachLocalArea)
			{
				list = (from x in existShipIcons
					where x.deck.AreaId == StrategyTopTaskManager.Instance.TileManager.FocusTile.areaID
					select x).ToList();
			}
			if (list.Count == 0)
			{
				DeckModel deckModel = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks().FirstOrDefault((DeckModel x) => x.Count == 0);
				if (deckModel != null)
				{
					nowFocusDisableDeckNo = deckModel.Id;
					return deckModel;
				}
				return SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].deck.Id == nowDeckID)
				{
					num = i;
					break;
				}
			}
			int num2 = isNext ? 1 : (-1);
			int index;
			if (nowFocusDisableDeckNo != -1 && !isSeachLocalArea)
			{
				index = ((!isNext) ? (list.Count - 1) : 0);
				nowFocusDisableDeckNo = -1;
			}
			else
			{
				if (isIndexOver(num + num2, list.Count - 1) && OrganizeMessage.isVisible && !isSeachLocalArea)
				{
					DeckModel deckModel2 = getDisableDecks(StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks())[0];
					nowFocusDisableDeckNo = deckModel2.Id;
					return deckModel2;
				}
				index = (int)Util.LoopValue(num + num2, 0f, list.Count - 1);
			}
			return list[index].deck;
		}

		private bool isIndexOver(int value, int MaxValue)
		{
			if (value < 0)
			{
				return true;
			}
			if (value > MaxValue)
			{
				return true;
			}
			return false;
		}

		private bool isDisableDeck(int deckNo)
		{
			return StrategyTopTaskManager.GetLogicManager().UserInfo.GetDeck(deckNo).Count == 0;
		}
	}
}
