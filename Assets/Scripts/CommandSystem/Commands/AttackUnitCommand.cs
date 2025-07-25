using FishNet.Object;
using UnityEngine;

public class AttackUnitCommand : BaseCommand
{
    readonly Army _attacker;
    readonly Unit _targetUnit;
    CombatInstance _combatInstance;

    public AttackUnitCommand(NetworkObject subject, NetworkObject target)
    {
        if (subject) _attacker = subject.GetComponent<Army>();
        if (target) _targetUnit = target.GetComponent<Unit>();
    }

    public override void Execute()
    {
        _attacker.MovementHandler.MoveToEntity(_targetUnit.gameObject);
        _attacker.MovementHandler.OnTargetReached.AddListener(InitiateAttack);
    }

    public void InitiateAttack()
    {
        _combatInstance = CombatManager.Instance.InitiateCombat(_attacker, _targetUnit);
    }

    public override void CommandExecutionFinished(bool isForcedExit)
    {
        base.CommandExecutionFinished(isForcedExit);
        _attacker.MovementHandler.OnTargetReached.RemoveListener(InitiateAttack);
        //_combatInstance?.EndCombatInstance();
    }

    public override bool IsContextValid(CommandContext context)
    {
        if (!TryParseContext(context, out NetworkObject subjectNetObj, out NetworkObject targetNetobj)) return false;

        if (!subjectNetObj.GetComponent<Army>() || !targetNetobj.GetComponent<Unit>()) return false; //Stop from attacking allies

        return true;
    }
}
