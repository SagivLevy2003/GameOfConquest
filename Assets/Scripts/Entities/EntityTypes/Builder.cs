using UnityEngine;

public class Builder : Unit
{
    [SerializeField] private GameObject structurePrefab;

    public bool TryBuildOnPosition()
    {
        if (structurePrefab == null)
        {
            Debug.LogWarning($"A structure prefab wasn't assigned to <color=cyan>{gameObject.name}</color>");
            return false;
        }

        if (!structurePrefab.TryGetComponent<Collider2D>(out var structureCollider)) //Return if no collider was assigned to the structure
        {
            Debug.LogWarning($"The prefab assigned to <color=cyan>{gameObject.name}</color> does not include a collider!");
            return false;
        }

        if (IsAreaOccupied(structureCollider.bounds.size)) return false; //Return if the area is occupied by another collider


        return true;
    }

    private bool IsAreaOccupied(Vector2 size)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            origin: transform.position,
            direction: Vector2.zero,
            size: size,
            angle: 0f
            );

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != gameObject) return true;
        }

        return false;
    }
}