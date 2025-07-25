using FishNet;
using FishNet.Managing.Server;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public class EntityMovementHandler : MonoBehaviour
{
    [SerializeField] private FloatStat _moveSpeed;

    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private NavMeshAgent _navAgent;

    [Header("Events")]
    public UnityEvent OnTargetReached = new();

    [Header("Debug")]
    [SerializeField] private bool _displayNavMeshTarget = false;
    [SerializeField] private bool _logActions = false;
    [SerializeField, ReadOnly] private Transform _movementTarget = null;

    //private unserialized
    private float _targetRadius;
    private float _selfRadius;

    private void Awake()
    {
        SetReferences();

        if (!InstanceFinder.NetworkManager.IsServerStarted)
        {
            _navAgent.enabled = false; //Disable the navmesh agent on the clients;
            enabled = false; //Disable the movement handler on clients
            return;
        }

        _selfRadius = HelperMethods.CalculateRadius(gameObject);

        //NavMeshAgent setup
        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
        _navAgent.isStopped = true;
        //

        Debug.Log($"movespeed: {_moveSpeed.Value}");
        _navAgent.speed = _moveSpeed.Value;
    }

    private void OnMoveSpeedChanged(float _, float currSpeed)
    {
        _navAgent.speed = currSpeed;
    }

    private void FixedUpdate()
    {
        if (!_movementTarget) return;

        _navAgent.destination = GetDestinationFromTarget();

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
        _targetRadius = HelperMethods.CalculateRadius(entity);

        _navAgent.isStopped = false;
        _navAgent.stoppingDistance = 0;

        if (_logActions) Debug.Log($"<color=cyan>{transform.root.gameObject.name}</color> is moving towards <color=cyan>{entity.name}</color>." +
            $"Heading towards the position: <color=cyan>{(Vector2)entity.transform.position}</color>");
    }

    private bool ReachedPosition()
    {
        //if (_logActions) Debug.Log($"pathPending: {_navAgent.pathPending}" +
        //    $" | remainingDistance: {_navAgent.remainingDistance}, stoppingDistance: {_navAgent.stoppingDistance}" +
        //    $" | hasPath: {_navAgent.hasPath}, velocity square magnitude: {_navAgent.velocity.sqrMagnitude}");

        if (_navAgent.pathPending) return false; //If a path hasn't been calculated yet return false

        if (_navAgent.remainingDistance > _navAgent.stoppingDistance + 0.1f) return false; //If the distance remaining is larger than the stopping distance return false

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

    private bool SetReferences()
    {
        if (_rigidbody == null) _rigidbody = transform.root.GetComponentInChildren<Rigidbody2D>();

        if (_rigidbody == null)
        {
            Debug.LogWarning($"EntityMovementHandler component exists without a <color=cyan>Rigidbody2D</color> on <color=cyan>{transform.root.name}</color>!\n" +
                $"Disabling component.");
            enabled = false;
            return false;
        }

        if (_navAgent == null) _navAgent = transform.root.GetComponentInChildren<NavMeshAgent>();
        if (_navAgent == null)
        {
            Debug.LogWarning($"EntityMovementHandler component exists without a <color=cyan>NavMeshAgent</color> on <color=cyan>{transform.root.name}</color>!\n" +
                $"Disabling component.");
            enabled = false;
            return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        if (_displayNavMeshTarget)
        {
            if (_navAgent.destination != null) Gizmos.DrawWireSphere(_navAgent.destination, 1);
        }
    }

    private void OnEnable()
    {
        _moveSpeed.OnValueChanged.AddListener(OnMoveSpeedChanged);
    }

    private void OnDisable()
    {
        _moveSpeed.OnValueChanged.RemoveListener(OnMoveSpeedChanged);
    }
}
