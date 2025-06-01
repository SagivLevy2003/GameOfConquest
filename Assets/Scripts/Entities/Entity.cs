using System.Buffers;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour, ITargetable, ISelectable
{    
    [field: SerializeField] public Player Owner { get; private set; }
    [SerializeField] protected float _currentManpower = 10;

    public UnityEvent OnEntitySelect = new();
    public UnityEvent OnEntityDeselect = new();

    private void Awake()
    {

    }

    public void OnDeselect()
    {
        OnEntityDeselect?.Invoke();
    }

    public void OnSelect()
    {
        OnEntitySelect?.Invoke();
    }
}