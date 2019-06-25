using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class BattleBaseData : IDisposable
	{
		public int Enemy_id;

		public BattleFormationKinds1 Formation;

		public BattleFormationKinds2 BattleFormation;

		public string Enemy_Name;

		public Mem_deck Deck;

		public List<Mem_ship> ShipData;

		public List<List<Mst_slotitem>> SlotData;

		public List<List<int>> SlotLevel;

		public List<bool> LostFlag;

		public Dictionary<int, int[]> SlotExperience;

		public readonly List<int> StartHp;

		private readonly bool haveBattleCommand;

		public BattleBaseData(Mem_deck deck, List<Mem_ship> ships, List<int> stypes, List<List<Mst_slotitem>> slotitems)
		{
			Deck = deck;
			ShipData = ships;
			SlotData = slotitems;
			LostFlag = new List<bool>();
			StartHp = new List<int>();
			SlotLevel = new List<List<int>>();
			SlotExperience = new Dictionary<int, int[]>();
			ships.ForEach(delegate(Mem_ship x)
			{
				bool item = (x.Get_DamageState() == DamageState.Taiha) ? true : false;
				LostFlag.Add(item);
				StartHp.Add(x.Nowhp);
				List<int> outlevel = null;
				setSlotLevel(x.Slot, areaEnemy: false, out outlevel, ref SlotExperience);
				SlotLevel.Add(outlevel);
			});
			LostFlag[0] = false;
			haveBattleCommand = true;
		}

		public BattleBaseData(int enemy_id)
		{
			Mst_mapenemy2 mst_mapenemy = Mst_DataManager.Instance.Mst_mapenemy[enemy_id];
			Enemy_id = enemy_id;
			Formation = (BattleFormationKinds1)mst_mapenemy.Formation_id;
			mst_mapenemy.GetEnemyShips(out ShipData, out SlotData);
			StartHp = new List<int>();
			Enemy_Name = mst_mapenemy.Deck_name;
			SlotLevel = new List<List<int>>();
			SlotExperience = new Dictionary<int, int[]>();
			ShipData.ForEach(delegate(Mem_ship x)
			{
				StartHp.Add(x.Nowhp);
				List<int> outlevel = null;
				setSlotLevel(x.Slot, areaEnemy: true, out outlevel, ref SlotExperience);
				SlotLevel.Add(outlevel);
			});
			haveBattleCommand = false;
		}

		public BattleBaseData(Mem_deck deck)
		{
			List<Mem_ship> memShip = deck.Ship.getMemShip();
			ShipData = new List<Mem_ship>();
			SlotData = new List<List<Mst_slotitem>>();
			StartHp = new List<int>();
			SlotLevel = new List<List<int>>();
			SlotExperience = new Dictionary<int, int[]>();
			memShip.ForEach(delegate(Mem_ship x)
			{
				SlotData.Add(x.GetMstSlotItems());
				List<int> outlevel = null;
				setSlotLevel(x.Slot, areaEnemy: false, out outlevel, ref SlotExperience);
				SlotLevel.Add(outlevel);
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship[x.Ship_id];
				Mem_shipBase baseData = new Mem_shipBase(x);
				Mem_ship mem_ship = new Mem_ship();
				mem_ship.Set_ShipParamPracticeShip(baseData, mst_data);
				ShipData.Add(mem_ship);
				StartHp.Add(mem_ship.Nowhp);
			});
			Enemy_Name = deck.Name;
			haveBattleCommand = false;
		}

		public List<BattleCommand> GetDeckBattleCommand()
		{
			if (!haveBattleCommand)
			{
				return null;
			}
			List<BattleCommand> battleCommad = ShipData[0].GetBattleCommad();
			int battleCommandEnableNum = ShipData[0].GetBattleCommandEnableNum();
			return battleCommad.Take(battleCommandEnableNum).ToList();
		}

		public void Dispose()
		{
			ShipData.Clear();
			SlotData.Clear();
			SlotLevel.Clear();
			LostFlag.Clear();
			StartHp.Clear();
			SlotExperience.Clear();
		}

		private void setSlotLevel(List<int> slot_rids, bool areaEnemy, out List<int> outlevel, ref Dictionary<int, int[]> outExp)
		{
			if (areaEnemy)
			{
				outlevel = Enumerable.Repeat(0, slot_rids.Count).ToList();
				return;
			}
			List<int> list = new List<int>();
			foreach (int slot_rid in slot_rids)
			{
				Mem_slotitem value = null;
				if (Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, out value))
				{
					list.Add(value.Level);
					int[] value2 = new int[2]
					{
						value.Experience,
						0
					};
					outExp.Add(value.Rid, value2);
				}
			}
			outlevel = list;
		}
	}
}
