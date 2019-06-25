namespace UniRx
{
	public interface IReactiveProperty<T> : IReadOnlyReactiveProperty<T>, IObservable<T>
	{
		new T Value
		{
			get;
			set;
		}
	}
}
