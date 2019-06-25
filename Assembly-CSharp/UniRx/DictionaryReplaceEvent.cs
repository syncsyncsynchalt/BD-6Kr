namespace UniRx
{
	public class DictionaryReplaceEvent<TKey, TValue>
	{
		public TKey Key
		{
			get;
			private set;
		}

		public TValue OldValue
		{
			get;
			private set;
		}

		public TValue NewValue
		{
			get;
			private set;
		}

		public DictionaryReplaceEvent(TKey key, TValue oldValue, TValue newValue)
		{
			Key = key;
			OldValue = oldValue;
			NewValue = newValue;
		}

		public override string ToString()
		{
			return $"Key:{Key} OldValue:{OldValue} NewValue:{NewValue}";
		}
	}
}
