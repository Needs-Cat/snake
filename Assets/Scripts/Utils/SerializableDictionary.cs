using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private List<TKey> m_keys = new();
	[SerializeField]
	private List<TValue> m_values = new();

	private Dictionary<TKey, TValue> m_dictionary = new();

	private bool ContainsDuplicateKey()
	{
		HashSet<TKey> keys = new();
		foreach (TKey key in m_keys)
		{
			if (!keys.Add(key))
			{
				// Found a duplicate
				return true;
			}
		}

		return false;
	}
	
#region IDictionaryMembers
	
	public int Count => m_dictionary.Count;
	public bool IsReadOnly => ((IDictionary<TKey, TValue>) m_dictionary).IsReadOnly;

	public ICollection<TKey> Keys => m_dictionary.Keys;
	public ICollection<TValue> Values => m_dictionary.Values;
	
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return m_dictionary.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(KeyValuePair<TKey, TValue> item)
	{
		m_dictionary.Add(item.Key, item.Value);
	}

	public void Clear()
	{
		m_dictionary.Clear();
	}

	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return ((IDictionary<TKey, TValue>) m_dictionary).Contains(item);
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((IDictionary<TKey, TValue>) m_dictionary).CopyTo(array, arrayIndex);
	}

	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return ((IDictionary<TKey, TValue>) m_dictionary).Remove(item);
	}

	public void Add(TKey key, TValue value)
	{
		m_dictionary.Add(key, value);
	}

	public bool ContainsKey(TKey key)
	{
		return m_dictionary.ContainsKey(key);
	}

	public bool Remove(TKey key)
	{
		return m_dictionary.Remove(key);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		return m_dictionary.TryGetValue(key, out value);
	}

	public TValue this[TKey key]
	{
		get => m_dictionary[key];
		set => m_dictionary[key] = value;
	}
	
#endregion
	
#region ISerializationCallbackReceiverMembers
	
	public void OnBeforeSerialize()
	{
		// Don't copy over if there are duplicate keys
		if (ContainsDuplicateKey())
		{
			return;
		}
		
		m_keys.Clear();
		m_values.Clear();
		
		// Convert dictionary to lists
		foreach (KeyValuePair<TKey, TValue> pair in m_dictionary)
		{
			m_keys.Add(pair.Key);
			m_values.Add(pair.Value);
		}
	}

	public void OnAfterDeserialize()
	{
		// Don't copy over if there are duplicate keys
		if (ContainsDuplicateKey())
		{
			return;
		}
		
		m_dictionary.Clear();

		// Convert lists to dictionary
		for (int i = 0; i < m_keys.Count; i++)
		{
			m_dictionary.TryAdd(m_keys[i], m_values[i]);
		}
	}
	
#endregion
	
}
