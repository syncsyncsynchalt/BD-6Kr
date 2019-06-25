namespace UniRx
{
	public interface IReadOnlyReactiveProperty<T> : IObservable<T>
	{
		T Value
		{
			get;
		}
	}
}
