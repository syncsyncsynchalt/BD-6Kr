using System;

namespace UniRx
{
	public class MultipleAssignmentDisposable : IDisposable, ICancelable
	{
		private static readonly BooleanDisposable True = new BooleanDisposable(isDisposed: true);

		private object gate = new object();

		private IDisposable current;

		public bool IsDisposed
		{
			get
			{
				lock (gate)
				{
					return current == True;
				}
			}
		}

		public IDisposable Disposable
		{
			get
			{
				lock (gate)
				{
					object result;
					if (current == True)
					{
						IDisposable empty = UniRx.Disposable.Empty;
						result = empty;
					}
					else
					{
						result = current;
					}
					return (IDisposable)result;
				}
			}
			set
			{
				bool flag = false;
				lock (gate)
				{
					flag = (current == True);
					if (!flag)
					{
						current = value;
					}
				}
				if (flag)
				{
					value?.Dispose();
				}
			}
		}

		public void Dispose()
		{
			IDisposable disposable = null;
			lock (gate)
			{
				if (current != True)
				{
					disposable = current;
					current = True;
				}
			}
			disposable?.Dispose();
		}
	}
}
