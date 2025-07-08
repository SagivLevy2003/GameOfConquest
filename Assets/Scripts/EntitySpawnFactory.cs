using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class EntitySpawnFactory
{
    public static Entity SpawnEntityAtPosition(GameObject entityPrefab, int newOwnerId, Vector2 position) //Spawns an entity and sets it's owner
    {
        if (!ValidateInput(entityPrefab, newOwnerId, out PlayerObject ownerPlayerObject)) return null;


        if (!InstanceFinder.ServerManager.Clients.TryGetValue(newOwnerId, out NetworkConnection ownerConnection))
        {
            Debug.LogWarning($"The ID [<color=cyan>{newOwnerId}</color>] sent to EntitySpawnFactory does not exist.");
            return null;
        }

        GameObject entityObject = Object.Instantiate(entityPrefab, position, Quaternion.identity); 
        Entity entityScript = entityObject.GetComponentInChildren<Entity>(); 

        InstanceFinder.ServerManager.Spawn(entityObject, ownerConnection);

        entityScript.SetOwner(ownerPlayerObject);
        return entityScript;
    }

    private static bool ValidateInput(GameObject entityPrefab, int newOwnerId, out PlayerObject ownerPlayerObject)
    {
        ownerPlayerObject = null;

        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogWarning("Attempted to call EntitySpawnFactory.SpawnEntityAtPosition on a client. Aborting.");
            return false;
        }

        if (!entityPrefab.GetComponent<NetworkObject>())
        {
            Debug.LogWarning("Attempted to spawn a prefab without a NetworkObject.");
            return false;
        }

        if (!NetworkSystemManager.Instance.PlayerObjectManager.PlayerObjects.TryGetValue(newOwnerId, out ownerPlayerObject))
        {
            Debug.LogWarning("Attempted to spawn an entity with a null Player Object.");
            return false;
        }

        if (!entityPrefab.GetComponent<Entity>())
        {
            Debug.LogWarning("Attempted to spawn an entity with no... entity script (???). ");
        }

        return true;
    }
}
