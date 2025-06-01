using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityMovementHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private NavMeshAgent _navAgent;

    [SerializeField] private Transform _movementTarget = null;

    public UnityEvent OnTargetReached = new();

    [Header("Debug")]
    [SerializeField] private bool _displayNavMeshTarget = false;
    [SerializeField] private bool _logActions = false;

    private void Awake()
    {
        if (_rigidbody == null) _rigidbody = transform.root.GetComponentInChildren<Rigidbody2D> ();
        if (_rigidbody == null)
        {
            Debug.LogWarning($"EntityMovementHandler component exists without a <color=cyan>Rigidbody2D</color> on <color=cyan>{transform.root.name}</color>!\n" +
                $"Disabling component.");
            enabled = false;
        }

        if (_navAgent == null) _navAgent = transform.root.GetComponentInChildren<NavMeshAgent>();
        if (_navAgent == null)
        {
            Debug.LogWarning($"EntityMovementHandler component exists without a <color=cyan>NavMeshAgent</color> on <color=cyan>{transform.root.name}</color>!\n" +
                $"Disabling component.");
            enabled = false;
        }

        //NavMeshAgent setup
        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
        _navAgent.isStopped = true;
        //
    }

    private void FixedUpdate()
    {
        if (_movementTarget)
        {
            _navAgent.destination = _movementTarget.position;
        }

        if (ReachedPosition() && !_navAgent.isStopped)
        {
            _navAgent.isStopped = true;
            if (_logActions) Debug.Log($"<color=cyan>{transform.root.gameObject.name}</color> has reached it's target.");
            OnTargetReached?.Invoke();
        }
    }

    public void MoveToPosition(Vector2 position)
    {
        _movementTarget = null;
        _navAgent.destination = position;
        _navAgent.isStopped = false;
        _navAgent.stoppingDistance = 0;

        if (_logActions) Debug.Log($"<color=cyan>{transform.root.gameObject.name}</color> is moving towards <color=cyan>{position}</color>.");
    }

    public void MoveToEntity(GameObject entity)
    {
        _movementTarget = entity.transform; 
        _navAgent.isStopped = false;
        _navAgent.stoppingDistance = StoppingDistanceForTarget(entity.transform);

        if (_logActions) Debug.Log($"<color=cyan>{transform.root.gameObject.name}</color> is moving towards <color=cyan>{entity.name}</color>." +
            $"The current position is: <color=cyan>{(Vector2)entity.transform.position}</color>");
    }

    private void OnDrawGizmos()
    {
        if (_displayNavMeshTarget)
        {
            if (_navAgent.destination != null) Gizmos.DrawWireSphere(_navAgent.destination, 1);
        }
    }

    private bool ReachedPosition()
    {
        if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
        {
            if (!_navAgent.hasPath || _navAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }

    float StoppingDistanceForTarget(Transform target)
    {
        Collider2D targetCollider = target.GetComponentInChildren<Collider2D>();

        if (targetCollider == null)
        {
            Debug.LogWarning("Attempted to calculate stopping distance from target without a collider");
            return 0;
        }

        float targetSize = targetCollider.bounds.extents.magnitude;
        return targetSize + 0.3f;
    }
}
