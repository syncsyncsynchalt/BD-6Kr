using DG.Tweening;
using KCV;
using KCV.Strategy;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceAlbumShipViewer : MonoBehaviour
{
	public enum State
	{
		None,
		SwitchRotation,
		Waiting,
		MovingCamera
	}

	public enum Orientation
	{
		Vertical,
		Horizontal
	}

	public enum AnimationType
	{
		RotateDisplay
	}

	private class StateManager<State>
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

	[SerializeField]
	private Camera mCamera_Main;

	[SerializeField]
	private StrategyShipCharacter mStrategyShipCharacter;

	[SerializeField]
	private UIAlbumShipViewerCameraController mUIAlbumShipViewerCameraController;

	[SerializeField]
	private UILabel mLabel_ShipName;

	private Vector3 mShipDefaultOffset;

	private StateManager<State> mStateManager;

	private int DISPLAY_WIDTH = 960;

	private int DISPLAY_HEIGHT = 544;

	private float ASPECT = 17f / 30f;

	private Orientation mDisplayOrientation = Orientation.Horizontal;

	private KeyControl mKeyController;

	private void Start()
	{
		mKeyController = new KeyControl();
		mStrategyShipCharacter.DebugChange(1);
		mStateManager = new StateManager<State>(State.None);
		mStateManager.OnPush = OnPushState;
		mStateManager.OnPop = OnPopState;
		mStateManager.OnResume = OnResumeState;
		mUIAlbumShipViewerCameraController.Initialize(1024, 1024, 1f);
		mStateManager.PushState(State.Waiting);
	}

	private void Update()
	{
		if (mKeyController != null)
		{
			mKeyController.Update();
			if (mKeyController.IsLDown())
			{
				SwitchOrientation();
			}
			else if (mKeyController.IsSankakuDown())
			{
				mStrategyShipCharacter.DebugChange(-1);
			}
			else if (mKeyController.IsShikakuDown())
			{
				mStrategyShipCharacter.DebugChange(1);
			}
		}
	}

	private void UpdateShipInfo(ShipModelMst shipModelMst)
	{
		mLabel_ShipName.text = shipModelMst.Name;
	}

	private void ChangeOrientation(Orientation changeToOrientation, Action onFinished)
	{
		mDisplayOrientation = changeToOrientation;
		Sequence sequence = DOTween.Sequence().SetId(AnimationType.RotateDisplay);
		Tween t = null;
		switch (changeToOrientation)
		{
		case Orientation.Vertical:
		{
			Screen.orientation = ScreenOrientation.Portrait;
			Tween t2 = mCamera_Main.transform.DOLocalMove(Vector3.zero, 0.3f);
			Tween t3 = DOVirtual.Float(mCamera_Main.orthographicSize, (float)DISPLAY_HEIGHT / (float)DISPLAY_WIDTH, 0.9f, delegate(float size)
			{
				mCamera_Main.orthographicSize = size;
			});
			Tween t4 = mCamera_Main.transform.DORotate(new Vector3(0f, 0f, 90f), 0.9f).OnComplete(delegate
			{
				mCamera_Main.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			});
			sequence.Append(t3);
			sequence.Join(t);
			sequence.Join(t2);
			sequence.Join(t4);
			break;
		}
		case Orientation.Horizontal:
		{
			Screen.orientation = ScreenOrientation.Landscape;
			Tween t2 = mCamera_Main.transform.DOLocalMove(Vector3.zero, 0.3f);
			Tween t3 = DOVirtual.Float(mCamera_Main.orthographicSize, 1f, 0.9f, delegate(float size)
			{
				mCamera_Main.orthographicSize = size;
			});
			Tween t4 = mCamera_Main.transform.DORotate(new Vector3(0f, 0f, 0f), 0.9f).OnComplete(delegate
			{
				mCamera_Main.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			});
			sequence.Append(t4);
			sequence.Join(t);
			sequence.Join(t2);
			sequence.Join(t3);
			break;
		}
		}
		sequence.OnComplete(delegate
		{
			if (onFinished != null)
			{
				onFinished();
			}
		});
	}

	private void OnCallShipDragListener(Vector2 delta)
	{
		if (mStateManager.CurrentState == State.Waiting)
		{
			switch (mDisplayOrientation)
			{
			case Orientation.Vertical:
			{
				Vector2 vector = delta * ASPECT;
				break;
			}
			}
		}
	}

	private void OnPushState(State pushState)
	{
		switch (pushState)
		{
		case State.Waiting:
			mUIAlbumShipViewerCameraController.SetKeyController(mKeyController);
			break;
		case State.SwitchRotation:
			OnPushSwitchRotation();
			break;
		}
	}

	private void OnPushSwitchRotation()
	{
		switch (mDisplayOrientation)
		{
		case Orientation.Horizontal:
			ChangeOrientation(Orientation.Vertical, delegate
			{
				mUIAlbumShipViewerCameraController.Initialize(1024, 1024, 1f);
				mStateManager.PopState();
				mStateManager.ResumeState();
			});
			break;
		case Orientation.Vertical:
			ChangeOrientation(Orientation.Horizontal, delegate
			{
				mUIAlbumShipViewerCameraController.Initialize(1024, 1024, 1f);
				mStateManager.PopState();
				mStateManager.ResumeState();
			});
			break;
		}
	}

	private void OnPopState(State popState)
	{
	}

	private void OnResumeState(State resumeState)
	{
		if (resumeState == State.Waiting)
		{
		}
	}

	private void SwitchOrientation()
	{
		if (mStateManager.CurrentState == State.Waiting)
		{
			mStateManager.PushState(State.SwitchRotation);
		}
	}
}
