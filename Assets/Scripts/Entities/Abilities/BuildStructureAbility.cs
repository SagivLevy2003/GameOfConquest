using FishNet;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "Abilities/Build Structure")]
public class BuildStructureAbility : EntityAbility
{
    [SerializeField] private GameObject _structurePrefab;

    protected override void ExecuteAbilityLogic(EntityAbilityHandler abilityHandler)
    {
        EntitySpawnFactory.SpawnEntityAtPosition(_structurePrefab, abilityHandler.OwnerId, abilityHandler.transform.position);
        InstanceFinder.ServerManager.Despawn(abilityHandler.transform.root.gameObject);
    }

    public override bool IsValid(EntityAbilityHandler abilityHandler)
    {
        if (_structurePrefab == null)
        {
            Debug.LogWarning($"A structure prefab wasn't assigned to <color=cyan>{name}</color> build structure ability.");
            return false;
        }

        if (!_structurePrefab.TryGetComponent<BoxCollider2D>(out var structureCollider)) //Return if no collider was assigned to the structure
        {
            Debug.LogWarning($"The prefab assigned to <color=cyan>{name}</color> build structure ability does not contain a box collider.");
            return false;
        }

        Vector2 worldSize = Vector2.Scale(structureCollider.size, _structurePrefab.transform.lossyScale); //Calculates the final size, with relation to world-size as well
        Entity[] blockingEntities = HelperMethods.GetEntitiesInArea(abilityHandler.transform.position, worldSize);
        Entity currentEntity = abilityHandler.GetComponent<Entity>();

        foreach (var blockingEntity in blockingEntities)
        {
            if (blockingEntity != currentEntity) return false;
        }

        return true;
    }
}