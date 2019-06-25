using Common.Enum;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace logic.debug.utils
{
	public class SearchValueUtil
	{
		private static Dictionary<MapBranchResult.enumScoutingKind, Dictionary<int, double>> _TYPE3_COEFFICIENT;

		private static Dictionary<MapBranchResult.enumScoutingKind, double> _OTHER_COEFFICIENT;

		private static Dictionary<int, double> _LEVEL_COEFFICIENT;

		private static Dictionary<DifficultKind, double> _DIFFICULTY_COEFFICIENT;

		public static double Get_K2(DeckModel deck, DifficultKind difficulty)
		{
			return GetSearchValue(deck, MapBranchResult.enumScoutingKind.K2, difficulty);
		}

		public static int Get_K2J(int user_level, double K2)
		{
			return (int)((double)(int)K2 - (Math.Sqrt(user_level) * 3.0 + (double)user_level * 0.2));
		}

		public static double GetSearchValue(DeckModel deck, MapBranchResult.enumScoutingKind type, DifficultKind difficulty)
		{
			double num = 0.0;
			for (int i = 0; i < deck.GetShips().Length; i++)
			{
				ShipModel ship = deck.GetShip(i);
				double shipValue = GetShipValue(ship);
				double num2 = 0.0;
				List<SlotitemModel> slotitemList = ship.SlotitemList;
				for (int j = 0; j < slotitemList.Count; j++)
				{
					num2 += GetSlotValue(type, slotitemList[j]);
				}
				double difficultyCorrectionValue = GetDifficultyCorrectionValue(difficulty, type);
				num += shipValue + num2 + difficultyCorrectionValue;
			}
			return num;
		}

		public static double GetShipValue(ShipModel ship)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship[ship.MstId];
			return Math.Sqrt(mst_ship.Saku) - 2.0;
		}

		public static double GetSlotValue(MapBranchResult.enumScoutingKind type, SlotitemModel slot)
		{
			if (slot == null)
			{
				return 0.0;
			}
			double type3Coefficient = GetType3Coefficient(type, slot);
			double otherCoefficient = GetOtherCoefficient(type);
			double levelCoefficient = GetLevelCoefficient(slot);
			double num = Math.Sqrt(slot.Level) * levelCoefficient;
			double num2 = (!(type3Coefficient > 0.0)) ? otherCoefficient : type3Coefficient;
			return ((double)slot.Sakuteki + num) * num2;
		}

		public static double GetDifficultyCorrectionValue(DifficultKind difficulty, MapBranchResult.enumScoutingKind type)
		{
			if (type == MapBranchResult.enumScoutingKind.K2)
			{
				double difficultyCoefficient = GetDifficultyCoefficient(difficulty);
				return 0.0 - difficultyCoefficient * 2.0;
			}
			return 0.0;
		}

		public static double GetType3Coefficient(MapBranchResult.enumScoutingKind type, SlotitemModel slot)
		{
			if (_TYPE3_COEFFICIENT == null)
			{
				_CreateType3Coefficient();
			}
			if (!_TYPE3_COEFFICIENT.TryGetValue(type, out Dictionary<int, double> value))
			{
				return 0.0;
			}
			value.TryGetValue(slot.Type3, out double value2);
			return value2;
		}

		public static double GetOtherCoefficient(MapBranchResult.enumScoutingKind type)
		{
			if (_OTHER_COEFFICIENT == null)
			{
				_CreateOtherCoefficient();
			}
			return _OTHER_COEFFICIENT[type];
		}

		public static double GetLevelCoefficient(SlotitemModel slot)
		{
			if (_LEVEL_COEFFICIENT == null)
			{
				_CreateLevelCoefficient();
			}
			_LEVEL_COEFFICIENT.TryGetValue(slot.Type3, out double value);
			return value;
		}

		public static double GetDifficultyCoefficient(DifficultKind difficulty)
		{
			if (_DIFFICULTY_COEFFICIENT == null)
			{
				_CreateDifficultyCoefficient();
			}
			return _DIFFICULTY_COEFFICIENT[difficulty];
		}

		public static void _CreateType3Coefficient()
		{
			_TYPE3_COEFFICIENT = new Dictionary<MapBranchResult.enumScoutingKind, Dictionary<int, double>>();
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			dictionary.Add(10, 1.2);
			dictionary.Add(11, 1.1);
			dictionary.Add(9, 1.0);
			dictionary.Add(8, 0.8);
			Dictionary<int, double> value = dictionary;
			_TYPE3_COEFFICIENT[MapBranchResult.enumScoutingKind.C] = value;
			_TYPE3_COEFFICIENT[MapBranchResult.enumScoutingKind.D] = value;
			dictionary = new Dictionary<int, double>();
			dictionary.Add(10, 4.8);
			dictionary.Add(11, 4.4);
			dictionary.Add(9, 4.0);
			dictionary.Add(8, 3.2);
			value = dictionary;
			_TYPE3_COEFFICIENT[MapBranchResult.enumScoutingKind.E] = value;
			dictionary = new Dictionary<int, double>();
			dictionary.Add(10, 3.5999999999999996);
			dictionary.Add(11, 3.3000000000000003);
			dictionary.Add(9, 3.0);
			dictionary.Add(8, 2.4000000000000004);
			value = dictionary;
			_TYPE3_COEFFICIENT[MapBranchResult.enumScoutingKind.E2] = value;
			_TYPE3_COEFFICIENT[MapBranchResult.enumScoutingKind.K1] = value;
			_TYPE3_COEFFICIENT[MapBranchResult.enumScoutingKind.K2] = value;
		}

		public static void _CreateOtherCoefficient()
		{
			Dictionary<MapBranchResult.enumScoutingKind, double> dictionary = new Dictionary<MapBranchResult.enumScoutingKind, double>();
			dictionary.Add(MapBranchResult.enumScoutingKind.C, 0.6);
			dictionary.Add(MapBranchResult.enumScoutingKind.D, 0.6);
			dictionary.Add(MapBranchResult.enumScoutingKind.E, 2.4);
			dictionary.Add(MapBranchResult.enumScoutingKind.E2, 1.7999999999999998);
			dictionary.Add(MapBranchResult.enumScoutingKind.K1, 1.7999999999999998);
			dictionary.Add(MapBranchResult.enumScoutingKind.K2, 1.7999999999999998);
			_OTHER_COEFFICIENT = dictionary;
		}

		public static void _CreateLevelCoefficient()
		{
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			dictionary.Add(9, 1.2);
			dictionary.Add(10, 1.2);
			dictionary.Add(11, 1.15);
			dictionary.Add(12, 1.25);
			dictionary.Add(13, 1.4);
			dictionary.Add(26, 1.0);
			dictionary.Add(41, 1.2);
			_LEVEL_COEFFICIENT = dictionary;
		}

		public static void _CreateDifficultyCoefficient()
		{
			Dictionary<DifficultKind, double> dictionary = new Dictionary<DifficultKind, double>();
			dictionary.Add(DifficultKind.TEI, 2.0);
			dictionary.Add(DifficultKind.HEI, 4.0);
			dictionary.Add(DifficultKind.OTU, 7.0);
			dictionary.Add(DifficultKind.KOU, 10.0);
			dictionary.Add(DifficultKind.SHI, 15.0);
			_DIFFICULTY_COEFFICIENT = dictionary;
		}
	}
}
