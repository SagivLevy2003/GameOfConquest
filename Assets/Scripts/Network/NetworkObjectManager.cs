using FishNet;
using FishNet.Object;
using UnityEngine;

public class NetworkObjectManager
{
    public NetworkObject GetNetworkObjectById(int id, bool log_on_fail = true)
    {
        NetworkObject netObj;

        if (InstanceFinder.IsServerStarted)
        {
            if (InstanceFinder.ServerManager.Objects.Spawned.TryGetValue(id, out netObj)) return netObj; //Return the object on the server
        }

        if (InstanceFinder.ClientManager.Objects.Spawned.TryGetValue(id, out netObj)) return netObj; //Return the object on the client

        if (log_on_fail) Debug.LogWarning($"Attempted to query for a NetworkObject with id <color=cyan>[{id}]</color>, but none was found.");
        return null;
    }
}