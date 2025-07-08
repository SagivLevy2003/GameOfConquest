using System;
using UnityEngine;

public abstract class CommandCandidate : ScriptableObject
{
    [field: SerializeField] public VisualCommandData VisualData { get; private set; } //Visual data related to the command (sprite, name etc)

    public abstract BaseCommand CommandInstance { get; } //A command instance for querying context validity

    public bool IsValidForContext(CommandContext context)
    {
        return CommandInstance.IsContextValid(context);
    }

    public abstract BaseCommand CreateCommand(CommandContext context); 
}