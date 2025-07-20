using FishNet.Object;
using System.Windows.Input;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Candidates/Attack Unit")]
public class AttackUnitCommandCandidate : CommandCandidate
{
    public override BaseCommand CommandInstance => new AttackUnitCommand(null, null);

    public override BaseCommand CreateCommand(CommandContext context)
    {
        NetworkObject subjectNetObj = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.SubjectId);
        NetworkObject targetNetObj = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.TargetId);

        return new AttackUnitCommand(subjectNetObj, targetNetObj);
    }
}
