using System;
using UnityEngine;

public class SceneTaskMono : MonoBehaviour
{
	public enum TaskState
	{
		TaskState_ST = 0,
		TaskState_BEF = -1,
		TaskState_Free = 0,
		TaskState_Init = 1,
		TaskState_Update = 2,
		TaskState_UnInit = 3,
		TaskState_Exit = 4,
		TaskState_Awake = 5,
		TaskState_Start = 6,
		TaskState_LateUpdate = 7,
		TaskState_AFT = 8,
		TaskState_NUM = 8,
		TaskState_ED = 7
	}

	private SceneTasksMono _pTasks;

	private int _nTask;

	private SceneTaskMono _pTaskParent;

	private TaskState _iState;

	private bool _isDraw;

	private bool _isActive;

	[SerializeField]
	protected Transform _traScenePrefab;

	public bool isRun => (_iState != 0) ? true : false;

	public int id => _nTask;

	public TaskState state => _iState;

	public bool isActive => _isActive;

	public bool isDraw => _isDraw;

	public Transform scenePrefab
	{
		get
		{
			return _traScenePrefab;
		}
		set
		{
			_traScenePrefab = value;
		}
	}

	protected virtual void Awake()
	{
		_pTasks = null;
		_nTask = -1;
		_pTaskParent = null;
		_iState = TaskState.TaskState_ST;
		_isDraw = false;
		_isActive = false;
	}

	protected virtual void Start()
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

	protected virtual bool Run()
	{
		return true;
	}

	protected virtual void Active()
	{
	}

	public bool Open(SceneTasksMono pTasks, int iTask, SceneTaskMono pTaskParent)
	{
		_pTasks = pTasks;
		if (null != pTasks)
		{
			pTasks.tasks[iTask] = this;
		}
		_nTask = iTask;
		_pTaskParent = pTaskParent;
		_isDraw = false;
		_iState = TaskState.TaskState_Init;
		return true;
	}

	public bool Close()
	{
		if (null != _pTasks)
		{
			for (int i = 0; i < _pTasks.taskNum; i++)
			{
				DebugUtils.dbgAssert(null != _pTasks.tasks);
				if (!(null == _pTasks.tasks[i]) && this == _pTasks.tasks[i]._pTaskParent && !_pTasks.tasks[i].Close())
				{
					return false;
				}
			}
		}
		_iState = TaskState.TaskState_UnInit;
		if (!UnInit())
		{
			return false;
		}
		if (null != _pTasks)
		{
			DebugUtils.dbgAssert(null != _pTasks.tasks);
			DebugUtils.dbgAssert(_nTask >= 0 && _nTask < _pTasks.taskNum);
			_pTasks.tasks[_nTask] = null;
		}
		_pTasks = null;
		_nTask = -1;
		_pTaskParent = null;
		_isDraw = false;
		_iState = TaskState.TaskState_ST;
		return true;
	}

	public bool Running()
	{
		switch (_iState)
		{
		case TaskState.TaskState_Init:
			Active();
			if (Init())
			{
				_isDraw = true;
				_iState = TaskState.TaskState_Update;
			}
			break;
		case TaskState.TaskState_Update:
			if (!Run())
			{
				_iState = TaskState.TaskState_UnInit;
			}
			break;
		case TaskState.TaskState_UnInit:
			if (Close())
			{
			}
			break;
		}
		return isRun;
	}

	public void SetActive(bool isActive)
	{
		_isActive = isActive;
	}

	protected void ImmediateTermination()
	{
		Close();
	}

	protected void InitState()
	{
		_iState = TaskState.TaskState_Init;
	}

	protected virtual void CreateScene(GameObject prefab)
	{
		if (base.transform.childCount <= 0)
		{
			_traScenePrefab = Util.InstantiateGameObject(prefab, base.transform).transform;
		}
	}

	protected virtual void DiscardScene()
	{
		if (_traScenePrefab != null)
		{
			UnityEngine.Object.Destroy(_traScenePrefab.gameObject);
		}
	}

	protected virtual void Discard()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[Obsolete("GetID() is the old format. please use the id.")]
	public int GetId()
	{
		return _nTask;
	}

	[Obsolete("IsRun() is the old format. please use the isRun.")]
	public bool IsRun()
	{
		return (_iState != 0) ? true : false;
	}

	[Obsolete("GetState() is the old format. please use the state.")]
	public TaskState GetState()
	{
		return _iState;
	}

	[Obsolete("IsActive() is the old format. please use the isActive.")]
	public bool IsActive()
	{
		return _isActive;
	}

	[Obsolete("IsDraw() is the old format. please use the isDraw.")]
	protected bool IsDraw()
	{
		return _isDraw;
	}
}
