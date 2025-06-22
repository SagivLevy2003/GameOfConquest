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
        int attackerPower = Attacker.CombatHandler.AttackPower.Value;
        AttackType attackerType = Attacker.CombatHandler.AttackType;

        int defenderPower = Attacker.CombatHandler.AttackPower.Value;
        AttackType defenderType = Attacker.CombatHandler.AttackType;

        Defender.CombatHandler.ApplyDamage(new() { AttackPower = attackerPower, Type = attackerType, Source = Attacker });
        Attacker.CombatHandler.ApplyDamage(new() { AttackPower = defenderPower, Type = defenderType, Source = Defender });
    }

    public void EndCombatInstance() //Called only when trying to end combat by moving one of the involved parties
    {
        Attacker.OnManpowerDepletion.RemoveListener((_) => EndCombatInstance());
        Defender.OnManpowerDepletion.RemoveListener((_) => EndCombatInstance());
        OnCombatEnd?.Invoke(this);
    }}