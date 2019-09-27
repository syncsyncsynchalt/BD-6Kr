using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_DataManager
	{
		private delegate KeyValuePair<string, IEnumerable<XElement>> masterAsyncDelegate(string mst_name);

		private static Mst_DataManager _instance;

		private Dictionary<int, Mst_ship> _mst_ship;

		private Dictionary<int, Mst_slotitem> _mst_slotitem;

		private Dictionary<int, Mst_slotitem_cost> _mst_slotitem_cost;

		private Dictionary<int, Mst_maparea> _mst_maparea;

		private Dictionary<int, Mst_mapinfo> _mst_mapinfo;

		private Dictionary<int, Mst_mapcell2> _mst_mapcell;

		private Dictionary<int, Mst_mapenemy2> _mst_mapenemy;

		private IEnumerable<XElement> _mst_shipget;

		private Dictionary<int, Mst_useitem> _mst_useitem;

		private Dictionary<int, Mst_stype> _mst_stype;

		private Dictionary<int, Mst_mission2> _mst_mission;

		private Dictionary<int, Mst_shipupgrade> _mst_upgrade;

		private Dictionary<int, Mst_furniture> _mst_furniture;

		private Dictionary<int, Mst_shipgraph> _mst_shipgraph;

		private Dictionary<int, Mst_item_limit> _mst_item_limit;

		private Dictionary<int, Mst_ship_resources> _mst_ship_resources;

		private Dictionary<int, Mst_equip_category> _mst_equip_category;

		private Dictionary<int, Mst_equip_ship> _mst_equip_ship;

		private Dictionary<int, Mst_shipgraphbattle> _mst_shipgraphbattle;

		private Dictionary<MstConstDataIndex, Mst_const> _mst_const;

		private Dictionary<int, Mst_questcount> _mst_questcount;

		private Dictionary<int, Mst_rebellionpoint> _mst_rebellionpoint;

		private Dictionary<int, Mst_bgm_jukebox> _mst_jukebox;

		private Dictionary<int, int> _mst_bgm_season;

		private Dictionary<int, List<Mst_radingtype>> _mst_RadingType;

		private Dictionary<int, Dictionary<int, Mst_radingrate>> _mst_RadingRate;

		private UIBattleRequireMaster _uiBattleMaster;

		private int callCount;

		private int isMasterInit;

		private Action initMasterCallback;

		private Dictionary<string, IEnumerable<XElement>> startMasterElement;

		private object lockObj = new object();

		public static Mst_DataManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Mst_DataManager();
				}
				return _instance;
			}
			private set
			{
				_instance = value;
			}
		}

		public Dictionary<int, Mst_ship> Mst_ship
		{
			get
			{
				return _mst_ship;
			}
			private set
			{
				_mst_ship = value;
			}
		}

		public Dictionary<int, Mst_slotitem> Mst_Slotitem
		{
			get
			{
				return _mst_slotitem;
			}
			private set
			{
				_mst_slotitem = value;
			}
		}

		public Dictionary<int, Mst_slotitem_cost> Mst_slotitem_cost
		{
			get
			{
				return _mst_slotitem_cost;
			}
			private set
			{
				_mst_slotitem_cost = value;
			}
		}

		public Dictionary<int, Mst_maparea> Mst_maparea
		{
			get
			{
				return _mst_maparea;
			}
			private set
			{
				_mst_maparea = value;
			}
		}

		public Dictionary<int, Mst_mapinfo> Mst_mapinfo
		{
			get
			{
				return _mst_mapinfo;
			}
			private set
			{
				_mst_mapinfo = value;
			}
		}

		public Dictionary<int, Mst_mapcell2> Mst_mapcell
		{
			get
			{
				return _mst_mapcell;
			}
			private set
			{
				_mst_mapcell = value;
			}
		}

		public Dictionary<int, Mst_mapenemy2> Mst_mapenemy
		{
			get
			{
				return _mst_mapenemy;
			}
			private set
			{
				_mst_mapenemy = value;
			}
		}

		public IEnumerable<XElement> Mst_shipget
		{
			get
			{
				return _mst_shipget;
			}
			private set
			{
				_mst_shipget = value;
			}
		}

		public Dictionary<int, Mst_useitem> Mst_useitem
		{
			get
			{
				return _mst_useitem;
			}
			private set
			{
				_mst_useitem = value;
			}
		}

		public Dictionary<int, Mst_stype> Mst_stype
		{
			get
			{
				return _mst_stype;
			}
			private set
			{
				_mst_stype = value;
			}
		}

		public Dictionary<int, Mst_mission2> Mst_mission
		{
			get
			{
				return _mst_mission;
			}
			private set
			{
				_mst_mission = value;
			}
		}

		public Dictionary<int, Mst_shipupgrade> Mst_upgrade
		{
			get
			{
				return _mst_upgrade;
			}
			private set
			{
				_mst_upgrade = value;
			}
		}

		public Dictionary<int, Mst_furniture> Mst_furniture
		{
			get
			{
				return _mst_furniture;
			}
			private set
			{
				_mst_furniture = value;
			}
		}

		public Dictionary<int, Mst_shipgraph> Mst_shipgraph
		{
			get
			{
				return _mst_shipgraph;
			}
			private set
			{
				_mst_shipgraph = value;
			}
		}

		public Dictionary<int, Mst_item_limit> Mst_item_limit
		{
			get
			{
				return _mst_item_limit;
			}
			private set
			{
				_mst_item_limit = value;
			}
		}

		public Dictionary<int, Mst_ship_resources> Mst_ship_resources
		{
			get
			{
				return _mst_ship_resources;
			}
			private set
			{
				_mst_ship_resources = value;
			}
		}

		public Dictionary<int, Mst_equip_category> Mst_equip_category
		{
			get
			{
				return _mst_equip_category;
			}
			private set
			{
				_mst_equip_category = value;
			}
		}

		public Dictionary<int, Mst_equip_ship> Mst_equip_ship
		{
			get
			{
				return _mst_equip_ship;
			}
			private set
			{
				_mst_equip_ship = value;
			}
		}

		public Dictionary<int, Mst_shipgraphbattle> Mst_shipgraphbattle
		{
			get
			{
				return _mst_shipgraphbattle;
			}
			private set
			{
				_mst_shipgraphbattle = value;
			}
		}

		public Dictionary<MstConstDataIndex, Mst_const> Mst_const
		{
			get
			{
				return _mst_const;
			}
			private set
			{
				_mst_const = value;
			}
		}

		public Dictionary<int, Mst_questcount> Mst_questcount
		{
			get
			{
				return _mst_questcount;
			}
			private set
			{
				_mst_questcount = value;
			}
		}

		public Dictionary<int, Mst_rebellionpoint> Mst_RebellionPoint
		{
			get
			{
				return _mst_rebellionpoint;
			}
			private set
			{
				_mst_rebellionpoint = value;
			}
		}

		public Dictionary<int, int> Mst_bgm_season
		{
			get
			{
				return _mst_bgm_season;
			}
			private set
			{
				_mst_bgm_season = value;
			}
		}

		public Dictionary<int, List<Mst_radingtype>> Mst_RadingType
		{
			get
			{
				return _mst_RadingType;
			}
			private set
			{
				_mst_RadingType = value;
			}
		}

		public Dictionary<int, Dictionary<int, Mst_radingrate>> Mst_RadingRate
		{
			get
			{
				return _mst_RadingRate;
			}
			private set
			{
				_mst_RadingRate = value;
			}
		}

		public UIBattleRequireMaster UiBattleMaster
		{
			get
			{
				return _uiBattleMaster;
			}
			private set
			{
				_uiBattleMaster = value;
			}
		}

		private Mst_DataManager()
		{
			Utils.initMasterPath();
			Mst_ship = new Dictionary<int, Mst_ship>();
			Mst_ship_resources = new Dictionary<int, Mst_ship_resources>();
			Mst_shipgraph = new Dictionary<int, Mst_shipgraph>();
			Mst_shipgraphbattle = new Dictionary<int, Mst_shipgraphbattle>();
			Mst_Slotitem = new Dictionary<int, Mst_slotitem>();
			Mst_slotitem_cost = new Dictionary<int, Mst_slotitem_cost>();
			Mst_maparea = new Dictionary<int, Mst_maparea>();
			Mst_mapinfo = new Dictionary<int, Mst_mapinfo>();
			Mst_mapcell = new Dictionary<int, Mst_mapcell2>();
			Mst_mapenemy = new Dictionary<int, Mst_mapenemy2>();
			Mst_useitem = new Dictionary<int, Mst_useitem>();
			Mst_stype = new Dictionary<int, Mst_stype>();
			Mst_mission = new Dictionary<int, Mst_mission2>();
			Mst_upgrade = new Dictionary<int, Mst_shipupgrade>();
			Mst_furniture = new Dictionary<int, Mst_furniture>();
			Mst_item_limit = new Dictionary<int, Mst_item_limit>();
			Mst_equip_category = new Dictionary<int, Mst_equip_category>();
			Mst_equip_ship = new Dictionary<int, Mst_equip_ship>();
			Mst_const = new Dictionary<MstConstDataIndex, Mst_const>();
			Mst_questcount = new Dictionary<int, Mst_questcount>();
			Mst_RebellionPoint = new Dictionary<int, Mst_rebellionpoint>();
			_mst_jukebox = new Dictionary<int, Mst_bgm_jukebox>();
			Mst_bgm_season = new Dictionary<int, int>();
			UiBattleMaster = new UIBattleRequireMaster();
			Mst_RadingRate = new Dictionary<int, Dictionary<int, Mst_radingrate>>();
			Mst_RadingType = new Dictionary<int, List<Mst_radingtype>>();
		}

		public void LoadStartMaster(Action callBack)
		{
			if (isMasterInit == 2 && startMasterElement == null)
			{
				callBack();
			}
			else if (isMasterInit == 0)
			{
				initMasterCallback = callBack;
				isMasterInit = 1;
				startMasterElement = new Dictionary<string, IEnumerable<XElement>>();
				loadStartMstData();
			}
		}

		public void SetStartMasterData()
		{
			if (startMasterElement != null)
			{
				Dictionary<string, Action<Model_Base, XElement>> masterSetter = getMasterSetter();
				foreach (KeyValuePair<string, IEnumerable<XElement>> item in startMasterElement)
				{
					string key2 = item.Key;
					if (masterSetter.ContainsKey(key2))
					{
						foreach (XElement item2 in item.Value)
						{
							Model_Base arg = null;
							masterSetter[key2](arg, item2);
						}
						item.Value.Remove();
					}
				}
				Dictionary<int, string> dictionary = startMasterElement["mst_equip"].ToDictionary((XElement x) => int.Parse(x.Element("Ship_type").Value), (XElement y) => y.Element("Equip_type").Value);
				foreach (Mst_stype value2 in Mst_stype.Values)
				{
					string value = null;
					if (dictionary.TryGetValue(value2.Id, out value))
					{
						List<int> equip = Array.ConvertAll(value.Split(','), (string eqp_val) => int.Parse(eqp_val)).ToList();
						value2.SetEquip(equip);
					}
					else
					{
						value2.SetEquip(new List<int>());
					}
				}
				startMasterElement["mst_equip"].Remove();
				dictionary.Clear();
				Mst_bgm_season = startMasterElement["mst_bgm_season"].ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement val) => int.Parse(val.Element("Bgm_id").Value));
				startMasterElement["mst_bgm_season"].Remove();
				startMasterElement.Clear();
				startMasterElement = null;
			}
		}

		private void loadStartMstData()
		{
			string tableDirMaster = Utils.getTableDirMaster("mst_files");
			string uri = tableDirMaster + "mst_files.xml";
			IEnumerable<XElement> source = from file in XElement.Load(uri).Elements("item")
				select (file);
			List<XElement> list = source.ToList();
			int num = callCount = list.Count();
			masterAsyncDelegate masterAsyncDelegate = LoadElements;
			for (int i = 0; i < num; i++)
			{
				string value = list[i].Value;

                var a = masterAsyncDelegate.Invoke(value);
                // masterAsyncDelegate.BeginInvoke(value, loadCompleteAsynch, masterAsyncDelegate);
                // loadCompleteAsynch();
                loadCompleteAsynch2(a);
            }
		}

		private KeyValuePair<string, IEnumerable<XElement>> LoadElements(string name)
		{
			return new KeyValuePair<string, IEnumerable<XElement>>(name, Utils.Xml_Result(name, name, null));
		}

        private void loadCompleteAsynch2(KeyValuePair<string, IEnumerable<XElement>> ret)
        {
            KeyValuePair<string, IEnumerable<XElement>> keyValuePair = ret;
            lock (lockObj)
            {
                startMasterElement.Add(keyValuePair.Key, keyValuePair.Value);
            }
            Interlocked.Decrement(ref callCount);
            if (callCount <= 0)
            {
                isMasterInit = 2;
                initMasterCallback();
                initMasterCallback = null;
            }
        }

        private void loadCompleteAsynch(IAsyncResult ar)
		{
			masterAsyncDelegate masterAsyncDelegate = (masterAsyncDelegate)ar.AsyncState;
			KeyValuePair<string, IEnumerable<XElement>> keyValuePair = masterAsyncDelegate.EndInvoke(ar);
			lock (lockObj)
			{
				startMasterElement.Add(keyValuePair.Key, keyValuePair.Value);
			}
			Interlocked.Decrement(ref callCount);
			if (callCount <= 0)
			{
				isMasterInit = 2;
				initMasterCallback();
				initMasterCallback = null;
			}
		}

		private Dictionary<string, Action<Model_Base, XElement>> getMasterSetter()
		{
			Dictionary<string, Action<Model_Base, XElement>> dictionary = new Dictionary<string, Action<Model_Base, XElement>>();
			dictionary.Add("mst_ship", delegate(Model_Base x, XElement y)
			{
				Mst_ship instance21 = (Mst_ship)x;
				Model_Base.SetMaster(out instance21, y);
				Mst_ship.Add(instance21.Id, instance21);
			});
			dictionary.Add("mst_ship_resources", delegate(Model_Base x, XElement y)
			{
				Mst_ship_resources instance20 = (Mst_ship_resources)x;
				Model_Base.SetMaster(out instance20, y);
				Mst_ship_resources.Add(instance20.Id, instance20);
			});
			dictionary.Add("mst_slotitem", delegate(Model_Base x, XElement y)
			{
				Mst_slotitem instance19 = (Mst_slotitem)x;
				Model_Base.SetMaster(out instance19, y);
				Mst_Slotitem.Add(instance19.Id, instance19);
			});
			dictionary.Add("mst_maparea", delegate(Model_Base x, XElement y)
			{
				Mst_maparea instance18 = (Mst_maparea)x;
				Model_Base.SetMaster(out instance18, y);
				Mst_maparea.Add(instance18.Id, instance18);
			});
			dictionary.Add("mst_mapinfo", delegate(Model_Base x, XElement y)
			{
				Mst_mapinfo instance17 = (Mst_mapinfo)x;
				Model_Base.SetMaster(out instance17, y);
				Mst_mapinfo.Add(instance17.Id, instance17);
			});
			dictionary.Add("mst_useitem", delegate(Model_Base x, XElement y)
			{
				Mst_useitem instance16 = (Mst_useitem)x;
				Model_Base.SetMaster(out instance16, y);
				Mst_useitem.Add(instance16.Id, instance16);
			});
			dictionary.Add("mst_stype", delegate(Model_Base x, XElement y)
			{
				Mst_stype instance15 = (Mst_stype)x;
				Model_Base.SetMaster(out instance15, y);
				Mst_stype.Add(instance15.Id, instance15);
			});
			dictionary.Add("mst_mission2", delegate(Model_Base x, XElement y)
			{
				Mst_mission2 instance14 = (Mst_mission2)x;
				Model_Base.SetMaster(out instance14, y);
				Mst_mission.Add(instance14.Id, instance14);
			});
			dictionary.Add("mst_shipupgrade", delegate(Model_Base x, XElement y)
			{
				Mst_shipupgrade instance13 = (Mst_shipupgrade)x;
				Model_Base.SetMaster(out instance13, y);
				Mst_upgrade.Add(instance13.Id, instance13);
			});
			dictionary.Add("mst_furniture", delegate(Model_Base x, XElement y)
			{
				Mst_furniture instance12 = (Mst_furniture)x;
				Model_Base.SetMaster(out instance12, y);
				Mst_furniture.Add(instance12.Id, instance12);
			});
			dictionary.Add("mst_shipgraph", delegate(Model_Base x, XElement y)
			{
				Mst_shipgraph instance11 = (Mst_shipgraph)x;
				Model_Base.SetMaster(out instance11, y);
				Mst_shipgraph.Add(instance11.Id, instance11);
			});
			dictionary.Add("mst_item_limit", delegate(Model_Base x, XElement y)
			{
				Mst_item_limit instance10 = (Mst_item_limit)x;
				Model_Base.SetMaster(out instance10, y);
				Mst_item_limit.Add(instance10.Id, instance10);
			});
			dictionary.Add("mst_equip_category", delegate(Model_Base x, XElement y)
			{
				Mst_equip_category instance9 = (Mst_equip_category)x;
				Model_Base.SetMaster(out instance9, y);
				Mst_equip_category.Add(instance9.Id, instance9);
			});
			dictionary.Add("mst_equip_ship", delegate(Model_Base x, XElement y)
			{
				Mst_equip_ship instance8 = (Mst_equip_ship)x;
				Model_Base.SetMaster(out instance8, y);
				Mst_equip_ship.Add(instance8.Id, instance8);
			});
			dictionary.Add("mst_shipgraphbattle", delegate(Model_Base x, XElement y)
			{
				Mst_shipgraphbattle instance7 = (Mst_shipgraphbattle)x;
				Model_Base.SetMaster(out instance7, y);
				Mst_shipgraphbattle.Add(instance7.Id, instance7);
			});
			dictionary.Add("mst_const", delegate(Model_Base x, XElement y)
			{
				Mst_const instance6 = (Mst_const)x;
				Model_Base.SetMaster(out instance6, y);
				Mst_const.Add(instance6.Id, instance6);
			});
			dictionary.Add("mst_questcount", delegate(Model_Base x, XElement y)
			{
				Mst_questcount instance5 = (Mst_questcount)x;
				Model_Base.SetMaster(out instance5, y);
				Mst_questcount.Add(instance5.Id, instance5);
			});
			dictionary.Add("mst_rebellionpoint", delegate(Model_Base x, XElement y)
			{
				Mst_rebellionpoint instance4 = (Mst_rebellionpoint)x;
				Model_Base.SetMaster(out instance4, y);
				Mst_RebellionPoint.Add(instance4.Id, instance4);
			});
			dictionary.Add(Mst_bgm_jukebox.tableName, delegate(Model_Base x, XElement y)
			{
				Mst_bgm_jukebox instance3 = (Mst_bgm_jukebox)x;
				Model_Base.SetMaster(out instance3, y);
				_mst_jukebox.Add(instance3.Bgm_id, instance3);
			});
			dictionary.Add(Mst_radingtype.tableName, delegate(Model_Base x, XElement y)
			{
				Mst_radingtype instance2 = (Mst_radingtype)x;
				Model_Base.SetMaster(out instance2, y);
				List<Mst_radingtype> value2 = null;
				if (!Mst_RadingType.TryGetValue(instance2.Difficult, out value2))
				{
					value2 = new List<Mst_radingtype>();
					Mst_RadingType.Add(instance2.Difficult, value2);
				}
				value2.Add(instance2);
			});
			dictionary.Add(Mst_radingrate.tableName, delegate(Model_Base x, XElement y)
			{
				Mst_radingrate instance = (Mst_radingrate)x;
				Model_Base.SetMaster(out instance, y);
				if (!Mst_RadingRate.ContainsKey(instance.Maparea_id))
				{
					Dictionary<int, Mst_radingrate> value = new Dictionary<int, Mst_radingrate>
					{
						{
							instance.Rading_type,
							instance
						}
					};
					Mst_RadingRate.Add(instance.Maparea_id, value);
				}
				else
				{
					Mst_RadingRate[instance.Maparea_id].Add(instance.Rading_type, instance);
				}
			});
			return dictionary;
		}

		public void Make_MapCell(int maparea_id, int mapinfo_no)
		{
			string text = Utils.getTableDirMaster(Mst_mapcell2.tableName) + "mst_mapcell/";
			string path = text + Mst_mapcell2.tableName + "_" + maparea_id.ToString() + mapinfo_no.ToString() + ".xml";
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, Mst_mapcell2.tableName, string.Empty);
			if (enumerable != null)
			{
				Mst_mapcell.Clear();
				foreach (XElement item in enumerable)
				{
					Mst_mapcell2 instance = null;
					Model_Base.SetMaster(out instance, item);
					Mst_mapcell.Add(instance.No, instance);
				}
			}
		}

		public void Make_Mapenemy(int maparea_id, int mapinfo_no)
		{
			string text = Utils.getTableDirMaster(Mst_mapenemy2.tableName) + "mst_mapenemy/";
			string path = text + Mst_mapenemy2.tableName + "_" + maparea_id.ToString() + mapinfo_no.ToString() + ".xml";
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, Mst_mapenemy2.tableName, string.Empty);
			if (enumerable != null)
			{
				Mst_mapenemy.Clear();
				foreach (XElement item in enumerable)
				{
					Mst_mapenemy2 instance = null;
					Model_Base.SetMaster(out instance, item);
					Mst_mapenemy.Add(instance.Id, instance);
				}
			}
		}

		public ILookup<int, Mst_mapenemylevel> GetMapenemyLevel(int maparea_id, int mapinfo_no)
		{
			string text = Utils.getTableDirMaster(Mst_mapenemylevel.tableName) + "mst_mapenemylevel/";
			string path = text + Mst_mapenemylevel.tableName + "_" + maparea_id.ToString() + mapinfo_no.ToString() + ".xml";
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, Mst_mapenemylevel.tableName, string.Empty);
			if (enumerable == null)
			{
				return null;
			}
			List<Mst_mapenemylevel> list = new List<Mst_mapenemylevel>();
			foreach (XElement item in enumerable)
			{
				Mst_mapenemylevel instance = null;
				Model_Base.SetMaster(out instance, item);
				list.Add(instance);
			}
			return list.ToLookup((Mst_mapenemylevel x) => x.Enemy_list_id);
		}

		public void Make_Mapshipget(int maparea_id, int mapinfo_no)
		{
			string text = Utils.getTableDirMaster(Mst_shipget2.tableName) + "mst_shipget/";
			string path = text + Mst_shipget2.tableName + "_" + maparea_id.ToString() + mapinfo_no.ToString() + ".xml";
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, Mst_shipget2.tableName, null);
			if (enumerable != null)
			{
				if (Mst_shipget != null)
				{
					Mst_shipget.Remove();
				}
				Mst_shipget = enumerable;
			}
		}

		public Dictionary<int, List<int>> GetMaproute(int mapinfoId)
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			string text = "mst_maproute";
			string text2 = Utils.getTableDirMaster(text) + text + "/";
			string path = text2 + text + "_" + mapinfoId.ToString() + ".xml";
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, text, null);
			if (enumerable == null)
			{
				return dictionary;
			}
			char c = ',';
			foreach (XElement item in enumerable)
			{
				int key = int.Parse(item.Element("No").Value);
				string[] array = item.Element("Next").Value.Split(c);
				List<int> list = Array.ConvertAll(array, (string x) => int.Parse(x)).ToList();
				list.RemoveAll((int x) => x == 0);
				dictionary.Add(key, list);
			}
			return dictionary;
		}

		public Dictionary<int, List<Mst_mapincentive>> GetMapIncentive(int mapinfoId)
		{
			Dictionary<int, List<Mst_mapincentive>> dictionary = new Dictionary<int, List<Mst_mapincentive>>();
			string tableName = Mst_mapincentive.tableName;
			string text = Utils.getTableDirMaster(tableName) + tableName + "/";
			string path = text + tableName + "_" + mapinfoId.ToString() + ".xml";
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, tableName, "Id");
			if (enumerable == null)
			{
				return dictionary;
			}
			List<Mst_mapincentive> list = new List<Mst_mapincentive>();
			foreach (XElement item in enumerable)
			{
				Mst_mapincentive instance = null;
				Model_Base.SetMaster(out instance, item);
				list.Add(instance);
			}
			ILookup<int, Mst_mapincentive> lookup = list.ToLookup((Mst_mapincentive x) => x.Map_cleared);
			foreach (IGrouping<int, Mst_mapincentive> item2 in lookup)
			{
				dictionary.Add(item2.Key, (from x in item2
					orderby x.Incentive_no
					select x).ToList());
			}
			return dictionary;
		}

		public Dictionary<int, List<Mst_mapcellincentive>> GetMapCellIncentive(int mapinfoId)
		{
			Dictionary<int, List<Mst_mapcellincentive>> dictionary = new Dictionary<int, List<Mst_mapcellincentive>>();
			string tableName = Mst_mapcellincentive.tableName;
			string text = Utils.getTableDirMaster(tableName) + tableName + "/";
			string path = text + tableName + "_" + mapinfoId.ToString() + ".xml";
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, tableName, "Id");
			if (enumerable == null)
			{
				return dictionary;
			}
			List<Mst_mapcellincentive> list = new List<Mst_mapcellincentive>();
			foreach (XElement item in enumerable)
			{
				Mst_mapcellincentive instance = null;
				Model_Base.SetMaster(out instance, item);
				list.Add(instance);
			}
			ILookup<int, Mst_mapcellincentive> lookup = list.ToLookup((Mst_mapcellincentive x) => x.Mapcell_id);
			foreach (IGrouping<int, Mst_mapcellincentive> item2 in lookup)
			{
				dictionary.Add(item2.Key, (from x in item2
					orderby x.Incentive_no
					select x).ToList());
			}
			return dictionary;
		}

		public Dictionary<int, int> Get_MstLevel(bool shipTable)
		{
			return (!shipTable) ? ArrayMaster.GetMstLevelUser() : ArrayMaster.GetMstLevel();
		}

		public Dictionary<int, List<Mst_item_shop>> GetMstCabinet()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_item_shop.tableName, Mst_item_shop.tableName, null);
			if (enumerable == null)
			{
				return null;
			}
			var source = (from key in enumerable.Elements("Cabinet_no")
				select new
				{
					no = int.Parse(key.Value)
				}).Distinct();
			Dictionary<int, List<Mst_item_shop>> dictionary = source.ToDictionary(key => key.no, val => new List<Mst_item_shop>());
			foreach (XElement item2 in enumerable)
			{
				Mst_item_shop instance = null;
				Model_Base.SetMaster(out instance, item2);
				dictionary[instance.Cabinet_no].Add(instance);
			}
			if (!Comm_UserDatas.Instance.User_ship.Values.Any((Mem_ship x) => x.Stype == 22))
			{
				Mst_item_shop item = dictionary[1].FirstOrDefault((Mst_item_shop x) => x.Item1_id == 23);
				dictionary[1].Remove(item);
			}
			return dictionary;
		}

		public Dictionary<int, List<Mst_slotitem_remodel>> Get_Mst_slotitem_remodel()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_slotitem_remodel.tableName, Mst_slotitem_remodel.tableName, "Id");
			if (enumerable == null)
			{
				return null;
			}
			List<Mst_slotitem_remodel> list = new List<Mst_slotitem_remodel>();
			foreach (XElement item in enumerable)
			{
				Mst_slotitem_remodel instance = null;
				Model_Base.SetMaster(out instance, item);
				if (instance.Enabled == 1)
				{
					list.Add(instance);
				}
			}
			return list.ToLookup((Mst_slotitem_remodel x) => x.Position).ToDictionary((IGrouping<int, Mst_slotitem_remodel> g_id) => g_id.Key, (IGrouping<int, Mst_slotitem_remodel> values) => values.ToList());
		}

		public Dictionary<int, List<Mst_slotitem_remodel_detail>> Get_Mst_slotitem_remodel_detail()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_slotitem_remodel_detail.tableName, Mst_slotitem_remodel_detail.tableName, string.Empty);
			if (enumerable == null)
			{
				return null;
			}
			List<Mst_slotitem_remodel_detail> list = new List<Mst_slotitem_remodel_detail>();
			foreach (XElement item in enumerable)
			{
				Mst_slotitem_remodel_detail instance = null;
				Model_Base.SetMaster(out instance, item);
				list.Add(instance);
			}
			return list.ToLookup((Mst_slotitem_remodel_detail x) => x.Id).ToDictionary((IGrouping<int, Mst_slotitem_remodel_detail> g_id) => g_id.Key, (IGrouping<int, Mst_slotitem_remodel_detail> values) => values.ToList());
		}

		public Dictionary<int, string> GetUseitemText()
		{
			IEnumerable<XElement> source = Utils.Xml_Result("mst_useitemtext", "mst_useitemtext", null);
			return source.ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement value) => value.Element("Description").Value);
		}

		public Dictionary<int, string> GetFurnitureText()
		{
			IEnumerable<XElement> source = Utils.Xml_Result("mst_furnituretext", "mst_furnituretext", null);
			return source.ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement value) => value.Element("Description").Value);
		}

		public Dictionary<int, Mst_payitem> GetPayitem()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result(Mst_payitem.tableName, Mst_payitem.tableName, null);
			IEnumerable<XElement> source = Utils.Xml_Result("mst_payitemtext", "mst_payitemtext", null);
			Dictionary<int, string> dictionary = source.ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement value) => value.Element("Description").Value);
			Dictionary<int, Mst_payitem> dictionary2 = new Dictionary<int, Mst_payitem>(enumerable.Count());
			foreach (XElement item in enumerable)
			{
				Mst_payitem instance = null;
				Model_Base.SetMaster(out instance, item);
				instance.setText(dictionary[instance.Id]);
				dictionary2.Add(instance.Id, instance);
			}
			return dictionary2;
		}

		public Dictionary<int, KeyValuePair<int, string>> GetSlotItemEquipTypeName()
		{
			IEnumerable<XElement> source = Utils.Xml_Result("mst_slotitem_equiptype", "mst_slotitem_equiptype", null);
			return source.ToDictionary((XElement key) => int.Parse(key.Element("Id").Value), (XElement value) => new KeyValuePair<int, string>(int.Parse(value.Element("Show_flg").Value), value.Element("Name").Value));
		}

		public List<Mst_mission2> GetSupportResistedData(int maparea_id)
		{
			List<Mst_mission2> list = new List<Mst_mission2>();
			XElement element = new XElement(new XElement("mst_mission2", new XElement("Id", "100000"), new XElement("Maparea_id", maparea_id.ToString()), new XElement("Name", "前線反抗支援"), new XElement("Details", "前線反抗支援"), new XElement("Mission_type", "2"), new XElement("Time", "2"), new XElement("Rp_sub", "0"), new XElement("Difficulty", "1"), new XElement("Use_mat", "0.5,0.8"), new XElement("Required_ids", string.Empty), new XElement("Win_exp", "0,0"), new XElement("Win_mat", "0,0,0,0"), new XElement("Win_item1", "0,0"), new XElement("Win_item2", "0,0"), new XElement("Win_spoint", "0,0"), new XElement("Level", "0"), new XElement("Flagship_level_check_type", "1"), new XElement("Flagship_level", "0"), new XElement("Stype_num", "0,0,0,0,0,0,0,0,0"), new XElement("Deck_num", "0"), new XElement("Drum_num", "0,0,0"), new XElement("Flagship_stype", "0,0"), new XElement("Tanker_num", "0,0")));
			Mst_mission2 instance = null;
			Model_Base.SetMaster(out instance, element);
			list.Add(instance);
			element = new XElement(new XElement("mst_mission2", new XElement("Id", "100001"), new XElement("Maparea_id", maparea_id.ToString()), new XElement("Name", "決戦反抗支援"), new XElement("Details", "決戦反抗支援"), new XElement("Mission_type", "3"), new XElement("Time", "2"), new XElement("Rp_sub", "0"), new XElement("Difficulty", "1"), new XElement("Use_mat", "0.5,0.8"), new XElement("Required_ids", string.Empty), new XElement("Win_exp", "0,0"), new XElement("Win_mat", "0,0,0,0"), new XElement("Win_item1", "0,0"), new XElement("Win_item2", "0,0"), new XElement("Win_spoint", "0,0"), new XElement("Level", "0"), new XElement("Flagship_level_check_type", "1"), new XElement("Flagship_level", "0"), new XElement("Stype_num", "0,0,0,0,0,0,0,0,0"), new XElement("Deck_num", "0"), new XElement("Drum_num", "0,0,0"), new XElement("Flagship_stype", "0,0"), new XElement("Tanker_num", "0,0")));
			Mst_mission2 instance2 = null;
			Model_Base.SetMaster(out instance2, element);
			list.Add(instance2);
			return list;
		}

		public List<Mst_bgm_jukebox> GetJukeBoxList()
		{
			return (from x in _mst_jukebox.Values
				orderby x.Id
				select x).ToList();
		}

		public Mst_bgm_jukebox GetJukeBoxItem(int bgmId)
		{
			Mst_bgm_jukebox value = null;
			_mst_jukebox.TryGetValue(bgmId, out value);
			return value;
		}

		public Dictionary<int, string> GetMstBgm()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_bgm", "mst_bgm", null);
			char c = ',';
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			foreach (XElement item in enumerable)
			{
				string[] array = item.Element("bgm_record").Value.Split(c);
				int key = int.Parse(array[0]);
				string value = array[1];
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		public bool MakeUIBattleMaster(int mapinfo_id)
		{
			if (UiBattleMaster != null && UiBattleMaster.IsAllive())
			{
				return true;
			}
			UiBattleMaster = new UIBattleRequireMaster(mapinfo_id);
			if (!UiBattleMaster.IsAllive())
			{
				UiBattleMaster.PurgeCollection();
				return false;
			}
			return true;
		}

		public void PurgeUIBattleMaster()
		{
			if (UiBattleMaster != null)
			{
				UiBattleMaster.PurgeCollection();
			}
		}
	}
}
