namespace Server_Lib
{
	public class SynchronizedList<T> : CustomCollectionBase<T>
	{
		public override T this[int index]
		{
			get
			{
				return (T)base.List[index];
			}
			set
			{
				lock (base.List.SyncRoot)
				{
					base.List[index] = value;
				}
			}
		}

		public override int Add(T item)
		{
			lock (base.List.SyncRoot)
			{
				return base.List.Add(item);
			}
		}

		public override void AddRange(T[] items)
		{
			lock (base.List.SyncRoot)
			{
				foreach (T item in items)
				{
					Add(item);
				}
			}
		}

		public override int IndexOf(T item)
		{
			lock (base.List.SyncRoot)
			{
				return base.List.IndexOf(item);
			}
		}

		public override void Insert(int index, T value)
		{
			lock (base.List.SyncRoot)
			{
				base.List.Insert(index, value);
			}
		}

		public override void Remove(T value)
		{
			lock (base.List.SyncRoot)
			{
				base.List.Remove(value);
			}
		}

		public override bool Contains(T value)
		{
			return base.List.Contains(value);
		}
	}
}
