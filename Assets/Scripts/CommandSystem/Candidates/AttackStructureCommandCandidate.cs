using FishNet.Object;
using UnityEngine;

[CreateAssetMenu(menuName = "Command Candidates/Attack City")]
public class AttackStructureCommandCandidate : CommandCandidate
{
    public override BaseCommand CommandInstance => new AttackStructureCommand(null, null);

    public override BaseCommand CreateCommand(CommandContext context)
    {
        NetworkObject subjectNetObj = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.SubjectId);
        NetworkObject targetNetObj = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.TargetId);

        return new AttackStructureCommand(subjectNetObj, targetNetObj);
    }
}
