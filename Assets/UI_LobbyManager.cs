using FishNet.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_LobbyManager : NetworkBehaviour //Server-sided lobby status manager
{
    public Dictionary<int, NetworkObject> SlotDictionary = new();

    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _playerListContainer;

    private bool _isServer = false;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        _isServer = IsServerInitialized;

        Clear();

        if (!IsServerInitialized) return; //All code below is server-side

        //Event subscription
        NetworkSystemManager.Instance.PlayerObjectManager.OnPlayerObjectSpawned += TryAddSlot;
        NetworkSystemManager.Instance.PlayerObjectManager.OnPlayerObjectDespawned += TryRemoveSlot;
        //
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        NetworkSystemManager.Instance.PlayerObjectManager.OnPlayerObjectSpawned -= TryAddSlot;
        NetworkSystemManager.Instance.PlayerObjectManager.OnPlayerObjectDespawned -= TryRemoveSlot;

        Clear();
    }

    private void OnDisable()
    {
        NetworkSystemManager.Instance.PlayerObjectManager.OnPlayerObjectSpawned -= TryAddSlot;
        NetworkSystemManager.Instance.PlayerObjectManager.OnPlayerObjectDespawned -= TryRemoveSlot;
    }

    private void Start()
    {
        if (!_isServer) return; //Return if running on client

        if (_slotPrefab == null || _playerListContainer == null) //Return if the assignments are null
        {
            Debug.LogWarning("Lobby UI references missing. Disabling component.");
            enabled = false;
            return;
        }
    }

    void TryAddSlot(int id)
    {
        if (!_isServer) return; //Return if running on client

        if (SlotDictionary.ContainsKey(id)) //Return if duplicate player slot
        {
            Debug.LogWarning($"Attempted to create duplicate player slots in Lobby UI for id: <color=cyan>{id}</color>");
            return;
        }

        GameObject slot = Instantiate(_slotPrefab, _playerListContainer);
        
        if (!slot.TryGetComponent<NetworkObject>(out var netObj))  //Checks if the object contains a network object
        {
            Debug.LogWarning($"Slot prefab does not contain a network object component!");
            return;
        }

        Spawn(netObj);
        slot.GetComponent<UI_PlayerLobbySlot>().Initialize(id);

        SlotDictionary.Add(id, netObj);
    }

    void TryRemoveSlot(int id)
    {
        if (!_isServer) return; //Return if running on client

        if (!SlotDictionary.ContainsKey(id))
        {
            Debug.LogWarning($"Attempted delete a non-existing slot in Lobby UI for id: <color=cyan>{id}</color>");
            return;
        }

        Despawn(SlotDictionary[id]);
        SlotDictionary.Remove(id);
    }

    void Clear()
    {
        if (!_isServer) return; //Return if running on client

        List<NetworkObject> slots = SlotDictionary.Select(pair => pair.Value).ToList();

        foreach (var slot in slots)
        {
            Despawn(slot);
        }

        SlotDictionary.Clear();
    }
}
