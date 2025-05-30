using System;
using UnityEngine;

public interface ICommand
{
    public string Name { get; }
    public event Action OnExecutionFinished;
    bool IsValidForInput();
    void Execute();
}