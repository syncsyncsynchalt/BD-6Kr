using Common.Enum;
using Common.SaveManager;
using Server_Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Common
{
	public class Comm_UserDatas
	{
		private static Comm_UserDatas _instance;

		private Dictionary<int, Mem_ship> _user_ship;

		private Dictionary<int, Mem_slotitem> _user_slot;

		private Dictionary<int, Mem_deck> _user_deck;

		private Dictionary<int, Mem_esccort_deck> _user_escortdeck;

		private Dictionary<enumMaterialCategory, Mem_material> _user_material;

		private Dictionary<int, Mem_useitem> _user_useItem;

		private Mem_basic _user_basic;

		private Mem_record _user_record;

		private Dictionary<int, Mem_ndock> _user_ndock;

		private Dictionary<int, Mem_kdock> _user_kdock;

		private Dictionary<int, Mem_room> _user_room;

		private Mem_turn _user_turn;

		private Dictionary<int, Mem_book> _ship_book;

		private Dictionary<int, Mem_book> _slot_book;

		private Dictionary<int, Mem_mapcomp> _user_mapcomp;

		private Dictionary<int, Mem_mapclear> _user_mapclear;

		private Dictionary<int, Mem_missioncomp> _user_missioncomp;

		private Dictionary<int, Mem_furniture> _user_furniture;

		private Dictionary<int, Mem_quest> _user_quest;

		private Dictionary<int, Mem_tanker> _user_tanker;

		private Dictionary<int, Mem_rebellion_point> _user_rebellion_point;

		private Mem_deckpractice _user_deckpractice;

		private Dictionary<int, Mem_questcount> _user_questcount;

		private HashSet<int> _temp_escortship;

		private HashSet<int> _temp_deckship;

		private Dictionary<int, List<Mem_history>> _user_history;

		private Mem_trophy _user_trophy;

		private Mem_newgame_plus _user_plus;

		public static Comm_UserDatas Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Comm_UserDatas();
				}
				return _instance;
			}
			private set
			{
				_instance = value;
			}
		}

		public Dictionary<int, Mem_ship> User_ship
		{
			get
			{
				return _user_ship;
			}
			private set
			{
				_user_ship = value;
			}
		}

		public Dictionary<int, Mem_slotitem> User_slot
		{
			get
			{
				return _user_slot;
			}
			private set
			{
				_user_slot = value;
			}
		}

		public Dictionary<int, Mem_deck> User_deck
		{
			get
			{
				return _user_deck;
			}
			private set
			{
				_user_deck = value;
			}
		}

		public Dictionary<int, Mem_esccort_deck> User_EscortDeck
		{
			get
			{
				return _user_escortdeck;
			}
			private set
			{
				_user_escortdeck = value;
			}
		}

		public Dictionary<enumMaterialCategory, Mem_material> User_material
		{
			get
			{
				return _user_material;
			}
			private set
			{
				_user_material = value;
			}
		}

		public Dictionary<int, Mem_useitem> User_useItem
		{
			get
			{
				return _user_useItem;
			}
			private set
			{
				_user_useItem = value;
			}
		}

		public Mem_basic User_basic
		{
			get
			{
				return _user_basic;
			}
			private set
			{
				_user_basic = value;
			}
		}

		public Mem_record User_record
		{
			get
			{
				return _user_record;
			}
			private set
			{
				_user_record = value;
			}
		}

		public Dictionary<int, Mem_ndock> User_ndock
		{
			get
			{
				return _user_ndock;
			}
			private set
			{
				_user_ndock = value;
			}
		}

		public Dictionary<int, Mem_kdock> User_kdock
		{
			get
			{
				return _user_kdock;
			}
			private set
			{
				_user_kdock = value;
			}
		}

		public Dictionary<int, Mem_room> User_room
		{
			get
			{
				return _user_room;
			}
			private set
			{
				_user_room = value;
			}
		}

		public Mem_turn User_turn
		{
			get
			{
				return _user_turn;
			}
			private set
			{
				_user_turn = value;
			}
		}

		public Dictionary<int, Mem_book> Ship_book
		{
			get
			{
				return _ship_book;
			}
			private set
			{
				_ship_book = value;
			}
		}

		public Dictionary<int, Mem_book> Slot_book
		{
			get
			{
				return _slot_book;
			}
			private set
			{
				_slot_book = value;
			}
		}

		public Dictionary<int, Mem_mapcomp> User_mapcomp
		{
			get
			{
				return _user_mapcomp;
			}
			private set
			{
				_user_mapcomp = value;
			}
		}

		public Dictionary<int, Mem_mapclear> User_mapclear
		{
			get
			{
				return _user_mapclear;
			}
			private set
			{
				_user_mapclear = value;
			}
		}

		public Dictionary<int, Mem_missioncomp> User_missioncomp
		{
			get
			{
				return _user_missioncomp;
			}
			private set
			{
				_user_missioncomp = value;
			}
		}

		public Dictionary<int, Mem_furniture> User_furniture
		{
			get
			{
				return _user_furniture;
			}
			private set
			{
				_user_furniture = value;
			}
		}

		public Dictionary<int, Mem_quest> User_quest
		{
			get
			{
				return _user_quest;
			}
			private set
			{
				_user_quest = value;
			}
		}

		public Dictionary<int, Mem_tanker> User_tanker
		{
			get
			{
				return _user_tanker;
			}
			private set
			{
				_user_tanker = value;
			}
		}

		public Dictionary<int, Mem_rebellion_point> User_rebellion_point
		{
			get
			{
				return _user_rebellion_point;
			}
			private set
			{
				_user_rebellion_point = value;
			}
		}

		public Mem_deckpractice User_deckpractice
		{
			get
			{
				return _user_deckpractice;
			}
			private set
			{
				_user_deckpractice = value;
			}
		}

		public Dictionary<int, Mem_questcount> User_questcount
		{
			get
			{
				return _user_questcount;
			}
			private set
			{
				_user_questcount = value;
			}
		}

		public HashSet<int> Temp_escortship
		{
			get
			{
				return _temp_escortship;
			}
			private set
			{
				_temp_escortship = value;
			}
		}

		public HashSet<int> Temp_deckship
		{
			get
			{
				return _temp_deckship;
			}
			private set
			{
				_temp_deckship = value;
			}
		}

		public Dictionary<int, List<Mem_history>> User_history
		{
			get
			{
				return _user_history;
			}
			set
			{
				_user_history = value;
			}
		}

		public Mem_trophy User_trophy
		{
			get
			{
				return _user_trophy;
			}
			private set
			{
				_user_trophy = value;
			}
		}

		public Mem_newgame_plus User_plus
		{
			get
			{
				return _user_plus;
			}
			private set
			{
				_user_plus = value;
			}
		}

		private Comm_UserDatas()
		{
			User_ship = new Dictionary<int, Mem_ship>();
			User_deck = new Dictionary<int, Mem_deck>(8);
			User_EscortDeck = new Dictionary<int, Mem_esccort_deck>(20);
			User_slot = new Dictionary<int, Mem_slotitem>();
			User_material = new Dictionary<enumMaterialCategory, Mem_material>();
			User_useItem = new Dictionary<int, Mem_useitem>();
			User_ndock = new Dictionary<int, Mem_ndock>();
			User_kdock = new Dictionary<int, Mem_kdock>(4);
			User_tanker = new Dictionary<int, Mem_tanker>();
			Ship_book = new Dictionary<int, Mem_book>();
			Slot_book = new Dictionary<int, Mem_book>();
			User_mapcomp = new Dictionary<int, Mem_mapcomp>();
			User_mapclear = new Dictionary<int, Mem_mapclear>();
			User_missioncomp = new Dictionary<int, Mem_missioncomp>();
			User_furniture = new Dictionary<int, Mem_furniture>();
			User_quest = new Dictionary<int, Mem_quest>();
			User_questcount = new Dictionary<int, Mem_questcount>();
			User_rebellion_point = new Dictionary<int, Mem_rebellion_point>();
			User_room = new Dictionary<int, Mem_room>();
			Temp_escortship = new HashSet<int>();
			Temp_deckship = new HashSet<int>();
			User_history = new Dictionary<int, List<Mem_history>>();
		}

		public bool SetUserData()
		{
			XElement elements = VitaSaveManager.Instance.Elements;
			if (elements == null)
			{
				return false;
			}
			Mem_basic user_basic = Model_Base.SetUserData<Mem_basic>(elements.Elements(Mem_basic.tableName).First());
			Mem_record user_record = Model_Base.SetUserData<Mem_record>(elements.Elements(Mem_record.tableName).First());
			Mem_turn user_turn = Model_Base.SetUserData<Mem_turn>(elements.Elements(Mem_turn.tableName).First());
			Mem_deckpractice user_deckpractice = Model_Base.SetUserData<Mem_deckpractice>(elements.Elements(Mem_deckpractice.tableName).First());
			XElement xElement = elements.Elements(Mem_trophy.tableName).FirstOrDefault();
			Mem_trophy user_trophy = (xElement != null) ? Model_Base.SetUserData<Mem_trophy>(xElement) : new Mem_trophy();
			XElement xElement2 = elements.Elements(Mem_newgame_plus.tableName).FirstOrDefault();
			Mem_newgame_plus user_plus = (xElement2 != null) ? Model_Base.SetUserData<Mem_newgame_plus>(xElement2) : new Mem_newgame_plus();
			User_basic = null;
			User_basic = user_basic;
			User_record = null;
			User_record = user_record;
			User_turn = null;
			User_turn = user_turn;
			User_deckpractice = null;
			User_deckpractice = user_deckpractice;
			User_trophy = user_trophy;
			User_plus = user_plus;
			Dictionary<int, Mem_book> dictionary = new Dictionary<int, Mem_book>();
			Dictionary<int, Mem_book> dictionary2 = new Dictionary<int, Mem_book>();
			Dictionary<int, Mem_deck> dictionary3 = new Dictionary<int, Mem_deck>();
			Dictionary<int, Mem_esccort_deck> dictionary4 = new Dictionary<int, Mem_esccort_deck>();
			Dictionary<int, Mem_furniture> dictionary5 = new Dictionary<int, Mem_furniture>();
			Dictionary<int, Mem_kdock> dictionary6 = new Dictionary<int, Mem_kdock>();
			Dictionary<int, Mem_mapcomp> dictionary7 = new Dictionary<int, Mem_mapcomp>();
			Dictionary<int, Mem_mapclear> dictionary8 = new Dictionary<int, Mem_mapclear>();
			Dictionary<enumMaterialCategory, Mem_material> dictionary9 = new Dictionary<enumMaterialCategory, Mem_material>();
			Dictionary<int, Mem_missioncomp> dictionary10 = new Dictionary<int, Mem_missioncomp>();
			Dictionary<int, Mem_ndock> dictionary11 = new Dictionary<int, Mem_ndock>();
			Dictionary<int, Mem_quest> dictionary12 = new Dictionary<int, Mem_quest>();
			Dictionary<int, Mem_questcount> dictionary13 = new Dictionary<int, Mem_questcount>();
			Dictionary<int, Mem_ship> dictionary14 = new Dictionary<int, Mem_ship>();
			Dictionary<int, Mem_slotitem> dictionary15 = new Dictionary<int, Mem_slotitem>();
			Dictionary<int, Mem_tanker> dictionary16 = new Dictionary<int, Mem_tanker>();
			Dictionary<int, Mem_useitem> dictionary17 = new Dictionary<int, Mem_useitem>();
			Dictionary<int, Mem_rebellion_point> dictionary18 = new Dictionary<int, Mem_rebellion_point>();
			Dictionary<int, Mem_room> dictionary19 = new Dictionary<int, Mem_room>();
			HashSet<int> hashSet = new HashSet<int>();
			HashSet<int> hashSet2 = new HashSet<int>();
			List<Mem_history> list = new List<Mem_history>();
			foreach (XElement item2 in elements.Elements("ship_books").Elements("mem_book"))
			{
				Mem_book mem_book = Model_Base.SetUserData<Mem_book>(item2);
				dictionary.Add(mem_book.Table_id, mem_book);
			}
			Ship_book.Clear();
			Ship_book = dictionary;
			foreach (XElement item3 in elements.Elements("slot_books").Elements("mem_book"))
			{
				Mem_book mem_book2 = Model_Base.SetUserData<Mem_book>(item3);
				dictionary2.Add(mem_book2.Table_id, mem_book2);
			}
			Slot_book.Clear();
			Slot_book = dictionary2;
			foreach (XElement item4 in elements.Elements(Mem_deck.tableName + "s").Elements(Mem_deck.tableName))
			{
				Mem_deck mem_deck = Model_Base.SetUserData<Mem_deck>(item4);
				dictionary3.Add(mem_deck.Rid, mem_deck);
			}
			User_deck.Clear();
			User_deck = dictionary3;
			foreach (XElement item5 in elements.Elements(Mem_esccort_deck.tableName + "s").Elements(Mem_esccort_deck.tableName))
			{
				Mem_esccort_deck mem_esccort_deck = Model_Base.SetUserData<Mem_esccort_deck>(item5);
				dictionary4.Add(mem_esccort_deck.Rid, mem_esccort_deck);
			}
			User_EscortDeck.Clear();
			User_EscortDeck = dictionary4;
			foreach (XElement item6 in elements.Elements(Mem_furniture.tableName + "s").Elements(Mem_furniture.tableName))
			{
				Mem_furniture mem_furniture = Model_Base.SetUserData<Mem_furniture>(item6);
				dictionary5.Add(mem_furniture.Rid, mem_furniture);
			}
			User_furniture.Clear();
			User_furniture = dictionary5;
			foreach (XElement item7 in elements.Elements(Mem_kdock.tableName + "s").Elements(Mem_kdock.tableName))
			{
				Mem_kdock mem_kdock = Model_Base.SetUserData<Mem_kdock>(item7);
				dictionary6.Add(mem_kdock.Rid, mem_kdock);
			}
			User_kdock.Clear();
			User_kdock = dictionary6;
			foreach (XElement item8 in elements.Elements(Mem_mapcomp.tableName + "s").Elements(Mem_mapcomp.tableName))
			{
				Mem_mapcomp mem_mapcomp = Model_Base.SetUserData<Mem_mapcomp>(item8);
				dictionary7.Add(mem_mapcomp.Rid, mem_mapcomp);
			}
			User_mapcomp.Clear();
			User_mapcomp = dictionary7;
			foreach (XElement item9 in elements.Elements(Mem_mapclear.tableName + "s").Elements(Mem_mapclear.tableName))
			{
				Mem_mapclear mem_mapclear = Model_Base.SetUserData<Mem_mapclear>(item9);
				dictionary8.Add(mem_mapclear.Rid, mem_mapclear);
			}
			User_mapclear.Clear();
			User_mapclear = dictionary8;
			foreach (XElement item10 in elements.Elements(Mem_material.tableName + "s").Elements(Mem_material.tableName))
			{
				Mem_material mem_material = Model_Base.SetUserData<Mem_material>(item10);
				dictionary9.Add(mem_material.Rid, mem_material);
			}
			User_material.Clear();
			User_material = dictionary9;
			foreach (XElement item11 in elements.Elements(Mem_missioncomp.tableName + "s").Elements(Mem_missioncomp.tableName))
			{
				Mem_missioncomp mem_missioncomp = Model_Base.SetUserData<Mem_missioncomp>(item11);
				dictionary10.Add(mem_missioncomp.Rid, mem_missioncomp);
			}
			User_missioncomp.Clear();
			User_missioncomp = dictionary10;
			foreach (XElement item12 in elements.Elements(Mem_ndock.tableName + "s").Elements(Mem_ndock.tableName))
			{
				Mem_ndock mem_ndock = Model_Base.SetUserData<Mem_ndock>(item12);
				dictionary11.Add(mem_ndock.Rid, mem_ndock);
			}
			User_ndock.Clear();
			User_ndock = dictionary11;
			foreach (XElement item13 in elements.Elements(Mem_quest.tableName + "s").Elements(Mem_quest.tableName))
			{
				Mem_quest mem_quest = Model_Base.SetUserData<Mem_quest>(item13);
				dictionary12.Add(mem_quest.Rid, mem_quest);
			}
			User_quest.Clear();
			User_quest = dictionary12;
			foreach (XElement item14 in elements.Elements(Mem_questcount.tableName + "s").Elements(Mem_questcount.tableName))
			{
				Mem_questcount mem_questcount = Model_Base.SetUserData<Mem_questcount>(item14);
				dictionary13.Add(mem_questcount.Rid, mem_questcount);
			}
			User_questcount.Clear();
			User_questcount = dictionary13;
			foreach (XElement item15 in elements.Elements(Mem_slotitem.tableName + "s").Elements(Mem_slotitem.tableName))
			{
				Mem_slotitem mem_slotitem = Model_Base.SetUserData<Mem_slotitem>(item15);
				dictionary15.Add(mem_slotitem.Rid, mem_slotitem);
			}
			User_slot.Clear();
			User_slot = dictionary15;
			foreach (XElement item16 in elements.Elements(Mem_ship.tableName + "s").Elements(Mem_ship.tableName))
			{
				Mem_ship mem_ship = Model_Base.SetUserData<Mem_ship>(item16);
				dictionary14.Add(mem_ship.Rid, mem_ship);
			}
			User_ship.Clear();
			User_ship = dictionary14;
			foreach (XElement item17 in elements.Elements(Mem_tanker.tableName + "s").Elements(Mem_tanker.tableName))
			{
				Mem_tanker mem_tanker = Model_Base.SetUserData<Mem_tanker>(item17);
				dictionary16.Add(mem_tanker.Rid, mem_tanker);
			}
			User_tanker.Clear();
			User_tanker = dictionary16;
			foreach (XElement item18 in elements.Elements(Mem_useitem.tableName + "s").Elements(Mem_useitem.tableName))
			{
				Mem_useitem mem_useitem = Model_Base.SetUserData<Mem_useitem>(item18);
				dictionary17.Add(mem_useitem.Rid, mem_useitem);
			}
			User_useItem.Clear();
			User_useItem = dictionary17;
			foreach (XElement item19 in elements.Elements(Mem_rebellion_point.tableName + "s").Elements(Mem_rebellion_point.tableName))
			{
				Mem_rebellion_point mem_rebellion_point = Model_Base.SetUserData<Mem_rebellion_point>(item19);
				dictionary18.Add(mem_rebellion_point.Rid, mem_rebellion_point);
			}
			User_rebellion_point.Clear();
			User_rebellion_point = dictionary18;
			foreach (XElement item20 in elements.Elements(Mem_room.tableName + "s").Elements(Mem_room.tableName))
			{
				Mem_room mem_room = Model_Base.SetUserData<Mem_room>(item20);
				dictionary19.Add(mem_room.Rid, mem_room);
			}
			User_room.Clear();
			User_room = dictionary19;
			foreach (XElement item21 in elements.Element("temp_escortships").Elements())
			{
				string value = item21.Value;
				hashSet.Add(int.Parse(value));
			}
			Temp_escortship.Clear();
			Temp_escortship = hashSet;
			foreach (XElement item22 in elements.Element("temp_deckships").Elements())
			{
				string value2 = item22.Value;
				hashSet2.Add(int.Parse(value2));
			}
			Temp_deckship.Clear();
			Temp_deckship = hashSet2;
			foreach (XElement item23 in elements.Elements(Mem_history.tableName + "s").Elements(Mem_history.tableName))
			{
				Mem_history item = Model_Base.SetUserData<Mem_history>(item23);
				list.Add(item);
			}
			User_history.Clear();
			list.ForEach(delegate(Mem_history x)
			{
				Add_History(x);
			});
			return true;
		}

		public void PurgeUserData(ICreateNewUser createInstance, bool plusGame)
		{
			if (!plusGame)
			{
				User_trophy = null;
				User_plus = null;
			}
			User_basic = null;
			User_record = null;
			Ship_book.Clear();
			Slot_book.Clear();
			User_ship.Clear();
			User_slot.Clear();
			User_useItem.Clear();
			User_turn = null;
			User_deckpractice = null;
			User_deck.Clear();
			User_EscortDeck.Clear();
			User_furniture.Clear();
			User_ndock.Clear();
			User_kdock.Clear();
			User_mapcomp.Clear();
			User_mapclear.Clear();
			User_material.Clear();
			User_missioncomp.Clear();
			User_quest.Clear();
			User_questcount.Clear();
			User_tanker.Clear();
			User_rebellion_point.Clear();
			User_room.Clear();
			Temp_escortship.Clear();
			Temp_deckship.Clear();
			User_history.Clear();
		}

		public bool CreateNewUser(ICreateNewUser createInstance, DifficultKind difficult, int firstShip)
		{
			if (User_basic != null || createInstance == null)
			{
				return false;
			}
			User_basic = new Mem_basic();
			User_basic.SetDifficult(difficult);
			User_record = new Mem_record();
			User_turn = new Mem_turn();
			User_trophy = new Mem_trophy();
			User_plus = new Mem_newgame_plus();
			User_deckpractice = new Mem_deckpractice();
			if (User_ndock.Count == 0)
			{
				Add_Ndock(1);
				Add_Ndock(1);
			}
			if (User_kdock.Count == 0)
			{
				Add_Kdock();
				Add_Kdock();
			}
			initMaterials(difficult);
			Add_Deck(1);
			List<int> list = Instance.Add_Ship(new List<int>
			{
				firstShip
			});
			Instance.User_deck[1].Ship[0] = list[0];
			List<Mst_furniture> furnitureDatas = User_room[1].getFurnitureDatas();
			Mem_furniture furniture = null;
			furnitureDatas.ForEach(delegate(Mst_furniture x)
			{
				furniture = new Mem_furniture(x.Id);
				User_furniture.Add(furniture.Rid, furniture);
			});
			Add_Slot(new List<int>
			{
				42,
				43
			});
			User_quest = new Dictionary<int, Mem_quest>();
			foreach (int key in Mst_DataManager.Instance.Mst_maparea.Keys)
			{
				Add_EscortDeck(key, key);
			}
			initTanker();
			UpdateDeckShipLocale();
			return true;
		}

		public bool NewGamePlus(ICreateNewUser createInstance, string nickName, DifficultKind selectedRank, int firstShipId)
		{
			if (createInstance == null)
			{
				return false;
			}
			bool flag = Utils.IsGameClear();
			List<DifficultKind> kind = User_record.ClearDifficult.ToList();
			PurgeUserData(createInstance, plusGame: true);
			if (flag)
			{
				Add_Useitem(55, 1);
			}
			foreach (Mem_book item in User_plus.Ship_book)
			{
				Ship_book.Add(item.Table_id, item);
			}
			foreach (Mem_book item2 in User_plus.Slot_book)
			{
				Slot_book.Add(item2.Table_id, item2);
			}
			User_basic = new Mem_basic();
			User_basic.UpdateNickName(nickName);
			User_basic.SetDifficult(selectedRank);
			User_record = new Mem_record(createInstance, User_plus, kind);
			User_turn = new Mem_turn();
			User_deckpractice = new Mem_deckpractice();
			if (User_ndock.Count == 0)
			{
				Add_Ndock(1);
				Add_Ndock(1);
			}
			if (User_kdock.Count == 0)
			{
				Add_Kdock();
				Add_Kdock();
			}
			initMaterials(selectedRank);
			Add_Deck(1);
			User_room[1].getFurnitureDatas();
			foreach (Mem_furniture item3 in User_plus.Furniture)
			{
				User_furniture.Add(item3.Rid, item3);
			}
			foreach (Mem_slotitem item4 in User_plus.Slotitem)
			{
				User_slot.Add(item4.Rid, item4);
			}
			foreach (Mem_shipBase item5 in User_plus.Ship)
			{
				Mem_ship mem_ship = new Mem_ship();
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[item5.Ship_id];
				mem_ship.Set_ShipParam(item5, mst_data, enemy_flag: false);
				mem_ship.Set_ShipParamNewGamePlus(createInstance);
				User_ship.Add(mem_ship.Rid, mem_ship);
			}
			List<int> list = Add_Ship(new List<int>
			{
				firstShipId
			});
			Instance.User_deck[1].Ship[0] = list[0];
			User_quest = new Dictionary<int, Mem_quest>();
			foreach (int key in Mst_DataManager.Instance.Mst_maparea.Keys)
			{
				Add_EscortDeck(key, key);
			}
			initTanker();
			UpdateDeckShipLocale();
			return true;
		}

		public void InitQuest(IQuestOperator instance, List<Mst_quest> mst_quset)
		{
			if (User_quest.Count == 0 && instance != null)
			{
				User_quest = Mem_quest.GetData(mst_quset);
			}
		}

		private void initMaterials(DifficultKind difficult)
		{
			if (User_material == null)
			{
				User_material = new Dictionary<enumMaterialCategory, Mem_material>();
			}
			else
			{
				User_material.Clear();
			}
			int value = 1500;
			switch (difficult)
			{
			case DifficultKind.KOU:
				value = 2000;
				break;
			case DifficultKind.OTU:
				value = 3000;
				break;
			case DifficultKind.HEI:
				value = 3000;
				break;
			case DifficultKind.TEI:
				value = 6000;
				break;
			}
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, value);
			dictionary.Add(enumMaterialCategory.Bull, 3000);
			dictionary.Add(enumMaterialCategory.Steel, 3000);
			dictionary.Add(enumMaterialCategory.Bauxite, 3000);
			dictionary.Add(enumMaterialCategory.Build_Kit, 5);
			dictionary.Add(enumMaterialCategory.Repair_Kit, 5);
			dictionary.Add(enumMaterialCategory.Dev_Kit, 10);
			dictionary.Add(enumMaterialCategory.Revamp_Kit, 0);
			Dictionary<enumMaterialCategory, int> dictionary2 = dictionary;
			foreach (KeyValuePair<enumMaterialCategory, int> item in dictionary2)
			{
				enumMaterialCategory key = item.Key;
				int value2 = item.Value;
				Mem_material value3 = new Mem_material(key, value2);
				User_material.Add(key, value3);
			}
		}

		private void initTanker()
		{
			if (User_tanker == null)
			{
				User_tanker = new Dictionary<int, Mem_tanker>();
			}
			else
			{
				User_tanker.Clear();
			}
			Add_Tanker(4);
			foreach (Mem_tanker value in User_tanker.Values)
			{
				value.GoArea(1);
			}
			Add_Tanker(2);
		}

		public List<int> Add_Slot(List<int> slot_ids)
		{
			if (slot_ids == null)
			{
				return null;
			}
			List<int> ret_rids = new List<int>();
			int nextSortNo = getNextSortNo(User_slot.Values, slot_ids.Count);
			slot_ids.ForEach(delegate(int x)
			{
				Mem_slotitem mem_slotitem = new Mem_slotitem();
				int newRid = getNewRid(User_slot.Keys.ToList());
				if (mem_slotitem.Set_New_SlotData(newRid, nextSortNo, x))
				{
					nextSortNo++;
					User_slot.Add(newRid, mem_slotitem);
					ret_rids.Add(newRid);
					Add_Book(2, mem_slotitem.Slotitem_id);
				}
			});
			return ret_rids;
		}

		public List<int> Add_Ship(List<int> ship_ids)
		{
			if (ship_ids == null)
			{
				return null;
			}
			List<int> ret_rids = new List<int>();
			int nextSortNo = getNextSortNo(User_ship.Values, ship_ids.Count);
			ship_ids.ForEach(delegate(int x)
			{
				Mem_ship mem_ship = new Mem_ship();
				int newRid = getNewRid(User_ship.Keys.ToList());
				if (mem_ship.Set_New_ShipData(newRid, nextSortNo, x))
				{
					nextSortNo++;
					User_ship.Add(newRid, mem_ship);
					ret_rids.Add(newRid);
					Add_Book(1, mem_ship.Ship_id);
				}
			});
			return ret_rids;
		}

		public List<int> Add_Tanker(int num)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < num; i++)
			{
				int newRid = getNewRid(User_tanker.Keys.ToList());
				Mem_tanker value = new Mem_tanker(newRid);
				User_tanker.Add(newRid, value);
				list.Add(newRid);
			}
			return list;
		}

		public void Remove_Tanker(int rid)
		{
			User_tanker.Remove(rid);
		}

		public bool Add_EscortDeck(int rid, int area_id)
		{
			if (rid > 20 || User_EscortDeck.ContainsKey(rid))
			{
				return false;
			}
			Mem_esccort_deck value = new Mem_esccort_deck(rid, area_id);
			User_EscortDeck.Add(rid, value);
			return true;
		}

		public Mem_book Add_Book(int type, int mst_id)
		{
			Dictionary<int, Mem_book> dictionary = (type != 1) ? Slot_book : Ship_book;
			Mem_book value = null;
			if (dictionary.TryGetValue(mst_id, out value))
			{
				return value;
			}
			value = new Mem_book(type, mst_id);
			if (type != 1)
			{
				dictionary.Add(mst_id, value);
				return value;
			}
			string yomi = Mst_DataManager.Instance.Mst_ship[value.Table_id].Yomi;
			if (Ship_book.Values.Any((Mem_book x) => (Mst_DataManager.Instance.Mst_ship[x.Table_id].Yomi.Equals(yomi) && x.Flag2 == 1) ? true : false))
			{
				value.UpdateShipBook(damage: true, mariage: false);
			}
			dictionary.Add(mst_id, value);
			return value;
		}

		public Mem_ndock Add_Ndock(int area_id)
		{
			int no = (from data in User_ndock.Values
				where data.Area_id == area_id
				select data).Count() + 1;
			int num = int.Parse(area_id.ToString() + no.ToString());
			Mem_ndock mem_ndock = new Mem_ndock(num, area_id, no);
			User_ndock.Add(num, mem_ndock);
			return mem_ndock;
		}

		public Mem_kdock Add_Kdock()
		{
			if (User_kdock.Count >= 4)
			{
				return null;
			}
			int num = User_kdock.Count + 1;
			Mem_kdock mem_kdock = new Mem_kdock(num);
			User_kdock.Add(num, mem_kdock);
			return mem_kdock;
		}

		public void Remove_Ship(List<int> ship_ids)
		{
			ship_ids?.ForEach(delegate(int x)
			{
				Remove_Slot(User_ship[x].Slot);
				User_ship.Remove(x);
			});
		}

		public void Remove_Ship(List<Mem_ship> ships)
		{
			ships?.ForEach(delegate(Mem_ship x)
			{
				Remove_Slot(x.Slot);
				if (x.IsOpenExSlot())
				{
					Remove_Slot(new List<int>
					{
						x.Exslot
					});
				}
				User_ship.Remove(x.Rid);
			});
		}

		public void Remove_Slot(List<int> slot_ids)
		{
			slot_ids.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					User_slot.Remove(x);
				}
			});
		}

		public void Add_Deck(int rid)
		{
			if (rid <= 8 && !User_deck.ContainsKey(rid))
			{
				Mem_deck value = new Mem_deck(rid);
				User_deck.Add(rid, value);
				Mem_room value2 = new Mem_room(rid);
				User_room.Add(rid, value2);
			}
		}

		public void Add_Useitem(int rid, int count)
		{
			if (Mst_DataManager.Instance.Mst_useitem[rid].Usetype == 6 && Mst_DataManager.Instance.Mst_useitem[rid].Category == 21)
			{
				User_basic.AddCoin(count);
				return;
			}
			Mem_useitem value = null;
			if (!User_useItem.TryGetValue(rid, out value))
			{
				value = new Mem_useitem(rid, count);
				User_useItem.Add(value.Rid, value);
			}
			else
			{
				value.Add_UseItem(count);
			}
		}

		public bool Add_Furniture(int furnitureId)
		{
			if (User_furniture.ContainsKey(furnitureId))
			{
				return false;
			}
			User_furniture.Add(furnitureId, new Mem_furniture(furnitureId));
			return true;
		}

		public void Add_History(Mem_history history)
		{
			List<Mem_history> value = null;
			if (!User_history.TryGetValue(history.Type, out value))
			{
				value = new List<Mem_history>();
				value.Add(history);
				User_history.Add(history.Type, value);
			}
			else
			{
				value.Add(history);
			}
		}

		public void UpdateEscortShipLocale()
		{
			Temp_escortship.Clear();
			foreach (Mem_esccort_deck value in Instance.User_EscortDeck.Values)
			{
				for (int i = 0; i < value.Ship.Count(); i++)
				{
					Temp_escortship.Add(value.Ship[i]);
				}
			}
		}

		public void UpdateDeckShipLocale()
		{
			Temp_deckship.Clear();
			foreach (Mem_deck value in Instance.User_deck.Values)
			{
				for (int i = 0; i < value.Ship.Count(); i++)
				{
					Temp_deckship.Add(value.Ship[i]);
				}
			}
		}

		public void UpdateShipBookBrokenClothState(List<int> targetShipIds)
		{
			HashSet<string> check_yomi = new HashSet<string>();
			targetShipIds.ForEach(delegate(int x)
			{
				check_yomi.Add(Mst_DataManager.Instance.Mst_ship[x].Yomi);
			});
			if (check_yomi.Count != 0)
			{
				List<Mst_ship> list = (from y in Mst_DataManager.Instance.Mst_ship.Values
					where check_yomi.Contains(y.Yomi)
					select y).ToList();
				list.ForEach(delegate(Mst_ship up_item)
				{
					Mem_book value = null;
					if (Instance.Ship_book.TryGetValue(up_item.Id, out value))
					{
						value.UpdateShipBook(damage: true, mariage: false);
					}
				});
			}
		}

		private int getNewRid(List<int> target)
		{
			target.Sort((int x, int y) => x.CompareTo(y));
			int i;
			for (i = 1; i <= target.Count(); i++)
			{
				if (target[i - 1] != i)
				{
					return i;
				}
			}
			return i;
		}

		private int getNextSortNo<T>(IEnumerable<T> targets, int addRecordNum) where T : IReqNewGetNo
		{
			if (targets.Count() == 0)
			{
				return 1;
			}
			List<T> list = targets.ToList();
			list.Sort((T x, T y) => x.GetSortNo().CompareTo(y.GetSortNo()));
			int num = list.Last().GetSortNo() + 1;
			int num2 = num + (addRecordNum - 1);
			int num3 = 10000;
			if (num < num3 && num2 < num3)
			{
				return num;
			}
			int num4 = 1;
			foreach (T item in list)
			{
				item.ChangeSortNo(num4);
				num4++;
			}
			return num4;
		}
	}
}
