using System;
using UnityEngine;

public class AttackStructureCommand : ICommand
{
    Unit attacker;
    Structure targetStructure;

    string ICommand.Name => "Attack Structure";

    public event Action OnExecutionFinished;

    public AttackStructureCommand(GameObject subject, GameObject target)
    {
        if (subject) attacker = subject.GetComponentInChildren<Unit>();
        if (target) targetStructure = target.GetComponentInChildren<Structure>();
    }

    public void Execute()
    {
        attacker.MovementHandler.MoveToPosition(targetStructure.transform.position);
    }

    public bool IsValidForInput()
    {
        if (attacker == null || targetStructure == null) return false; //Return false if entity scripts are missing

        if (attacker.Type != EntityType.Unit && targetStructure.Type != EntityType.Structure) return false; //Returns false if the attacker isn't a unit or target isn't a structure

        if (attacker.Owner.Id == targetStructure.Owner.Id) return false; //Returns false if the attacker and target are owned by the same player

        return true;
    }
}
