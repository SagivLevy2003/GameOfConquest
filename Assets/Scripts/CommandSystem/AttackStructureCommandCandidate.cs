using UnityEngine;

[CreateAssetMenu(menuName = "Command Candidates/Attack City")]
public class AttackStructureCommandCandidate : CommandCandidate
{
    public override bool IsValidForInput(GameObject subject, GameObject target)
    {
        return new AttackStructureCommand(subject, target).IsValidForInput();
    }

    public override ICommand CreateCommand(GameObject subject, GameObject target)
    {
        return new AttackStructureCommand(subject, target);
    }
}
