using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

[System.Serializable]
public class PlayerObject : NetworkBehaviour
{
    public int Id;
    public readonly SyncVar<string> Name = new();

    public override void OnStartServer()
    {
        base.OnStartServer();
        Id = OwnerId;
    }

    public static PlayerObject GetPlayerObject(int clientId)
    {
        if (!InstanceFinder.ServerManager) Debug.Log("error");

        if (InstanceFinder.ServerManager.Clients.TryGetValue(clientId, out NetworkConnection conn))
        {
            return conn.FirstObject.GetComponent<PlayerObject>();
        }

        return null;
    }
}
