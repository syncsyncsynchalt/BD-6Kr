namespace UniRx
{
	public class CollectionAddEvent<T>
	{
		public int Index
		{
			get;
			private set;
		}

		public T Value
		{
			get;
			private set;
		}

		public CollectionAddEvent(int index, T value)
		{
			Index = index;
			Value = value;
		}

		public override string ToString()
		{
			return $"Index:{Index} Value:{Value}";
		}
	}
}
