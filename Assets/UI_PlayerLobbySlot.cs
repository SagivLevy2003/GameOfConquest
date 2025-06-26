using FishNet;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class UI_PlayerLobbySlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private PlayerObject _assignedPlayerObject;

    public void Initialize(int id)
    {
        _assignedPlayerObject = PlayerObject.GetPlayerObject(id);

        if (!_assignedPlayerObject)
        {
            Debug.LogWarning($"Attempted to initialize a player lobby slot for a player who lacks an object. Id: {id}");
            return;
        }

        _name.text = _assignedPlayerObject.name;
    }
}
