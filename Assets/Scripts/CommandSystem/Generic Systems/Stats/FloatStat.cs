using System.Linq;
using UnityEngine;

[System.Serializable]
public class FloatStat : Stat<float>, ISerializationCallbackReceiver
{
    public void OnAfterDeserialize()
    {
        RecalculateValue();
    }

    public void OnBeforeSerialize() { }
    protected override void UpdateValue()
    {
        float multiplicativeModifier = 1;
        foreach (var modifier in MultiplicativeValueModifiers)
        {
            multiplicativeModifier *= modifier;
        }

        Value = (_baseValue + AdditiveValueModifiers.Sum()) * multiplicativeModifier;
    }

}

