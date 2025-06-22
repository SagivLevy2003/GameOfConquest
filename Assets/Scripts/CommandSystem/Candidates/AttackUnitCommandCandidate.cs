using UnityEngine;

[CreateAssetMenu(menuName = "Command Candidates/Attack Unit")]
public class AttackUnitCommandCandidate : CommandCandidate
{
    public override bool IsValidForInput(GameObject subject, ICommandContext target)
    {
        GameObject targetObject = target is GameObjectCommandContext context ? context.Object : null;

        return new AttackUnitCommand(subject, targetObject).IsValidForInput();
    }

    public override ICommand CreateCommand(GameObject subject, ICommandContext target)
    {
        GameObject targetObject = target is GameObjectCommandContext context ? context.Object : null;

        return new AttackUnitCommand(subject, targetObject);
    }
}
