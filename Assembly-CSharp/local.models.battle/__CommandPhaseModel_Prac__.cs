using Common.Enum;
using local.managers;
using Server_Controllers;
using System.Collections.Generic;

namespace local.models.battle
{
	public class __CommandPhaseModel_Prac__ : CommandPhaseModel
	{
		private Api_req_PracticeBattle _req;

		public override int Num
		{
			get
			{
				List<BattleCommand> command;
				return _req.GetBattleCommand(out command);
			}
		}

		public __CommandPhaseModel_Prac__(BattleManager manager, Api_req_PracticeBattle req)
			: base(manager)
		{
			_req = req;
		}

		public override List<BattleCommand> GetPresetCommand()
		{
			List<BattleCommand> command;
			int battleCommand = _req.GetBattleCommand(out command);
			return command.GetRange(0, battleCommand);
		}

		public override HashSet<BattleCommand> GetSelectableCommands()
		{
			return _req.GetEnableBattleCommand();
		}

		public override bool IsValidCommand(List<BattleCommand> command)
		{
			if (_take_command)
			{
				return false;
			}
			if (command == null)
			{
				return true;
			}
			return _req.ValidBattleCommand(command);
		}

		public override bool SetCommand(List<BattleCommand> command)
		{
			if (_take_command)
			{
				return false;
			}
			if (command != null)
			{
				if (!_req.ValidBattleCommand(command))
				{
					return false;
				}
				_req.SetBattleCommand(command);
			}
			_take_command = true;
			if (_manager.WarType == enumMapWarType.Normal)
			{
				_req.DayBattle();
			}
			else if (_manager.WarType != enumMapWarType.AirBattle)
			{
			}
			_manager.__createCacheDataAfterCommand__();
			return true;
		}
	}
}
