using FishNet;
using UnityEngine;

public abstract class EntityAbility : ScriptableObject //SYNC TIMES ARE A THING, USE IT WHEN IMPLEMENTING COOLDOWNS
{
    [field: SerializeField] public string DisplayName { get; set; }

    public void ExecuteAbility(EntityAbilityHandler abilityHandler) //Shared execution logic, regardless of the ability's implementation
    {
        if (!InstanceFinder.IsServerStarted) return;
        
        if (abilityHandler == null)
        {
            Debug.LogWarning($"Attempted to execute an ability with a null ability handler: <color=cyan>{DisplayName}</cyan>");
            return;
        }

        ExecuteAbilityLogic(abilityHandler);
    }

    protected abstract void ExecuteAbilityLogic(EntityAbilityHandler abilityHandler);
    public abstract bool IsValid(EntityAbilityHandler abilityHandler);
}
