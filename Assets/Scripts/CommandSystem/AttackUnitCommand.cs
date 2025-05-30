using System;
using UnityEngine;

public class AttackUnitCommand : ICommand
{
    Unit attacker;
    Unit targetUnit;

    string ICommand.Name => "Attack Unit";

    public event Action OnExecutionFinished;

    public AttackUnitCommand(GameObject subject, GameObject target)
    {
        if (subject) attacker = subject.GetComponent<Unit>();
        if (target) targetUnit = target.GetComponent<Unit>();
    }

    public void Execute()
    {
        attacker.MovementHandler.MoveToEntity(targetUnit.gameObject);
    }

    public bool IsValidForInput()
    {
        if (attacker == null || targetUnit == null) return false; //Returns false if subject/target unit script is null

        if (attacker.Type != EntityType.Unit && targetUnit.Type != EntityType.Unit) return false; //Returns false if the attacker or target aren't units

        if (attacker.Owner.Id == targetUnit.Owner.Id) return false; //Returns false if the attacker and target are owned by the same player

        return true;
    }
}