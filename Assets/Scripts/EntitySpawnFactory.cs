using UnityEngine;

public class EntitySpawnFactory : Singleton<EntitySpawnFactory>
{
    public Entity SpawnEntityAtPosition(GameObject entityPrefab, Player owner, Vector2 position)
    {
        GameObject unit = Instantiate(entityPrefab, position, Quaternion.identity); //Creates the unit object
        Entity entityScript = unit.GetComponentInChildren<Entity>();

        if (entityScript == null) 
        {
            Debug.LogWarning($"A prefab was spawned using the entity spawn factory, but it doesn't contain an entity script: <color=cyan>{unit.name}</color>");
            return null;
        }

        entityScript.SetOwner(owner);
        return entityScript;
    }
}
