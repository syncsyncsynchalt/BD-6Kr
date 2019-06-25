using Common.Enum;
using Common.Struct;
using local.utils;
using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class DutyModel
	{
		public enum InvalidType
		{
			MAX_SHIP,
			MAX_SLOT,
			LOCK_TARGET_SLOT
		}

		protected User_QuestFmt _fmt;

		public int No => _fmt.No;

		public int Category => _fmt.Category;

		public int Type => _fmt.Type;

		public QuestState State => _fmt.State;

		public string Title => _fmt.Title;

		public string Description => _fmt.Detail;

		public Dictionary<enumMaterialCategory, int> RewardMaterials => _fmt.GetMaterial;

		public int Fuel => _fmt.GetMaterial[enumMaterialCategory.Fuel];

		public int Ammo => _fmt.GetMaterial[enumMaterialCategory.Bull];

		public int Steel => _fmt.GetMaterial[enumMaterialCategory.Steel];

		public int Baux => _fmt.GetMaterial[enumMaterialCategory.Bauxite];

		public int BuildKit => _fmt.GetMaterial[enumMaterialCategory.Build_Kit];

		public int RepairKit => _fmt.GetMaterial[enumMaterialCategory.Repair_Kit];

		public int DevKit => _fmt.GetMaterial[enumMaterialCategory.Dev_Kit];

		public int RevKit => _fmt.GetMaterial[enumMaterialCategory.Revamp_Kit];

		public int SPoint => _fmt.GetSpoint;

		public QuestProgressKinds Progress => _fmt.Progress;

		public int ProgressPercent
		{
			get
			{
				if (_fmt.Progress == QuestProgressKinds.MORE_THAN_80)
				{
					return 80;
				}
				if (_fmt.Progress == QuestProgressKinds.MORE_THAN_50)
				{
					return 50;
				}
				return 0;
			}
		}

		[Obsolete("非推奨  GetInvalidTypes()を使用してください。", false)]
		public bool InvalidFlag => _fmt.InvalidFlag;

		public DutyModel(User_QuestFmt fmt)
		{
			_fmt = fmt;
		}

		public List<InvalidType> GetInvalidTypes()
		{
			List<InvalidType> list = new List<InvalidType>();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < _fmt.RewardTypes.Count; i++)
			{
				if (_fmt.RewardTypes[i] == QuestItemGetKind.Ship)
				{
					num += _fmt.RewardCount[i];
					num2 += 4;
				}
				else if (_fmt.RewardTypes[i] == QuestItemGetKind.SlotItem)
				{
					num2 += _fmt.RewardCount[i];
				}
			}
			if (num > 0)
			{
				MemberMaxInfo memberMaxInfo = Utils.ShipCountData();
				if (memberMaxInfo.NowCount + num > memberMaxInfo.MaxCount)
				{
					list.Add(InvalidType.MAX_SHIP);
				}
			}
			if (num2 > 0)
			{
				MemberMaxInfo memberMaxInfo2 = Utils.SlotitemCountData();
				if (memberMaxInfo2.NowCount + num2 > memberMaxInfo2.MaxCount)
				{
					list.Add(InvalidType.MAX_SLOT);
				}
			}
			if (_fmt.InvalidFlag)
			{
				list.Add(InvalidType.LOCK_TARGET_SLOT);
			}
			return list;
		}

		public override string ToString()
		{
			string str = $"[{No}] Category:{Category} Type:{Type} 状態:{State} {Title}\n";
			if (SPoint > 0)
			{
				str += $"獲得戦略ポイント:{SPoint} ";
			}
			str += $"獲得資材:{Fuel}/{Ammo}/{Steel}/{Baux}";
			str += $" 高速建造:{BuildKit} 高速修復:{RepairKit} 開発資材:{DevKit} 改修資材:{RevKit}";
			str += $" 進行度:{ProgressPercent} ";
			List<InvalidType> invalidTypes = GetInvalidTypes();
			if (invalidTypes.Count > 0)
			{
				str += "[";
				for (int i = 0; i < invalidTypes.Count; i++)
				{
					str = str + invalidTypes[i] + ",";
				}
				str = str.Remove(str.Length - 1);
				str += "]";
			}
			str += "\n";
			return str + $"{Description}\n";
		}
	}
}
