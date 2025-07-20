using UnityEngine;

public class DontDestroyOnLoadInitializer : MonoBehaviour
{
    bool _initialized = false;

    void Start()
    {
        if (!_initialized)
        {
            DontDestroyOnLoad(gameObject);
            _initialized = true;
        }
    }
}
