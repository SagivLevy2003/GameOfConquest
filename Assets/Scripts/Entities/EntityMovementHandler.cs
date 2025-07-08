using FishNet;
using FishNet.Managing.Server;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public class EntityMovementHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private NavMeshAgent _navAgent;

    [SerializeField] private Transform _movementTarget = null;
    private float _targetRadius;
    private float _selfRadius;

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


        if (!InstanceFinder.NetworkManager.IsServerStarted)
        {
            _navAgent.enabled = false; //Disable the navmesh agent on the clients;
            enabled = false; //Disable the movement handler on clients
            return;
        }

        _selfRadius = CalculateRadius(gameObject);

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
            _navAgent.destination = GetDestinationFromTarget();
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
        _targetRadius = CalculateRadius(entity);

        _navAgent.isStopped = false;
        _navAgent.stoppingDistance = 0;

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
        if (_navAgent.pathPending) return false; //If a path hasn't been calculated yet return false

        if (_navAgent.remainingDistance > _navAgent.stoppingDistance) return false; //If the distance remaining is larger than the stopping distance return false

        if (!_navAgent.hasPath || _navAgent.velocity.sqrMagnitude == 0f) //
        {
            return true;
        }

        return false;
    }

    private Vector2 GetDestinationFromTarget() //Calculates the destination based on the target's size
    {
        float offsetDistance = _targetRadius + _selfRadius + _navAgent.stoppingDistance; //How far from the center the agent should go

        Vector2 offsetDirection = (_movementTarget.position - transform.position).normalized; 
        
        Vector2 offsetVector = offsetDistance * offsetDirection; 

        return (Vector2)_movementTarget.position - offsetVector;
    }
    
    public bool CanReachPosition(Vector2 position) //Returns whether a point is inside the navmesh area
    {
        return NavMesh.SamplePosition((Vector3)position, out _, 0.1f, NavMesh.AllAreas);
    }

    private float CalculateRadius(GameObject obj)
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
