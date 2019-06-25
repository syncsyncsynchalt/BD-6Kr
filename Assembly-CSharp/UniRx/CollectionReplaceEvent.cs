namespace UniRx
{
	public class CollectionReplaceEvent<T>
	{
		public int Index
		{
			get;
			private set;
		}

		public T OldValue
		{
			get;
			private set;
		}

		public T NewValue
		{
			get;
			private set;
		}

		public CollectionReplaceEvent(int index, T oldValue, T newValue)
		{
			Index = index;
			OldValue = oldValue;
			NewValue = newValue;
		}

		public override string ToString()
		{
			return $"Index:{Index} OldValue:{OldValue} NewValue:{NewValue}";
		}
	}
}
