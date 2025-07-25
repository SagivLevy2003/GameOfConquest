using FishNet;
using FishNet.Object;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class NetworkObjectManager
{
    public bool TryGetNetworkObjectById(int id, out NetworkObject networkObject, bool log_on_fail = true)
    {
        networkObject = GetNetworkObjectById(id, log_on_fail);
        return networkObject != null;
    }

    public NetworkObject GetNetworkObjectById(int id, bool log_on_fail = true)
    {
        if (id == -1) return null;
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