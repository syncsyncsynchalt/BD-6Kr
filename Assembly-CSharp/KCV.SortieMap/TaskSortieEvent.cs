using Common.Enum;
using KCV.SortieBattle;
using Librarys.State;
using local.managers;
using System;

namespace KCV.SortieMap
{
	public class TaskSortieEvent : Task
	{
		private Events _clsEvents;

		private Action<ShipRecoveryType> _actOnGoNext;

		public TaskSortieEvent(Action<ShipRecoveryType> onGoNext)
		{
			_actOnGoNext = onGoNext;
		}

		protected override void Dispose(bool isDisposing)
		{
			Mem.DelIDisposableSafe(ref _clsEvents);
			Mem.Del(ref _actOnGoNext);
		}

		protected override bool Init()
		{
			_clsEvents = new Events();
			SortieMapTaskManager.GetUIAreaMapFrame().ClearMessage();
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			_clsEvents.Play(mapManager.NextCategory, mapManager.NextEventType, OnEventFinished);
			return true;
		}

		protected override bool UnInit()
		{
			Mem.DelIDisposableSafe(ref _clsEvents);
			return true;
		}

		protected override bool Update()
		{
			if (SortieMapTaskManager.GetMode() != SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF)
			{
				return (SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.Event) ? true : false;
			}
			return true;
		}

		private void OnEventFinished(bool isBattle)
		{
			if (isBattle)
			{
				SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.Formation);
				return;
			}
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (!mapManager.IsNextFinal())
			{
				Dlg.Call(ref _actOnGoNext, ShipRecoveryType.None);
			}
			else
			{
				SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.Result);
			}
		}
	}
}
