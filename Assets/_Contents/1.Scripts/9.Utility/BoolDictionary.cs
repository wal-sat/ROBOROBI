using System.Collections.Generic;

public class BoolDictionary<T>
{
    private const int MaxCount = 10;
    private Dictionary<T, bool> _dictionary = new Dictionary<T, bool>();

    // ----- Life Cycle Methods -----

    private void Update()
    {
        if (_dictionary.Count > MaxCount)
        {
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
        _dictionary[key] = true;
    }

    public bool GetValue(T key)
    {
        return _dictionary.TryGetValue(key, out bool value) ? value : false;
    }

    public void ClearAll()
    {
        _dictionary.Clear();
    }
}
