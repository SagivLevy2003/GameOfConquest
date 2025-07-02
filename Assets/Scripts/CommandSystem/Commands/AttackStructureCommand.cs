//using FishNet.Object;
//using System;
//using UnityEngine;

//public class AttackStructureCommand : BaseCommand
//{
//    readonly Army _attacker;
//    readonly Structure _targetStructure;
//    CombatInstance _combatInstance;

//    public override string Name => "Attack Structure";

//    public AttackStructureCommand(NetworkObject subject, NetworkObject target)
//    {
//        if (subject) _attacker = subject.GetComponentInChildren<Army>();
//        if (target) _targetStructure = target.GetComponentInChildren<Structure>();
//    }

//    public override void Execute()
//    {
//        _attacker.MovementHandler.MoveToEntity(_targetStructure.gameObject);
//        _attacker.MovementHandler.OnTargetReached.AddListener(InitiateAttack);
//    }

//    public void InitiateAttack()
//    {
//        _combatInstance = CombatManager.Instance.InitiateCombat(_attacker, _targetStructure);
//        Debug.Log($"attacking, {Id}");
//    }

//    public override bool IsValidForInput()
//    {
//        if (_attacker == null || _targetStructure == null) return false; //Return false if entity scripts are missing

//        if (_attacker.OwnerPlayerObject.Id == _targetStructure.OwnerPlayerObject.Id) return false; //Returns false if the attacker and target are owned by the same player

//        return true;
//    }

//    public override void CommandExecutionFinished(bool isForcedExit)
//    {
//        base.CommandExecutionFinished(isForcedExit);
//        if (_attacker != null) _attacker.MovementHandler.OnTargetReached.RemoveListener(InitiateAttack);
//        _combatInstance?.EndCombatInstance();
//    }
//}
