using UnityEngine;
using UnityEngine.Events;
using FishNet.Object;
using FishNet;
using FishNet.Connection;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using System;
using UnityEditor;

public class Entity : NetworkBehaviour, ITargetable, ISelectable
{
    [Header("Properties")]
    [field: SerializeField] public List<GameObject> EntitySelectionOptionPrefabs { get; private set; } = new();

    [Header("Debug Information")]
    [field:SerializeField, ReadOnly] public PlayerObject OwnerPlayerObject { get; private set; }
    [field: SerializeField, ReadOnly] public int CurrentManpower { get; private set; }
    [SerializeField] private bool _logActions;

    [Header("References")]
    public CombatHandler CombatHandler { get; private set; }


    [Header("Event Callbacks")]
    public UnityEvent OnEntitySelect = new();
    public UnityEvent OnEntityDeselect = new();
    public UnityEvent<Entity> OnManpowerDepletion = new();


    //Unserialized
    private readonly SyncVar<int> _manpowerSync = new(10);

    private void Start()
    {
        _manpowerSync.OnChange += (_, newVal, __) => CurrentManpower = newVal;
    }

    public void OnDeselect()
    {
        OnEntityDeselect?.Invoke();
    }

    public void OnSelect()
    {
        OnEntitySelect?.Invoke();
    }

    public void SetOwner(PlayerObject newOwner)
    {
        if (!InstanceFinder.IsServerStarted) //Return if isn't on server
        {
            Debug.LogWarning($"Attempted to call SetOwner on client for: <color=cyan>{gameObject.name}</color>");
            return;
        }

        if (_logActions) Debug.Log($"<color=cyan>{transform.root.gameObject.name}</color> has switched owners from <color=cyan>{OwnerPlayerObject}</color> to: <color=cyan>{newOwner}</color>.");
        
        if (!InstanceFinder.ServerManager.Clients.TryGetValue(newOwner.Id.Value, out NetworkConnection ownerConnection))
        {
            Debug.LogWarning($"No NetworkConnection object with the ID: [<color=cyan>{newOwner.Id.Value}</color>]");
        }

        GetComponent<NetworkObject>().GiveOwnership(ownerConnection);
        OwnerPlayerObject = newOwner;
    }

    public int RemoveManpower(int mp, Entity source = null) //Directly reduces manpower, skipping combat calculations
    {
        if (!InstanceFinder.IsServerStarted) //Return if isn't on server
        {
            Debug.LogWarning($"Attempted to call ReduceManpower on client for: <color=cyan>{gameObject.name}</color>");
            return _manpowerSync.Value;
        }

        _manpowerSync.Value -= mp;

        if (_manpowerSync.Value <= 0)
        {
            _manpowerSync.Value = 0;
            OnManpowerDepleted(source);
        }
        
        return _manpowerSync.Value;
    }

    protected virtual void OnManpowerDepleted(Entity source = null) //Handles manpower depletion (death) scenario. specific behaviour is implemented by inherited classes.
    {
        _manpowerSync.Value = 0;
        OnManpowerDepletion?.Invoke(source);
    }

    public int AddManpower(int mp)
    {
        if (!InstanceFinder.IsServerStarted) //Return if isn't on server
        {
            Debug.LogWarning($"Attempted to call AddManpower on client for: <color=cyan>{gameObject.name}</color>");
            return _manpowerSync.Value;
        }

        _manpowerSync.Value += mp;
        return _manpowerSync.Value;
    }
}
