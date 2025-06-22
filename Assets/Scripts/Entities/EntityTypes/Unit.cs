using UnityEngine;

public class Unit : Entity
{
    public EntityMovementHandler MovementHandler { get; private set; }

    protected override void OnManpowerDepleted(Entity source = null)
    {
        base.OnManpowerDepleted(source);
        Destroy(transform.root.gameObject);
    }

    private void Awake()
    {
        MovementHandler = GetComponentInChildren<EntityMovementHandler>();
        if (!MovementHandler) Debug.LogWarning($"Unit component exists without a <color=cyan>MovementHandler</color> on <color=cyan>{transform.root.name}</color>!\n");
    }
}
