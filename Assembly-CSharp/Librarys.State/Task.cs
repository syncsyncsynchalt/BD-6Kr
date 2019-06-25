using System;

namespace Librarys.State
{
	public class Task : IDisposable
	{
		public enum TaskMode
		{
			TaskMode_ST = 0,
			TaskMode_BEF = -1,
			TaskMode_Free = 0,
			TaskMode_Init = 1,
			TaskMode_Update = 2,
			TaskMode_UnInit = 3,
			TaskMode_AFT = 4,
			TaskMode_NUM = 4,
			TaskMode_ED = 3
		}

		private Tasks _pTasks;

		private int _iTask;

		private Task _pTaskParent;

		private TaskMode _iMode;

		public Task()
		{
			_pTasks = null;
			_iTask = -1;
			_pTaskParent = null;
			_iMode = TaskMode.TaskMode_ST;
		}

		public void Dispose()
		{
			Mem.Del(ref _pTasks);
			Mem.Del(ref _iTask);
			Mem.Del(ref _pTaskParent);
			Mem.Del(ref _iMode);
			Dispose(isDisposing: true);
		}

		public int GetId()
		{
			return _iTask;
		}

		public TaskMode GetMode()
		{
			return _iMode;
		}

		public bool IsRun()
		{
			return (_iMode != 0) ? true : false;
		}

		public bool Open(Tasks pTasks, int iTask, Task pTaskParent)
		{
			_pTasks = pTasks;
			DebugUtils.dbgAssert(_iMode == TaskMode.TaskMode_ST);
			if (pTasks != null)
			{
				pTasks.tasks[iTask] = this;
			}
			_iTask = iTask;
			_pTaskParent = pTaskParent;
			_iMode = TaskMode.TaskMode_Init;
			return true;
		}

		public bool Close()
		{
			if (_pTasks != null)
			{
				for (int i = 0; i < _pTasks.taskNum; i++)
				{
					DebugUtils.dbgAssert(null != _pTasks.tasks);
					if (_pTasks.tasks[i] != null && this == _pTasks.tasks[i]._pTaskParent && !_pTasks.tasks[i].Close())
					{
						return false;
					}
				}
			}
			_iMode = TaskMode.TaskMode_UnInit;
			if (!UnInit())
			{
				return false;
			}
			if (_pTasks != null)
			{
				DebugUtils.dbgAssert(null != _pTasks.tasks);
				DebugUtils.dbgAssert(_iTask >= 0 && _iTask < _pTasks.taskNum);
				_pTasks.tasks[_iTask] = null;
			}
			_pTasks = null;
			_iTask = -1;
			_pTaskParent = null;
			_iMode = TaskMode.TaskMode_ST;
			return true;
		}

		public bool Run()
		{
			switch (_iMode)
			{
			case TaskMode.TaskMode_Init:
				if (Init())
				{
					_iMode = TaskMode.TaskMode_Update;
				}
				break;
			case TaskMode.TaskMode_Update:
				if (!Update())
				{
					_iMode = TaskMode.TaskMode_UnInit;
				}
				break;
			case TaskMode.TaskMode_UnInit:
				if (Close())
				{
				}
				break;
			}
			return IsRun();
		}

		protected virtual void Dispose(bool isDisposing)
		{
		}

		protected virtual bool Init()
		{
			return true;
		}

		protected virtual bool UnInit()
		{
			return true;
		}

		protected virtual bool Update()
		{
			return true;
		}

		protected void ImmediateTermination()
		{
			Close();
		}
	}
}
