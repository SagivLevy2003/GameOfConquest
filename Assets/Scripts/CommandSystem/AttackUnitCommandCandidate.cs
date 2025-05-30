using UnityEngine;

[CreateAssetMenu(menuName = "Command Candidates/Attack Unit")]
public class AttackUnitCommandCandidate : CommandCandidate
{
    public override bool IsValidForInput(GameObject subject, GameObject target)
    {
        return new AttackUnitCommand(subject, target).IsValidForInput();
    }

    public override ICommand CreateCommand(GameObject subject, GameObject target)
    {
        return new AttackUnitCommand(subject, target);
    }
}
