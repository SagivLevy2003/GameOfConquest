using UnityEngine.Events;

public class NetworkSystemManager : Singleton<NetworkSystemManager>
{
    //Non-monobehaviour managers
    public AuthManager AuthenticationManager = new();
    public ConnectionManager ConnectionManager = new();
    public PlayerObjectManager PlayerObjectManager = new();
    public NetworkObjectManager NetworkObjectManager = new();

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
