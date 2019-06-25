namespace Librarys.State
{
	public class Tasks
	{
		public const int TASK_CTRL_NUM_DEFAULT = 32;

		private int _nTask;

		private Task[] _clsTasks;

		public int taskNum => _nTask;

		public Task[] tasks => _clsTasks;

		public Tasks()
		{
			_nTask = 0;
			_clsTasks = null;
		}

		public bool Init(int nTask = 32)
		{
			DebugUtils.dbgAssert(nTask > 0);
			_nTask = nTask;
			DebugUtils.dbgAssert(null == _clsTasks);
			Mem.NewAry(ref _clsTasks, nTask);
			for (int i = 0; i < _nTask; i++)
			{
				_clsTasks[i] = null;
			}
			return true;
		}

		public bool UnInit()
		{
			if (CloseAll())
			{
				Mem.DelArySafe(ref _clsTasks);
				_nTask = 0;
				return true;
			}
			return false;
		}

		public void Update()
		{
			if (_clsTasks == null)
			{
				return;
			}
			for (int i = 0; i < _nTask; i++)
			{
				if (_clsTasks[i] != null && _clsTasks[i].IsRun())
				{
					_clsTasks[i].Run();
				}
			}
		}

		public int Open(ref Task pTask, Task pTaskParent = null)
		{
			if (_clsTasks != null)
			{
				for (int i = 0; i < _nTask; i++)
				{
					if (_clsTasks[i] == null && pTask.Open(this, i, pTaskParent))
					{
						return i;
					}
				}
			}
			return -1;
		}

		public int Open(Task pTask, Task pTaskParent = null)
		{
			if (_clsTasks != null)
			{
				for (int i = 0; i < _nTask; i++)
				{
					if (_clsTasks[i] == null && pTask.Open(this, i, pTaskParent))
					{
						return i;
					}
				}
			}
			return -1;
		}

		public bool Close(int iTask)
		{
			if (_clsTasks != null && _clsTasks[iTask] != null)
			{
				return _clsTasks[iTask].Close();
			}
			return false;
		}

		public bool CloseAll()
		{
			if (_clsTasks != null)
			{
				for (int i = 0; i < _nTask; i++)
				{
					Close(i);
				}
				return true;
			}
			return false;
		}

		public Task GetTask(int iTask)
		{
			return _clsTasks[iTask];
		}

		public int GetTaskLength()
		{
			int num = 0;
			for (int i = 0; i < _nTask; i++)
			{
				if (_clsTasks[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		public int GetTaskNum()
		{
			return _nTask;
		}

		public int GetTaskNumRun()
		{
			int num = 0;
			if (_clsTasks != null)
			{
				for (int i = 0; i < _nTask; i++)
				{
					if (_clsTasks[i] != null)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int ChkRun(ref Task pTask)
		{
			if (_clsTasks != null)
			{
				for (int i = 0; i < _nTask; i++)
				{
					if (_clsTasks[i] != null && pTask == _clsTasks[i])
					{
						return i;
					}
				}
			}
			return -1;
		}
	}
}
