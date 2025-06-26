using FishNet;
using FishNet.Managing.Client;
using FishNet.Transporting;
using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_JoinButtonLogic : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _joinBtn;

    private TextMeshProUGUI _buttonText;
    private string _originalJoinBtnText;

    private void Start()
    {
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        _originalJoinBtnText = _buttonText.text;
        _joinBtn.onClick.AddListener(OnJoinBtnClicked);

        InstanceFinder.ClientManager.OnClientConnectionState += ClientConnectionStateChanged;
    }

    private void OnJoinBtnClicked()
    {
        _joinBtn.interactable = false;
        NetworkSystemManager.Instance.ConnectionManager.TryConnectWithTimeout(_inputField.text);
        _buttonText.text = "Connecting...";
    }

    void ClientConnectionStateChanged(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Stopped) StartCoroutine(DisplayConnectionFailedWarning());
    }

    IEnumerator DisplayConnectionFailedWarning()
    {
        _buttonText.text = "Connection failed.";
        _buttonText.color = new(128, 0, 0);

        yield return new WaitForSeconds(2);

        _joinBtn.interactable = true;
        _buttonText.text = _originalJoinBtnText;
        _buttonText.color = Color.black;
    }
}
