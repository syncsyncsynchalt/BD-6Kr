using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : IEnumerable, IEnumerable<KeyValuePair<TKey, TValue>>
{
	private class Enumerator : IEnumerator, IDisposable, IEnumerator<KeyValuePair<TKey, TValue>>
	{
		private readonly SerializableDictionary<TKey, TValue> Dictionary;

		private int current = -1;

		object IEnumerator.Current => Dictionary.GetAt(current);

		public KeyValuePair<TKey, TValue> Current => Dictionary.GetAt(current);

		public Enumerator(SerializableDictionary<TKey, TValue> dictionary)
		{
			Dictionary = dictionary;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			current++;
			return current < Dictionary.Count;
		}

		public void Reset()
		{
			current = -1;
		}
	}

	private Dictionary<TKey, int> Dictionary = new Dictionary<TKey, int>();

	[SerializeField]
	private List<TKey> KeysList = new List<TKey>();

	[SerializeField]
	private List<TValue> ValuesList = new List<TValue>();

	[NonSerialized]
	private bool dictionaryRestored;

	public TValue this[TKey key]
	{
		get
		{
			if (!dictionaryRestored)
			{
				RestoreDictionary();
			}
			return ValuesList[Dictionary[key]];
		}
		set
		{
			if (!dictionaryRestored)
			{
				RestoreDictionary();
			}
			if (Dictionary.TryGetValue(key, out int value2))
			{
				ValuesList[value2] = value;
			}
			else
			{
				Add(key, value);
			}
		}
	}

	public int Count => ValuesList.Count;

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}

	public void Add(TKey key, TValue value)
	{
		Dictionary.Add(key, ValuesList.Count);
		KeysList.Add(key);
		ValuesList.Add(value);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return new Enumerator(this);
	}

	public TValue Get(TKey key, TValue defaultValue)
	{
		if (!dictionaryRestored)
		{
			RestoreDictionary();
		}
		if (Dictionary.TryGetValue(key, out int value))
		{
			return ValuesList[value];
		}
		return defaultValue;
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		if (!dictionaryRestored)
		{
			RestoreDictionary();
		}
		if (Dictionary.TryGetValue(key, out int value2))
		{
			value = ValuesList[value2];
			return true;
		}
		value = default(TValue);
		return false;
	}

	public bool Remove(TKey key)
	{
		if (!dictionaryRestored)
		{
			RestoreDictionary();
		}
		if (Dictionary.TryGetValue(key, out int value))
		{
			RemoveAt(value);
			return true;
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		if (!dictionaryRestored)
		{
			RestoreDictionary();
		}
		TKey key = KeysList[index];
		Dictionary.Remove(key);
		KeysList.RemoveAt(index);
		ValuesList.RemoveAt(index);
		for (int i = index; i < KeysList.Count; i++)
		{
			Dictionary<TKey, int> dictionary;
			Dictionary<TKey, int> dictionary2 = dictionary = Dictionary;
			TKey key2;
			TKey key3 = key2 = KeysList[i];
			int num = dictionary[key2];
			dictionary2[key3] = num - 1;
		}
	}

	public KeyValuePair<TKey, TValue> GetAt(int index)
	{
		return new KeyValuePair<TKey, TValue>(KeysList[index], ValuesList[index]);
	}

	public TValue GetValueAt(int index)
	{
		return ValuesList[index];
	}

	public bool ContainsKey(TKey key)
	{
		if (!dictionaryRestored)
		{
			RestoreDictionary();
		}
		return Dictionary.ContainsKey(key);
	}

	public void Clear()
	{
		Dictionary.Clear();
		KeysList.Clear();
		ValuesList.Clear();
	}

	private void RestoreDictionary()
	{
		for (int i = 0; i < KeysList.Count; i++)
		{
			Dictionary[KeysList[i]] = i;
		}
		dictionaryRestored = true;
	}
}
