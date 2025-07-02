using FishNet;
using FishNet.Object;
using UnityEngine;

[CreateAssetMenu(menuName = "Command Candidates/Move To Position")]
public class MoveCommandCandidate : CommandCandidate<PositionCommandContext>
{
    protected override bool IsValidForInput(PositionCommandContext context)
    {
        Vector2 position = context.Position;

        if (!InstanceFinder.ServerManager.Objects.Spawned.TryGetValue(context.SubjectId, out NetworkObject subject))
        {
            Debug.LogWarning($"Couldn't find a subject with Id: <color=cyan>{context.SubjectId}</color>");
            return false;
        }

        return new MoveCommand(subject, position).IsValidForInput();
    }

    protected override ICommand CreateCommand(PositionCommandContext context)
    {
        InstanceFinder.ServerManager.Objects.Spawned.TryGetValue(context.SubjectId, out NetworkObject subject);
        return new MoveCommand(subject, context.Position);
    }
}

