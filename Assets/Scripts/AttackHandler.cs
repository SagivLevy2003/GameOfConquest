using System.Collections;
using UnityEngine;

public abstract class AttackHandler : MonoBehaviour
{
    [SerializeField] private float _range = 2f;
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private bool _canFire = true;

    [field: SerializeField] public Entity Target;

    public void TryAttackTarget(Entity target)
    {
        if (!_canFire) return;

        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.transform.root.position);
        if (distance > _range) return;

        AttackTarget(target);
        _canFire = false;
        StartCoroutine(ActivateCooldown());
    }

    private IEnumerator ActivateCooldown()
    {
        yield return new WaitForSeconds(_attackRate);
        _canFire = true;
    }

    protected abstract void AttackTarget(Entity target);

    protected abstract float CalculateDamage(Entity attackingEntity);
}
