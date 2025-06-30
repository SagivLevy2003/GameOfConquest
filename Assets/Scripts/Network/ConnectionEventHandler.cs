using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.Events;

public class ConnectionEventHandler : MonoBehaviour
{
    public UnityEvent OnHostStarted;
    public UnityEvent OnHostStopped;
    public UnityEvent OnLocalClientConnected;
    public UnityEvent OnLocalClientDisconnected;
    public UnityEvent<int> OnRemoteClientConnected;
    public UnityEvent<int> OnRemoteClientDisconnected;

    private void OnEnable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState += HandleLocalClientState;
        InstanceFinder.ServerManager.OnServerConnectionState += HandleServerState;
        InstanceFinder.ServerManager.OnRemoteConnectionState += HandleRemoteClientState;
    }

    private void OnDisable()
    {
        if (InstanceFinder.ClientManager == null || InstanceFinder.ServerManager == null) return;

        InstanceFinder.ClientManager.OnClientConnectionState -= HandleLocalClientState;
        InstanceFinder.ServerManager.OnServerConnectionState -= HandleServerState;
        InstanceFinder.ServerManager.OnRemoteConnectionState -= HandleRemoteClientState;
    }

    private void HandleLocalClientState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            OnLocalClientConnected?.Invoke();
            Debug.Log($"LocalClientConnected event was fired!");
        }
        else if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            OnLocalClientDisconnected?.Invoke();
            Debug.Log($"LocalClientDisconnected event was fired!");
        }
    }

    private void HandleServerState(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            OnHostStarted?.Invoke();
            Debug.Log($"HostStarted event was fired!");
        }
        else if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            OnHostStopped?.Invoke();
            Debug.Log($"HostStopped event was fired!");
        }
    }

    private void HandleRemoteClientState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            OnRemoteClientConnected?.Invoke(conn.ClientId);
            Debug.Log($"RemoteClientConnected event was fired! Client id: {conn.ClientId}");
        }
        else if (args.ConnectionState == RemoteConnectionState.Stopped)
        {
            OnRemoteClientDisconnected?.Invoke(conn.ClientId);
            Debug.Log($"RemoteClientConnected event was fired! Client id: {conn.ClientId}");
        }
    }
}