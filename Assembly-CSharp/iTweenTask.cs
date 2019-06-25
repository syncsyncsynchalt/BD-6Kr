using System;
using System.Collections;
using UnityEngine;

public class iTweenTask
{
	public delegate void FinishedHandler(params object[] param);

	private Hashtable tweenArguments;

	public event FinishedHandler Finished;

	public void Task(GameObject go, Hashtable hash)
	{
		this.Finished = (FinishedHandler)Delegate.Combine(this.Finished, new FinishedHandler(TaskFnished));
	}

	private void TaskFnished(params object[] param)
	{
		this.Finished?.Invoke(param);
	}
}
