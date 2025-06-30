using System;
using UnityEngine;

public class AttackUnitCommand : BaseCommand
{
    readonly Army _attacker;
    readonly Unit _targetUnit;
    CombatInstance _combatInstance;

    public override string Name => "Attack Unit";

    public AttackUnitCommand(GameObject subject, GameObject target)
    {
        if (subject) _attacker = subject.GetComponent<Army>();
        if (target) _targetUnit = target.GetComponent<Unit>();
    }

    public override void Execute()
    {
        Debug.Log($"executing command with id {Id}");
        _attacker.MovementHandler.MoveToEntity(_targetUnit.gameObject);

        _attacker.MovementHandler.OnTargetReached.AddListener(InitiateAttack);
    }

    public void InitiateAttack()
    {
        _combatInstance = CombatManager.Instance.InitiateCombat(_attacker, _targetUnit);
        Debug.Log($"attacking, {Id}");
    }

    public override bool IsValidForInput()
    {
        if (_attacker == null || _targetUnit == null) return false; //Returns false if subject/target unit script is null

        if (_attacker.OwnerPlayerObject.Id == _targetUnit.OwnerPlayerObject.Id) return false; //Returns false if the attacker and target are owned by the same player

        return true;
    }

    public override void CommandExecutionFinished(bool isForcedExit)
    {
        base.CommandExecutionFinished(isForcedExit);
        _attacker.MovementHandler.OnTargetReached.RemoveListener(InitiateAttack);
        _combatInstance?.EndCombatInstance();
    }
}