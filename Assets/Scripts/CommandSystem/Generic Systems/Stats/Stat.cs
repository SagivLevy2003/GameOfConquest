using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class Stat<T>
{
    [SerializeField] protected T _baseValue;
    [field: SerializeField, ReadOnly] public T Value { get; protected set; }

    [Header("Debug Lists - Do Not Modify")]
    [field: SerializeField, ReadOnly] protected List<int> AdditiveValueModifiers { get; private set; } = new();
    [field: SerializeField, ReadOnly] protected List<float> MultiplicativeValueModifiers { get; private set; } = new();

    public UnityEvent<T,T> OnValueChanged = new();

    public Stat()
    {
        RecalculateValue();
    }
    
    public void AddModifier(int value, bool isAdditive)
    {
        if (isAdditive) AdditiveValueModifiers.Add(value);
        else MultiplicativeValueModifiers.Add(value);
        RecalculateValue();
    }

    public void RemoveModifier(int value, bool isAdditive)
    {
        if (isAdditive) AdditiveValueModifiers.Remove(value);
        else MultiplicativeValueModifiers.Remove(value);
        RecalculateValue();
    }

    protected void RecalculateValue()
    {
        T previousValue = Value;

        UpdateValue();

        if (!previousValue.Equals(Value))
        {
            OnValueChanged?.Invoke(previousValue, Value);
        }
    }

    protected abstract void UpdateValue();
}


