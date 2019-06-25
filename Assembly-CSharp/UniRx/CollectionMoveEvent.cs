namespace UniRx
{
	public class CollectionMoveEvent<T>
	{
		public int OldIndex
		{
			get;
			private set;
		}

		public int NewIndex
		{
			get;
			private set;
		}

		public T Value
		{
			get;
			private set;
		}

		public CollectionMoveEvent(int oldIndex, int newIndex, T value)
		{
			OldIndex = oldIndex;
			NewIndex = newIndex;
			Value = value;
		}

		public override string ToString()
		{
			return $"OldIndex:{OldIndex} NewIndex:{NewIndex} Value:{Value}";
		}
	}
}
