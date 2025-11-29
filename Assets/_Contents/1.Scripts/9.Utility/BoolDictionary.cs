using UnityEngine;
using System.Collections.Generic;

public class BoolDictionary<T>
{
    private const int MaxCount = 25;
    private Dictionary<T, bool> _dictionary = new Dictionary<T, bool>();

    // ----- Life Cycle Methods -----

    private void Update()
    {
        if (_dictionary.Count > MaxCount)
        {
            Debug.LogWarning($"BoolDictionary exceeded max count of {MaxCount}. Clearing all entries.");
            ClearAll();
        }
    }

    // ----- Public Methods -----

    public bool CheckValue(T key)
    {
        if (!_dictionary.ContainsKey(key) || _dictionary[key] == true)
        {
            _dictionary[key] = false;
            return true;
        }
        return false;
    }

    public void ResetValue(T key)
    {
        if (!_dictionary.ContainsKey(key)) return;
        _dictionary[key] = true;
    }

    public bool GetValue(T key)
    {
        return _dictionary.TryGetValue(key, out bool value) ? value : false;
    }

    public List<T> GetAllKeys()
    {
        return new List<T>(_dictionary.Keys);
    }

    public void RemoveKey(T key)
    {
        if (_dictionary.ContainsKey(key))
        {
            _dictionary.Remove(key);
        }
    }

    public void ClearAll()
    {
        _dictionary.Clear();
    }
}
