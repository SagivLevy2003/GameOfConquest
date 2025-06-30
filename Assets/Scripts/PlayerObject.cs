using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

[System.Serializable]
public class PlayerObject : NetworkBehaviour
{
    public readonly SyncVar<int> Id = new();
    public readonly SyncVar<string> Name = new();

    public override void OnSpawnServer(NetworkConnection connection)
    {
        base.OnSpawnServer(connection);

        if (connection == Owner) AskClientToUpdateData(connection);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        NetworkSystemManager.Instance.PlayerObjectManager?.RemoveObject(Id.Value);
    }

    [TargetRpc]
    private void AskClientToUpdateData(NetworkConnection conn)
    {
        UpdatePlayerDataOnServer(NetworkSystemManager.Instance.ConnectionManager.PlayerName);
    }

    [ServerRpc(RequireOwnership = true)]
    private void UpdatePlayerDataOnServer(string username)
    {
        Name.Value = username;
        Id.Value = OwnerId;

        NetworkSystemManager.Instance.PlayerObjectManager?.AddObject(this);
    }
}
