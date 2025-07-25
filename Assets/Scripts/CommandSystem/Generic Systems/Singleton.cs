using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
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
