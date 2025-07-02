using System;
using System.Diagnostics;

public abstract class BaseCommand : ICommand
{
    public abstract string Name { get; }
    public readonly int Id = CommandIdGenerator.GenerateId();

    public event Action OnExecutionFinished;

    public abstract void Execute();

    public abstract bool IsValidForInput(); //Checks if the input meets the requirements of the command

    public virtual void CommandExecutionFinished(bool isForcedExit) //Signals the command finished execution
    {
        if (!isForcedExit) OnExecutionFinished?.Invoke();
        OnExecutionFinished = null;  //Clears the event to avoid the memory being left
    }
}
