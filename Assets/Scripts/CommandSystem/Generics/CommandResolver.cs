using System.Collections.Generic;
using UnityEngine;

public class CommandResolver : Singleton<CommandResolver>
{
    [SerializeField] private bool _logInteractions = false;

    [SerializeField] private List<CommandCandidate> _commandCandidates = new(); 

    public ICommand TryResolveCommand(GameObject subject, ICommandContext targetContext)
    {
        foreach (var candidate in _commandCandidates)
        {
            if (candidate.IsValidForInput(subject, targetContext))
            {
                if (_logInteractions) Debug.Log($"Attempting to create command: <color=cyan>{candidate.name}</color>. [Subject: <color=cyan>{subject}</color> | Target: <color=cyan>{targetContext}</color>]");
                return candidate.CreateCommand(subject, targetContext);
            }
        }

        if (_logInteractions) Debug.Log($"No appropriate command was found. [Subject: <color=cyan>{subject}</color> | Target: <color=cyan>{targetContext}</color>]");
        return null;
    }
}
