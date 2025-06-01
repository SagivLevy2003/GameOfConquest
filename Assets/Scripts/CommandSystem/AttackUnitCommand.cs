using System;
using UnityEngine;

public class AttackUnitCommand : ICommand
{
    Army _attacker;
    Unit _targetUnit;

    string ICommand.Name => "Attack Unit";

    public event Action OnExecutionFinished;
      
    public AttackUnitCommand(GameObject subject, GameObject target)
    {
        if (subject) _attacker = subject.GetComponent<Army>();
        if (target) _targetUnit = target.GetComponent<Unit>();
    }

    public void Execute()
    {
        _attacker.MovementHandler.MoveToEntity(_targetUnit.gameObject);

        _attacker.MovementHandler.OnTargetReached.RemoveListener(InitiateAttack);
        _attacker.MovementHandler.OnTargetReached.AddListener(InitiateAttack);
    }

    public void InitiateAttack()
    {
        _attacker.AttackHandler.TryAttackTarget(_targetUnit);
        Debug.Log("attack");
    }

    public bool IsValidForInput()
    {
        if (_attacker == null || _targetUnit == null) return false; //Returns false if subject/target unit script is null

        if (_attacker.Owner.Id == _targetUnit.Owner.Id) return false; //Returns false if the attacker and target are owned by the same player

        return true;
    }
}
