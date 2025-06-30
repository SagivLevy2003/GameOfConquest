using UnityEngine;
using UnityEngine.UI;

public class UI_LeaveButtonLogic : MonoBehaviour
{
    [SerializeField] private Button _btn;

    void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(NetworkSystemManager.Instance.ConnectionManager.Disconnect);
    }
}
