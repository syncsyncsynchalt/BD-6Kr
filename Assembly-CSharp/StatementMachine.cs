using System.Collections.Generic;

public class StatementMachine
{
	private class Statement
	{
		private bool _isInitialize;

		private bool _isFinished;

		private float _fStartDuration;

		private string _strGroupName;

		private object _objData;

		private StatementMachineInitialize _delInitialize;

		private StatementMachineUpdate _delUpdate;

		private StatementMachineTerminate _delTerminate;

		public bool isInitialize
		{
			get
			{
				return _isInitialize;
			}
			set
			{
				_isInitialize = value;
			}
		}

		public bool isFinished
		{
			get
			{
				return _isFinished;
			}
			set
			{
				_isFinished = value;
			}
		}

		public float startDuration
		{
			get
			{
				return _fStartDuration;
			}
			set
			{
				_fStartDuration = value;
			}
		}

		public string groupName
		{
			get
			{
				return _strGroupName;
			}
			set
			{
				_strGroupName = value;
			}
		}

		public object data
		{
			get
			{
				return _objData;
			}
			set
			{
				_objData = value;
			}
		}

		public StatementMachineInitialize initializeDelegate
		{
			get
			{
				return _delInitialize;
			}
			set
			{
				_delInitialize = value;
			}
		}

		public StatementMachineUpdate updateDelegate
		{
			get
			{
				return _delUpdate;
			}
			set
			{
				_delUpdate = value;
			}
		}

		public StatementMachineTerminate terminateDelegate
		{
			get
			{
				return _delTerminate;
			}
			set
			{
				_delTerminate = value;
			}
		}

		internal void Release()
		{
			_delInitialize = null;
			_delUpdate = null;
			_delTerminate = null;
			_objData = null;
			_strGroupName = null;
		}
	}

	public delegate bool StatementMachineInitialize(object obj);

	public delegate bool StatementMachineUpdate(object obj);

	public delegate bool StatementMachineTerminate(object obj);

	private Stack<Statement> _stackState;

	private List<Statement> _listStateNew;

	public StatementMachine()
	{
		_stackState = new Stack<Statement>(16);
		_listStateNew = new List<Statement>(4);
	}

	public void Clear()
	{
		_stackState.Clear();
		_listStateNew.Clear();
	}

	public void AddState(StatementMachineInitialize init, StatementMachineUpdate update, StatementMachineTerminate terminate, string groupName, object data, float startDuration)
	{
		Statement statement = new Statement();
		statement.isInitialize = false;
		statement.isFinished = false;
		statement.groupName = groupName;
		statement.startDuration = startDuration;
		statement.data = data;
		statement.initializeDelegate = init;
		statement.updateDelegate = update;
		statement.terminateDelegate = terminate;
		_listStateNew.Add(statement);
	}

	public void AddState(StatementMachineInitialize init, StatementMachineUpdate update, StatementMachineTerminate terminate)
	{
		Statement statement = new Statement();
		statement.isInitialize = false;
		statement.isFinished = false;
		statement.groupName = null;
		statement.startDuration = 0f;
		statement.data = null;
		statement.initializeDelegate = init;
		statement.updateDelegate = update;
		statement.terminateDelegate = terminate;
		_listStateNew.Add(statement);
	}

	public void AddState(StatementMachineInitialize init, StatementMachineUpdate update)
	{
		Statement statement = new Statement();
		statement.isInitialize = false;
		statement.isFinished = false;
		statement.groupName = null;
		statement.startDuration = 0f;
		statement.data = null;
		statement.initializeDelegate = init;
		statement.updateDelegate = update;
		statement.terminateDelegate = null;
		_listStateNew.Add(statement);
	}

	public void AddState(StatementMachineInitialize init, StatementMachineUpdate update, object data)
	{
		Statement statement = new Statement();
		statement.isInitialize = false;
		statement.isFinished = false;
		statement.groupName = null;
		statement.startDuration = 0f;
		statement.data = data;
		statement.initializeDelegate = init;
		statement.updateDelegate = update;
		statement.terminateDelegate = null;
		_listStateNew.Add(statement);
	}

	public void OnUpdate(float deltaTime)
	{
		if (_stackState.Count > 0)
		{
			Statement statement = _stackState.Peek();
			if (!statement.isInitialize)
			{
				statement.startDuration -= deltaTime;
				if (statement.startDuration > 0f)
				{
					return;
				}
				if (statement.initializeDelegate != null)
				{
					statement.isFinished = statement.initializeDelegate(statement.data);
				}
				statement.isInitialize = true;
			}
			if (statement.updateDelegate != null && !statement.isFinished)
			{
				statement.isFinished = statement.updateDelegate(statement.data);
			}
			else
			{
				statement.isFinished = true;
			}
			GarbageFinished();
		}
		if (_listStateNew.Count > 0)
		{
			Statement[] array = _listStateNew.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				_stackState.Push(array[i]);
			}
			_listStateNew.Clear();
		}
	}

	private void GarbageFinished()
	{
		while (_stackState.Count > 0)
		{
			Statement statement = _stackState.Peek();
			if (!statement.isFinished)
			{
				break;
			}
			if (statement.terminateDelegate != null)
			{
				statement.terminateDelegate(statement.data);
			}
			statement.Release();
			_stackState.Pop();
		}
	}

	public void DeleteGroup(string groupName)
	{
		Statement[] array = _stackState.ToArray();
		for (int i = 0; i < array.Length && !(array[i].groupName != groupName); i++)
		{
			array[i].isFinished = true;
		}
	}
}
