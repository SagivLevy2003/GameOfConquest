using FishNet.Object;
using UnityEngine;

public class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"Detected duplicate singleton script of type {Instance}!");
            Debug.LogWarning($"Roots: {transform.root},  {Instance.transform.root}");
            Destroy(this);
        }
        else
        {
            Instance = this as T;
        }
    }
}