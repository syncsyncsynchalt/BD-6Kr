using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UniRx
{
	[Serializable]
	public class ReactiveCollection<T> : Collection<T>
	{
		[NonSerialized]
		private Subject<int> countChanged;

		[NonSerialized]
		private Subject<Unit> collectionReset;

		[NonSerialized]
		private Subject<CollectionAddEvent<T>> collectionAdd;

		[NonSerialized]
		private Subject<CollectionMoveEvent<T>> collectionMove;

		[NonSerialized]
		private Subject<CollectionRemoveEvent<T>> collectionRemove;

		[NonSerialized]
		private Subject<CollectionReplaceEvent<T>> collectionReplace;

		public ReactiveCollection()
		{
		}

		public ReactiveCollection(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (T item in collection)
			{
				Add(item);
			}
		}

		public ReactiveCollection(List<T> list)
			: base((IList<T>)((list == null) ? null : new List<T>(list)))
		{
		}

		protected override void ClearItems()
		{
			int count = Count;
			base.ClearItems();
			if (collectionReset != null)
			{
				collectionReset.OnNext(Unit.Default);
			}
			if (count > 0 && countChanged != null)
			{
				countChanged.OnNext(Count);
			}
		}

		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			if (collectionAdd != null)
			{
				collectionAdd.OnNext(new CollectionAddEvent<T>(index, item));
			}
			if (countChanged != null)
			{
				countChanged.OnNext(Count);
			}
		}

		public void Move(int oldIndex, int newIndex)
		{
			MoveItem(oldIndex, newIndex);
		}

		protected virtual void MoveItem(int oldIndex, int newIndex)
		{
			T val = this[oldIndex];
			base.RemoveItem(oldIndex);
			base.InsertItem(newIndex, val);
			if (collectionMove != null)
			{
				collectionMove.OnNext(new CollectionMoveEvent<T>(oldIndex, newIndex, val));
			}
		}

		protected override void RemoveItem(int index)
		{
			T value = this[index];
			base.RemoveItem(index);
			if (collectionRemove != null)
			{
				collectionRemove.OnNext(new CollectionRemoveEvent<T>(index, value));
			}
			if (countChanged != null)
			{
				countChanged.OnNext(Count);
			}
		}

		protected override void SetItem(int index, T item)
		{
			T oldValue = this[index];
			base.SetItem(index, item);
			if (collectionReplace != null)
			{
				collectionReplace.OnNext(new CollectionReplaceEvent<T>(index, oldValue, item));
			}
		}

		public IObservable<int> ObserveCountChanged()
		{
			return countChanged ?? (countChanged = new Subject<int>());
		}

		public IObservable<Unit> ObserveReset()
		{
			return collectionReset ?? (collectionReset = new Subject<Unit>());
		}

		public IObservable<CollectionAddEvent<T>> ObserveAdd()
		{
			return collectionAdd ?? (collectionAdd = new Subject<CollectionAddEvent<T>>());
		}

		public IObservable<CollectionMoveEvent<T>> ObserveMove()
		{
			return collectionMove ?? (collectionMove = new Subject<CollectionMoveEvent<T>>());
		}

		public IObservable<CollectionRemoveEvent<T>> ObserveRemove()
		{
			return collectionRemove ?? (collectionRemove = new Subject<CollectionRemoveEvent<T>>());
		}

		public IObservable<CollectionReplaceEvent<T>> ObserveReplace()
		{
			return collectionReplace ?? (collectionReplace = new Subject<CollectionReplaceEvent<T>>());
		}
	}
}
