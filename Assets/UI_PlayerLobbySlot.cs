using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class UI_PlayerLobbySlot : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private PlayerObject _assignedPlayerObject;

    private readonly SyncVar<string> _name = new();

    private void OnEnable()
    {
        _name.OnChange += UpdatePlayerName;
    }

    private void OnDisable()
    {
        _name.OnChange -= UpdatePlayerName;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        Transform container = GameObject.Find("PlayerListContainer").transform;
        transform.SetParent(container, worldPositionStays: false);
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        UpdatePlayerName("", _name.Value, false);
    }

    public void Initialize(int id) //Called on the SERVER
    {
        if (!IsServerInitialized) return;

        _assignedPlayerObject = NetworkSystemManager.Instance.PlayerObjectManager.PlayerObjects[id];

        if (!_assignedPlayerObject)
        {
            Debug.LogWarning($"A player object was not assigned for Id: <color=cyan>{id}</color>");
            return;
        }

        _name.Value = _assignedPlayerObject.Name.Value;

        UpdatePlayerName("", _name.Value, false); //Updates the player name incase the initialization happens before OnEnable for some reason
    }

    private void UpdatePlayerName(string oldName, string newName, bool asServer)
    {
        _nameText.text = newName;
    }
}
