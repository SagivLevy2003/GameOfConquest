using UnityEngine;
using UnityEngine.Events;

using FishNet.Object;

public class Entity : NetworkBehaviour, ITargetable, ISelectable
{    
    [field: SerializeField] public PlayerObject OwnerPlayerObject { get; private set; }
    [field: SerializeField] public int Manpower { get; private set; } = 10;
    [field: SerializeField] public CombatHandler CombatHandler { get; private set; }


    public UnityEvent OnEntitySelect = new();
    public UnityEvent OnEntityDeselect = new();
    public UnityEvent<Entity> OnManpowerDepletion = new();

    [SerializeField] private bool _logActions;

    public void OnDeselect()
    {
        OnEntityDeselect?.Invoke();
    }

    public void OnSelect()
    {
        OnEntitySelect?.Invoke();
    }

    public void SetOwner(PlayerObject newOwner)
    {
        if (_logActions) Debug.Log($"<color=cyan>{transform.root.gameObject.name}</color> has switched owners from <color=cyan>{OwnerPlayerObject}</color> to: <color=cyan>{newOwner}</color>.");

        OwnerPlayerObject = newOwner;
    }

    public int RemoveManpower(int mp, Entity source = null) //Directly reduces manpower, skipping combat calculations
    {
        Manpower -= mp;

        if (Manpower <= 0)
        {
            OnManpowerDepleted(source);
        }
        
        return Manpower;
    }

    protected virtual void OnManpowerDepleted(Entity source = null) //Handles manpower delpetion (death) scenario. specific behaviour is implemented by inherited classes.
    {
        Manpower = 0;
        OnManpowerDepletion?.Invoke(source);
    }

    public int AddManpower(int mp)
    {
        Manpower += mp;
        return Manpower;
    }
}