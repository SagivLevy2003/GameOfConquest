using System.Linq;
using UnityEngine;

[System.Serializable]
public class IntStat : Stat<int>, ISerializationCallbackReceiver
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

        Value = (int)Mathf.Round((_baseValue + AdditiveValueModifiers.Sum()) * multiplicativeModifier);
    }
}
