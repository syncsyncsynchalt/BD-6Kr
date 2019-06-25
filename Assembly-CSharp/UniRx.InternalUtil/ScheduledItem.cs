using System;

namespace UniRx.InternalUtil
{
	public class ScheduledItem : IComparable<ScheduledItem>
	{
		private readonly BooleanDisposable _disposable = new BooleanDisposable();

		private readonly TimeSpan _dueTime;

		private readonly Action _action;

		public TimeSpan DueTime => _dueTime;

		public IDisposable Cancellation => _disposable;

		public bool IsCanceled => _disposable.IsDisposed;

		public ScheduledItem(Action action, TimeSpan dueTime)
		{
			_dueTime = dueTime;
			_action = action;
		}

		public void Invoke()
		{
			if (!_disposable.IsDisposed)
			{
				_action();
			}
		}

		public int CompareTo(ScheduledItem other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return 1;
			}
			return DueTime.CompareTo(other.DueTime);
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator <(ScheduledItem left, ScheduledItem right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator <=(ScheduledItem left, ScheduledItem right)
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool operator >(ScheduledItem left, ScheduledItem right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool operator >=(ScheduledItem left, ScheduledItem right)
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool operator ==(ScheduledItem left, ScheduledItem right)
		{
			return object.ReferenceEquals(left, right);
		}

		public static bool operator !=(ScheduledItem left, ScheduledItem right)
		{
			return !(left == right);
		}
	}
}
