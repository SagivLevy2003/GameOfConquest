using FishNet.Object;
using System;

public abstract class BaseCommand
{
    public readonly int Id = CommandIdGenerator.GenerateId(); //Generates a unique ID for the command for debugging purposes
    public event Action OnExecutionFinished; //Called after the command is finished

    public abstract void Execute();

    public abstract bool IsContextValid(CommandContext context); //Checks if the input meets the requirements of the command

    public virtual void CommandExecutionFinished(bool isForcedExit) //Signals the command finished execution
    {
        if (!isForcedExit) OnExecutionFinished?.Invoke();
        OnExecutionFinished = null;  //Clears the event to avoid the memory being left
    }

    protected bool TryParseContext(CommandContext context, out NetworkObject subject, out NetworkObject target)
    {
        subject = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.SubjectId);
        target = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.TargetId);

        return subject && target;
    }
}