using System;

namespace UniRx
{
	public class CancellationToken
	{
		private readonly ICancelable source;

		public static CancellationToken Empty = new CancellationToken(new BooleanDisposable());

		public bool IsCancellationRequested => source.IsDisposed;

		public CancellationToken(ICancelable source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this.source = source;
		}

		public void ThrowIfCancellationRequested()
		{
			if (IsCancellationRequested)
			{
				throw new OperationCanceledException();
			}
		}
	}
}
