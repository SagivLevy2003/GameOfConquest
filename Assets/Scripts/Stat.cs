using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int _baseValue = 1;
    public int Value { get; private set; }

    private readonly List<int> _valueModifiers = new();

    Stat()
    {
        RecalculateValue();
    }

    private void RecalculateValue()
    {
        Value = _baseValue + _valueModifiers.Sum();
    }

    public void AddModifier(int modValue)
    {
        _valueModifiers.Add(modValue);
        RecalculateValue();
    }

    public void RemoveModifier(int modValue)
    {
        _valueModifiers.Remove(modValue);
        RecalculateValue();
    }
}

