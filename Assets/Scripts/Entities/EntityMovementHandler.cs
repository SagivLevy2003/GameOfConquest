using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EntityMovementHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private NavMeshAgent _navAgent;

    [SerializeField] private Transform _movementTarget = null;

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
        //
    }

    private void FixedUpdate()
    {
        if (_movementTarget)
        {
            _navAgent.destination = _movementTarget.position;
        }
    }

    public void MoveToPosition(Vector2 position)
    {
        _movementTarget = null;
        _navAgent.destination = position;
        if (_logActions) Debug.Log($"<color=cyan>{transform.root.gameObject.name}</color> is moving towards <color=cyan>{position}</color>.");
    }

    public void MoveToEntity(GameObject entity)
    {
        _movementTarget = entity.transform;
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
}
