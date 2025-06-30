using FishNet;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class ConnectionManager
{
    public ConnectionEventHandler EventHandler; //ConnectionEventHandler is a monobehaviour

    public UnityEvent<bool> OnSuccessfulConnection = new(); //Arguement - IsHost

    public string PlayerName { get; private set; } = "tester";

    [SerializeField] private ushort _port = 7770;

    public void StartHost()
    {
        var transport = InstanceFinder.TransportManager.Transport;

        transport.SetPort(_port);

        // Start the server
        InstanceFinder.ServerManager.StartConnection();

        InstanceFinder.ServerManager.OnServerConnectionState += OnServerHosted;
    }

    private void OnServerHosted(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            InstanceFinder.ClientManager.StartConnection();
            OnSuccessfulConnection?.Invoke(true);
            InstanceFinder.ServerManager.OnServerConnectionState -= OnServerHosted;
        }
        else if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            InstanceFinder.ServerManager.OnServerConnectionState -= OnServerHosted;
        }
    }

    public void StopHost()
    {
        InstanceFinder.ServerManager.StopConnection(true);
    }

    public void TryConnectWithTimeout(string ip)
    {
        if (InstanceFinder.ClientManager.Connection.IsActive)
        {
            Debug.LogWarning("Client is already connected.");
            return;
        }

        InstanceFinder.TransportManager.Transport.SetClientAddress(ip);
        InstanceFinder.TransportManager.Transport.SetPort(_port);

        InstanceFinder.ClientManager.StartConnection();
    }

    public void Disconnect()
    {
        if (InstanceFinder.IsServerStarted)
            InstanceFinder.ServerManager.StopConnection(true); // true = kick all clients

        if (InstanceFinder.IsClientStarted)
            InstanceFinder.ClientManager.StopConnection();
    }

    public void ChangeName(string newName)
    {
        PlayerName = newName;
    }
}
