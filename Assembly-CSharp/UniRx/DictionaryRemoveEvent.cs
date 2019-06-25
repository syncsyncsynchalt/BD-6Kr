namespace UniRx
{
	public class DictionaryRemoveEvent<TKey, TValue>
	{
		public TKey Key
		{
			get;
			private set;
		}

		public TValue Value
		{
			get;
			private set;
		}

		public DictionaryRemoveEvent(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public override string ToString()
		{
			return $"Key:{Key} Value:{Value}";
		}
	}
}
