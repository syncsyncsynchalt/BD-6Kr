using System;
using System.Collections.Generic;

namespace KCV.Arsenal
{
	internal class StateManager<State>
	{
		private Stack<State> mStateStack;

		private State mEmptyState;

		public Action<State> OnPush
		{
			private get;
			set;
		}

		public Action<State> OnPop
		{
			private get;
			set;
		}

		public Action<State> OnResume
		{
			private get;
			set;
		}

		public Action<State> OnSwitch
		{
			private get;
			set;
		}

		public State CurrentState
		{
			get
			{
				if (0 < mStateStack.Count)
				{
					return mStateStack.Peek();
				}
				return mEmptyState;
			}
		}

		public StateManager(State emptyState)
		{
			mEmptyState = emptyState;
			mStateStack = new Stack<State>();
		}

		public void PushState(State state)
		{
			mStateStack.Push(state);
			Notify(OnPush, mStateStack.Peek());
			Notify(OnSwitch, mStateStack.Peek());
		}

		public void ReplaceState(State state)
		{
			if (0 < mStateStack.Count)
			{
				PopState();
			}
			mStateStack.Push(state);
			Notify(OnPush, mStateStack.Peek());
			Notify(OnSwitch, mStateStack.Peek());
		}

		public void PopState()
		{
			if (0 < mStateStack.Count)
			{
				State state = mStateStack.Pop();
				Notify(OnPop, state);
			}
		}

		public void ResumeState()
		{
			if (0 < mStateStack.Count)
			{
				Notify(OnResume, mStateStack.Peek());
				Notify(OnSwitch, mStateStack.Peek());
			}
		}

		public void ClearStates()
		{
			mStateStack.Clear();
		}

		public override string ToString()
		{
			mStateStack.ToArray();
			string text = string.Empty;
			foreach (State item in mStateStack)
			{
				text = item + " > " + text;
			}
			return text;
		}

		private void Notify(Action<State> target, State state)
		{
			target?.Invoke(state);
		}
	}
}
