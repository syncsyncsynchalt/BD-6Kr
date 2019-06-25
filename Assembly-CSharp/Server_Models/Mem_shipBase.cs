using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_ship", Namespace = "")]
	public class Mem_shipBase
	{
		[DataMember]
		public int Rid;

		[DataMember]
		public int GetNo;

		[DataMember]
		public int Ship_id;

		[DataMember]
		public int Level;

		[DataMember]
		public int Exp;

		[DataMember]
		public int Nowhp;

		[DataMember]
		public List<int> Slot;

		[DataMember]
		public List<int> Onslot;

		[DataMember]
		public int Exslot;

		[DataMember]
		public int C_houg;

		[DataMember]
		public int C_raig;

		[DataMember]
		public int C_tyku;

		[DataMember]
		public int C_souk;

		[DataMember]
		public int C_kaihi;

		[DataMember]
		public int C_taisen;

		[DataMember]
		public int C_taik;

		[DataMember]
		public int C_taik_powerup;

		[DataMember]
		public int C_luck;

		[DataMember]
		public int Fuel;

		[DataMember]
		public int Bull;

		[DataMember]
		public int Cond;

		[DataMember]
		public int Locked;

		[DataMember]
		public bool Escape_sts;

		[DataMember]
		public Mem_ship.BlingKind BlingType;

		[DataMember]
		public int BlingWaitArea;

		[DataMember]
		public int BlingWaitDeck;

		[DataMember]
		public int Bling_start;

		[DataMember]
		public int Bling_end;

		[DataMember]
		public List<BattleCommand> BattleCommand;

		[DataMember]
		public int Lov;

		[DataMember]
		public Dictionary<byte, byte> Lov_back_processed;

		[DataMember]
		public List<int> Lov_back_value;

		[DataMember]
		public Dictionary<byte, byte> Lov_front_processed;

		[DataMember]
		public List<int> Lov_front_value;

		public Mem_shipBase()
		{
			Slot = new List<int>(5);
			Onslot = new List<int>(5);
			Escape_sts = false;
			Bling_start = 0;
			Bling_end = 0;
			BlingType = Mem_ship.BlingKind.None;
			BlingWaitDeck = 0;
			Lov_back_processed = new Dictionary<byte, byte>();
			Lov_back_value = new List<int>();
			Lov_front_processed = new Dictionary<byte, byte>();
			Lov_front_value = new List<int>();
			Exslot = -2;
		}

		public Mem_shipBase(int rid, int getNo, Mst_ship mst_data)
			: this()
		{
			Rid = rid;
			GetNo = getNo;
			Ship_id = mst_data.Id;
			Level = 1;
			Exp = 0;
			Nowhp = mst_data.Taik;
			List<int> list = Comm_UserDatas.Instance.Add_Slot(mst_data.Defeq);
			for (int i = 0; i < mst_data.Slot_num; i++)
			{
				if (list.Count() > i)
				{
					Slot.Add(list[i]);
					Mem_slotitem mem_slotitem = Comm_UserDatas.Instance.User_slot[list[i]];
					mem_slotitem.Equip(rid);
				}
				else
				{
					Slot.Add(mst_data.Defeq[i]);
				}
				Onslot.Add(mst_data.Maxeq[i]);
			}
			C_houg = 0;
			C_raig = 0;
			C_tyku = 0;
			C_souk = 0;
			C_kaihi = 0;
			C_taisen = 0;
			C_taik = 0;
			C_taik_powerup = 0;
			C_luck = 0;
			Fuel = mst_data.Fuel_max;
			Bull = mst_data.Bull_max;
			Locked = 0;
			Cond = 40;
			Lov = 50;
		}

		public Mem_shipBase(Mem_ship base_ship)
			: this()
		{
			Rid = base_ship.Rid;
			GetNo = base_ship.GetNo;
			Ship_id = base_ship.Ship_id;
			Level = base_ship.Level;
			Exp = base_ship.Exp;
			Nowhp = base_ship.Nowhp;
			Slot = base_ship.Slot.ToList();
			Onslot = base_ship.Onslot.ToList();
			SetKyoukaValue(base_ship.Kyouka);
			Fuel = base_ship.Fuel;
			Bull = base_ship.Bull;
			Locked = base_ship.Locked;
			Cond = base_ship.Cond;
			Escape_sts = base_ship.Escape_sts;
			BlingType = base_ship.BlingType;
			Bling_start = base_ship.Bling_start;
			Bling_end = base_ship.Bling_end;
			BlingWaitArea = base_ship.BlingWaitArea;
			base_ship.GetBattleCommand(out BattleCommand);
			Lov = base_ship.Lov;
			Lov_back_processed = base_ship.Lov_back_processed;
			Lov_back_value = base_ship.Lov_back_value;
			Lov_front_processed = base_ship.Lov_front_processed;
			Lov_front_value = base_ship.Lov_front_value;
			Exslot = base_ship.Exslot;
		}

		public Mem_shipBase(int rid, Mst_ship mst_ship, int level, Dictionary<Mem_ship.enumKyoukaIdx, int> kyouka)
			: this()
		{
			Rid = rid;
			Ship_id = mst_ship.Id;
			Level = level;
			Nowhp = mst_ship.Taik;
			for (int i = 0; i < mst_ship.Slot_num; i++)
			{
				Slot.Add(mst_ship.Defeq[i]);
				Onslot.Add(mst_ship.Maxeq[i]);
			}
			SetKyoukaValue(kyouka);
			Fuel = mst_ship.Fuel_max;
			Bull = mst_ship.Bull_max;
			Locked = 0;
			Cond = 40;
			BlingType = Mem_ship.BlingKind.None;
		}

		public void SetKyoukaValue(Dictionary<Mem_ship.enumKyoukaIdx, int> kyouka)
		{
			C_houg = kyouka[Mem_ship.enumKyoukaIdx.Houg];
			C_raig = kyouka[Mem_ship.enumKyoukaIdx.Raig];
			C_tyku = kyouka[Mem_ship.enumKyoukaIdx.Tyku];
			C_souk = kyouka[Mem_ship.enumKyoukaIdx.Souk];
			C_taik = kyouka[Mem_ship.enumKyoukaIdx.Taik];
			C_taik_powerup = kyouka[Mem_ship.enumKyoukaIdx.Taik_Powerup];
			C_luck = kyouka[Mem_ship.enumKyoukaIdx.Luck];
			C_taisen = kyouka[Mem_ship.enumKyoukaIdx.Taisen];
			C_kaihi = kyouka[Mem_ship.enumKyoukaIdx.Kaihi];
		}

		public void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("Rid").Value);
			GetNo = int.Parse(element.Element("GetNo").Value);
			Ship_id = int.Parse(element.Element("Ship_id").Value);
			Level = int.Parse(element.Element("Level").Value);
			Exp = int.Parse(element.Element("Exp").Value);
			Nowhp = int.Parse(element.Element("Nowhp").Value);
			C_houg = int.Parse(element.Element("C_houg").Value);
			C_raig = int.Parse(element.Element("C_raig").Value);
			C_tyku = int.Parse(element.Element("C_tyku").Value);
			C_souk = int.Parse(element.Element("C_souk").Value);
			C_taik = int.Parse(element.Element("C_taik").Value);
			C_taik_powerup = int.Parse(element.Element("C_taik_powerup").Value);
			C_luck = int.Parse(element.Element("C_luck").Value);
			C_taisen = int.Parse(element.Element("C_taisen").Value);
			C_kaihi = int.Parse(element.Element("C_kaihi").Value);
			Fuel = int.Parse(element.Element("Fuel").Value);
			Bull = int.Parse(element.Element("Bull").Value);
			Cond = int.Parse(element.Element("Cond").Value);
			Locked = int.Parse(element.Element("Locked").Value);
			Escape_sts = bool.Parse(element.Element("Escape_sts").Value);
			BlingType = (Mem_ship.BlingKind)(int)Enum.Parse(typeof(Mem_ship.BlingKind), element.Element("BlingType").Value);
			BlingWaitDeck = int.Parse(element.Element("BlingWaitDeck").Value);
			BlingWaitArea = int.Parse(element.Element("BlingWaitArea").Value);
			Bling_start = int.Parse(element.Element("Bling_start").Value);
			Bling_end = int.Parse(element.Element("Bling_end").Value);
			foreach (XElement item4 in element.Element("Slot").Elements())
			{
				Slot.Add(int.Parse(item4.Value));
			}
			foreach (var item5 in element.Element("Onslot").Elements().Select((XElement obj, int idx) => new
			{
				obj,
				idx
			}))
			{
				Onslot.Add(int.Parse(item5.obj.Value));
			}
			if (element.Element("Exslot") != null)
			{
				Exslot = int.Parse(element.Element("Exslot").Value);
			}
			else
			{
				Exslot = -2;
			}
			if (element.Element("BattleCommand").Value != string.Empty)
			{
				BattleCommand = new List<BattleCommand>();
				foreach (XElement item6 in element.Element("BattleCommand").Elements())
				{
					BattleCommand item = (BattleCommand)(int)Enum.Parse(typeof(BattleCommand), item6.Value);
					BattleCommand.Add(item);
				}
			}
			Lov = int.Parse(element.Element("Lov").Value);
			foreach (XElement item7 in element.Element("Lov_back_processed").Elements())
			{
				XNode firstNode = item7.FirstNode;
				XNode nextNode = firstNode.NextNode;
				byte key = byte.Parse(((XElement)firstNode).Value);
				byte value = byte.Parse(((XElement)nextNode).Value);
				Lov_back_processed.Add(key, value);
			}
			foreach (XElement item8 in element.Element("Lov_back_value").Elements())
			{
				int item2 = int.Parse(item8.Value);
				Lov_back_value.Add(item2);
			}
			foreach (XElement item9 in element.Element("Lov_front_processed").Elements())
			{
				XNode firstNode2 = item9.FirstNode;
				XNode nextNode2 = firstNode2.NextNode;
				byte key2 = byte.Parse(((XElement)firstNode2).Value);
				byte value2 = byte.Parse(((XElement)nextNode2).Value);
				Lov_front_processed.Add(key2, value2);
			}
			foreach (XElement item10 in element.Element("Lov_front_value").Elements())
			{
				int item3 = int.Parse(item10.Value);
				Lov_front_value.Add(item3);
			}
		}
	}
}
