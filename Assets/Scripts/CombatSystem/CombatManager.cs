using FishNet;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
	[SerializeField] private List<CombatInstance> _combatInstances = new();
	[SerializeField] private float _combatTickRate = 1; //How many seconds between each combat tick
    [SerializeField] private bool _logActions = false;

    private void Start()
    {
        if (!InstanceFinder.IsServerStarted) //Disable on clients
        {
            enabled = false;
            return;
        }

        StartCoroutine(TickCombatInstances());
    }

    IEnumerator TickCombatInstances()
    {
        List<CombatInstance> temp = _combatInstances.ToList();

        foreach (var instance in temp)
        {
            instance.TickCombat();
        }

        yield return new WaitForSeconds(_combatTickRate);

        StartCoroutine(TickCombatInstances());
    }

    public CombatInstance InitiateCombat(Entity attacker, Entity defender) //Called to initiate combat between two entites, returns the combat instance 
	{
        if (_logActions) Debug.Log($"Initiating combat between: <color=cyan>{attacker.transform.root.name}</color> and <color=cyan>{defender.transform.root.name}</color>");
        CombatInstance combat = new(attacker, defender);
        _combatInstances.Add(combat);

        combat.OnCombatEnd += CombatEnded;

        return combat;
	}

    private void CombatEnded(CombatInstance instance) //Gets called through an event callback when a combat instance ends
    {
        if (_logActions) Debug.Log($"Combat ended between: <color=cyan>{instance.Attacker.transform.root.name}</color> and <color=cyan>{instance.Defender.transform.root.name}</color>");
        _combatInstances.Remove(instance);
    }
}