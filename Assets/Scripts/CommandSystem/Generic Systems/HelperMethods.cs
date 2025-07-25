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
}