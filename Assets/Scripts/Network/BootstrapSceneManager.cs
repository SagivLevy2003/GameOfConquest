using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapSceneManager : Singleton<BootstrapSceneManager>
{
    [SerializeField] private string _currentScene;

    private void Start()
    {
        DontDestroyOnLoad(this);
        _currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string sceneName)
    {
        if (!InstanceFinder.NetworkManager.IsServerStarted) return;

        Debug.Log($"Loading scene: <color=cyan>{sceneName}</color>");

        SceneLoadData sld = new(sceneName);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);

        SceneUnloadData sud = new(_currentScene);
        InstanceFinder.SceneManager.UnloadGlobalScenes(sud);

        _currentScene = sceneName; 
    }
}