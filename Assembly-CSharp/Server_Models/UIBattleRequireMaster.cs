using Server_Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class UIBattleRequireMaster
	{
		private int mapBgm;

		private Dictionary<int, List<int>> battleBgm;

		private Dictionary<int, int> mst_slotitem_comvert;

		public int MapBgm
		{
			get
			{
				return mapBgm;
			}
			private set
			{
				mapBgm = value;
			}
		}

		public Dictionary<int, List<int>> BattleBgm
		{
			get
			{
				return battleBgm;
			}
			private set
			{
				battleBgm = value;
			}
		}

		public Dictionary<int, int> Mst_slotitem_comvert
		{
			get
			{
				return mst_slotitem_comvert;
			}
			private set
			{
				mst_slotitem_comvert = value;
			}
		}

		public UIBattleRequireMaster()
		{
			MapBgm = 103;
		}

		public UIBattleRequireMaster(int mapinfo_id)
		{
			makeMapBgm(mapinfo_id);
			makeSlotConvert();
		}

		public bool IsAllive()
		{
			if (BattleBgm != null && Mst_slotitem_comvert != null)
			{
				return true;
			}
			return false;
		}

		public void PurgeCollection()
		{
			if (BattleBgm != null)
			{
				BattleBgm.Clear();
				BattleBgm = null;
			}
			if (Mst_slotitem_comvert != null)
			{
				Mst_slotitem_comvert.Clear();
				Mst_slotitem_comvert = null;
			}
		}

		private void makeMapBgm(int mapInfoId)
		{
			if (BattleBgm != null)
			{
				return;
			}
			string text = "mst_mapbgm";
			string text2 = Utils.getTableDirMaster(text) + text + "/";
			string path = text2 + text + "_" + mapInfoId.ToString() + ".xml";
			IEnumerable<XElement> enumerable = Utils.Xml_Result_To_Path(path, text, null);
			if (enumerable != null)
			{
				BattleBgm = new Dictionary<int, List<int>>
				{
					{
						0,
						new List<int>()
					},
					{
						1,
						new List<int>()
					}
				};
				XElement xElement = enumerable.First();
				BattleBgm[0].Add(int.Parse(xElement.Element("Mapd_bgm_id").Value));
				BattleBgm[0].Add(int.Parse(xElement.Element("Bossd_bgm_id").Value));
				BattleBgm[1].Add(int.Parse(xElement.Element("Mapn_bgm_id").Value));
				BattleBgm[1].Add(int.Parse(xElement.Element("Bossn_bgm_id").Value));
				int num = int.Parse(xElement.Element("Map_bgm").Value);
				if (num != 0)
				{
					MapBgm = num;
				}
			}
		}

		private void makeSlotConvert()
		{
			if (Mst_slotitem_comvert == null)
			{
				IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_slotitem_convert", "mst_slotitem_convert", null);
				if (enumerable != null)
				{
					Mst_slotitem_comvert = enumerable.ToDictionary((XElement key) => int.Parse(key.Element("Slotitem_id").Value), (XElement value) => int.Parse(value.Element("Convert_id").Value));
				}
			}
		}
	}
}
