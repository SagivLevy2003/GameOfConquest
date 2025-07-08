using FishNet;
using FishNet.Object;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Candidates/Move To Position")]
public class MoveCommandCandidate : CommandCandidate
{
    public override BaseCommand CommandInstance => new MoveCommand(null, Vector2.zero);


    public override BaseCommand CreateCommand(CommandContext context)
    {
        NetworkObject netObj = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.SubjectId);

        if (!netObj) 
        {
            Debug.LogWarning($"Attempted to create a command for a null NetworkObject with id: <color=cyan>{context.SubjectId}</color>");
            return null;
        }

        return new MoveCommand(netObj, context.Position);
    }
}

