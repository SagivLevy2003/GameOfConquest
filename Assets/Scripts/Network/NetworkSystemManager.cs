using FishNet;
using FishNet.Connection;
using FishNet.Transporting;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class NetworkSystemManager : Singleton<NetworkSystemManager>
{
    //Non-monobehaviour managers
    public AuthManager AuthenticationManager = new();
    public ConnectionManager ConnectionManager = new();

    //Monobehaviour managers
    public NetworkSystemBridge NetworkSystemBridge;

    public UnityEvent<int, bool> OnPlayerConnectionStateChange = new(); //Gets called on both the clients and server when a client connects, arguement is whether it was a connect or disconnect.

    private void Start()
    {
        AuthenticationManager.AuthenticatePlayer();
    }

    private void OnEnable()
    {
        InstanceFinder.ServerManager.OnRemoteConnectionState += OnRemoteClientConnectionState;
    }

    private void OnDisable()
    {
        if (InstanceFinder.ServerManager) InstanceFinder.ServerManager.OnRemoteConnectionState -= OnRemoteClientConnectionState;
    }

    private void OnRemoteClientConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            Debug.Log($"Client {conn.ClientId} connected.");
            NetworkSystemBridge.FireConnectionStateEvent(conn.ClientId, true);
        }
        else if (args.ConnectionState == RemoteConnectionState.Stopped)
        {
            Debug.Log($"Client {conn.ClientId} disconnected.");
            NetworkSystemBridge.FireConnectionStateEvent(conn.ClientId, false);
        }
    }

    private void OnApplicationQuit()
    {
        ConnectionManager.Shutdown();
    }
}
