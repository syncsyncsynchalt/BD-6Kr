namespace UniRx
{
	public class DictionaryAddEvent<TKey, TValue>
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

		public DictionaryAddEvent(TKey Key, TValue value)
		{
			this.Key = Key;
			Value = value;
		}

		public override string ToString()
		{
			return $"Key:{Key} Value:{Value}";
		}
	}
}
