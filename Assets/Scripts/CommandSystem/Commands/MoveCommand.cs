using FishNet.Object;
using UnityEngine;

public class MoveCommand : BaseCommand //perhaps try to overhaul the command logic such that a command is in charge of executing it's exit logic rather than the Queue
{
    private readonly Vector2 _movePos;
    private readonly Unit _unit;
    public override string Name => "Move To Position";
    public MoveCommand(NetworkObject subject, Vector2 target)
    {
        if (subject) _unit = subject.GetComponentInChildren<Unit>();
        _movePos = target;
    }

    public override void Execute()
    {
        _unit.MovementHandler.MoveToPosition(_movePos);
        _unit.MovementHandler.OnTargetReached.AddListener(() => CommandExecutionFinished(false));
    }

    public override bool IsValidForInput()
    {
        if (!_unit) return false; //null check
        return _unit.MovementHandler.CanReachPosition(_movePos); //returns whether the position can be reached
    }
}

