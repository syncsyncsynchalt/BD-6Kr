using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Common.SaveManager
{
	[DataContract(Name = "member_header", Namespace = "")]
	public class SaveHeaderFmt
	{
		[DataMember]
		public string Nickname;

		[DataMember]
		public DifficultKind Difficult;

		[DataMember]
		public int Level;

		[DataMember]
		public int DeckNum;

		[DataMember]
		public int KdockNum;

		[DataMember]
		public int NdockNum;

		[DataMember]
		public int ShipNum;

		[DataMember]
		public int ShipMax;

		[DataMember]
		public int SlotNum;

		[DataMember]
		public int SlotMax;

		[DataMember]
		public int FurnitureNum;

		public static string tableaName = "member_header";

		public bool SetPropertie()
		{
			Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
			Mem_record user_record = Comm_UserDatas.Instance.User_record;
			Dictionary<int, Mem_deck> user_deck = Comm_UserDatas.Instance.User_deck;
			Dictionary<int, Mem_kdock> user_kdock = Comm_UserDatas.Instance.User_kdock;
			Dictionary<int, Mem_ndock> user_ndock = Comm_UserDatas.Instance.User_ndock;
			Dictionary<int, Mem_ship> user_ship = Comm_UserDatas.Instance.User_ship;
			Dictionary<int, Mem_slotitem> user_slot = Comm_UserDatas.Instance.User_slot;
			Dictionary<int, Mem_furniture> user_furniture = Comm_UserDatas.Instance.User_furniture;
			Nickname = user_basic.Nickname;
			Difficult = user_basic.Difficult;
			Level = user_record.Level;
			DeckNum = user_deck.Count;
			KdockNum = user_kdock.Count;
			NdockNum = user_ndock.Count;
			ShipNum = user_ship.Count;
			ShipMax = user_basic.Max_chara;
			SlotNum = user_slot.Count;
			SlotMax = user_basic.Max_slotitem;
			FurnitureNum = user_furniture.Count;
			return true;
		}

		public bool SetPropertie(XElement element)
		{
			try
			{
				Nickname = element.Element("Nickname").Value;
				Difficult = (DifficultKind)(int)System.Enum.Parse(typeof(DifficultKind), element.Element("Difficult").Value);
				Level = int.Parse(element.Element("Level").Value);
				DeckNum = int.Parse(element.Element("DeckNum").Value);
				KdockNum = int.Parse(element.Element("KdockNum").Value);
				NdockNum = int.Parse(element.Element("NdockNum").Value);
				ShipNum = int.Parse(element.Element("ShipNum").Value);
				ShipMax = int.Parse(element.Element("ShipMax").Value);
				SlotNum = int.Parse(element.Element("SlotNum").Value);
				SlotMax = int.Parse(element.Element("SlotMax").Value);
				FurnitureNum = int.Parse(element.Element("FurnitureNum").Value);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
			return true;
		}
	}
}
