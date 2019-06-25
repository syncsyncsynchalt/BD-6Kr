using Common.Enum;
using local.managers;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Utils
{
	public class BattleUtils
	{
		public static HitState ConvertBattleHitState2HitState(DamageModelBase model)
		{
			HitState result = HitState.Miss;
			if (model == null)
			{
				return result;
			}
			switch (model.GetHitState())
			{
			case BattleHitStatus.Clitical:
				if (model.GetGurdEffect())
				{
				}
				return HitState.CriticalDamage;
			case BattleHitStatus.Normal:
				if (model.GetGurdEffect())
				{
				}
				return HitState.NomalDamage;
			default:
				return HitState.Miss;
			}
		}

		public static BattlePhase NextPhase(BattlePhase iPhase)
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			if (battleManager == null)
			{
				return BattlePhase.BattlePhase_BEF;
			}
			BattlePhase result = BattlePhase.BattlePhase_BEF;
			switch (iPhase)
			{
			case BattlePhase.BattlePhase_ST:
				result = BattlePhase.FleetAdvent;
				break;
			case BattlePhase.FleetAdvent:
				if (battleManager is SortieBattleManager || battleManager is RebellionBattleManager)
				{
					BattleManager battleManager2 = BattleTaskManager.GetBattleManager();
					if (GetStarBattleFleetAdventNextPhase(battleManager2.WarType) == BattlePhase.NightCombat)
					{
						result = BattlePhase.NightCombat;
						break;
					}
				}
				result = ((!battleManager.IsExistSakutekiData()) ? ((!battleManager.IsExistCommandPhase()) ? ((!battleManager.IsExistKoukuuData()) ? ((!battleManager.IsExistShienData()) ? ((!battleManager.IsExistKaimakuData()) ? ((!battleManager.IsExistHougekiPhase_Day()) ? ((!battleManager.IsExistRaigekiData()) ? BattlePhase.WithdrawalDecision : BattlePhase.TorpedoSalvo) : BattlePhase.Shelling) : BattlePhase.OpeningTorpedoSalvo) : BattlePhase.SupportingFire) : BattlePhase.AerialCombat) : BattlePhase.Command) : BattlePhase.Detection);
				break;
			case BattlePhase.Detection:
				result = ((!battleManager.IsExistCommandPhase()) ? ((!battleManager.IsExistKoukuuData()) ? ((!battleManager.IsExistShienData()) ? ((!battleManager.IsExistKaimakuData()) ? ((!battleManager.IsExistHougekiPhase_Day()) ? ((!battleManager.IsExistRaigekiData()) ? BattlePhase.WithdrawalDecision : BattlePhase.TorpedoSalvo) : BattlePhase.Shelling) : BattlePhase.OpeningTorpedoSalvo) : BattlePhase.SupportingFire) : BattlePhase.AerialCombat) : BattlePhase.Command);
				break;
			case BattlePhase.Command:
				result = ((!battleManager.IsExistKoukuuData()) ? ((!battleManager.IsExistShienData()) ? ((!battleManager.IsExistKaimakuData()) ? ((!battleManager.IsExistHougekiPhase_Day()) ? ((!battleManager.IsExistRaigekiData()) ? BattlePhase.WithdrawalDecision : BattlePhase.TorpedoSalvo) : BattlePhase.Shelling) : BattlePhase.OpeningTorpedoSalvo) : BattlePhase.SupportingFire) : BattlePhase.AerialCombat);
				break;
			case BattlePhase.AerialCombat:
				result = ((!battleManager.IsExistShienData()) ? ((!battleManager.IsExistKaimakuData()) ? ((!battleManager.IsExistHougekiPhase_Day()) ? ((!battleManager.IsExistRaigekiData()) ? BattlePhase.WithdrawalDecision : BattlePhase.TorpedoSalvo) : BattlePhase.Shelling) : BattlePhase.OpeningTorpedoSalvo) : BattlePhase.SupportingFire);
				break;
			case BattlePhase.AerialCombatSecond:
				result = ((!battleManager.IsExistShienData()) ? ((!battleManager.IsExistKaimakuData()) ? ((!battleManager.IsExistHougekiPhase_Day()) ? ((!battleManager.IsExistRaigekiData()) ? BattlePhase.WithdrawalDecision : BattlePhase.TorpedoSalvo) : BattlePhase.Shelling) : BattlePhase.OpeningTorpedoSalvo) : BattlePhase.SupportingFire);
				break;
			case BattlePhase.SupportingFire:
				result = ((!battleManager.IsExistKaimakuData()) ? ((!battleManager.IsExistHougekiPhase_Day()) ? ((!battleManager.IsExistRaigekiData()) ? BattlePhase.WithdrawalDecision : BattlePhase.TorpedoSalvo) : BattlePhase.Shelling) : BattlePhase.OpeningTorpedoSalvo);
				break;
			case BattlePhase.OpeningTorpedoSalvo:
				result = ((!battleManager.IsExistHougekiPhase_Day()) ? ((!battleManager.IsExistRaigekiData()) ? BattlePhase.WithdrawalDecision : BattlePhase.TorpedoSalvo) : BattlePhase.Shelling);
				break;
			case BattlePhase.Shelling:
				result = ((!battleManager.IsExistRaigekiData()) ? BattlePhase.WithdrawalDecision : BattlePhase.TorpedoSalvo);
				break;
			case BattlePhase.TorpedoSalvo:
				result = BattlePhase.WithdrawalDecision;
				break;
			case BattlePhase.WithdrawalDecision:
				result = ((!battleManager.HasNightBattle()) ? BattlePhase.Result : BattlePhase.NightCombat);
				break;
			case BattlePhase.NightCombat:
				result = BattlePhase.Result;
				break;
			case BattlePhase.Result:
				result = BattlePhase.ClearReward;
				break;
			case BattlePhase.FlagshipWreck:
				result = BattlePhase.MapOpen;
				break;
			case BattlePhase.EscortShipEvacuation:
				result = BattlePhase.AdvancingWithdrawal;
				break;
			case BattlePhase.ClearReward:
				if (battleManager.GetEscapeCandidate() != null)
				{
				}
				result = BattlePhase.MapOpen;
				break;
			}
			return result;
		}

		public static List<BattlePhase> GetBattlePhaseList()
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			if (battleManager == null)
			{
				return null;
			}
			List<BattlePhase> list = new List<BattlePhase>();
			list.Add(BattlePhase.BattlePhase_ST);
			list.Add(BattlePhase.FleetAdvent);
			list.Add(BattlePhase.Detection);
			if (battleManager.IsExistKoukuuData())
			{
				list.Add(BattlePhase.AerialCombat);
			}
			if (battleManager.IsExistShienData())
			{
				list.Add(BattlePhase.SupportingFire);
			}
			if (battleManager.IsExistKaimakuData())
			{
				list.Add(BattlePhase.OpeningTorpedoSalvo);
			}
			if (battleManager.IsExistHougekiPhase_Day())
			{
				list.Add(BattlePhase.Shelling);
			}
			if (battleManager.IsExistRaigekiData())
			{
				list.Add(BattlePhase.TorpedoSalvo);
			}
			if (battleManager.HasNightBattle())
			{
				list.Add(BattlePhase.NightCombat);
			}
			return list;
		}

		public static BattlePhase GetStarBattleFleetAdventNextPhase(enumMapWarType iType)
		{
			BattlePhase result = BattlePhase.BattlePhase_BEF;
			switch (iType)
			{
			case enumMapWarType.Normal:
				result = BattlePhase.Detection;
				break;
			case enumMapWarType.Midnight:
			case enumMapWarType.Night_To_Day:
				result = BattlePhase.NightCombat;
				break;
			}
			return result;
		}

		public static bool IsPlayMVPVoice(BattleWinRankKinds iKind)
		{
			switch (iKind)
			{
			case BattleWinRankKinds.B:
			case BattleWinRankKinds.A:
			case BattleWinRankKinds.S:
				return true;
			default:
				return false;
			}
		}

		public static Hashtable GetRetentionDataAdvancingWithdrawal(MapManager manager, ShipRecoveryType iRecovery)
		{
			Hashtable hashtable = new Hashtable();
			if (manager is SortieMapManager)
			{
				SortieMapManager value = manager as SortieMapManager;
				hashtable.Add("sortieMapManager", value);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			else if (manager is RebellionMapManager)
			{
				RebellionMapManager value2 = manager as RebellionMapManager;
				hashtable.Add("rebellionMapManager", value2);
				hashtable.Add("rootType", 2);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			return hashtable;
		}

		public static Hashtable GetRetentionDataAdvancingWithdrawalDC(MapManager manager, ShipRecoveryType iRecovery)
		{
			Hashtable hashtable = new Hashtable();
			if (manager is SortieMapManager)
			{
				SortieMapManager value = manager as SortieMapManager;
				hashtable.Add("sortieMapManager", value);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			else if (manager is RebellionMapManager)
			{
				RebellionMapManager value2 = manager as RebellionMapManager;
				hashtable.Add("rebellionMapManager", value2);
				hashtable.Add("rootType", 2);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			return hashtable;
		}

		public static Hashtable GetRetentionDataFlagshipWreck(MapManager manager, ShipRecoveryType iRecovery)
		{
			Hashtable hashtable = new Hashtable();
			if (manager is SortieMapManager)
			{
				SortieMapManager value = manager as SortieMapManager;
				hashtable.Add("sortieMapManager", value);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			else if (manager is RebellionMapManager)
			{
				RebellionMapManager value2 = manager as RebellionMapManager;
				hashtable.Add("rebellionMapManager", value2);
				hashtable.Add("rootType", 2);
				hashtable.Add("shipRecoveryType", iRecovery);
			}
			return hashtable;
		}

		public static Hashtable GetRetentionDataMapOpen(MapManager manager, BattleResultModel model)
		{
			Hashtable hashtable = new Hashtable();
			if (manager is SortieMapManager)
			{
				SortieMapManager value = manager as SortieMapManager;
				hashtable.Add("sortieMapManager", value);
				hashtable.Add("newOpenAreaIDs", model.NewOpenAreaIDs);
				hashtable.Add("newOpenMapIDs", model.NewOpenMapIDs);
				hashtable.Add("rootType", 1);
			}
			else if (manager is RebellionMapManager)
			{
				RebellionMapManager value2 = manager as RebellionMapManager;
				hashtable.Add("rebellionMapManager", value2);
				hashtable.Add("newOpenAreaIDs", model.NewOpenAreaIDs);
				hashtable.Add("newOpenMapIDs", model.NewOpenMapIDs);
				hashtable.Add("rootType", 2);
			}
			return hashtable;
		}

		public static ShipRecoveryType GetShipRecoveryType(AdvancingWithdrawalDCType iType)
		{
			switch (iType)
			{
			case AdvancingWithdrawalDCType.Megami:
				return ShipRecoveryType.Goddes;
			case AdvancingWithdrawalDCType.Youin:
				return ShipRecoveryType.Personnel;
			default:
				return ShipRecoveryType.None;
			}
		}

		public static AsyncOperation GetNextLoadLevelAsync()
		{
			return Application.LoadLevelAsync(Generics.Scene.Strategy.ToString());
		}

		public static AsyncOperation GetNextLoadLevelAsync(BattlePhase iPhase)
		{
			if (iPhase == BattlePhase.FlagshipWreck)
			{
				return Application.LoadLevelAsync(Generics.Scene.Strategy.ToString());
			}
			return null;
		}

		public static Generics.Scene GetNextLoadLevelAsync(UIHexButtonEx btn)
		{
			switch (btn.index)
			{
			case 1:
				return Generics.Scene.SortieAreaMap;
			case 2:
				return Generics.Scene.SortieAreaMap;
			case 0:
				return Generics.Scene.Strategy;
			default:
				return Generics.Scene.Scene_BEF;
			}
		}

		public static AsyncOperation GetNextLoadLevelAsync(UIHexButton btn)
		{
			return (btn.index != 1) ? Application.LoadLevelAsync(Generics.Scene.Strategy.ToString()) : Application.LoadLevelAsync(Generics.Scene.SortieAreaMap.ToString());
		}

		public static IEnumerator ClearMemory()
		{
			yield return new WaitForEndOfFrame();
			AsyncOperation ap = Resources.UnloadUnusedAssets();
			while (!ap.isDone)
			{
				yield return null;
			}
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			yield return new WaitForEndOfFrame();
		}

		public static string log(ShipModel_Battle[] ships)
		{
			string text = string.Empty;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel_Battle shipModel_Battle = ships[i];
				text = ((shipModel_Battle != null) ? (text + shipModel_Battle.ToString() + "\n") : (text + $"[{i}] - \n"));
			}
			return text;
		}
	}
}
