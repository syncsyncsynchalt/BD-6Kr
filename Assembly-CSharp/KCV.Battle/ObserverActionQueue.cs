using System;

namespace KCV.Battle
{
	public class ObserverActionQueue : ObserverQueue<Action>
	{
		public Action Execute()
		{
			Action callback = base.observerQueue.Dequeue();
			Dlg.Call(ref callback);
			return callback;
		}

		public ObserverActionQueue Executions()
		{
			while (base.observerQueue.Count != 0)
			{
				Action callback = base.observerQueue.Dequeue();
				Dlg.Call(ref callback);
			}
			return this;
		}
	}
}
