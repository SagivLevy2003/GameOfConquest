using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;


[System.Serializable]
public class PlayerObject : NetworkBehaviour
{
    public readonly SyncVar<int> Id = new();
    public readonly SyncVar<string> Name = new();

    public readonly List<Entity> OwnedEntities = new();

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
        name = username; //Sets the object name to the username
        NetworkSystemManager.Instance.PlayerObjectManager?.AddObject(this);
    }
}
