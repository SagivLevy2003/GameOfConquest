using FishNet;
using FishNet.Object;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom UI/Entity Ability Section")]
public class UI_EntityAbilityOptionSection : UI_EntityOptionSectionBase 
{
    [SerializeField] private GameObject _abilityOptionPrefab;

    public override void GenerateSection(NetworkObject selectedObject, UI_EntityOptionsManager manager)
    {
        if (_abilityOptionPrefab == null)
        {
            Debug.LogWarning($"No ability option prefab was assigned on the ability option section!");
            return;
        }

        if (selectedObject == null) return;

        if (!selectedObject.TryGetComponent(out EntityAbilityHandler abilityHandler)) return;

        foreach (var ability in abilityHandler.Abilities)
        {
            GenerateSlot(ability, abilityHandler, manager);
        }
    }

    private void GenerateSlot(EntityAbility ability, EntityAbilityHandler entity, UI_EntityOptionsManager manager)
    {  
        if (ability == null) return;

        GameObject slotObject = manager.CreateSlot(_abilityOptionPrefab);

        if (!slotObject.TryGetComponent(out UI_AbilitySlot slot)) return;

        slot.InitializeSlot(ability, entity);
    }
}