using UnityEngine;

public class SceneTasksMono : MonoBehaviour
{
	public const int TASK_CTRL_NUM_DEFAULT = 32;

	private int _nTaskNum;

	private SceneTaskMono[] _clsTasks;

	public int taskNum => _nTaskNum;

	public SceneTaskMono[] tasks => _clsTasks;

	private void Awake()
	{
		_nTaskNum = 0;
		_clsTasks = null;
	}

	private void OnDestroy()
	{
		UnInit();
	}

	public bool Init()
	{
		return Init(32);
	}

	public bool Init(int nTask)
	{
		DebugUtils.dbgAssert(nTask > 0);
		_nTaskNum = nTask;
		DebugUtils.dbgAssert(null == _clsTasks);
		Mem.NewAry(ref _clsTasks, nTask);
		for (int i = 0; i < _nTaskNum; i++)
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
			_nTaskNum = 0;
			return true;
		}
		return false;
	}

	public void Run()
	{
		if (_clsTasks == null)
		{
			return;
		}
		for (int i = 0; i < _nTaskNum; i++)
		{
			if (!(null == _clsTasks[i]) && _clsTasks[i].isRun)
			{
				_clsTasks[i].Running();
			}
		}
	}

	public int Open(SceneTaskMono pTask)
	{
		return Open(pTask, null);
	}

	public int Open(SceneTaskMono pTask, SceneTaskMono pTaskParent)
	{
		if (_clsTasks != null)
		{
			for (int i = 0; i < _nTaskNum; i++)
			{
				if (!(null != _clsTasks[i]) && pTask.Open(this, i, pTaskParent))
				{
					return i;
				}
			}
		}
		return -1;
	}

	public bool Close(int iTask)
	{
		if (_clsTasks != null && null != _clsTasks[iTask])
		{
			return _clsTasks[iTask].Close();
		}
		return true;
	}

	public bool CloseAll()
	{
		if (_clsTasks != null)
		{
			for (int i = 0; i < _nTaskNum; i++)
			{
				Close(i);
			}
			return true;
		}
		return true;
	}

	public SceneTaskMono GetTask(int iTask)
	{
		return _clsTasks[iTask];
	}

	public int GetTaskNumRun()
	{
		int num = 0;
		if (_clsTasks != null)
		{
			for (int i = 0; i < _nTaskNum; i++)
			{
				if (!(null == _clsTasks[i]))
				{
					num++;
				}
			}
		}
		return num;
	}

	public int ChkRun(SceneTaskMono pTask)
	{
		if (_clsTasks != null)
		{
			for (int i = 0; i < _nTaskNum; i++)
			{
				if (!(null == _clsTasks[i]) && !(pTask != _clsTasks[i]))
				{
					return i;
				}
			}
		}
		return -1;
	}
}
