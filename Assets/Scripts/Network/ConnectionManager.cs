using FishNet;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ConnectionManager
{
    public UnityEvent<bool> OnSuccessfulConnection = new(); //Arguement - IsHost

    [SerializeField] private ushort port = 7770;

    public void StartHost()
    {
        var transport = InstanceFinder.TransportManager.Transport;

        transport.SetPort(port);

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
        InstanceFinder.TransportManager.Transport.SetPort(port);

        InstanceFinder.ClientManager.StartConnection();
    }

    //public async void TryConnectWithTimeout(string ip)
    //{
    //    InstanceFinder.TransportManager.Transport.SetClientAddress(ip);
    //    InstanceFinder.TransportManager.Transport.SetPort(port);

    //    InstanceFinder.ClientManager.StartConnection();

    //    // Wait up to 5 seconds for connection success
    //    var timeout = Task.Delay(5000);
    //    var connectionCompleted = new TaskCompletionSource<bool>();

    //    void OnClientConnectionState(ClientConnectionStateArgs args)
    //    {
    //        if (args.ConnectionState == LocalConnectionState.Started)
    //        {
    //            connectionCompleted.TrySetResult(true);
    //        }
    //        else if (args.ConnectionState == LocalConnectionState.Stopped)
    //        {
    //            connectionCompleted.TrySetResult(false);
    //        }

    //        InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
    //    }

    //    InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;

    //    var completed = await Task.WhenAny(connectionCompleted.Task, timeout);
    //    if (completed == timeout)
    //    {
    //        Debug.LogWarning("Connection attempt timed out.");
    //        InstanceFinder.ClientManager.StopConnection(); // prevents loop
    //    }
    //}


    public void Shutdown()
    {
        if (!InstanceFinder.ServerManager || !InstanceFinder.ClientManager) return;
        InstanceFinder.ServerManager.StopConnection(true);
        InstanceFinder.ClientManager.StopConnection();
    }
}