using System.Linq;
using UnityEngine;

public static class HelperMethods
{
    public static float CalculateRadius(GameObject obj)
    {
        Collider2D collider = obj.transform.root.GetComponentInChildren<Collider2D>();

        if (collider == null)
        {
            Debug.LogWarning($"Attempted to calculate radius of a target without a collider: {obj.transform.root.name}");
            return 0;
        }

        return collider.bounds.extents.magnitude;
    }

    public static Entity[] GetEntitiesInArea(Vector2 position, Vector2 size)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            point: position,   
            size: size,        
            angle: 0f          
        );

        return hits
            .Select(collider => collider.GetComponent<Entity>())
            .Where(entity => entity != null)
            .ToArray();
    }
}