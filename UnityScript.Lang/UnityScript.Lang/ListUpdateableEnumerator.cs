using System;
using System.Collections;

namespace UnityScript.Lang
{
	[Serializable]
	internal class ListUpdateableEnumerator : IEnumerator
	{
		protected IList _list;

		protected int _current;

		public object Current => _list[_current];

		public ListUpdateableEnumerator(IList list)
		{
			_current = -1;
			_list = list;
		}

		public void Reset()
		{
			_current = -1;
		}

		public bool MoveNext()
		{
			checked
			{
				_current++;
				return _current < _list.Count;
			}
		}

		public void Update(object newValue)
		{
			_list[_current] = newValue;
		}
	}
}
