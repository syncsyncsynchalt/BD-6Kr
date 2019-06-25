using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Controllers.BattleLogic;
using Server_Controllers.QuestLogic;
using Server_Models;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_PracticeBattle : BattleControllerBase
	{
		private int deckRid;

		private int deckRidEnemy;

		public Api_req_PracticeBattle(int deck_rid, int deck_ridEnemy)
		{
			deckRid = deck_rid;
			deckRidEnemy = deck_ridEnemy;
			init();
		}

		protected override void init()
		{
			practiceFlag = true;
			Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck[deckRid];
			mem_deck.ActionEnd();
			userData = getUserData(mem_deck);
			enemyData = new BattleBaseData(Comm_UserDatas.Instance.User_deck[deckRidEnemy]);
			setBattleSubInfo(userData, out userSubInfo);
			setBattleSubInfo(enemyData, out enemySubInfo);
			battleKinds = ExecBattleKinds.None;
			battleCommandParams = new BattleCommandParams(userData);
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override Api_Result<AllBattleFmt> GetDayPreBattleInfo(BattleFormationKinds1 formationKind)
		{
			Api_Result<AllBattleFmt> dayPreBattleInfo = base.GetDayPreBattleInfo(formationKind);
			if (dayPreBattleInfo.state != Api_Result_State.Parameter_Error)
			{
				dayPreBattleInfo.data.DayBattle.Header.E_DeckShip1.Deck_Id = deckRidEnemy;
			}
			return dayPreBattleInfo;
		}

		public override Api_Result<AllBattleFmt> DayBattle()
		{
			return base.DayBattle();
		}

		public override Api_Result<AllBattleFmt> NightBattle()
		{
			Api_Result<AllBattleFmt> api_Result = base.NightBattle();
			if (api_Result.state != Api_Result_State.Parameter_Error)
			{
				api_Result.data.NightBattle.Header.E_DeckShip1.Deck_Id = deckRidEnemy;
			}
			return api_Result;
		}

		public override Api_Result<BattleResultFmt> BattleResult()
		{
			Api_Result<BattleResultFmt> api_Result = base.BattleResult();
			int count = userData.StartHp.Count;
			for (int i = 0; i < count; i++)
			{
				userData.ShipData[i].SetHp(this, userData.StartHp[i]);
			}
			if (api_Result.state == Api_Result_State.Parameter_Error)
			{
				return api_Result;
			}
			Dictionary<int, int> slotExpBattleData = GetSlotExpBattleData();
			foreach (KeyValuePair<int, int> item in slotExpBattleData)
			{
				Mem_slotitem value = null;
				if (Comm_UserDatas.Instance.User_slot.TryGetValue(item.Key, out value))
				{
					value.ChangeExperience(item.Value);
				}
			}
			QuestPractice questPractice = new QuestPractice(api_Result.data);
			questPractice.ExecuteCheck();
			return api_Result;
		}

		public override void GetBattleResultBase(BattleResultBase out_data)
		{
			base.GetBattleResultBase(out_data);
		}

		private BattleBaseData getUserData(Mem_deck deck)
		{
			List<Mem_ship> memShip = deck.Ship.getMemShip();
			List<List<Mst_slotitem>> list = new List<List<Mst_slotitem>>();
			List<int> list2 = new List<int>();
			foreach (Mem_ship item in memShip)
			{
				list2.Add(item.Stype);
				IEnumerable<Mst_slotitem> source = from mem_id in item.Slot
					where Comm_UserDatas.Instance.User_slot.ContainsKey(mem_id)
					select Mst_DataManager.Instance.Mst_Slotitem[Comm_UserDatas.Instance.User_slot[mem_id].Slotitem_id];
				list.Add(source.ToList());
			}
			return new BattleBaseData(deck, memShip, list2, list);
		}
	}
}
