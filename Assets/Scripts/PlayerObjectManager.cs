using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerObjectManager
{
    public Dictionary<int, PlayerObject> PlayerObjects { get; private set; } = new();

    public event Action<int> OnPlayerObjectSpawned;
    public event Action<int> OnPlayerObjectDespawned;

    [SerializeField] Transform PlayerObjectContainer;

    public void AddObject(PlayerObject playerObj)
    {
        int id = playerObj.Id.Value;

        if (PlayerObjects.ContainsKey(id))
        {
            Debug.LogWarning($"Player ID {id} already exists in PlayerObjectManager.");
            return;
        }
        
        PlayerObjects.Add(id, playerObj);
        OnPlayerObjectSpawned?.Invoke(id);
    }

    public void RemoveObject(int id)
    {
        PlayerObjects.Remove(id);
        OnPlayerObjectDespawned?.Invoke(id);
    }

    public void Reset()
    {
        PlayerObjects = new();
    }
}