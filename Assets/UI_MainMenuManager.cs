using FishNet;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UI_MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _joinMenu;
    [SerializeField] private GameObject _createButton;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _leaveButton;
    [SerializeField] private GameObject _nameMenu;

    UnityAction _clientConnectedHandler;
    UnityAction _clientDisconnectedHandler;

    public enum MenuState
    {
        Main,
        LobbyHost,
        LobbyClient,
    }

    public MenuState State;

    private void OnEnable()
    {
        StartCoroutine(SetUpMenu());
    }

    private void OnDisable()
    {
        ConnectionEventHandler eventHandler = NetworkSystemManager.Instance.ConnectionManager.EventHandler;
        eventHandler.OnLocalClientDisconnected.RemoveListener(_clientDisconnectedHandler);
        eventHandler.OnLocalClientConnected.RemoveListener(_clientConnectedHandler);
    }

    private void ChangeState(MenuState state)
    {
        State = state;
        Debug.Log($"Main menu state was changed to: <color=cyan>{state}</color>.");
        UpdateMenuUI();
    }

    private void UpdateMenuUI()
    {
        switch (State)
        {
            case MenuState.Main:
                _joinMenu.SetActive(true);
                _createButton.SetActive(true);
                _startButton.SetActive(false);
                _leaveButton.SetActive(false);
                _nameMenu.SetActive(true);
                break;
            case MenuState.LobbyHost:
                _joinMenu.SetActive(false);
                _createButton.SetActive(false);
                _startButton.SetActive(true);
                _leaveButton.SetActive(true);
                _nameMenu.SetActive(false);
                break;
            case MenuState.LobbyClient:
                _joinMenu.SetActive(false);
                _createButton.SetActive(false);
                _startButton.SetActive(false);
                _leaveButton.SetActive(true);
                _nameMenu.SetActive(false);
                break;
        }
    }

    IEnumerator SetUpMenu()
    {
        yield return null; //Delayed by a frame, since singletons aren't initialized yet

        ConnectionEventHandler eventHandler = NetworkSystemManager.Instance.ConnectionManager.EventHandler;

        _clientConnectedHandler = () =>
        {
            if (InstanceFinder.IsServerStarted) ChangeState(MenuState.LobbyHost);
            else ChangeState(MenuState.LobbyClient);
        };

        eventHandler.OnLocalClientConnected.AddListener(_clientConnectedHandler);

        _clientDisconnectedHandler = () => ChangeState(MenuState.Main);
        eventHandler.OnLocalClientDisconnected.AddListener(_clientDisconnectedHandler);

        UpdateMenuUI();
    }
}