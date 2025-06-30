using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NameChangeLogic : MonoBehaviour
{
    TMP_InputField _nameInputField;

    void Start()
    {
        _nameInputField = GetComponent<TMP_InputField>();
        _nameInputField.onEndEdit.AddListener(NetworkSystemManager.Instance.ConnectionManager.ChangeName);
    }
}
