using FishNet;
using FishNet.Managing;
using FishNet.Transporting;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_CreateButtonLogic : MonoBehaviour
{
    private Button _createBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _createBtn = GetComponent<Button>();
        _createBtn.onClick.AddListener(OnCreateBtnClicked);
    }

    public void OnCreateBtnClicked()
    {
        NetworkSystemManager.Instance.ConnectionManager.StartHost();
        _createBtn.interactable = false;
    }

    void ClientConnectionStateChanged(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Stopped && !_createBtn.interactable)
        {
            Debug.LogWarning("Failed to host!");
            _createBtn.interactable = true;
        }
    }

    //private void OnEnable()
    //{
    //    StartCoroutine(SubscribeToConnectionEventDelayed());
    //}

    //private void OnDisable()
    //{
    //    InstanceFinder.ClientManager.OnClientConnectionState -= ClientConnectionStateChanged;
    //}

    //IEnumerator SubscribeToConnectionEventDelayed() //Delays subscribing to the event because some times the NetworkSystemManager singleton isn't initialized at OnEnable()
    //{
    //    yield return null;
    //    InstanceFinder.ClientManager.OnClientConnectionState += ClientConnectionStateChanged;
    //}
}
