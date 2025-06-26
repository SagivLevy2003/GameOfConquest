using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UI_LobbyManager : MonoBehaviour
{
    public Dictionary<int, GameObject> SlotDictionary = new();

    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _playerListContainer;

    private void Start()
    {
        if (_slotPrefab == null || _playerListContainer == null)
        {
            Debug.LogWarning("Lobby UI references missing. Disabling component.");
            enabled = false;
            return;
        }

        Clear();
    }

    private void OnEnable()
    {
        StartCoroutine(SubscribeToConnectionEventDelayed());
    }

    private void OnDisable()
    {
        NetworkSystemManager.Instance.OnPlayerConnectionStateChange.RemoveListener(OnPlayerConnectionStateChanged);
    }

    void OnPlayerConnectionStateChanged(int id, bool connected)
    {
        if (connected)
        {
            TryAddSlot(id);
        }
        else
        {
            TryDeleteSlot(id);
        }
    }

    void TryAddSlot(int id)
    {
        if (SlotDictionary.ContainsKey(id))
        {
            Debug.LogWarning($"Attempted to create duplicate player slots in Lobby UI for id: <color=cyan>{id}</color>");
            return;
        }

        GameObject slot = Instantiate(_slotPrefab, _playerListContainer);
        slot.GetComponent<UI_PlayerLobbySlot>().Initialize(id);

        SlotDictionary.Add(id, slot);
    }

    void TryDeleteSlot(int id)
    {
        if (!SlotDictionary.ContainsKey(id))
        {
            Debug.LogWarning($"Attempted delete a non-existing slot in Lobby UI for id: <color=cyan>{id}</color>");
            return;
        }

        Destroy(SlotDictionary[id]);
        SlotDictionary.Remove(id);
    }

    void Clear()
    {
        List<GameObject> slots = SlotDictionary.Select(pair => pair.Value).ToList();

        foreach (var slot in slots)
        {
            Destroy(slot);
        }

        SlotDictionary.Clear();
    }

    IEnumerator SubscribeToConnectionEventDelayed() //Delays subscribing to the event because some times the NetworkSystemManager singleton isn't initialized at OnEnable()
    {
        yield return null;
        NetworkSystemManager.Instance.OnPlayerConnectionStateChange.AddListener(OnPlayerConnectionStateChanged);
    }
}