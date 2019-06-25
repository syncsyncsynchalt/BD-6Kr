using UnityEngine;

public class RepairTaskManager : SceneTaskMono
{
	private StatementMachine _state;

	protected override void Awake()
	{
		_state = new StatementMachine();
		_state.AddState(Init, Update_);
	}

	private bool Init(object obj)
	{
		return true;
	}

	private bool Update_(object obj)
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			_state.AddState(Init2, Update2);
		}
		return true;
	}

	private bool Init2(object obj)
	{
		return true;
	}

	private bool Update2(object obj)
	{
		return true;
	}
}
