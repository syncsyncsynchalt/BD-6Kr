using Server_Common;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_slotitem", Namespace = "")]
	public class Mem_slotitem : Model_Base, IReqNewGetNo
	{
		public interface IMemSlotIdOperator
		{
			void ChangeSlotId(Mem_slotitem obj, int changeId);
		}

		[DataContract(Namespace = "")]
		public enum enumEquipSts
		{
			[EnumMember]
			Unset,
			[EnumMember]
			Equip
		}

		[DataMember]
		private int _rid;

		[DataMember]
		private int _getNO;

		[DataMember]
		private int _slotitem_id;

		[DataMember]
		private enumEquipSts _equip_flag;

		[DataMember]
		private int _equip_rid;

		[DataMember]
		private bool _lock;

		[DataMember]
		private int _level;

		[DataMember]
		private int _experience;

		private static string _tableName = "mem_slotitem";

		public int Rid
		{
			get
			{
				return _rid;
			}
			private set
			{
				_rid = value;
			}
		}

		public int GetNo
		{
			get
			{
				return _getNO;
			}
			private set
			{
				_getNO = value;
			}
		}

		public int Slotitem_id
		{
			get
			{
				return _slotitem_id;
			}
			private set
			{
				_slotitem_id = value;
			}
		}

		public enumEquipSts Equip_flag
		{
			get
			{
				return _equip_flag;
			}
			private set
			{
				_equip_flag = value;
			}
		}

		public int Equip_rid
		{
			get
			{
				return _equip_rid;
			}
			private set
			{
				_equip_rid = value;
			}
		}

		public bool Lock
		{
			get
			{
				return _lock;
			}
			private set
			{
				_lock = value;
			}
		}

		public int Level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}

		public int Experience
		{
			get
			{
				return _experience;
			}
			private set
			{
				_experience = value;
			}
		}

		public static string tableName => _tableName;

		public bool Set_New_SlotData(int rid, int getNo, int mst_id)
		{
			if (!Mst_DataManager.Instance.Mst_Slotitem.ContainsKey(mst_id))
			{
				return false;
			}
			Rid = rid;
			GetNo = getNo;
			Slotitem_id = mst_id;
			Equip_flag = enumEquipSts.Unset;
			Equip_rid = 0;
			Lock = false;
			Level = 0;
			Experience = Mst_DataManager.Instance.Mst_Slotitem[Slotitem_id].Default_exp;
			return true;
		}

		public int GetSkillLevel()
		{
			if (Experience >= 0 && Experience <= 9)
			{
				return 0;
			}
			if (Experience >= 10 && Experience <= 24)
			{
				return 1;
			}
			if (Experience >= 25 && Experience <= 39)
			{
				return 2;
			}
			if (Experience >= 40 && Experience <= 54)
			{
				return 3;
			}
			if (Experience >= 55 && Experience <= 69)
			{
				return 4;
			}
			if (Experience >= 70 && Experience <= 84)
			{
				return 5;
			}
			if (Experience >= 85 && Experience <= 99)
			{
				return 6;
			}
			return 7;
		}

		public bool IsMaxSkillLevel()
		{
			return (GetSkillLevel() == 7) ? true : false;
		}

		public int GetSortNo()
		{
			return _getNO;
		}

		public void ChangeSortNo(int no)
		{
			_getNO = no;
		}

		public void Equip(int ship_rid)
		{
			Equip_flag = enumEquipSts.Equip;
			Equip_rid = ship_rid;
		}

		public void UnEquip()
		{
			Equip_flag = enumEquipSts.Unset;
			Equip_rid = 0;
		}

		public void LockChange()
		{
			Lock = !Lock;
		}

		public void SetLevel(int setVal)
		{
			Level = setVal;
		}

		public void ChangeExperience(int changeValue)
		{
			Experience += changeValue;
			if (Experience > 120)
			{
				Experience = 120;
			}
			if (Experience <= 0)
			{
				Experience = 0;
			}
		}

		public void ChangeSlotId(IMemSlotIdOperator instance, int changeId)
		{
			if (instance != null)
			{
				Slotitem_id = changeId;
				Comm_UserDatas.Instance.Add_Book(2, changeId);
			}
		}

		protected override void setProperty(XElement element)
		{
			Rid = int.Parse(element.Element("_rid").Value);
			GetNo = int.Parse(element.Element("_getNO").Value);
			Slotitem_id = int.Parse(element.Element("_slotitem_id").Value);
			Equip_flag = (enumEquipSts)(int)Enum.Parse(typeof(enumEquipSts), element.Element("_equip_flag").Value);
			Equip_rid = int.Parse(element.Element("_equip_rid").Value);
			Lock = bool.Parse(element.Element("_lock").Value);
			Level = int.Parse(element.Element("_level").Value);
			if (element.Element("_experience") == null)
			{
				Experience = Mst_DataManager.Instance.Mst_Slotitem[Slotitem_id].Default_exp;
			}
			else
			{
				Experience = int.Parse(element.Element("_experience").Value);
			}
		}
	}
}
