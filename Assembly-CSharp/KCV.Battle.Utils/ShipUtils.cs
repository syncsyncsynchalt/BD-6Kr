using Common.Enum;
using local.managers;
using local.models;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Utils
{
	public class ShipUtils
	{
		public static bool UnInit()
		{
			return true;
		}

		public static Texture2D LoadTexture(int graphicsMstId, bool isDamaged)
		{
			return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(graphicsMstId, (!isDamaged) ? 9 : 10);
		}

		public static Texture2D LoadTexture(ShipModel_BattleResult model)
		{
			return LoadTexture(model.GetGraphicsMstId(), model.IsDamaged());
		}

		public static Texture2D LoadTexture(ShipModel_BattleAll model, bool isStart)
		{
			return LoadTexture(model.GetGraphicsMstId(), (!isStart) ? model.DamagedFlgEnd : model.DamagedFlgStart);
		}

		public static Texture2D LoadTexture(ShipModel_Attacker model)
		{
			return LoadTexture(model.GetGraphicsMstId(), model.IsFriend() && model.DamagedFlg);
		}

		public static Texture2D LoadTexture(ShipModel_Defender model, bool isAfter)
		{
			return LoadTexture(model.GetGraphicsMstId(), (!isAfter) ? model.DamagedFlgBefore : model.DamagedFlgAfter);
		}

		public static Texture2D LoadTexture(ShipModel_Eater model)
		{
			return LoadTexture(model.GetGraphicsMstId(), model.IsFriend() && model.DamagedFlg);
		}

		public static List<Texture2D> LoadTexture2Sinking(ShipModel_Defender model, bool isRepair)
		{
			List<Texture2D> list = new List<Texture2D>();
			list.Add(LoadTexture(model.GetGraphicsMstId(), model.DamagedFlgBefore));
			list.Add(LoadTexture(model.GetGraphicsMstId(), (!isRepair) ? model.DamagedFlgAfterRecovery : model.DamagedFlgAfterRecovery));
			return list;
		}

		public static Texture2D LoadTexture2Restore(ShipModel_Defender defender)
		{
			return LoadTexture(defender.GetGraphicsMstId(), defender.DamagedFlgAfterRecovery);
		}

		public static Texture2D LoadBannerTextureInTacticalSituation(ShipModel_BattleAll model)
		{
			int texNum = (!model.IsFriend()) ? ((!model.IsPractice()) ? 1 : ((!model.DamagedFlgEnd) ? 1 : 2)) : ((!model.DamagedFlgEnd) ? 1 : 2);
			return SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(SPConvertShipBannerID(model), texNum);
		}

		public static Texture2D LoadBannerTextureInVeteransReport(ShipModel_BattleResult model)
		{
			return LoadBannerTextureInTacticalSituation(model);
		}

		private static int SPConvertShipBannerID(ShipModel_BattleAll model)
		{
			if (model.MstId == 610)
			{
				return 609;
			}
			if (model.MstId == 612)
			{
				return 611;
			}
			return model.MstId;
		}

		public static Vector3 GetShipOffsPos(int graphicsMstId, bool isFriend, bool isDamaged, MstShipGraphColumn iColumn)
		{
			Vector3 zero = Vector3.zero;
			Mst_shipgraphbattle mst_shipgraphbattle = Mst_DataManager.Instance.Mst_shipgraphbattle[graphicsMstId];
			switch (iColumn)
			{
			case MstShipGraphColumn.Foot:
				if (isFriend && isDamaged)
				{
					zero.x = mst_shipgraphbattle.Foot_d_x;
					zero.y = mst_shipgraphbattle.Foot_d_y;
				}
				else
				{
					zero.x = mst_shipgraphbattle.Foot_x;
					zero.y = mst_shipgraphbattle.Foot_y;
				}
				break;
			case MstShipGraphColumn.CutIn:
				if (isFriend && isDamaged)
				{
					zero.x = mst_shipgraphbattle.Cutin_d_x;
					zero.y = mst_shipgraphbattle.Cutin_d_y;
				}
				else
				{
					zero.x = mst_shipgraphbattle.Cutin_x;
					zero.y = mst_shipgraphbattle.Cutin_y;
				}
				break;
			case MstShipGraphColumn.CutInSp1:
				if (isFriend && isDamaged)
				{
					zero.x = mst_shipgraphbattle.Cutin_sp1_d_x;
					zero.y = mst_shipgraphbattle.Cutin_sp1_d_y;
				}
				else
				{
					zero.x = mst_shipgraphbattle.Cutin_sp1_x;
					zero.y = mst_shipgraphbattle.Cutin_sp1_y;
				}
				break;
			case MstShipGraphColumn.PointOfGaze:
				if (isFriend && isDamaged)
				{
					zero.x = mst_shipgraphbattle.Pog_d_x;
					zero.y = mst_shipgraphbattle.Pog_d_y;
				}
				else
				{
					zero.x = mst_shipgraphbattle.Pog_x;
					zero.y = mst_shipgraphbattle.Pog_y;
				}
				break;
			case MstShipGraphColumn.SPPointOfGaze:
				if (isFriend && isDamaged)
				{
					zero.x = mst_shipgraphbattle.Pog_sp_d_x;
					zero.y = mst_shipgraphbattle.Pog_sp_d_y;
				}
				else
				{
					zero.x = mst_shipgraphbattle.Pog_sp_x;
					zero.y = mst_shipgraphbattle.Pog_sp_y;
				}
				break;
			}
			zero.z = 0f;
			return zero;
		}

		[Obsolete("削除予定のメソッドです", false)]
		public static Vector3 GetShipOffsPos(ShipModel_Attacker model, MstShipGraphColumn iColumn)
		{
			if (model == null)
			{
				return Vector3.zero;
			}
			return GetShipOffsPos(model.GetGraphicsMstId(), model.IsFriend(), model.DamagedFlg, iColumn);
		}

		[Obsolete("削除予定のメソッドです", false)]
		public static Vector3 GetShipOffsPos(ShipModel_Defender model, DamageState_Battle damageState, MstShipGraphColumn iColumn)
		{
			if (model == null)
			{
				return Vector3.zero;
			}
			bool isDamaged = false;
			if (damageState == DamageState_Battle.Tyuuha || damageState == DamageState_Battle.Taiha || damageState == DamageState_Battle.Gekichin)
			{
				isDamaged = true;
			}
			return GetShipOffsPos(model.GetGraphicsMstId(), model.IsFriend(), isDamaged, iColumn);
		}

		public static Vector3 GetShipOffsPos(ShipModel_Battle model, bool isDamaged, MstShipGraphColumn iColumn)
		{
			bool damaged = model.IsFriend() ? isDamaged : (model.IsPractice() && isDamaged);
			switch (iColumn)
			{
			case MstShipGraphColumn.Foot:
				return Util.Poi2Vec(model.Offsets.GetFoot_InBattle(damaged));
			case MstShipGraphColumn.PointOfGaze:
				return Util.Poi2Vec(model.Offsets.GetPog_InBattle(damaged));
			case MstShipGraphColumn.SPPointOfGaze:
				return (!model.IsPractice() || model.IsFriend()) ? Util.Poi2Vec(model.Offsets.GetPogSp_InBattle(damaged)) : Util.Poi2Vec(model.Offsets.GetPogSpEnsyu_InBattle(damaged));
			case MstShipGraphColumn.CutIn:
				return Util.Poi2Vec(model.Offsets.GetCutin_InBattle(damaged));
			case MstShipGraphColumn.CutInSp1:
				return Util.Poi2Vec(model.Offsets.GetCutinSp1_InBattle(damaged));
			default:
				return Vector3.zero;
			}
		}

		public static List<Vector3> GetShipOffsPos2Sinking(ShipModel_Defender model, bool isRepair, MstShipGraphColumn iColumn)
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(GetShipOffsPos(model, model.DamagedFlgBefore, iColumn));
			list.Add(GetShipOffsPos(model, (!isRepair) ? model.DamagedFlgAfter : model.DamagedFlgAfterRecovery, iColumn));
			return list;
		}

		public static int GetShipStandingTexID(bool isFriend, bool isPractice, bool isDamaged)
		{
			if (!isFriend)
			{
				if (isPractice)
				{
					return (!isDamaged) ? 9 : 10;
				}
				return 9;
			}
			if (isDamaged)
			{
				return 10;
			}
			return 9;
		}

		public static bool IsDefenderDamaged(ShipModel_Defender model, bool isAfter)
		{
			return (!isAfter) ? model.DamagedFlgBefore : model.DamagedFlgAfter;
		}

		public static ShipModel_Battle GetDetectionPrimaryShip(List<List<SlotitemModel_Battle>> models, bool isFriend)
		{
			ShipModel_Battle result = null;
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			if (battleManager == null)
			{
				return result;
			}
			int num = 0;
			foreach (List<SlotitemModel_Battle> model in models)
			{
				if (model == null)
				{
					num++;
				}
				else
				{
					foreach (SlotitemModel_Battle item in model)
					{
						if (item != null)
						{
							result = battleManager.GetShip(num, isFriend);
							break;
						}
						num++;
					}
				}
			}
			return result;
		}

		public static bool HasRepair(ShipModel_BattleAll model)
		{
			return model.HasRecoverYouin() || model.HasRecoverMegami();
		}

		public static void PlayShipVoice(ShipModel_Battle model, int voiceNum)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null && model != null)
			{
				int index = (!model.IsFriend()) ? (model.Index + 6) : model.Index;
				SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(model.GetVoiceMstId(voiceNum), voiceNum), index);
			}
		}

		public static void PlayBattleStartVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, 15);
		}

		public static void PlayEatingVoice(ShipModel_Eater model)
		{
			PlayShipVoice(model, 26);
		}

		public static void PlayAircraftCutInVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, (!XorRandom.GetIs()) ? 17 : 15);
		}

		public static void PlaySupportingFireVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, XorRandom.GetILim(16, 18));
		}

		public static void PlayShellingVoive(ShipModel_Battle model)
		{
			int voiceNum = (model.ShipType == 7 || model.ShipType == 11 || model.ShipType == 18) ? ((!XorRandom.GetIs()) ? 18 : 16) : ((!XorRandom.GetIs()) ? 16 : 15);
			PlayShipVoice(model, voiceNum);
		}

		public static void PlayObservedShellingVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, 17);
		}

		public static void PlayTorpedoVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, (!XorRandom.GetIs()) ? 16 : 15);
		}

		public static void PlayStartNightCombatVoice(ShipModel_Battle model)
		{
			int voiceNum = (!(model.Yomi == "グラーフ・ツェッペリン")) ? 18 : 918;
			PlayShipVoice(model, voiceNum);
		}

		public static void PlayNightShellingVoice(ShipModel_Battle model)
		{
			int voiceNum = (!(model.Yomi == "グラーフ・ツェッペリン")) ? 17 : 917;
			PlayShipVoice(model, voiceNum);
		}

		public static void PlayMildCaseVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, (!XorRandom.GetIs()) ? 20 : 19);
		}

		public static void PlayDamageCutInVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, 21);
		}

		public static void PlaySinkingVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, 22);
		}

		public static void PlayMVPVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, 23);
		}

		public static void PlayFlagshipWreckVoice(ShipModel_Battle model)
		{
			PlayShipVoice(model, 20);
		}

		public static void PlayBossInsertVoice(ShipModelMst model)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(model.MstId, 1), 6);
			}
		}

		public static void PlayBossDeathCryVoice(ShipModelMst model)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(model.MstId, 22));
			}
		}

		public static float GetVoiceLength(ShipModelMst model, int voiceNum)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				return SingletonMonoBehaviour<SoundManager>.Instance.VoiceLength(SingletonMonoBehaviour<ResourceManager>.Instance.ShipVoice.Load(model.MstId, voiceNum));
			}
			return 0f;
		}

		public static void StopShipVoice(ShipModel_Battle model)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null && SingletonMonoBehaviour<ResourceManager>.Instance != null)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.StopVoice(model.Index);
			}
		}
	}
}
