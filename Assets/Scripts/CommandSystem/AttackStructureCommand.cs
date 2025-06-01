using System;
using UnityEngine;

public class AttackStructureCommand : ICommand
{
    Army _attacker;
    Structure _targetStructure;

    string ICommand.Name => "Attack Structure";

    public event Action OnExecutionFinished;

    public AttackStructureCommand(GameObject subject, GameObject target)
    {
        if (subject) _attacker = subject.GetComponentInChildren<Army>();
        if (target) _targetStructure = target.GetComponentInChildren<Structure>();
    }

    public void Execute()
    {
        _attacker.MovementHandler.MoveToEntity(_targetStructure.gameObject);

        _attacker.MovementHandler.OnTargetReached.RemoveListener(InitiateAttack);
        _attacker.MovementHandler.OnTargetReached.AddListener(InitiateAttack);
    }

    public void InitiateAttack()
    {
        _attacker.AttackHandler.TryAttackTarget(_targetStructure);
        Debug.Log("attack");
    }

    public bool IsValidForInput()
    {
        if (_attacker == null || _targetStructure == null) return false; //Return false if entity scripts are missing

        if (_attacker.Owner.Id == _targetStructure.Owner.Id) return false; //Returns false if the attacker and target are owned by the same player

        return true;
    }
}
