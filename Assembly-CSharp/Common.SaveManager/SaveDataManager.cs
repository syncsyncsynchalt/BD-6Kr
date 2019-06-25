using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Common.SaveManager
{
	public class SaveDataManager
	{
		private string outDir = "../";

		private static SaveDataManager _instance;

		private readonly int slotCount = 4;

		private Dictionary<int, SaveHeaderFmt> header = new Dictionary<int, SaveHeaderFmt>();

		private Dictionary<int, SaveHeaderFmt> cacheHeader = new Dictionary<int, SaveHeaderFmt>();

		private XElement _elements;

		public string OutDir
		{
			get
			{
				return outDir;
			}
			set
			{
				OutDir = value;
			}
		}

		public static SaveDataManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SaveDataManager();
				}
				return _instance;
			}
			private set
			{
				_instance = value;
			}
		}

		public int SlotCount => slotCount;

		public Dictionary<int, SaveHeaderFmt> Header
		{
			get
			{
				return header;
			}
			private set
			{
				header = value;
			}
		}

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

		public SaveDataManager()
		{
			Elements = null;
			Header = new Dictionary<int, SaveHeaderFmt>();
			cacheHeader = new Dictionary<int, SaveHeaderFmt>();
			for (int i = 1; i <= slotCount; i++)
			{
				Header.Add(i, null);
				SaveHeaderFmt headerInfo = getHeaderInfo(i);
				cacheHeader.Add(i, headerInfo);
			}
		}

		public void UpdateHeader()
		{
			for (int i = 1; i <= slotCount; i++)
			{
				if (cacheHeader[i] == null)
				{
					cacheHeader[i] = getHeaderInfo(i);
				}
				header[i] = cacheHeader[i];
			}
		}

		public bool Save(int slot_no)
		{
			if (Comm_UserDatas.Instance.User_basic.Starttime == 0)
			{
				return false;
			}
			SaveHeaderFmt saveHeaderFmt = new SaveHeaderFmt();
			saveHeaderFmt.SetPropertie();
			List<SaveTarget> saveTarget = getSaveTarget(saveHeaderFmt);
			byte[] bytes = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream))
				{
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement(getTableName(slot_no));
					foreach (SaveTarget item in saveTarget)
					{
						DataContractSerializer dataContractSerializer = (!item.IsCollection) ? new DataContractSerializer(item.ClassType) : new DataContractSerializer(item.ClassType, item.TableName + "s", string.Empty);
						dataContractSerializer.WriteObject(xmlWriter, item.Data);
						xmlWriter.Flush();
					}
					xmlWriter.WriteEndElement();
					xmlWriter.Flush();
					bytes = memoryStream.ToArray();
				}
			}
			File.WriteAllBytes(getMemberFilePath(slot_no), bytes);
			cacheHeader[slot_no] = saveHeaderFmt;
			return true;
		}

		public bool Load(int slot_no)
		{
			if (!object.ReferenceEquals(Header[slot_no], cacheHeader[slot_no]))
			{
				return false;
			}
			if (!File.Exists(getMemberFilePath(slot_no)))
			{
				return false;
			}
			Elements = XElement.Load(getMemberFilePath(slot_no));
			Comm_UserDatas.Instance.SetUserData();
			DestroyElements();
			return true;
		}

		private SaveHeaderFmt getHeaderInfo(int slot_no)
		{
			SaveHeaderFmt saveHeaderFmt = null;
			XElement xElement;
			try
			{
				xElement = XElement.Load(getMemberFilePath(slot_no)).Element(SaveHeaderFmt.tableaName);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				xElement = null;
			}
			if (xElement != null)
			{
				saveHeaderFmt = new SaveHeaderFmt();
				if (!saveHeaderFmt.SetPropertie(xElement))
				{
					saveHeaderFmt = null;
				}
			}
			return saveHeaderFmt;
		}

		private string getTableName(int slot_no)
		{
			return "member_datas" + slot_no;
		}

		private string getMemberFilePath(int slot_no)
		{
			return OutDir + getTableName(slot_no) + ".xml";
		}

		private void DestroyElements()
		{
			Elements.RemoveAll();
			Elements = null;
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
	}
}
