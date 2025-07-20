using FishNet.Object;
using System;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class AttackStructureCommand : BaseCommand
{
    readonly Army _attacker;
    readonly Structure _targetStructure;
    CombatInstance _combatInstance;

    public AttackStructureCommand(NetworkObject subject, NetworkObject target)
    {
        if (subject) _attacker = subject.GetComponentInChildren<Army>();
        if (target) _targetStructure = target.GetComponentInChildren<Structure>();
    }

    public override void Execute()
    {
        _attacker.MovementHandler.MoveToEntity(_targetStructure.gameObject);
        _attacker.MovementHandler.OnTargetReached.AddListener(InitiateAttack);
    }

    public void InitiateAttack()
    {
        _combatInstance = CombatManager.Instance.InitiateCombat(_attacker, _targetStructure);
        Debug.Log($"attacking, {Id}");
    }

    public override void CommandExecutionFinished(bool isForcedExit)
    {
        base.CommandExecutionFinished(isForcedExit);
        if (_attacker != null) _attacker.MovementHandler.OnTargetReached.RemoveListener(InitiateAttack);
        _combatInstance?.EndCombatInstance();
    }

    public override bool IsContextValid(CommandContext context)
    {
        if (TryParseContext(context, out NetworkObject subjectNetObj, out NetworkObject targetNetobj)) return false;

        if (!subjectNetObj.GetComponent<Army>() || !targetNetobj.GetComponent<Structure>()) return false;

        return true;
    }
}
