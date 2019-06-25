using System;

namespace UniRx.InternalUtil
{
	public class ThreadSafeQueueWorker
	{
		private const int InitialSize = 10;

		private object gate = new object();

		private bool dequing;

		private int actionListCount;

		private Action[] actionList = new Action[10];

		private int waitingListCount;

		private Action[] waitingList = new Action[10];

		public void Enqueue(Action action)
		{
			lock (gate)
			{
				if (dequing)
				{
					if (waitingList.Length == waitingListCount)
					{
						Action[] destinationArray = new Action[checked(waitingListCount * 2)];
						Array.Copy(waitingList, destinationArray, waitingListCount);
						waitingList = destinationArray;
					}
					waitingList[waitingListCount++] = action;
				}
				else
				{
					if (actionList.Length == actionListCount)
					{
						Action[] destinationArray2 = new Action[checked(actionListCount * 2)];
						Array.Copy(actionList, destinationArray2, actionListCount);
						actionList = destinationArray2;
					}
					actionList[actionListCount++] = action;
				}
			}
		}

		public void ExecuteAll(Action<Exception> unhandledExceptionCallback)
		{
			lock (gate)
			{
				if (actionListCount == 0)
				{
					return;
				}
				dequing = true;
			}
			for (int i = 0; i < actionListCount; i++)
			{
				Action action = actionList[i];
				try
				{
					action();
				}
				catch (Exception obj)
				{
					unhandledExceptionCallback(obj);
				}
			}
			lock (gate)
			{
				dequing = false;
				Array.Clear(actionList, 0, actionListCount);
				Action[] array = actionList;
				actionListCount = waitingListCount;
				actionList = waitingList;
				waitingListCount = 0;
				waitingList = array;
			}
		}
	}
}
