//using UnityEngine;

//[CreateAssetMenu(menuName = "Command Candidates/Attack City")]
//public class AttackStructureCommandCandidate : CommandCandidate
//{
//    public override bool IsValidForInput(GameObject subject, ICommandContext target)
//    {
//        GameObject targetObject = target is GameObjectCommandContext context ? context.Object : null;

//        return new AttackStructureCommand(subject, targetObject).IsValidForInput();
//    }

//    public override ICommand CreateCommand(GameObject subject, ICommandContext target)
//    {
//        GameObject targetObject = target is GameObjectCommandContext context ? context.Object : null;

//        return new AttackStructureCommand(subject, targetObject);
//    }
//}
