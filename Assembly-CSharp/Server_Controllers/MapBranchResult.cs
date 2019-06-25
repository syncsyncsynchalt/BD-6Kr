using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class MapBranchResult
	{
		public enum enumScoutingKind
		{
			None,
			C,
			D,
			E,
			E2,
			K1,
			K2
		}

		public enum enumProductionKind
		{
			None,
			A,
			B,
			C1,
			C2
		}

		private Api_req_Map mapInstance;

		private Mst_mapcell2 mst_cell;

		private List<Mem_ship> mem_ship;

		private Dictionary<int, List<Mst_slotitem>> mst_slotitems;

		private Dictionary<int, int> slotitem_level;

		private Mem_mapclear mapClear;

		private MapCommentKind comment_kind;

		private MapProductionKind production_kind;

		private int user_level;

		private DifficultKind user_difficult;

		private Func<int> mapFunc;

		public MapBranchResult(Api_req_Map mapInstance)
		{
			this.mapInstance = mapInstance;
			mapFunc = null;
		}

		public bool getNextCellNo(out int cellNo, out MapCommentKind comment_kind, out MapProductionKind production_kind)
		{
			cellNo = 0;
			comment_kind = MapCommentKind.None;
			production_kind = MapProductionKind.None;
			init();
			if (mapFunc == null)
			{
				return false;
			}
			cellNo = mapFunc();
			if (cellNo == 0)
			{
				return false;
			}
			comment_kind = this.comment_kind;
			production_kind = this.production_kind;
			return true;
		}

		private void init()
		{
			mst_cell = mapInstance.GetPrevCell();
			mapClear = mapInstance.GetMapClearState();
			mapInstance.GetSortieDeckInfo(this, out mem_ship, out mst_slotitems);
			slotitem_level = getSlotitemLevel();
			comment_kind = MapCommentKind.None;
			production_kind = MapProductionKind.None;
			user_level = Comm_UserDatas.Instance.User_record.Level;
			user_difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			selectFunc();
		}

		private void selectFunc()
		{
			if (mapFunc == null)
			{
				if (mst_cell.Maparea_id == 1 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_11;
				}
				else if (mst_cell.Maparea_id == 1 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_12;
				}
				else if (mst_cell.Maparea_id == 1 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_13;
				}
				else if (mst_cell.Maparea_id == 1 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_14;
				}
				else if (mst_cell.Maparea_id == 1 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_15;
				}
				else if (mst_cell.Maparea_id == 1 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_16;
				}
				else if (mst_cell.Maparea_id == 1 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_17;
				}
				else if (mst_cell.Maparea_id == 2 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_21;
				}
				else if (mst_cell.Maparea_id == 2 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_22;
				}
				else if (mst_cell.Maparea_id == 2 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_23;
				}
				else if (mst_cell.Maparea_id == 2 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_24;
				}
				else if (mst_cell.Maparea_id == 2 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_25;
				}
				else if (mst_cell.Maparea_id == 2 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_26;
				}
				else if (mst_cell.Maparea_id == 2 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_27;
				}
				else if (mst_cell.Maparea_id == 3 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_31;
				}
				else if (mst_cell.Maparea_id == 3 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_32;
				}
				else if (mst_cell.Maparea_id == 3 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_33;
				}
				else if (mst_cell.Maparea_id == 3 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_34;
				}
				else if (mst_cell.Maparea_id == 3 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_35;
				}
				else if (mst_cell.Maparea_id == 3 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_36;
				}
				else if (mst_cell.Maparea_id == 3 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_37;
				}
				else if (mst_cell.Maparea_id == 4 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_41;
				}
				else if (mst_cell.Maparea_id == 4 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_42;
				}
				else if (mst_cell.Maparea_id == 4 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_43;
				}
				else if (mst_cell.Maparea_id == 4 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_44;
				}
				else if (mst_cell.Maparea_id == 4 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_45;
				}
				else if (mst_cell.Maparea_id == 4 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_46;
				}
				else if (mst_cell.Maparea_id == 4 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_47;
				}
				else if (mst_cell.Maparea_id == 5 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_51;
				}
				else if (mst_cell.Maparea_id == 5 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_52;
				}
				else if (mst_cell.Maparea_id == 5 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_53;
				}
				else if (mst_cell.Maparea_id == 5 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_54;
				}
				else if (mst_cell.Maparea_id == 5 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_55;
				}
				else if (mst_cell.Maparea_id == 5 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_56;
				}
				else if (mst_cell.Maparea_id == 5 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_57;
				}
				else if (mst_cell.Maparea_id == 6 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_61;
				}
				else if (mst_cell.Maparea_id == 6 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_62;
				}
				else if (mst_cell.Maparea_id == 6 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_63;
				}
				else if (mst_cell.Maparea_id == 6 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_64;
				}
				else if (mst_cell.Maparea_id == 6 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_65;
				}
				else if (mst_cell.Maparea_id == 6 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_66;
				}
				else if (mst_cell.Maparea_id == 6 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_67;
				}
				else if (mst_cell.Maparea_id == 7 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_71;
				}
				else if (mst_cell.Maparea_id == 7 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_72;
				}
				else if (mst_cell.Maparea_id == 7 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_73;
				}
				else if (mst_cell.Maparea_id == 7 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_74;
				}
				else if (mst_cell.Maparea_id == 7 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_75;
				}
				else if (mst_cell.Maparea_id == 7 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_76;
				}
				else if (mst_cell.Maparea_id == 7 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_77;
				}
				else if (mst_cell.Maparea_id == 8 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_81;
				}
				else if (mst_cell.Maparea_id == 8 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_82;
				}
				else if (mst_cell.Maparea_id == 8 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_83;
				}
				else if (mst_cell.Maparea_id == 8 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_84;
				}
				else if (mst_cell.Maparea_id == 8 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_85;
				}
				else if (mst_cell.Maparea_id == 8 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_86;
				}
				else if (mst_cell.Maparea_id == 8 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_87;
				}
				else if (mst_cell.Maparea_id == 9 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_91;
				}
				else if (mst_cell.Maparea_id == 9 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_92;
				}
				else if (mst_cell.Maparea_id == 9 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_93;
				}
				else if (mst_cell.Maparea_id == 9 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_94;
				}
				else if (mst_cell.Maparea_id == 9 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_95;
				}
				else if (mst_cell.Maparea_id == 9 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_96;
				}
				else if (mst_cell.Maparea_id == 9 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_97;
				}
				else if (mst_cell.Maparea_id == 10 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_101;
				}
				else if (mst_cell.Maparea_id == 10 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_102;
				}
				else if (mst_cell.Maparea_id == 10 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_103;
				}
				else if (mst_cell.Maparea_id == 10 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_104;
				}
				else if (mst_cell.Maparea_id == 10 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_105;
				}
				else if (mst_cell.Maparea_id == 10 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_106;
				}
				else if (mst_cell.Maparea_id == 10 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_107;
				}
				else if (mst_cell.Maparea_id == 11 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_111;
				}
				else if (mst_cell.Maparea_id == 11 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_112;
				}
				else if (mst_cell.Maparea_id == 11 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_113;
				}
				else if (mst_cell.Maparea_id == 11 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_114;
				}
				else if (mst_cell.Maparea_id == 11 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_115;
				}
				else if (mst_cell.Maparea_id == 11 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_116;
				}
				else if (mst_cell.Maparea_id == 11 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_117;
				}
				else if (mst_cell.Maparea_id == 12 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_121;
				}
				else if (mst_cell.Maparea_id == 12 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_122;
				}
				else if (mst_cell.Maparea_id == 12 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_123;
				}
				else if (mst_cell.Maparea_id == 12 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_124;
				}
				else if (mst_cell.Maparea_id == 12 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_125;
				}
				else if (mst_cell.Maparea_id == 12 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_126;
				}
				else if (mst_cell.Maparea_id == 12 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_127;
				}
				else if (mst_cell.Maparea_id == 13 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_131;
				}
				else if (mst_cell.Maparea_id == 13 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_132;
				}
				else if (mst_cell.Maparea_id == 13 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_133;
				}
				else if (mst_cell.Maparea_id == 13 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_134;
				}
				else if (mst_cell.Maparea_id == 13 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_135;
				}
				else if (mst_cell.Maparea_id == 13 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_136;
				}
				else if (mst_cell.Maparea_id == 13 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_137;
				}
				else if (mst_cell.Maparea_id == 14 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_141;
				}
				else if (mst_cell.Maparea_id == 14 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_142;
				}
				else if (mst_cell.Maparea_id == 14 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_143;
				}
				else if (mst_cell.Maparea_id == 14 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_144;
				}
				else if (mst_cell.Maparea_id == 14 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_145;
				}
				else if (mst_cell.Maparea_id == 14 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_146;
				}
				else if (mst_cell.Maparea_id == 14 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_147;
				}
				else if (mst_cell.Maparea_id == 15 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_151;
				}
				else if (mst_cell.Maparea_id == 15 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_152;
				}
				else if (mst_cell.Maparea_id == 15 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_153;
				}
				else if (mst_cell.Maparea_id == 15 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_154;
				}
				else if (mst_cell.Maparea_id == 15 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_155;
				}
				else if (mst_cell.Maparea_id == 15 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_156;
				}
				else if (mst_cell.Maparea_id == 15 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_157;
				}
				else if (mst_cell.Maparea_id == 16 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_161;
				}
				else if (mst_cell.Maparea_id == 16 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_162;
				}
				else if (mst_cell.Maparea_id == 16 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_163;
				}
				else if (mst_cell.Maparea_id == 16 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_164;
				}
				else if (mst_cell.Maparea_id == 16 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_165;
				}
				else if (mst_cell.Maparea_id == 16 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_166;
				}
				else if (mst_cell.Maparea_id == 16 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_167;
				}
				else if (mst_cell.Maparea_id == 17 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_171;
				}
				else if (mst_cell.Maparea_id == 17 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_172;
				}
				else if (mst_cell.Maparea_id == 17 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_173;
				}
				else if (mst_cell.Maparea_id == 17 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_174;
				}
				else if (mst_cell.Maparea_id == 17 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_175;
				}
				else if (mst_cell.Maparea_id == 17 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_176;
				}
				else if (mst_cell.Maparea_id == 17 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_177;
				}
				else if (mst_cell.Maparea_id == 18 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_181;
				}
				else if (mst_cell.Maparea_id == 18 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_182;
				}
				else if (mst_cell.Maparea_id == 18 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_183;
				}
				else if (mst_cell.Maparea_id == 18 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_184;
				}
				else if (mst_cell.Maparea_id == 18 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_185;
				}
				else if (mst_cell.Maparea_id == 18 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_186;
				}
				else if (mst_cell.Maparea_id == 18 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_187;
				}
				else if (mst_cell.Maparea_id == 19 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_191;
				}
				else if (mst_cell.Maparea_id == 19 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_192;
				}
				else if (mst_cell.Maparea_id == 19 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_193;
				}
				else if (mst_cell.Maparea_id == 19 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_194;
				}
				else if (mst_cell.Maparea_id == 19 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_195;
				}
				else if (mst_cell.Maparea_id == 19 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_196;
				}
				else if (mst_cell.Maparea_id == 19 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_197;
				}
				else if (mst_cell.Maparea_id == 20 && mst_cell.Mapinfo_no == 1)
				{
					mapFunc = getMapCell_201;
				}
				else if (mst_cell.Maparea_id == 20 && mst_cell.Mapinfo_no == 2)
				{
					mapFunc = getMapCell_202;
				}
				else if (mst_cell.Maparea_id == 20 && mst_cell.Mapinfo_no == 3)
				{
					mapFunc = getMapCell_203;
				}
				else if (mst_cell.Maparea_id == 20 && mst_cell.Mapinfo_no == 4)
				{
					mapFunc = getMapCell_204;
				}
				else if (mst_cell.Maparea_id == 20 && mst_cell.Mapinfo_no == 5)
				{
					mapFunc = getMapCell_205;
				}
				else if (mst_cell.Maparea_id == 20 && mst_cell.Mapinfo_no == 6)
				{
					mapFunc = getMapCell_206;
				}
				else if (mst_cell.Maparea_id == 20 && mst_cell.Mapinfo_no == 7)
				{
					mapFunc = getMapCell_207;
				}
			}
		}

		private int getMapCell_11()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 35)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_12()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_13()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 65)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 <= 70)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 70)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_14()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				List<double> list = new List<double>();
				list.Add(30.0);
				list.Add(40.0);
				list.Add(30.0);
				switch (Utils.GetRandomRateIndex(list))
				{
				case 0:
					return mst_cell.Next_no_3;
				case 1:
					return mst_cell.Next_no_2;
				default:
					return mst_cell.Next_no_1;
				}
			}
			if (mst_cell.No == 4 || mst_cell.No == 11)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 <= 55)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_15()
		{
			return 0;
		}

		private int getMapCell_16()
		{
			return 0;
		}

		private int getMapCell_17()
		{
			return 0;
		}

		private int getMapCell_21()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[11] + dictionary[18];
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 1)
				{
					return mst_cell.Next_no_3;
				}
				if (num2 >= 3)
				{
					return mst_cell.Next_no_3;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 >= 2 && num4 <= 35)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5 || mst_cell.No == 12)
			{
				double num5 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num5 += shipSakuParam;
				}
				int num6 = dictionary[13] + dictionary[14];
				if (num6 >= 6)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 < (double)((int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 40))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6)
			{
				bool flag2 = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num7 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num8 = dictionary[11] + dictionary[18] + dictionary[7];
				int num9 = dictionary[11] + dictionary[18];
				if (num9 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num8 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8 || mst_cell.No == 13)
			{
				double num10 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item5.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num10 += shipSakuParam2;
				}
				int num11 = dictionary[13] + dictionary[14];
				if (num11 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num10 < (double)((int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 12))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_22()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[6] + dictionary[7] + dictionary[10] + dictionary[11] + dictionary[16] + dictionary[18];
				if (num >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3 && dictionary[5] <= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				if (num3 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 9)
			{
				double num4 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num4 += shipSakuParam;
				}
				int num5 = dictionary[13] + dictionary[14];
				if (num5 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 < (double)((int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 25))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_23()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num4 = dictionary[5] + dictionary[6];
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[10] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num4 >= 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 <= 35)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num6 = dictionary[11] + dictionary[18];
				int num7 = dictionary[13] + dictionary[14];
				int num8 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num8 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num7 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 2)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 12)
			{
				double num9 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num9 += shipSakuParam;
				}
				int num10 = dictionary[13] + dictionary[14];
				if (num10 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 30))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7)
			{
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item5.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
				}
				int num11 = dictionary[13] + dictionary[14];
				int num12 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num13 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num12 >= 6)
				{
					return mst_cell.Next_no_1;
				}
				if (num12 == 5 && num13 <= 75)
				{
					return mst_cell.Next_no_1;
				}
				if (num12 == 4 && num13 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				if (num11 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_24()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[11] + dictionary[18];
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 3 && num4 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 6)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num5 = dictionary[11] + dictionary[18];
				int num6 = dictionary[5] + dictionary[6];
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num7 <= 2 && dictionary[3] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 4 && dictionary[3] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 3 && dictionary[2] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7 || mst_cell.No == 13)
			{
				double num8 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num8 += shipSakuParam;
				}
				if (num8 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 33))
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8)
			{
				double num9 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num9 += shipSakuParam2;
				}
				if (num9 < (double)((int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 40))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_25()
		{
			return 0;
		}

		private int getMapCell_26()
		{
			return 0;
		}

		private int getMapCell_27()
		{
			return 0;
		}

		private int getMapCell_31()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 70)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[16] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3)
			{
				bool flag = false;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 == 0 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (flag)
				{
					return mst_cell.Next_no_2;
				}
				if (num4 <= 60)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_32()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[2] <= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 4)
				{
					return mst_cell.Next_no_3;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 4 && num <= 25)
				{
					return mst_cell.Next_no_3;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] == 6)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				if (dictionary[2] <= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 5 || mst_cell.No == 10)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[2] == 6)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 5 && num3 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 4 && num2 <= 90)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_33()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 3)
			{
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8 || mst_cell.No == 12)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num4 = dictionary[8] + dictionary[9] + dictionary[10];
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_3;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[3] >= 1 && dictionary[2] >= 2 && num5 <= 80)
				{
					return mst_cell.Next_no_3;
				}
				num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 == 0 && dictionary[3] >= 1 && num5 <= 70)
				{
					return mst_cell.Next_no_3;
				}
				num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_3;
			}
			return 0;
		}

		private int getMapCell_34()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[7];
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 <= 2 && num5 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				bool flag = true;
				bool flag2 = false;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Yomi == "あきつしま")
					{
						flag2 = true;
					}
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num6 = dictionary[11] + dictionary[18] + dictionary[7];
				int num7 = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				if (flag2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num7 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num6 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_35()
		{
			return 0;
		}

		private int getMapCell_36()
		{
			return 0;
		}

		private int getMapCell_37()
		{
			return 0;
		}

		private int getMapCell_41()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				int num2 = dictionary[11] + dictionary[18];
				int num3 = dictionary[13] + dictionary[14];
				if (num2 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 <= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 2)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7 || mst_cell.No == 14)
			{
				double num4 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num4 += shipSakuParam;
				}
				if (dictionary[2] == 0 && getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 46)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 1 && getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 37)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 2 && getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 28)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_42()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				if (num + dictionary[7] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num + dictionary[7] >= 3 && dictionary[2] == 0)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 3 && dictionary[2] <= 1)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5)
			{
				double num2 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num2 += shipSakuParam;
				}
				if (getMapSakuParam(num2, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 40)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9)
			{
				double num3 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item3.Ship_id, slotSakuParam2);
					num3 += shipSakuParam2;
				}
				if (getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 22)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_43()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[11] + dictionary[18];
				int num4 = dictionary[13] + dictionary[14];
				int num5 = dictionary[8] + dictionary[9] + dictionary[10];
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2 && num5 == 0)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 <= 2 && num6 <= 75)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 6 || mst_cell.No == 15)
			{
				bool flag = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num8 = dictionary[11] + dictionary[18] + dictionary[7];
				int num9 = dictionary[13] + dictionary[14];
				int num10 = dictionary[8] + dictionary[9] + dictionary[10];
				int num11 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num8 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag && num7 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag && num7 >= 2 && num11 <= 75)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag && num7 >= 1 && num11 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2 && num10 == 0 && num8 >= 1)
				{
					return mst_cell.Next_no_3;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2 && num7 == 0)
				{
					return mst_cell.Next_no_3;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_3;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7)
			{
				bool flag2 = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				int num12 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num13 = dictionary[11] + dictionary[18] + dictionary[7];
				int num14 = dictionary[13] + dictionary[14];
				if (num12 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num13 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num14 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag2)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8 || mst_cell.No == 16)
			{
				double num15 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num15 += shipSakuParam;
				}
				int num16 = dictionary[13] + dictionary[14];
				if (num16 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (getMapSakuParam(num15, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 55)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 10 || mst_cell.No == 18)
			{
				double num17 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num17 += shipSakuParam2;
				}
				if (getMapSakuParam(num17, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 27)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_44()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (flag)
				{
					return mst_cell.Next_no_2;
				}
				if (num <= 30)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 1)
			{
				bool flag2 = false;
				bool flag3 = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Yomi == "あきつしま")
					{
						flag2 = true;
					}
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag3 = false;
					}
				}
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (flag2)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag3)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 17)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num4 = dictionary[13] + dictionary[14];
				int num5 = dictionary[11] + dictionary[18] + dictionary[7];
				int num6 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num4 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 == 6)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_3;
				}
				if (num6 == 5)
				{
					return mst_cell.Next_no_3;
				}
				switch (num5)
				{
				case 6:
					return mst_cell.Next_no_1;
				case 5:
					return mst_cell.Next_no_3;
				default:
					return mst_cell.Next_no_2;
				}
			}
			if (mst_cell.No == 9 || mst_cell.No == 19)
			{
				double num7 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num7 += shipSakuParam;
				}
				if (getMapSakuParam(num7, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 50)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 12)
			{
				double num8 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item5.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num8 += shipSakuParam2;
				}
				int num9 = dictionary[13] + dictionary[14];
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				int mapSakuParam = getMapSakuParam(num8, enumScoutingKind.K2);
				if (num9 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (mapSakuParam < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 32 && num10 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				if (mapSakuParam < (int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 40)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_3;
			}
			return 0;
		}

		private int getMapCell_45()
		{
			return 0;
		}

		private int getMapCell_46()
		{
			return 0;
		}

		private int getMapCell_47()
		{
			return 0;
		}

		private int getMapCell_51()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[5] + dictionary[6];
				int num2 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 3 && dictionary[3] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 2)
			{
				bool flag2 = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					return mst_cell.Next_no_2;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num4 = 0;
				foreach (KeyValuePair<int, List<Mst_slotitem>> mst_slotitem in mst_slotitems)
				{
					num4 += getDrumCount(mst_slotitem.Value);
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[10] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[6] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 5)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 <= 40)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num6 = 0;
				foreach (KeyValuePair<int, List<Mst_slotitem>> mst_slotitem2 in mst_slotitems)
				{
					num6 += getDrumCount(mst_slotitem2.Value);
				}
				if (dictionary[10] >= 2 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 60)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[6] >= 2 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 60)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if ((int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 40)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_52()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18];
				if (num == 2 && dictionary[3] == 1)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18];
				int num3 = dictionary[8] + dictionary[9] + dictionary[10];
				if (num2 == 2 && dictionary[7] == 1 && num3 <= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				bool flag = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num4 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4 || mst_cell.No == 11)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num5 = 0;
				foreach (KeyValuePair<int, List<Mst_slotitem>> mst_slotitem in mst_slotitems)
				{
					num5 += getDrumCount(mst_slotitem.Value);
				}
				int num6 = dictionary[11] + dictionary[18];
				int num7 = dictionary[8] + dictionary[9] + dictionary[10];
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 2 && dictionary[7] >= 1 && num7 <= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num8 <= 30)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7)
			{
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item5.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
				}
				int num9 = 0;
				foreach (KeyValuePair<int, List<Mst_slotitem>> mst_slotitem2 in mst_slotitems)
				{
					num9 += getDrumCount(mst_slotitem2.Value);
				}
				if (num9 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_53()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 2)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (!flag && num <= 75)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7 || mst_cell.No == 12)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18];
				int num3 = dictionary[11] + dictionary[18] + dictionary[7];
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num4 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[5] + dictionary[6] >= 2 && dictionary[3] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				List<double> list = new List<double>();
				list.Add(10.0);
				list.Add(30.0);
				list.Add(60.0);
				switch (Utils.GetRandomRateIndex(list))
				{
				case 0:
					return mst_cell.Next_no_2;
				case 1:
					return mst_cell.Next_no_1;
				default:
					return mst_cell.Next_no_3;
				}
			}
			if (mst_cell.No == 8)
			{
				bool flag2 = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				int num5 = dictionary[5] + dictionary[6];
				int num6 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (!flag2)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 4 && dictionary[3] == 1 && dictionary[2] == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3 || mst_cell.No == 13 || mst_cell.No == 14)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num7 <= 25)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_54()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num8 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num2 >= 2)
				{
					return mst_cell.Next_no_3;
				}
				if (!flag)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num <= 1)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				bool flag2 = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				int num3 = dictionary[8] + dictionary[9] + dictionary[10];
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num4 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag2)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 9)
			{
				bool flag3 = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag3 = false;
					}
				}
				int num5 = dictionary[8] + dictionary[9] + dictionary[10];
				if (!flag3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 <= 1)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 12 || mst_cell.No == 18)
			{
				double num6 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num6 += shipSakuParam;
				}
				int num7 = 0;
				foreach (KeyValuePair<int, List<Mst_slotitem>> mst_slotitem in mst_slotitems)
				{
					num7 += getDrumCount(mst_slotitem.Value);
				}
				if (num6 < (double)((int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 40))
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 2 && num7 >= 3)
				{
					return mst_cell.Next_no_3;
				}
				if (num6 < (double)((int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 50))
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_55()
		{
			return 0;
		}

		private int getMapCell_56()
		{
			return 0;
		}

		private int getMapCell_57()
		{
			return 0;
		}

		private int getMapCell_61()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[5] + dictionary[6];
				int num4 = dictionary[13] + dictionary[14];
				int num5 = dictionary[2] + dictionary[3];
				if (num + num2 + num3 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 2)
				{
					return mst_cell.Next_no_2;
				}
				switch (num4)
				{
				case 6:
					return mst_cell.Next_no_1;
				case 5:
					if (mem_ship.Count == 5)
					{
						return mst_cell.Next_no_1;
					}
					break;
				}
				if (num4 == 4 && mem_ship.Count == 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 == 3 && mem_ship.Count == 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[20] == 1 && num4 == 3 && dictionary[2] == 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[20] == 1 && num4 == 4 && num5 == 1)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[20] == 1 && num4 == 5)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[20] == 1 && num4 == 4 && mem_ship.Count == 5)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[20] == 1 && num4 == 3 && mem_ship.Count == 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 == 0)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				if (dictionary[20] == 1)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7)
			{
				double num6 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num6 += shipSakuParam;
				}
				int num7 = dictionary[8] + dictionary[9] + dictionary[10];
				int num8 = dictionary[11] + dictionary[18] + dictionary[7];
				int num9 = dictionary[5] + dictionary[6];
				int num10 = dictionary[13] + dictionary[14];
				int num11 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 + num8 + num9 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num10 <= 2 && num11 <= 35)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8)
			{
				double num12 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num12 += shipSakuParam2;
				}
				int num13 = dictionary[13] + dictionary[14];
				int num14 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (getMapSakuParam(num12, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 7)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[20] == 1)
				{
					setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
					return mst_cell.Next_no_3;
				}
				if (getMapSakuParam(num12, enumScoutingKind.K2) > (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 10)
				{
					setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
					return mst_cell.Next_no_3;
				}
				if (num13 <= 3 && num14 <= 35 && dictionary[2] <= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num13 <= 2 && num14 <= 70 && dictionary[2] <= 1)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_3;
			}
			return 0;
		}

		private int getMapCell_62()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num >= 5)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 <= 20)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 <= 35)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if ((int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[7];
				int num4 = dictionary[11] + dictionary[18];
				int num5 = dictionary[13] + dictionary[14];
				if (num3 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				if (num4 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				bool flag = false;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num6 = dictionary[11] + dictionary[18] + dictionary[7];
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num8 = dictionary[5] + dictionary[6];
				int num9 = dictionary[3] + dictionary[4] + dictionary[21];
				if (num6 == 0 && dictionary[3] >= 1 && dictionary[2] >= 2 && num7 <= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3 && num8 == 1 && num9 == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3 && num8 == 2)
				{
					return mst_cell.Next_no_2;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 2 && flag)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3 && num7 <= 1)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num10 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num11 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num10 >= 4 && num11 <= 60)
				{
					return mst_cell.Next_no_1;
				}
				if (num10 >= 3 && num11 <= 30)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] == 0)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5 || mst_cell.No == 10)
			{
				double num12 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item5.Ship_id, slotSakuParam);
					num12 += shipSakuParam;
				}
				if (getMapSakuParam(num12, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 12)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_63()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 5 || mst_cell.No == 11)
			{
				bool flag = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (flag)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[16] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] <= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8 || mst_cell.No == 12)
			{
				double num2 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num2 += shipSakuParam;
				}
				if (getMapSakuParam(num2, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 27)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_64()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[7] + dictionary[6] + dictionary[16] + dictionary[17];
				int num2 = dictionary[13] + dictionary[14];
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 <= 35)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num5 = dictionary[11] + dictionary[18] + dictionary[7];
				int num6 = dictionary[8] + dictionary[9] + dictionary[10];
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num6 == 0 && num7 <= 30)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3 || mst_cell.No == 12)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num8 = dictionary[13] + dictionary[14];
				int num9 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num10 = dictionary[5] + dictionary[6];
				int num11 = dictionary[11] + dictionary[18] + dictionary[7];
				int num12 = dictionary[8] + dictionary[9] + dictionary[10];
				if (num8 == 6)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 + num10 == 6)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 + num8 == 6)
				{
					return mst_cell.Next_no_1;
				}
				if (num11 >= 3)
				{
					return mst_cell.Next_no_3;
				}
				if (num11 + num12 >= 3)
				{
					return mst_cell.Next_no_3;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4 || mst_cell.No == 13)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num13 = dictionary[2] + dictionary[3];
				int num14 = dictionary[11] + dictionary[18] + dictionary[7];
				int num15 = dictionary[8] + dictionary[9] + dictionary[10];
				if (dictionary[2] <= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num13 == 6)
				{
					return mst_cell.Next_no_2;
				}
				if (num14 + num15 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num13 == 5)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 5)
			{
				double num16 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item5.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item5.Ship_id, slotSakuParam);
					num16 += shipSakuParam;
				}
				int num17 = dictionary[13] + dictionary[14];
				int num18 = dictionary[11] + dictionary[18] + dictionary[7];
				int num19 = dictionary[8] + dictionary[9] + dictionary[10];
				if (num19 >= 2)
				{
					return mst_cell.Next_no_3;
				}
				if (num18 >= 2)
				{
					return mst_cell.Next_no_3;
				}
				if (num18 + num19 >= 3)
				{
					return mst_cell.Next_no_3;
				}
				if (dictionary[2] <= 1)
				{
					return mst_cell.Next_no_3;
				}
				if (num17 >= 5)
				{
					return mst_cell.Next_no_3;
				}
				if (getMapSakuParam(num16, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 45)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				double num20 = 0.0;
				foreach (Mem_ship item6 in mem_ship)
				{
					Dictionary<int, int> dictionary12;
					Dictionary<int, int> dictionary13 = dictionary12 = dictionary;
					int stype;
					int key6 = stype = item6.Stype;
					stype = dictionary12[stype];
					dictionary13[key6] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item6, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item6.Ship_id, slotSakuParam2);
					num20 += shipSakuParam2;
				}
				int num21 = dictionary[13] + dictionary[14];
				if (getMapSakuParam(num20, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 13)
				{
					return mst_cell.Next_no_1;
				}
				if (num21 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9 || mst_cell.No == 16)
			{
				double num22 = 0.0;
				foreach (Mem_ship item7 in mem_ship)
				{
					Dictionary<int, int> dictionary14;
					Dictionary<int, int> dictionary15 = dictionary14 = dictionary;
					int stype;
					int key7 = stype = item7.Stype;
					stype = dictionary14[stype];
					dictionary15[key7] = stype + 1;
					double slotSakuParam3 = getSlotSakuParam(item7, enumScoutingKind.K2);
					double shipSakuParam3 = getShipSakuParam(item7.Ship_id, slotSakuParam3);
					num22 += shipSakuParam3;
				}
				int num23 = dictionary[13] + dictionary[14];
				if (getMapSakuParam(num22, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 36)
				{
					return mst_cell.Next_no_1;
				}
				if (num23 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_65()
		{
			return 0;
		}

		private int getMapCell_66()
		{
			return 0;
		}

		private int getMapCell_67()
		{
			return 0;
		}

		private int getMapCell_71()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5)
			{
				double num2 = 0.0;
				foreach (Mem_ship item in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item.Ship_id, slotSakuParam);
					num2 += shipSakuParam;
				}
				if (num2 < (double)((int)Utils.GetRandDouble(0.0, 0.0, 1.0, 1) + 5))
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				double num3 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item2.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item2.Ship_id, slotSakuParam2);
					num3 += shipSakuParam2;
				}
				if (dictionary[3] <= 1)
				{
					setCommentData(enumProductionKind.A, ref comment_kind, ref production_kind);
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 < (double)((int)Utils.GetRandDouble(0.0, 0.0, 1.0, 1) + 3))
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 9 || mst_cell.No == 13)
			{
				double num4 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item3.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					double slotSakuParam3 = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam3 = getShipSakuParam(item3.Ship_id, slotSakuParam3);
					num4 += shipSakuParam3;
				}
				if (dictionary[3] >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 < (double)((int)Utils.GetRandDouble(0.0, 0.0, 1.0, 1) + 5))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.A, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_72()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[8] + dictionary[9] + dictionary[10];
				int num3 = dictionary[11] + dictionary[18];
				int num4 = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				if (num >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[4] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 + num4 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num5 = dictionary[13] + dictionary[14];
				int num6 = dictionary[11] + dictionary[18];
				int num7 = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				if (num5 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 + num7 >= 6)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				bool flag = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				if (dictionary[3] == 2 && dictionary[2] == 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] == 1 && dictionary[2] >= 4 && flag)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[6] == 1 && dictionary[3] == 1 && dictionary[2] == 3 && flag)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[6] == 2 && dictionary[3] == 1 && dictionary[2] == 3)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7 || mst_cell.No == 13)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num8 = dictionary[8] + dictionary[9] + dictionary[10];
				int num9 = dictionary[11] + dictionary[18];
				if (dictionary[4] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 + num8 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8 || mst_cell.No == 14)
			{
				double num10 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item5.Ship_id, slotSakuParam);
					num10 += shipSakuParam;
				}
				if (num10 < (double)((int)Utils.GetRandDouble(0.0, 0.0, 1.0, 1) + 15))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9)
			{
				double num11 = 0.0;
				foreach (Mem_ship item6 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item6, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item6.Ship_id, slotSakuParam2);
					num11 += shipSakuParam2;
				}
				if (num11 < (double)((int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 20))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_73()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				double num2 = 0.0;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item.Ship_id, slotSakuParam);
					num2 += shipSakuParam;
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num4 = dictionary[13] + dictionary[14];
				if (num3 >= 2)
				{
					return mst_cell.Next_no_3;
				}
				if (num4 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 < (double)((int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 20))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4 || mst_cell.No == 12)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num5 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num6 = dictionary[5] + dictionary[6];
				int num7 = dictionary[2] + dictionary[3];
				if (num5 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 + num6 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num7 == 0)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7 || mst_cell.No == 14)
			{
				double num9 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num9 += shipSakuParam2;
				}
				int num10 = dictionary[13] + dictionary[14];
				if (num10 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 25))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_74()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[4] == 0)
				{
					return mst_cell.Next_no_2;
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num4 = dictionary[13] + dictionary[14];
				if (num3 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5 || mst_cell.No == 14)
			{
				bool flag = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num5 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num6 = dictionary[13] + dictionary[14];
				if (num6 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (!flag && num7 <= 70)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 6 || mst_cell.No == 15)
			{
				double num8 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num8 += shipSakuParam;
				}
				if (num8 < (double)((int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 35))
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num9 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 7)
			{
				double num10 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item5.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num10 += shipSakuParam2;
				}
				int num11 = dictionary[13] + dictionary[14];
				if (num10 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 30))
				{
					return mst_cell.Next_no_1;
				}
				if (num11 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				int num12 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num12 <= 50)
				{
					return mst_cell.Next_no_3;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_75()
		{
			return 0;
		}

		private int getMapCell_76()
		{
			return 0;
		}

		private int getMapCell_77()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = dictionary[13] + dictionary[14];
				if (num2 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 0 && dictionary[3] == 0)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_81()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_82()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num3 = dictionary[7] + dictionary[6] + dictionary[16] + dictionary[17];
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 35)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num5 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num6 = dictionary[7] + dictionary[6] + dictionary[16] + dictionary[17];
				if (num5 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num6 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_83()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num2 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[5] >= 1 && dictionary[5] <= 3 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 30)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 4 || mst_cell.No == 9)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num5 = dictionary[7] + dictionary[6] + dictionary[16] + dictionary[17];
				if (num4 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 1 && num5 <= 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[17] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_84()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num3 = dictionary[13] + dictionary[14];
				if (num2 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[5] >= 1 && dictionary[5] <= 2 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 25)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num5 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num6 = dictionary[13] + dictionary[14];
				if (num5 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num6 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] <= 1)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 5 || mst_cell.No == 10)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num8 = dictionary[13] + dictionary[14];
				if (num7 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[17] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num8 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num9 <= 35)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_85()
		{
			return 0;
		}

		private int getMapCell_86()
		{
			return 0;
		}

		private int getMapCell_87()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num >= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3 || mst_cell.No == 8)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num2 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[5] >= 1 && dictionary[5] <= 2 && dictionary[3] >= 1 && dictionary[3] <= 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_91()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[6] + dictionary[7] + dictionary[10] + dictionary[11] + dictionary[16] + dictionary[18];
				if (num >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[10] >= 2 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[17] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 40)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num5 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num6 = dictionary[13] + dictionary[14];
				if (num6 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] == 0)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 12)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num8 = dictionary[13] + dictionary[14];
				if (num8 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num7 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_92()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			int count = mem_ship.Count;
			if (mst_cell.No == 2)
			{
				if (count <= 4)
				{
					return mst_cell.Next_no_2;
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num2 = dictionary[13] + dictionary[14];
				if (count >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 25)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num4 = dictionary[13] + dictionary[14];
				int num5 = dictionary[3] + dictionary[4] + dictionary[21];
				int num6 = dictionary[8] + dictionary[9] + dictionary[10];
				if (num4 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num6 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (count >= 5)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num8 = dictionary[13] + dictionary[14];
				if (count <= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[7] + dictionary[17] == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[16] + dictionary[6] == 1)
				{
					return mst_cell.Next_no_2;
				}
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_93()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				if (dictionary[7] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[10] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[5] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[6] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] <= 3)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				if (dictionary[3] == 1 && dictionary[2] == 5)
				{
					return mst_cell.Next_no_2;
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 75)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7)
			{
				double num2 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num2 += shipSakuParam;
				}
				if (dictionary[10] + dictionary[5] + dictionary[7] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[10] + dictionary[5] + dictionary[6] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 < (double)((int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 7))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_94()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = dictionary[13] + dictionary[14];
				if (num >= 3)
				{
					return mst_cell.Next_no_3;
				}
				if (num2 >= 1)
				{
					return mst_cell.Next_no_3;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 1 || mst_cell.No == 11)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num5 = dictionary[13] + dictionary[14];
				if (num4 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 1 || dictionary[3] == 0)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num6 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (dictionary[10] >= 2 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[17] >= 1 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_95()
		{
			return 0;
		}

		private int getMapCell_96()
		{
			return 0;
		}

		private int getMapCell_97()
		{
			return 0;
		}

		private int getMapCell_101()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 1 && num2 <= 60)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_102()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[10] >= 2 && dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (flag && num == 0 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 2 && num == 0 && num2 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8)
			{
				double num3 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num3 += shipSakuParam;
				}
				if (getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_103()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 3)
			{
				bool flag = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = dictionary[8] + dictionary[9] + dictionary[10];
				int num3 = dictionary[11] + dictionary[18] + dictionary[7];
				if (num >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 2 && num3 <= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 2 && flag)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5)
			{
				double num4 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num4 += shipSakuParam;
				}
				if (getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 37)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_104()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num = dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = dictionary[11] + dictionary[18];
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num <= 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (flag && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 6)
			{
				double num3 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num3 += shipSakuParam;
				}
				if (getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 32)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 10)
			{
				double num4 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item3.Ship_id, slotSakuParam2);
					num4 += shipSakuParam2;
				}
				if (getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 42)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_105()
		{
			return 0;
		}

		private int getMapCell_106()
		{
			return 0;
		}

		private int getMapCell_107()
		{
			return 0;
		}

		private int getMapCell_111()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 <= 30)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 2 || mst_cell.No == 8)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[6] + dictionary[7] + dictionary[10] + dictionary[11] + dictionary[16] + dictionary[18];
				if (num3 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 30)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3)
			{
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_112()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num2 = dictionary[6] + dictionary[7] + dictionary[10] + dictionary[11] + dictionary[16] + dictionary[18];
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 >= 1 && num3 <= 75)
				{
					return mst_cell.Next_no_2;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 60)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 12)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				if (dictionary[16] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 70)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				if (mem_ship[0].Stype == 14)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship[0].Stype == 3)
				{
					return mst_cell.Next_no_1;
				}
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 40)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[10] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[17] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 <= 30)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_113()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[3] >= 2 && num == 0 && num2 <= 80)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && num == 0 && num2 <= 65)
				{
					return mst_cell.Next_no_2;
				}
				List<double> list = new List<double>();
				list.Add(35.0);
				list.Add(35.0);
				list.Add(30.0);
				switch (Utils.GetRandomRateIndex(list))
				{
				case 0:
					return mst_cell.Next_no_3;
				case 1:
					return mst_cell.Next_no_2;
				default:
					return mst_cell.Next_no_1;
				}
			}
			if (mst_cell.No == 6)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[13] + dictionary[14];
				if (num3 >= 3 && dictionary[20] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 <= 4 && dictionary[14] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 == 0)
				{
					return mst_cell.Next_no_2;
				}
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 70)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 11 || mst_cell.No == 19)
			{
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 14)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num7 = dictionary[6] + dictionary[7] + dictionary[10] + dictionary[11] + dictionary[16] + dictionary[18];
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num7 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 <= 65)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_114()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				int num = 0;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (getDrumCount(mst_slotitems[item.Rid]) > 0)
					{
						num++;
					}
				}
				int num2 = dictionary[13] + dictionary[14];
				int num3 = dictionary[11] + dictionary[18];
				int num4 = dictionary[8] + dictionary[9] + dictionary[10];
				if (num2 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 1 || dictionary[7] >= 1 || dictionary[16] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 <= 30)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 1)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num6 = dictionary[13] + dictionary[14];
				if (num6 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num7 = dictionary[13] + dictionary[14];
				int num8 = dictionary[11] + dictionary[18];
				int num9 = dictionary[9] + dictionary[10];
				if (num7 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num8 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num8 >= 2 && dictionary[7] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 >= 2 && num10 <= 25)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 1 && num10 <= 30)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[16] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[8] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num11 = dictionary[8] + dictionary[9] + dictionary[10];
				int num12 = dictionary[9] + dictionary[10];
				if (num11 == 0 || dictionary[2] >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[6] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[16] >= 1)
				{
					return mst_cell.Next_no_2;
				}
				int num13 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num12 >= 1 && num13 <= 40)
				{
					return mst_cell.Next_no_1;
				}
				int num14 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num14 <= 25)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6)
			{
				int num15 = 0;
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item5.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
					if (getDrumCount(mst_slotitems[item5.Rid]) > 0)
					{
						num15++;
					}
				}
				int num16 = dictionary[11] + dictionary[18];
				int num17 = dictionary[8] + dictionary[9] + dictionary[10];
				if (num16 >= 1 || dictionary[7] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num17 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num15 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[5] == 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[6] == 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				int num18 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num18 <= 35)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8 || mst_cell.No == 13 || mst_cell.No == 14)
			{
				double num19 = 0.0;
				foreach (Mem_ship item6 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item6, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item6.Ship_id, slotSakuParam);
					num19 += shipSakuParam;
				}
				if (num19 < (double)((int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9)
			{
				int num20 = 0;
				double num21 = 0.0;
				foreach (Mem_ship item7 in mem_ship)
				{
					if (getDrumCount(mst_slotitems[item7.Rid]) > 0)
					{
						num20++;
					}
					double slotSakuParam2 = getSlotSakuParam(item7, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item7.Ship_id, slotSakuParam2);
					num21 += shipSakuParam2;
				}
				if (num20 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num21 < (double)((int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25))
				{
					return mst_cell.Next_no_2;
				}
				if (num21 < (double)((int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 40))
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_3;
			}
			return 0;
		}

		private int getMapCell_115()
		{
			return 0;
		}

		private int getMapCell_116()
		{
			return 0;
		}

		private int getMapCell_117()
		{
			return 0;
		}

		private int getMapCell_121()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[3] + dictionary[21];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num >= 2 && num2 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship.Count == 6)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 13)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (mem_ship.Count <= 4 && num3 <= 75)
				{
					return mst_cell.Next_no_2;
				}
				if (mem_ship.Count <= 3)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8)
			{
				double num4 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num4 += shipSakuParam;
				}
				if (getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 6)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9)
			{
				double num5 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num5 += shipSakuParam2;
				}
				if (getMapSakuParam(num5, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 7)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_122()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 1)
			{
				bool flag = false;
				bool flag2 = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				int num2 = dictionary[8] + dictionary[9] + dictionary[10];
				int num3 = dictionary[11] + dictionary[18];
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num4 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (flag && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (flag2 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 5)
			{
				double num5 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num5 += shipSakuParam;
				}
				int num6 = dictionary[13] + dictionary[14];
				if (num6 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (getMapSakuParam(num5, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 30)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8 || mst_cell.No == 12)
			{
				double num7 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item3.Ship_id, slotSakuParam2);
					num7 += shipSakuParam2;
				}
				if (getMapSakuParam(num7, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 45)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_123()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (dictionary[17] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (flag && num >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[7];
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				double num5 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num5 += shipSakuParam;
				}
				if (getMapSakuParam(num5, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 27)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5 || mst_cell.No == 12)
			{
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item4.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num6 = dictionary[11] + dictionary[18] + dictionary[7];
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num6 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				if (num7 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7)
			{
				double num8 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item5.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num8 += shipSakuParam2;
				}
				int num9 = dictionary[13] + dictionary[14];
				if (num9 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (getMapSakuParam(num8, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 42)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				double num10 = 0.0;
				foreach (Mem_ship item6 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item6.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
					double slotSakuParam3 = getSlotSakuParam(item6, enumScoutingKind.K2);
					double shipSakuParam3 = getShipSakuParam(item6.Ship_id, slotSakuParam3);
					num10 += shipSakuParam3;
				}
				int num11 = dictionary[13] + dictionary[14];
				if (num11 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (getMapSakuParam(num10, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 32)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_124()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (flag && num <= 2 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (flag && dictionary[6] >= 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num4 = dictionary[11] + dictionary[18];
				int num5 = dictionary[11] + dictionary[18] + dictionary[7];
				int num6 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3)
			{
				bool flag2 = true;
				double num7 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num7 += shipSakuParam;
				}
				int num8 = dictionary[13] + dictionary[14];
				int num9 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num8 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (getMapSakuParam(num7, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 28)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag2)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 <= 2 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[6] >= 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num9 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num10 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				double num11 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num11 += shipSakuParam2;
				}
				int num12 = dictionary[13] + dictionary[14];
				if (num12 >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (getMapSakuParam(num11, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 43)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 5)
			{
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item5.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
				}
				int num13 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num14 = dictionary[13] + dictionary[14];
				int num15 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num13 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num14 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num15 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7)
			{
				double num16 = 0.0;
				foreach (Mem_ship item6 in mem_ship)
				{
					Dictionary<int, int> dictionary12;
					Dictionary<int, int> dictionary13 = dictionary12 = dictionary;
					int stype;
					int key6 = stype = item6.Stype;
					stype = dictionary12[stype];
					dictionary13[key6] = stype + 1;
					double slotSakuParam3 = getSlotSakuParam(item6, enumScoutingKind.K2);
					double shipSakuParam3 = getShipSakuParam(item6.Ship_id, slotSakuParam3);
					num16 += shipSakuParam3;
				}
				int num17 = dictionary[13] + dictionary[14];
				if (num17 >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (getMapSakuParam(num16, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8 || mst_cell.No == 14)
			{
				double num18 = 0.0;
				foreach (Mem_ship item7 in mem_ship)
				{
					double slotSakuParam4 = getSlotSakuParam(item7, enumScoutingKind.K2);
					double shipSakuParam4 = getShipSakuParam(item7.Ship_id, slotSakuParam4);
					num18 += shipSakuParam4;
				}
				if (getMapSakuParam(num18, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 48)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_125()
		{
			return 0;
		}

		private int getMapCell_126()
		{
			return 0;
		}

		private int getMapCell_127()
		{
			return 0;
		}

		private int getMapCell_131()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				if (dictionary[17] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[22] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[19] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 2)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3)
			{
				double num = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num += shipSakuParam;
				}
				int num2 = dictionary[13] + dictionary[14];
				int num6 = dictionary[3] + dictionary[4] + dictionary[21];
				if (getMapSakuParam(num, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 6)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5 || mst_cell.No == 8)
			{
				double num3 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item3.Ship_id, slotSakuParam2);
					num3 += shipSakuParam2;
				}
				int num4 = dictionary[13] + dictionary[14];
				int num7 = dictionary[3] + dictionary[4] + dictionary[21];
				int num5 = dictionary[3] + dictionary[21];
				if (getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 7)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 + dictionary[2] <= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num4 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_132()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				int num2 = dictionary[13] + dictionary[14];
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 <= 30)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 3)
			{
				bool flag = false;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num4 = dictionary[7] + dictionary[6] + dictionary[16] + dictionary[17];
				int num5 = dictionary[11] + dictionary[18] + dictionary[7];
				int num6 = dictionary[13] + dictionary[14];
				int num7 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num5 >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_3;
				}
				if (flag && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num4 >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num7 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 7 || mst_cell.No == 13)
			{
				double num8 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num8 += shipSakuParam;
				}
				if (getMapSakuParam(num8, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				double num9 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num9 += shipSakuParam2;
				}
				if (getMapSakuParam(num9, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 1.0, 1.0, 1) + 7)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_133()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				int num2 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 <= 1 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 2)
			{
				double num3 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num3 += shipSakuParam;
				}
				int num4 = dictionary[11] + dictionary[18] + dictionary[7];
				int num5 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 20)
				{
					return mst_cell.Next_no_2;
				}
				if (num4 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num6 = dictionary[11] + dictionary[18] + dictionary[7];
				int num7 = dictionary[5] + dictionary[6];
				if (num6 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num7 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 5 || mst_cell.No == 13)
			{
				double num8 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num8 += shipSakuParam2;
				}
				if (getMapSakuParam(num8, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 33)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7)
			{
				double num9 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item5.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam3 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam3 = getShipSakuParam(item5.Ship_id, slotSakuParam3);
					num9 += shipSakuParam3;
				}
				int num10 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (getMapSakuParam(num9, enumScoutingKind.K2) > (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 15)
				{
					setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
					return mst_cell.Next_no_3;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num10 <= 30)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] >= 2)
				{
					return mst_cell.Next_no_3;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				foreach (Mem_ship item6 in mem_ship)
				{
					Dictionary<int, int> dictionary10;
					Dictionary<int, int> dictionary11 = dictionary10 = dictionary;
					int stype;
					int key5 = stype = item6.Stype;
					stype = dictionary10[stype];
					dictionary11[key5] = stype + 1;
				}
				int num11 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num11 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_134()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				if (num >= 1)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num2 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 <= 3 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 <= 2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num4 = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				int num5 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num5 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num4 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 9 || mst_cell.No == 15)
			{
				double num6 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num6 += shipSakuParam;
				}
				if (getMapSakuParam(num6, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 36)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 11)
			{
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item5.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				if (dictionary[2] + dictionary[3] <= 1)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			return 0;
		}

		private int getMapCell_135()
		{
			return 0;
		}

		private int getMapCell_136()
		{
			return 0;
		}

		private int getMapCell_137()
		{
			return 0;
		}

		private int getMapCell_141()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 4)
			{
				bool flag = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num = dictionary[3] + dictionary[4] + dictionary[21];
				int num2 = dictionary[5] + dictionary[6];
				if (dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num + num2 <= 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (flag && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5 || mst_cell.No == 10)
			{
				double num3 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num3 += shipSakuParam;
				}
				int num4 = dictionary[3] + dictionary[4] + dictionary[21];
				int num5 = dictionary[5] + dictionary[6];
				if (getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				if (dictionary[2] <= 1)
				{
					return mst_cell.Next_no_3;
				}
				if (num4 + num5 >= 5)
				{
					return mst_cell.Next_no_3;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 11)
			{
				double num6 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item3.Ship_id, slotSakuParam2);
					num6 += shipSakuParam2;
				}
				if (getMapSakuParam(num6, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 20)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 1)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_142()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (flag)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[17] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[13] + dictionary[14];
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[17] >= 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7)
			{
				double num5 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num5 += shipSakuParam;
				}
				int num6 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (getMapSakuParam(num5, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				double num7 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num7 += shipSakuParam2;
				}
				if (getMapSakuParam(num7, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 33)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 12)
			{
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item5.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
				}
				int num8 = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				int num9 = dictionary[3] + dictionary[21];
				int num10 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num10 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num8 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num9 + dictionary[2] <= 1)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_143()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 2)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num3 >= 4 && num4 <= 60)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 3 && num4 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 4 && num4 <= 40)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				if (dictionary[17] >= 1 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num5 = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				int num6 = dictionary[11] + dictionary[18];
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num8 = dictionary[3] + dictionary[21];
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 >= 4 && num9 <= 60)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 3 && num9 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 4 && num9 <= 40)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] + num8 == 0)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				double num10 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num10 += shipSakuParam;
				}
				if (getMapSakuParam(num10, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 36)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9)
			{
				double num11 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item5.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num11 += shipSakuParam2;
				}
				int num12 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num13 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (getMapSakuParam(num11, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 46)
				{
					return mst_cell.Next_no_2;
				}
				if (num12 == 6)
				{
					return mst_cell.Next_no_1;
				}
				if (num12 >= 5 && num13 <= 75)
				{
					return mst_cell.Next_no_1;
				}
				if (num12 >= 4 && num13 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_3;
			}
			return 0;
		}

		private int getMapCell_144()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				int num2 = dictionary[11] + dictionary[18] + dictionary[7];
				int num3 = dictionary[11] + dictionary[18];
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 == 6)
				{
					return mst_cell.Next_no_3;
				}
				if (num4 >= 5 && num5 <= 75)
				{
					return mst_cell.Next_no_3;
				}
				if (num4 >= 4 && num5 <= 50)
				{
					return mst_cell.Next_no_3;
				}
				if (num2 >= 4)
				{
					return mst_cell.Next_no_3;
				}
				if (num3 >= 3)
				{
					return mst_cell.Next_no_3;
				}
				if (num >= 5)
				{
					return mst_cell.Next_no_3;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 15)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num6 = dictionary[8] + dictionary[9] + dictionary[10] + dictionary[5] + dictionary[6] + dictionary[4];
				int num7 = dictionary[11] + dictionary[18] + dictionary[7];
				int num8 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num8 == 6)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num8 == 4)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[17] >= 1)
				{
					return mst_cell.Next_no_3;
				}
				if (num7 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num6 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 9)
			{
				double num9 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num9 += shipSakuParam;
				}
				if (getMapSakuParam(num9, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 3.0, 1.0, 1) + 38)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 10 || mst_cell.No == 16)
			{
				double num10 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num10 += shipSakuParam2;
				}
				if (getMapSakuParam(num10, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 43)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_145()
		{
			return 0;
		}

		private int getMapCell_146()
		{
			return 0;
		}

		private int getMapCell_147()
		{
			return 0;
		}

		private int getMapCell_151()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 4)
			{
				bool flag = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag = true;
					}
				}
				if (dictionary[2] >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (flag && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 6)
			{
				bool flag2 = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 7 || mst_cell.No == 13)
			{
				double num = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num += shipSakuParam;
				}
				if (getMapSakuParam(num, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8)
			{
				double num2 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item4.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num2 += shipSakuParam2;
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (getMapSakuParam(num2, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_152()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[7];
				if (!flag)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 0)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6)
			{
				double num2 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num2 += shipSakuParam;
				}
				if (getMapSakuParam(num2, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 35)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 10)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item3.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num3 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 1)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 12)
			{
				double num4 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num4 += shipSakuParam2;
				}
				if (getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_153()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				bool flag2 = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag2 = true;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (flag && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_3;
				}
				if (flag2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 5)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9)
			{
				double num2 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num2 += shipSakuParam;
				}
				int num3 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (getMapSakuParam(num2, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				if (num3 <= 40)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 11)
			{
				double num4 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item3.Ship_id, slotSakuParam2);
					num4 += shipSakuParam2;
				}
				if (getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 35)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_154()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				bool flag2 = false;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Yomi == "あきつしま")
					{
						flag2 = true;
					}
				}
				int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (flag && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (flag2 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2 && num <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4 || mst_cell.No == 12)
			{
				bool flag3 = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag3 = false;
					}
				}
				int num2 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (flag3 && mem_ship[0].Stype == 3 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (flag3 && dictionary[3] >= 1 && dictionary[2] >= 2 && num2 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				if (flag3 && dictionary[3] >= 1 && dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9)
			{
				double num3 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num3 += shipSakuParam;
				}
				if (dictionary[2] >= 3 && getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 15)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] == 2 && getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 20)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[2] <= 1 && getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 30)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_155()
		{
			return 0;
		}

		private int getMapCell_156()
		{
			return 0;
		}

		private int getMapCell_157()
		{
			return 0;
		}

		private int getMapCell_161()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[20];
				int count = mem_ship.Count;
				if (num == count)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 5 && num2 == 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 4 && num2 == 1 && count == 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 3 && num2 == 1 && count == 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 2 && num2 == 1 && count == 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 4 && num2 == 1 && dictionary[2] == 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 3 && num2 == 1 && dictionary[2] == 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 2 && num2 == 1 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 2 && num2 == 1 && dictionary[2] == 2 && count == 5)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7)
			{
				bool flag = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num3 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num3 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[16] > 0)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8)
			{
				bool flag2 = false;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (mst_slotitems[item3.Rid].Count((Mst_slotitem x) => x.Id == 68) > 0)
					{
						flag2 = true;
					}
				}
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num4 >= 4)
				{
					return mst_cell.Next_no_3;
				}
				if (dictionary[17] + dictionary[16] > 0)
				{
					return mst_cell.Next_no_1;
				}
				if (flag2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 9 || mst_cell.No == 16 || mst_cell.No == 17)
			{
				double num5 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num5 += shipSakuParam;
				}
				if (getMapSakuParam(num5, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 8)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 12 || mst_cell.No == 18)
			{
				double num6 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num6 += shipSakuParam2;
				}
				if (getMapSakuParam(num6, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_162()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[20];
				int count = mem_ship.Count;
				if (num == count)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 5 && num2 == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 4 && num2 == 1 && count == 5)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 3 && num2 == 1 && count == 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 2 && num2 == 1 && count == 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 4 && num2 == 1 && dictionary[2] == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 3 && num2 == 1 && dictionary[2] == 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 2 && num2 == 1 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 2 && num2 == 1 && dictionary[2] == 2 && count == 5)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 4)
			{
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
				}
				int num3 = dictionary[13] + dictionary[14];
				int num4 = dictionary[20];
				int count2 = mem_ship.Count;
				int num5 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num4 == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num3 == count2 && num5 <= 40)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 6)
			{
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
				}
				int num6 = dictionary[13] + dictionary[14];
				int num7 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num8 = dictionary[11] + dictionary[18];
				int count3 = mem_ship.Count;
				if (num6 == count3)
				{
					return mst_cell.Next_no_1;
				}
				if (num7 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				if (num8 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 8 || mst_cell.No == 15)
			{
				double num9 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num9 += shipSakuParam;
				}
				if (getMapSakuParam(num9, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 8)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 10)
			{
				double num10 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num10 += shipSakuParam2;
				}
				if (getMapSakuParam(num10, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 40)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_163()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[20];
				int count = mem_ship.Count;
				if (num == count)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 5 && num2 == 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 4 && num2 == 1 && count == 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 3 && num2 == 1 && count == 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 2 && num2 == 1 && count == 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 4 && num2 == 1 && dictionary[2] == 1)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 3 && num2 == 1 && dictionary[2] == 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 2 && num2 == 1 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (num == 2 && num2 == 1 && dictionary[2] == 2 && count == 5)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6 || mst_cell.No == 14)
			{
				double num3 = 0.0;
				foreach (Mem_ship item2 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item2, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item2.Ship_id, slotSakuParam);
					num3 += shipSakuParam;
				}
				if (getMapSakuParam(num3, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 9)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 11 || mst_cell.No == 16)
			{
				double num4 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item3.Ship_id, slotSakuParam2);
					num4 += shipSakuParam2;
				}
				if (getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 40)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_164()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[20];
				int count = mem_ship.Count;
				if (num == count)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 5 && num2 == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 4 && num2 == 1 && count == 5)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 3 && num2 == 1 && count == 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 2 && num2 == 1 && count == 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 4 && num2 == 1 && dictionary[2] == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 3 && num2 == 1 && dictionary[2] == 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 2 && num2 == 1 && dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (num == 2 && num2 == 1 && dictionary[2] == 2 && count == 5)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 4)
			{
				bool flag = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num3 = dictionary[11] + dictionary[18];
				int num4 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (num4 >= 5)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8 || mst_cell.No == 15 || mst_cell.No == 16)
			{
				double num5 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num5 += shipSakuParam;
				}
				int num6 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (getMapSakuParam(num5, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 45)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 3 || dictionary[2] == 0)
				{
					return mst_cell.Next_no_2;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 10 || mst_cell.No == 18)
			{
				double num7 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num7 += shipSakuParam2;
				}
				if (getMapSakuParam(num7, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1) + 10)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_165()
		{
			return 0;
		}

		private int getMapCell_166()
		{
			return 0;
		}

		private int getMapCell_167()
		{
			return 0;
		}

		private int getMapCell_171()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 3)
			{
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
				}
				int num = dictionary[13] + dictionary[14];
				int num2 = dictionary[5] + dictionary[6];
				int num3 = dictionary[11] + dictionary[18] + dictionary[7];
				int num4 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num2 + num3 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (num >= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[2] == 0)
				{
					return mst_cell.Next_no_2;
				}
				if (num4 <= 50)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_1;
			}
			if (mst_cell.No == 4)
			{
				bool flag = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num5 = dictionary[11] + dictionary[18] + dictionary[7];
				if (dictionary[2] <= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag)
				{
					return mst_cell.Next_no_2;
				}
				if (num5 >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[6] + dictionary[16] + dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 5 || mst_cell.No == 13)
			{
				bool flag2 = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				int num13 = dictionary[13] + dictionary[14];
				int num6 = dictionary[5] + dictionary[6];
				int num7 = dictionary[11] + dictionary[18] + dictionary[7];
				int num8 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num6 + num7 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag2)
				{
					return mst_cell.Next_no_1;
				}
				if (num6 >= 2 && num8 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 6)
			{
				double num9 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					double slotSakuParam = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item4.Ship_id, slotSakuParam);
					num9 += shipSakuParam;
				}
				int num10 = dictionary[11] + dictionary[18] + dictionary[7];
				if (getMapSakuParam(num9, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 25)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				if (dictionary[17] + num10 >= 1)
				{
					return mst_cell.Next_no_3;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 7 || mst_cell.No == 14)
			{
				double num11 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item5.Ship_id, slotSakuParam2);
					num11 += shipSakuParam2;
				}
				if (getMapSakuParam(num11, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 36)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9 || mst_cell.No == 15)
			{
				double num12 = 0.0;
				foreach (Mem_ship item6 in mem_ship)
				{
					double slotSakuParam3 = getSlotSakuParam(item6, enumScoutingKind.K2);
					double shipSakuParam3 = getShipSakuParam(item6.Ship_id, slotSakuParam3);
					num12 += shipSakuParam3;
				}
				if (getMapSakuParam(num12, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 18)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_172()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (!flag)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 5)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 3)
			{
				bool flag2 = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				int num2 = dictionary[13] + dictionary[14];
				int num3 = dictionary[20];
				int num4 = dictionary[11] + dictionary[18] + dictionary[7];
				int num5 = dictionary[8] + dictionary[9] + dictionary[10];
				if (num2 >= 4)
				{
					return mst_cell.Next_no_2;
				}
				if (num2 >= 3 && num3 == 1)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag2)
				{
					return mst_cell.Next_no_1;
				}
				if (num4 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				if (num5 >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 4 || mst_cell.No == 12)
			{
				bool flag3 = true;
				foreach (Mem_ship item3 in mem_ship)
				{
					Dictionary<int, int> dictionary6;
					Dictionary<int, int> dictionary7 = dictionary6 = dictionary;
					int stype;
					int key3 = stype = item3.Stype;
					stype = dictionary6[stype];
					dictionary7[key3] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item3.Ship_id].Soku < 10)
					{
						flag3 = false;
					}
				}
				int num6 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (dictionary[2] <= 1)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag3)
				{
					return mst_cell.Next_no_2;
				}
				if (num6 >= 3)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[6] + dictionary[16] + dictionary[2] >= 4)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 7)
			{
				bool flag4 = true;
				foreach (Mem_ship item4 in mem_ship)
				{
					Dictionary<int, int> dictionary8;
					Dictionary<int, int> dictionary9 = dictionary8 = dictionary;
					int stype;
					int key4 = stype = item4.Stype;
					stype = dictionary8[stype];
					dictionary9[key4] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item4.Ship_id].Soku < 10)
					{
						flag4 = false;
					}
				}
				int num7 = dictionary[13] + dictionary[14];
				int num8 = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				int num9 = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
				if (num7 >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (!flag4)
				{
					return mst_cell.Next_no_2;
				}
				if (num8 >= 5)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_1;
				}
				if (dictionary[16] + dictionary[2] >= 3)
				{
					return mst_cell.Next_no_1;
				}
				if (num8 <= 2 && num9 <= 70)
				{
					return mst_cell.Next_no_1;
				}
				if (num8 == 3 && num9 <= 50)
				{
					return mst_cell.Next_no_1;
				}
				if (num8 == 4 && num9 <= 30)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 8 || mst_cell.No == 14 || mst_cell.No == 15)
			{
				double num10 = 0.0;
				foreach (Mem_ship item5 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item5, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item5.Ship_id, slotSakuParam);
					num10 += shipSakuParam;
				}
				if (getMapSakuParam(num10, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 45)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 9)
			{
				double num11 = 0.0;
				foreach (Mem_ship item6 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item6, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item6.Ship_id, slotSakuParam2);
					num11 += shipSakuParam2;
				}
				if (getMapSakuParam(num11, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 16)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_173()
		{
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Mst_stype.Values.ToDictionary((Mst_stype x) => x.Id, (Mst_stype value) => 0);
			if (mst_cell.No == 0)
			{
				bool flag = true;
				foreach (Mem_ship item in mem_ship)
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary3 = dictionary2 = dictionary;
					int stype;
					int key = stype = item.Stype;
					stype = dictionary2[stype];
					dictionary3[key] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item.Ship_id].Soku < 10)
					{
						flag = false;
					}
				}
				int num = dictionary[11] + dictionary[18] + dictionary[8] + dictionary[9] + dictionary[10];
				if (!flag)
				{
					return mst_cell.Next_no_1;
				}
				if (num >= 5)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 2 || mst_cell.No == 15)
			{
				bool flag2 = true;
				foreach (Mem_ship item2 in mem_ship)
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary5 = dictionary4 = dictionary;
					int stype;
					int key2 = stype = item2.Stype;
					stype = dictionary4[stype];
					dictionary5[key2] = stype + 1;
					if (Mst_DataManager.Instance.Mst_ship[item2.Ship_id].Soku < 10)
					{
						flag2 = false;
					}
				}
				int num2 = dictionary[11] + dictionary[18];
				int num3 = dictionary[8] + dictionary[9] + dictionary[10];
				if (dictionary[3] >= 1 && dictionary[2] >= 2)
				{
					return mst_cell.Next_no_2;
				}
				if (dictionary[16] + dictionary[2] >= 2 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 80)
				{
					return mst_cell.Next_no_2;
				}
				if (!flag2 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 75)
				{
					return mst_cell.Next_no_1;
				}
				if (num2 >= 3 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 60)
				{
					return mst_cell.Next_no_1;
				}
				if (num3 >= 3 && (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 60)
				{
					return mst_cell.Next_no_1;
				}
				if ((int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1) <= 35)
				{
					return mst_cell.Next_no_1;
				}
				return mst_cell.Next_no_3;
			}
			if (mst_cell.No == 10)
			{
				double num4 = 0.0;
				foreach (Mem_ship item3 in mem_ship)
				{
					double slotSakuParam = getSlotSakuParam(item3, enumScoutingKind.K2);
					double shipSakuParam = getShipSakuParam(item3.Ship_id, slotSakuParam);
					num4 += shipSakuParam;
				}
				if (getMapSakuParam(num4, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 55)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			if (mst_cell.No == 11)
			{
				double num5 = 0.0;
				foreach (Mem_ship item4 in mem_ship)
				{
					double slotSakuParam2 = getSlotSakuParam(item4, enumScoutingKind.K2);
					double shipSakuParam2 = getShipSakuParam(item4.Ship_id, slotSakuParam2);
					num5 += shipSakuParam2;
				}
				if (getMapSakuParam(num5, enumScoutingKind.K2) < (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1) + 35)
				{
					return mst_cell.Next_no_1;
				}
				setCommentData(enumProductionKind.B, ref comment_kind, ref production_kind);
				return mst_cell.Next_no_2;
			}
			return 0;
		}

		private int getMapCell_174()
		{
			return 0;
		}

		private int getMapCell_175()
		{
			return 0;
		}

		private int getMapCell_176()
		{
			return 0;
		}

		private int getMapCell_177()
		{
			return 0;
		}

		private int getMapCell_181()
		{
			return 0;
		}

		private int getMapCell_182()
		{
			return 0;
		}

		private int getMapCell_183()
		{
			return 0;
		}

		private int getMapCell_184()
		{
			return 0;
		}

		private int getMapCell_185()
		{
			return 0;
		}

		private int getMapCell_186()
		{
			return 0;
		}

		private int getMapCell_187()
		{
			return 0;
		}

		private int getMapCell_191()
		{
			return 0;
		}

		private int getMapCell_192()
		{
			return 0;
		}

		private int getMapCell_193()
		{
			return 0;
		}

		private int getMapCell_194()
		{
			return 0;
		}

		private int getMapCell_195()
		{
			return 0;
		}

		private int getMapCell_196()
		{
			return 0;
		}

		private int getMapCell_197()
		{
			return 0;
		}

		private int getMapCell_201()
		{
			return 0;
		}

		private int getMapCell_202()
		{
			return 0;
		}

		private int getMapCell_203()
		{
			return 0;
		}

		private int getMapCell_204()
		{
			return 0;
		}

		private int getMapCell_205()
		{
			return 0;
		}

		private int getMapCell_206()
		{
			return 0;
		}

		private int getMapCell_207()
		{
			return 0;
		}

		private Dictionary<int, int> getSlotitemLevel()
		{
			Dictionary<int, int> ret = new Dictionary<int, int>();
			mem_ship.ForEach(delegate(Mem_ship ship)
			{
				ship.Slot.ForEach(delegate(int slot)
				{
					if (slot > 0)
					{
						ret.Add(slot, Comm_UserDatas.Instance.User_slot[slot].Level);
					}
				});
			});
			return ret;
		}

		private double getSlotSakuParam(Mem_ship ship, enumScoutingKind scouting_kind)
		{
			double num = 0.0;
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			new Dictionary<int, double>();
			double num2 = 0.0;
			Dictionary<int, double> dictionary2;
			switch (scouting_kind)
			{
			case enumScoutingKind.C:
			case enumScoutingKind.D:
				dictionary2 = new Dictionary<int, double>();
				dictionary2.Add(10, 1.2);
				dictionary2.Add(11, 1.1);
				dictionary2.Add(9, 1.0);
				dictionary2.Add(8, 0.8);
				dictionary = dictionary2;
				num2 = 0.6;
				break;
			case enumScoutingKind.E:
				dictionary2 = new Dictionary<int, double>();
				dictionary2.Add(10, 4.8);
				dictionary2.Add(11, 4.4);
				dictionary2.Add(9, 4.0);
				dictionary2.Add(8, 3.2);
				dictionary = dictionary2;
				num2 = 2.4;
				break;
			case enumScoutingKind.E2:
			case enumScoutingKind.K1:
			case enumScoutingKind.K2:
				dictionary2 = new Dictionary<int, double>();
				dictionary2.Add(10, 3.5999999999999996);
				dictionary2.Add(11, 3.3000000000000003);
				dictionary2.Add(9, 3.0);
				dictionary2.Add(8, 2.4000000000000004);
				dictionary = dictionary2;
				num2 = 1.7999999999999998;
				break;
			}
			List<int> list = new List<int>();
			list.Add(9);
			list.Add(10);
			list.Add(11);
			list.Add(12);
			list.Add(13);
			list.Add(26);
			list.Add(41);
			List<int> list2 = list;
			dictionary2 = new Dictionary<int, double>();
			dictionary2.Add(13, 1.4);
			dictionary2.Add(12, 1.25);
			dictionary2.Add(9, 1.2);
			dictionary2.Add(10, 1.2);
			dictionary2.Add(41, 1.2);
			dictionary2.Add(11, 1.15);
			dictionary2.Add(26, 1.0);
			Dictionary<int, double> dictionary3 = dictionary2;
			double value = 0.0;
			double value2 = 0.0;
			for (int i = 0; i < mst_slotitems[ship.Rid].Count(); i++)
			{
				Mst_slotitem mst_slotitem = mst_slotitems[ship.Rid][i];
				int api_mapbattle_type = mst_slotitem.Api_mapbattle_type3;
				if (!dictionary.TryGetValue(api_mapbattle_type, out value))
				{
					value = num2;
				}
				double num3 = 0.0;
				if (list2.Contains(api_mapbattle_type))
				{
					if (!dictionary3.TryGetValue(api_mapbattle_type, out value2))
					{
						value2 = 1.0;
					}
					num3 = Math.Sqrt(slotitem_level[ship.Slot[i]]) * value2;
				}
				num += ((double)mst_slotitem.Saku + num3) * value;
			}
			if (scouting_kind == enumScoutingKind.K2)
			{
				Dictionary<DifficultKind, int> dictionary4 = new Dictionary<DifficultKind, int>();
				dictionary4.Add(DifficultKind.TEI, 1);
				dictionary4.Add(DifficultKind.HEI, 3);
				dictionary4.Add(DifficultKind.OTU, 6);
				dictionary4.Add(DifficultKind.KOU, 12);
				dictionary4.Add(DifficultKind.SHI, 18);
				Dictionary<DifficultKind, int> dictionary5 = dictionary4;
				num -= (double)(dictionary5[user_difficult] * 2);
			}
			return num;
		}

		private double getShipSakuParam(int ship_id, double slot_saku)
		{
			return Math.Sqrt(Mst_DataManager.Instance.Mst_ship[ship_id].Saku) - 2.0 + slot_saku;
		}

		private int getMapSakuParam(double saku_total, enumScoutingKind scouting_kind)
		{
			return (int)saku_total;
		}

		public void setCommentData(enumProductionKind type, ref MapCommentKind comment, ref MapProductionKind production)
		{
			switch (type)
			{
			case enumProductionKind.A:
				comment = MapCommentKind.Enemy;
				production = MapProductionKind.None;
				break;
			case enumProductionKind.B:
				comment = MapCommentKind.Enemy;
				production = MapProductionKind.WaterPlane;
				break;
			case enumProductionKind.C1:
				comment = MapCommentKind.Atack;
				production = MapProductionKind.WaterPlane;
				break;
			case enumProductionKind.C2:
				comment = MapCommentKind.CoursePatrol;
				production = MapProductionKind.WaterPlane;
				break;
			}
		}

		private int getDrumCount(List<Mst_slotitem> slotitems)
		{
			return slotitems.Count((Mst_slotitem x) => x.Id == 75);
		}
	}
}
