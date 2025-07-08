using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CommandResolver
{
    [SerializeField] private List<CommandCandidate> _commandCandidates = new();

    public BaseCommand TryResolveCommand(CommandContext commandContext) //Try to return a command based on the context
    {
        CommandCandidate candidate = GetCommandCandidate(commandContext);

        if (candidate)
        {
            return candidate.CreateCommand(commandContext);
        }

        return null;
    }

    public CommandCandidate GetCommandCandidate(CommandContext commandContext) //Get the appropriate command candidate according to the context
    {
        foreach (var candidate in _commandCandidates)
        {
            if (candidate.IsValidForContext(commandContext))
            {
                return candidate;
            }
        }

        return null;
    }

    public VisualCommandData GetVisualDataFromCommand(BaseCommand command)
    {
        var match = _commandCandidates //Where the type of the returned command equals to the candidate
            .FirstOrDefault(candidate => candidate.CommandInstance.GetType() == command.GetType());

        if (match == null)
        {
            Debug.LogWarning($"No visual data found for command type: {command.GetType().Name}");
            return null;
        }

        return match.VisualData; 
    }
}