using FishNet;
using FishNet.Object;
using UnityEngine;

public class EntitySpawnFactory
{
    public static Entity SpawnEntityAtPosition(GameObject entityPrefab, PlayerObject owner, Vector2 position)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogWarning("EntitySpawnFactory.SpawnEntityAtPosition was called on a client. Aborting.");
            return null;
        }

        if (entityPrefab.GetComponent<NetworkObject>() == null)
        {
            Debug.LogError("Tried to spawn a prefab without a NetworkObject.");
            return null;
        }

        GameObject entityObject = Object.Instantiate(entityPrefab, position, Quaternion.identity); //Creates the entity object
        Entity entityScript = entityObject.GetComponentInChildren<Entity>();

        if (entityScript == null) 
        {
            Debug.LogWarning($"A prefab was instantiated using the entity spawn factory, but it doesn't contain an entity script: <color=cyan>{entityObject.name}</color>\n" +
                $"Destroying the instantiated object.");
            Object.Destroy(entityObject);
            return null;
        }

        InstanceFinder.ServerManager.Spawn(entityObject);

        entityScript.SetOwner(owner);
        return entityScript;
    }
}
