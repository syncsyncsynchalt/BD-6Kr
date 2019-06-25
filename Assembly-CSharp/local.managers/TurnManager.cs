using Common.Enum;
using Common.Struct;
using local.models;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class TurnManager : ManagerBase, ITurnOperator
	{
		public TurnManager()
		{
		}

		public virtual UserPreActionPhaseResultModel GetResult_UserPreActionPhase()
		{
			TurnWorkResult turnWorkResult = _PhaseEnd(force: false, new List<TurnState>
			{
				TurnState.TURN_START
			});
			if (turnWorkResult == null)
			{
				return null;
			}
			return new UserPreActionPhaseResultModel(turnWorkResult, this);
		}

		public virtual UserActionPhaseResultModel GetResult_UserActionPhase()
		{
			TurnWorkResult turnWorkResult = _PhaseEnd(force: true, new List<TurnState>
			{
				TurnState.CONTINOUS,
				TurnState.OWN_END
			});
			return (turnWorkResult == null) ? null : new UserActionPhaseResultModel(turnWorkResult);
		}

		public EnemyPreActionPhaseResultModel GetResult_EnemyPreActionPhase()
		{
			TurnWorkResult turnWorkResult = _PhaseEnd(force: false, new List<TurnState>
			{
				TurnState.ENEMY_START
			});
			return (turnWorkResult == null) ? null : new EnemyPreActionPhaseResultModel(turnWorkResult);
		}

		public EnemyActionPhaseResultModel GetResult_EnemyActionPhase()
		{
			TurnWorkResult turnWorkResult = _PhaseEnd(force: false, new List<TurnState>
			{
				TurnState.ENEMY_END
			});
			return (turnWorkResult == null) ? null : new EnemyActionPhaseResultModel(turnWorkResult);
		}

		public TurnResultModel GetResult_Turn()
		{
			TurnWorkResult turnWorkResult = _PhaseEnd(force: false, new List<TurnState>
			{
				TurnState.TURN_END
			});
			if (turnWorkResult == null)
			{
				return null;
			}
			TurnResultModel turnResultModel = new TurnResultModel(turnWorkResult);
			if (turnResultModel.RadingResult == null)
			{
				return turnResultModel;
			}
			for (int i = 0; i < turnResultModel.RadingResult.Count; i++)
			{
				RadingResultData radingResultData = turnResultModel.RadingResult[i];
				if (radingResultData.RadingDamage != null)
				{
					List<int> list = radingResultData.RadingDamage.FindAll((RadingDamageData item) => item.DamageState == DamagedStates.Gekichin).ConvertAll((RadingDamageData item) => item.Rid);
					if (list.Count > 0)
					{
						base.UserInfo.__RemoveGekichinShips__(list);
						base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
						break;
					}
				}
			}
			return turnResultModel;
		}

		public TurnWorkResult ExecTurnStateChange()
		{
			return null;
		}

		public List<PhaseResultModel> DebugTurnEnd()
		{
			List<PhaseResultModel> list = new List<PhaseResultModel>();
			return _DebugTurnEnd(list);
		}

		private List<PhaseResultModel> _DebugTurnEnd(List<PhaseResultModel> list)
		{
			PhaseResultModel phaseResultModel = null;
			if (base.TurnState == TurnState.TURN_START)
			{
				phaseResultModel = GetResult_UserPreActionPhase();
				if (phaseResultModel != null)
				{
					list.Add(phaseResultModel);
				}
				return list;
			}
			if (base.TurnState == TurnState.CONTINOUS || base.TurnState == TurnState.OWN_END)
			{
				phaseResultModel = GetResult_UserActionPhase();
			}
			else if (base.TurnState == TurnState.ENEMY_START)
			{
				phaseResultModel = GetResult_EnemyPreActionPhase();
			}
			else if (base.TurnState == TurnState.ENEMY_END)
			{
				phaseResultModel = GetResult_EnemyActionPhase();
			}
			else if (base.TurnState == TurnState.TURN_END)
			{
				phaseResultModel = GetResult_Turn();
			}
			if (phaseResultModel == null)
			{
				return list;
			}
			list.Add(phaseResultModel);
			return _DebugTurnEnd(list);
		}

		private TurnWorkResult _PhaseEnd(bool force, List<TurnState> now_states)
		{
			if (now_states.Contains(base.TurnState))
			{
				TurnWorkResult turnWorkResult = new Api_TurnOperator().ExecTurnStateChange(this, force, ManagerBase._turn_state);
				if (turnWorkResult != null)
				{
					ManagerBase._turn_state = turnWorkResult.ChangeState;
					return turnWorkResult;
				}
				return null;
			}
			return null;
		}

		public override string ToString()
		{
			string str = $"{base.UserInfo}\n{base.Material}\n";
			str += $"総タ\u30fcン数:{base.Turn}\t日時:{base.Datetime}";
			string str2 = str;
			object[] array = new object[4];
			TurnString datetimeString = base.DatetimeString;
			array[0] = datetimeString.Year;
			TurnString datetimeString2 = base.DatetimeString;
			array[1] = datetimeString2.Month;
			TurnString datetimeString3 = base.DatetimeString;
			array[2] = datetimeString3.Day;
			TurnString datetimeString4 = base.DatetimeString;
			array[3] = datetimeString4.DayOfWeek;
			str = str2 + string.Format("({0}年{1} {2}日 {3})\n", array);
			Mem_trophy user_trophy = Comm_UserDatas.Instance.User_trophy;
			str += $"累計デ\u30fcタ:[出撃-{user_trophy.Start_map_count}, S勝利-{user_trophy.Win_S_count}, 応急修理-{user_trophy.Use_recovery_item_count}, 改修工廠-{user_trophy.Revamp_count}\n";
			return str + $"{base.Settings}";
		}
	}
}
