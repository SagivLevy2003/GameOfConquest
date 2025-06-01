using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitTrainingManager : MonoBehaviour
{
    [field: SerializeField] public List<UnitTrainingData> TrainableUnits { get; private set; }

    public UnityEvent<UnitTrainingData> OnUnitTrained = new();

    public GameObject TrainUnit(UnitTrainingData data)
    {
        GameObject unit = Instantiate(data.Prefab, transform.position, Quaternion.identity);
        OnUnitTrained?.Invoke(data);

        return unit;
    }
}