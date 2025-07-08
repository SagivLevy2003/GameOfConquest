﻿using FishNet.Object;
using UnityEngine;

public class MoveCommand : BaseCommand //perhaps try to overhaul the command logic such that a command is in charge of executing it's exit logic rather than the Queue
{
    private readonly Vector2 _movePos;
    private readonly Unit _unit;


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

    public override bool IsContextValid(CommandContext context)
    {
        NetworkObject subjectNetObj = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.SubjectId);
        if (!subjectNetObj) return false;

        Unit unit = subjectNetObj.GetComponentInChildren<Unit>();
        if (!unit) return false;

        if (!unit.MovementHandler.CanReachPosition(context.Position)) return false;

        return true;
    }
}

