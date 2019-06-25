using Common.Enum;
using Common.Struct;
using Server_Common;
using Server_Models;
using Sony.Vita.SavedGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

namespace Common.SaveManager
{
	public class VitaSaveManager : MonoBehaviour
	{
		private static VitaSaveManager _instance;

		private readonly int slotCount = 5;

		private XElement _elements;

		private ResultCode lastErrorCode;

		private ISaveDataOperator operatorInstance;

		private bool isOpen;

		private bool isMainInit;

		public static VitaSaveManager Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}
				Type typeFromHandle = typeof(VitaSaveManager);
				VitaSaveManager vitaSaveManager = UnityEngine.Object.FindObjectOfType(typeFromHandle) as VitaSaveManager;
				if (vitaSaveManager == null)
				{
					string text = typeFromHandle.ToString();
					GameObject gameObject = new GameObject(text, new Type[1]
					{
						typeFromHandle
					});
					vitaSaveManager = gameObject.GetComponent<VitaSaveManager>();
				}
				if (vitaSaveManager != null)
				{
					Initialise(vitaSaveManager);
				}
				return _instance;
			}
		}

		public int SlotCount => slotCount;

		public XElement Elements
		{
			get
			{
				return _elements;
			}
			private set
			{
				_elements = value;
			}
		}

		public bool IsDialogOpen => SaveLoad.IsDialogOpen;

		public bool IsBusy => SaveLoad.IsBusy;

		public ResultCode LastErrorCode
		{
			get
			{
				return lastErrorCode;
			}
			private set
			{
				lastErrorCode = value;
			}
		}

		private static void Initialise(VitaSaveManager instance)
		{
			if (_instance == null)
			{
				_instance = instance;
				instance.OnInitialize();
				UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
			}
			else if (_instance != instance)
			{
				UnityEngine.Object.DestroyImmediate(instance);
			}
		}

		public void OnInitialize()
		{
			try
			{
				SaveLoad.OnGameSaved += OnSavedGameSaved;
				SaveLoad.OnGameLoaded += OnSavedGameLoaded;
				SaveLoad.OnCanceled += OnSavedGameCanceled;
				SaveLoad.OnSaveError += OnSaveError;
				SaveLoad.OnLoadError += OnLoadError;
				SaveLoad.OnLoadNoData += OnLoadNoData;
				Main.Initialise();
				SaveLoad.SetEmptySlotIconPath(Application.streamingAssetsPath + "/SaveIconEmpty.png");
				SaveLoad.SetSlotCount(SlotCount);
				isMainInit = true;
			}
			catch
			{
				isMainInit = false;
			}
		}

		public void Open(ISaveDataOperator instance)
		{
			operatorInstance = instance;
			DestroyElements();
			isOpen = true;
		}

		public void Close()
		{
			operatorInstance = null;
			DestroyElements();
			isOpen = false;
		}

		public bool Save()
		{
			if (operatorInstance == null || Comm_UserDatas.Instance.User_basic.Starttime == 0)
			{
				return false;
			}
			if (SaveLoad.IsDialogOpen || IsBusy)
			{
				return false;
			}
			SaveHeaderFmt saveHeaderFmt = new SaveHeaderFmt();
			saveHeaderFmt.SetPropertie();
			List<SaveTarget> saveTarget = getSaveTarget(saveHeaderFmt);
			byte[] data = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream))
				{
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement(getTableName());
					foreach (SaveTarget item in saveTarget)
					{
						DataContractSerializer dataContractSerializer = (!item.IsCollection) ? new DataContractSerializer(item.ClassType) : new DataContractSerializer(item.ClassType, item.TableName + "s", string.Empty);
						dataContractSerializer.WriteObject(xmlWriter, item.Data);
					}
					xmlWriter.WriteEndElement();
					xmlWriter.Flush();
					data = memoryStream.ToArray();
				}
			}
			SaveLoad.SavedGameSlotParams slotParams = default(SaveLoad.SavedGameSlotParams);
			TurnString turnString = Comm_UserDatas.Instance.User_turn.GetTurnString();
			string text = (Comm_UserDatas.Instance.User_plus.GetLapNum() <= 0) ? string.Empty : "★";
			string subTitle = $"{text}{turnString.Year}の年 {turnString.Month} {turnString.Day}日";
			string nickname = Comm_UserDatas.Instance.User_basic.Nickname;
			string datail = getDatail();
			slotParams.title = nickname;
			slotParams.subTitle = subTitle;
			slotParams.detail = datail;
			slotParams.iconPath = Application.streamingAssetsPath + "/SaveIcon.png";
			SaveLoad.ControlFlags controlFlags = SaveLoad.ControlFlags.NOSPACE_DIALOG_CONTINUABLE;
			return (SaveLoad.SaveGameList(data, slotParams, controlFlags) == ErrorCode.SG_OK) ? true : false;
		}

		private string getDatail()
		{
			Comm_UserDatas instance = Comm_UserDatas.Instance;
			string empty = string.Empty;
			int num = (int)(instance.User_basic.Difficult - 1);
			string[] array = new string[5]
			{
				"丁",
				"丙",
				"乙",
				"甲",
				"史"
			};
			empty = empty + "難易度:" + array[num] + "\n";
			string text = empty;
			empty = text + "提督レベル:" + instance.User_basic.UserLevel() + "\n";
			text = empty;
			empty = text + "艦隊保有数:" + instance.User_deck.Count + "\n";
			text = empty;
			empty = text + "艦娘保有数:" + instance.User_ship.Count + "\n";
			text = empty;
			empty = text + "戦略ポイント:" + instance.User_basic.Strategy_point + "\n";
			text = empty;
			empty = text + "燃料:" + instance.User_material[enumMaterialCategory.Fuel].Value + "\n";
			text = empty;
			empty = text + "弾薬:" + instance.User_material[enumMaterialCategory.Bull].Value + "\n";
			text = empty;
			empty = text + "鋼材:" + instance.User_material[enumMaterialCategory.Steel].Value + "\n";
			text = empty;
			return text + "ボ\u30fcキサイト:" + instance.User_material[enumMaterialCategory.Bauxite].Value + "\n";
		}

		public bool IsAllEmpty()
		{
			for (int i = 0; i < SlotCount; i++)
			{
				if (SaveLoad.GetSlotInfo(i, out SaveLoad.SavedGameSlotInfo slotInfo) == ErrorCode.SG_OK && slotInfo.status != SaveLoad.SlotStatus.EMPTY)
				{
					return false;
				}
			}
			return true;
		}

		public bool Load()
		{
			if (operatorInstance == null)
			{
				return false;
			}
			if (SaveLoad.IsDialogOpen || IsBusy)
			{
				return false;
			}
			return (SaveLoad.LoadGameList() == ErrorCode.SG_OK) ? true : false;
		}

		public bool Delete()
		{
			if (operatorInstance == null)
			{
				return false;
			}
			if (SaveLoad.IsDialogOpen || IsBusy)
			{
				return false;
			}
			return (SaveLoad.DeleteGameList() == ErrorCode.SG_OK) ? true : false;
		}

		private List<SaveTarget> getSaveTarget(SaveHeaderFmt header)
		{
			List<SaveTarget> list = new List<SaveTarget>();
			Comm_UserDatas instance = Comm_UserDatas.Instance;
			list.Add(new SaveTarget(typeof(SaveHeaderFmt), header, SaveHeaderFmt.tableaName));
			list.Add(new SaveTarget(typeof(Mem_basic), instance.User_basic, Mem_basic.tableName));
			list.Add(new SaveTarget(typeof(Mem_newgame_plus), instance.User_plus, Mem_newgame_plus.tableName));
			list.Add(new SaveTarget(typeof(Mem_record), instance.User_record, Mem_record.tableName));
			list.Add(new SaveTarget(typeof(Mem_trophy), instance.User_trophy, Mem_trophy.tableName));
			list.Add(new SaveTarget(typeof(Mem_turn), instance.User_turn, Mem_turn.tableName));
			list.Add(new SaveTarget(typeof(Mem_deckpractice), instance.User_deckpractice, Mem_deckpractice.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_book>), instance.Ship_book.Values.ToList(), "ship_book"));
			list.Add(new SaveTarget(typeof(List<Mem_book>), instance.Slot_book.Values.ToList(), "slot_book"));
			list.Add(new SaveTarget(typeof(List<Mem_deck>), instance.User_deck.Values.ToList(), Mem_deck.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_esccort_deck>), instance.User_EscortDeck.Values.ToList(), Mem_esccort_deck.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_furniture>), instance.User_furniture.Values.ToList(), Mem_furniture.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_kdock>), instance.User_kdock.Values.ToList(), Mem_kdock.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_mapcomp>), instance.User_mapcomp.Values.ToList(), Mem_mapcomp.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_mapclear>), instance.User_mapclear.Values.ToList(), Mem_mapclear.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_material>), instance.User_material.Values.ToList(), Mem_material.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_missioncomp>), instance.User_missioncomp.Values.ToList(), Mem_missioncomp.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_ndock>), instance.User_ndock.Values.ToList(), Mem_ndock.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_quest>), instance.User_quest.Values.ToList(), Mem_quest.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_questcount>), instance.User_questcount.Values.ToList(), Mem_questcount.tableName));
			list.Add(new SaveTarget(instance.User_ship.Values));
			list.Add(new SaveTarget(typeof(List<Mem_slotitem>), instance.User_slot.Values.ToList(), Mem_slotitem.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_tanker>), instance.User_tanker.Values.ToList(), Mem_tanker.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_useitem>), instance.User_useItem.Values.ToList(), Mem_useitem.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_rebellion_point>), instance.User_rebellion_point.Values.ToList(), Mem_rebellion_point.tableName));
			list.Add(new SaveTarget(typeof(List<Mem_room>), instance.User_room.Values.ToList(), Mem_room.tableName));
			list.Add(new SaveTarget(typeof(List<int>), instance.Temp_escortship.ToList(), "temp_escortship"));
			list.Add(new SaveTarget(typeof(List<int>), instance.Temp_deckship.ToList(), "temp_deckship"));
			List<Mem_history> list2 = new List<Mem_history>();
			foreach (List<Mem_history> value in instance.User_history.Values)
			{
				list2.AddRange(value);
			}
			list.Add(new SaveTarget(typeof(List<Mem_history>), list2, Mem_history.tableName));
			return list;
		}

		private void DestroyElements()
		{
			if (Elements != null)
			{
				Elements.RemoveAll();
				Elements = null;
			}
		}

		private string getTableName()
		{
			return "member_datas";
		}

		private void OnSavedGameSaved(Messages.PluginMessage msg)
		{
			operatorInstance.SaveComplete();
		}

		private void OnSavedGameLoaded(Messages.PluginMessage msg)
		{
			byte[] loadedGame = SaveLoad.GetLoadedGame();
			if (loadedGame == null || loadedGame.Length == 0)
			{
				OnLoadNoData(msg);
				return;
			}
			using (MemoryStream input = new MemoryStream(loadedGame))
			{
				XmlReader xmlReader = XmlReader.Create(input);
				Elements = XElement.Load(xmlReader);
				xmlReader.Close();
			}
			if (Comm_UserDatas.Instance.SetUserData())
			{
				operatorInstance.LoadComplete();
			}
			else
			{
				operatorInstance.LoadError();
			}
			DestroyElements();
		}

		private void OnSavedGameCanceled(Messages.PluginMessage msg)
		{
			operatorInstance.Canceled();
		}

		private void OnSaveError(Messages.PluginMessage msg)
		{
			ResultCode result = default(ResultCode);
			SaveLoad.GetLastError(out result);
			LastErrorCode = result;
			operatorInstance.SaveError();
		}

		private void OnLoadError(Messages.PluginMessage msg)
		{
			ResultCode result = default(ResultCode);
			SaveLoad.GetLastError(out result);
			LastErrorCode = result;
			operatorInstance.LoadError();
		}

		private void OnLoadNoData(Messages.PluginMessage msg)
		{
			operatorInstance.LoadNothing();
		}

		private void OnDeleted(Messages.PluginMessage msg)
		{
			operatorInstance.DeleteComplete();
		}

		private void Update()
		{
			if (isOpen && isMainInit)
			{
				Main.Update();
			}
		}

		private void Awake()
		{
			Initialise(this);
		}
	}
}
