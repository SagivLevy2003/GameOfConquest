using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectManager
{
    public Dictionary<int, PlayerObject> PlayerObjects { get; private set; } = new();

    public event Action<int> OnPlayerObjectSpawned;
    public event Action<int> OnPlayerObjectDespawned;

    public void AddObject(PlayerObject obj)
    {
        int id = obj.Id.Value;

        if (PlayerObjects.ContainsKey(id))
        {
            Debug.LogWarning($"Player ID {id} already exists in PlayerObjectManager.");
            return;
        }

        PlayerObjects.Add(id, obj);
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