using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandResolver
{
    [SerializeField] private List<CommandCandidate> _commandCandidates = new();


    public ICommand TryResolveCommand(ICommandContext commandContext)
    {
        foreach (var candidate in _commandCandidates)
        {
            if (candidate.IsValidForInput(commandContext))
            {
                return candidate.CreateCommand(commandContext);
            }
        }

        return null;
    }
}