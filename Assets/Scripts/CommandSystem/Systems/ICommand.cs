using System;
using UnityEngine;

public interface ICommand
{
    public abstract string Name { get; }
    public event Action OnExecutionFinished;
    bool IsValidForInput();
    void Execute();
    void CommandExecutionFinished(bool isForcedExit);
}
