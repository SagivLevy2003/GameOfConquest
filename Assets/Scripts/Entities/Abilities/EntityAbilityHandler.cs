using FishNet.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityAbilityHandler : NetworkBehaviour
{
    public List<EntityAbility> Abilities = new();

    private Entity _entity;

    private void Start()
    {
        _entity = GetComponent<Entity>();
    }

    public bool TryExecuteAbility(EntityAbility ability)
    {
        if (!Abilities.Contains(ability)) 
        {
            Debug.LogWarning($"Attempted to execute an ability that isn't assigned in the handler. [Ability: {ability.DisplayName}, Entity: {_entity}]");
            return false;
        }

        if (!ability.IsValid(this)) return false;

        TryExecuteAbilityOnServer(ability.DisplayName);

        return true;
    }

    [ServerRpc(RequireOwnership = true)]
    private void TryExecuteAbilityOnServer(string abilityName)
    {
        EntityAbility ability = Abilities.FirstOrDefault(ability => ability.DisplayName == abilityName);

        if (!ability)
        {
            Debug.LogWarning($"<color=cyan>{gameObject.name}</color> received a ServerRPC to execute an ability that doesn't exist: <color=cyan>{abilityName}</color>");
            return;
        }

        if (!ability.IsValid(this))
        {
            Debug.LogWarning($"<color=cyan>{gameObject.name}</color> received a ServerRPC to execute an ability that is invalid on server: <color=cyan>{abilityName}</color>");
            return;
        }

        ability.ExecuteAbility(this);
    }
}