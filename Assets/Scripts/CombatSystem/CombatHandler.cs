using FishNet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatHandler : MonoBehaviour //A generic class that inherits from interfaces to define it's behaviour
{
    [field: SerializeField] public Entity OwnerEntity { get; private set; }
    [field: SerializeField] public Stat AttackPower { get; private set; }
    [field: SerializeField] public AttackType AttackType { get; private set; }

    public List<UnitResistanceModifier> ResistanceModifiers = new();

    private void Awake()
    {
        if (!InstanceFinder.IsServerStarted) //Disable on clients
        {
            enabled = false;
            return;
        }

        OwnerEntity = transform.root.GetComponentInChildren<Entity>();
    }

    public void ApplyDamage(AttackArgs args)
    {
        int attackPower = args.AttackPower;
        float resistanceMulti = 1;

        List<float> multis = ResistanceModifiers
            .Where(mod => mod.Type == args.Type || mod.Type == AttackType.Generic) //Filter to only the multis relevant to the specific AttackArgs
            .Select(mod => mod.DamageMultiplier).ToList(); //Select the damage multipliers

        foreach (var mod in multis) resistanceMulti *= mod; //Calculate the total multiplier

        attackPower = (int)Math.Ceiling(attackPower * resistanceMulti); //Multiplies the attack power by the multiplier and rounds it up

        OwnerEntity.RemoveManpower(attackPower, args.Source); //Reduces the manpower from the entity
    }
}



