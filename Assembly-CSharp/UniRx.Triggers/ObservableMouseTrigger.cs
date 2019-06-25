using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableMouseTrigger : ObservableTriggerBase
	{
		private Subject<Unit> onMouseDown;

		private Subject<Unit> onMouseDrag;

		private Subject<Unit> onMouseEnter;

		private Subject<Unit> onMouseExit;

		private Subject<Unit> onMouseOver;

		private Subject<Unit> onMouseUp;

		private Subject<Unit> onMouseUpAsButton;

		private void OnMouseDown()
		{
			if (onMouseDown != null)
			{
				onMouseDown.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDownAsObservable()
		{
			return onMouseDown ?? (onMouseDown = new Subject<Unit>());
		}

		private void OnMouseDrag()
		{
			if (onMouseDrag != null)
			{
				onMouseDrag.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseDragAsObservable()
		{
			return onMouseDrag ?? (onMouseDrag = new Subject<Unit>());
		}

		private void OnMouseEnter()
		{
			if (onMouseEnter != null)
			{
				onMouseEnter.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseEnterAsObservable()
		{
			return onMouseEnter ?? (onMouseEnter = new Subject<Unit>());
		}

		private void OnMouseExit()
		{
			if (onMouseExit != null)
			{
				onMouseExit.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseExitAsObservable()
		{
			return onMouseExit ?? (onMouseExit = new Subject<Unit>());
		}

		private void OnMouseOver()
		{
			if (onMouseOver != null)
			{
				onMouseOver.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseOverAsObservable()
		{
			return onMouseOver ?? (onMouseOver = new Subject<Unit>());
		}

		private void OnMouseUp()
		{
			if (onMouseUp != null)
			{
				onMouseUp.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseUpAsObservable()
		{
			return onMouseUp ?? (onMouseUp = new Subject<Unit>());
		}

		private void OnMouseUpAsButton()
		{
			if (onMouseUpAsButton != null)
			{
				onMouseUpAsButton.OnNext(Unit.Default);
			}
		}

		public IObservable<Unit> OnMouseUpAsButtonAsObservable()
		{
			return onMouseUpAsButton ?? (onMouseUpAsButton = new Subject<Unit>());
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (onMouseDown != null)
			{
				onMouseDown.OnCompleted();
			}
			if (onMouseDrag != null)
			{
				onMouseDrag.OnCompleted();
			}
			if (onMouseEnter != null)
			{
				onMouseEnter.OnCompleted();
			}
			if (onMouseExit != null)
			{
				onMouseExit.OnCompleted();
			}
			if (onMouseOver != null)
			{
				onMouseOver.OnCompleted();
			}
			if (onMouseUp != null)
			{
				onMouseUp.OnCompleted();
			}
			if (onMouseUpAsButton != null)
			{
				onMouseUpAsButton.OnCompleted();
			}
		}
	}
}
