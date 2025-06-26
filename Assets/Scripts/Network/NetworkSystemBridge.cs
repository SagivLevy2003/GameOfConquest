using FishNet.Object;

public class NetworkSystemBridge : NetworkBehaviour
{
    [ObserversRpc]
    public void FireConnectionStateEvent(int id, bool connection)
    {
        NetworkSystemManager.Instance.OnPlayerConnectionStateChange?.Invoke(id, connection);
    }
}