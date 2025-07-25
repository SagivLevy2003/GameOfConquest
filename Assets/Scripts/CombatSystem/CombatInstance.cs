using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public class CombatInstance
{
    [field: SerializeField] public Entity Attacker { get; private set; }
    [field: SerializeField] public Entity Defender { get; private set; }

    public event Action<CombatInstance> OnCombatEnd;


    public CombatInstance(Entity attacker, Entity defender)
    {
        Attacker = attacker;
        Defender = defender;

        Attacker.OnManpowerDepletion.AddListener((_) => EndCombatInstance());
        Defender.OnManpowerDepletion.AddListener((_) => EndCombatInstance());
    }

    public void TickCombat() //Calculates the damage each party in combat should deal, and deals them at the same time
    {
        float distance = Vector2.Distance(Attacker.transform.position, Defender.transform.position);

        if (Attacker.CombatHandler.AttackRange < distance && Defender.CombatHandler.AttackRange < distance) EndCombatInstance();

        if (Attacker.CombatHandler.AttackRange >= distance) ApplyDamage(Attacker, Defender);
        if (Defender.CombatHandler.AttackRange >= distance) ApplyDamage(Defender, Attacker);
    }

    public void EndCombatInstance() //End combat immidiately
    {
        Attacker.OnManpowerDepletion.RemoveListener((_) => EndCombatInstance());
        Defender.OnManpowerDepletion.RemoveListener((_) => EndCombatInstance());
        OnCombatEnd?.Invoke(this);
    }

    private void ApplyDamage(Entity attackingEntity, Entity targetEntity)
    {
        int power = attackingEntity.CombatHandler.AttackPower.Value;
        AttackType type = attackingEntity.CombatHandler.AttackType;

        targetEntity.CombatHandler.ApplyDamage(new() { AttackPower = power, Type = type, Source = attackingEntity});
    }
}