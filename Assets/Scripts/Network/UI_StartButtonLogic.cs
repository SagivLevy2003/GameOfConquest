using UnityEngine;
using UnityEngine.UI;

public class UI_StartButtonLogic : MonoBehaviour
{
    private Button _btn;

    private void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        BootstrapSceneManager.Instance.LoadScene("GameScene");
    }
}
