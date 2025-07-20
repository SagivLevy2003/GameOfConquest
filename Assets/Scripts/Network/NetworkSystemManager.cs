using UnityEngine.Events;

public class NetworkSystemManager : Singleton<NetworkSystemManager>
{
    //Non-monobehaviour managers
    public AuthManager AuthenticationManager = new();
    public ConnectionManager ConnectionManager = new();
    public PlayerObjectManager PlayerObjectManager = new();
    public NetworkObjectManager NetworkObjectManager = new();

    public UnityEvent<int, bool> OnPlayerConnectionStateChange = new(); //Gets called on both the clients and server when a client connects, arguement is whether it was a connect or disconnect.

    private void Start()
    {
        DontDestroyOnLoad(this);
        AuthenticationManager.AuthenticatePlayer();
        ConnectionManager.EventHandler = GetComponentInChildren<ConnectionEventHandler>();

        
    }

    private void OnApplicationQuit()
    {
        ConnectionManager.Disconnect();
    }
}
