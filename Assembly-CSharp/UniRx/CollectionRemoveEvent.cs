namespace UniRx
{
	public class CollectionRemoveEvent<T>
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

		public CollectionRemoveEvent(int index, T value)
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
