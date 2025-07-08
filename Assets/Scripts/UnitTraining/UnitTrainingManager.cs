using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitTrainingManager : MonoBehaviour //Currently out of scope, but I can add a unit training factory to allow the creation of units without an entity :)
{
    [field: SerializeField] public List<UnitTrainingData> TrainableUnits { get; private set; }
    public Entity ParentEntity { get; private set; }

    public UnityEvent<Entity> OnUnitTrained = new();

    private void Start()
    {
        ParentEntity = transform.root.GetComponentInChildren<Entity>();

        if (ParentEntity == null)
        {
            Debug.LogWarning($"<color=cyan>{transform.root.name}</color> has a unity training manager but no entity script. Disabling the training manager");
            enabled = false;
        }
    }

    public Entity TrainUnit(UnitTrainingData data)
    {
        Entity unit = EntitySpawnFactory.SpawnEntityAtPosition(data.Prefab, ParentEntity.OwnerPlayerObject.Id.Value, transform.position);

        OnUnitTrained?.Invoke(unit); //Fires the appropriate event

        return unit;
    }
}
