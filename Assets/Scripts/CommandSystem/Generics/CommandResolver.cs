using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CommandResolver : Singleton<CommandResolver>
{
    [SerializeField] private bool _logInteractions = false;

    [SerializeField] private List<CommandCandidate> _commandCandidates = new(); 

    public ICommand TryResolveCommand(GameObject subject, GameObject target)
    {
        foreach (var candidate in _commandCandidates)
        {
            if (candidate.IsValidForInput(subject, target))
            {
                if (_logInteractions) Debug.Log($"Attempting to create command: <color=cyan>{candidate.name}</color>. [Subject: <color=cyan>{subject}</color> | Target: <color=cyan>{target}</color>]");
                return candidate.CreateCommand(subject, target);
            }
        }

        if (_logInteractions) Debug.Log($"No appropriate command was found. [Subject: <color=cyan>{subject}</color> | Target: <color=cyan>{target}</color>]");
        return null;
    }
}
