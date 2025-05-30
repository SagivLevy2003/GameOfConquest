using System.Buffers;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour, ITargetable, ISelectable
{
    [HideInInspector] public virtual EntityType Type => EntityType.Unit;
    
    [field: SerializeField] public Player Owner { get; private set; }

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

public enum EntityType
{
    Structure,
    Unit
}