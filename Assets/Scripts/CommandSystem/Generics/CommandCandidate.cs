using UnityEngine;

public abstract class CommandCandidate : ScriptableObject
{
    public abstract bool IsValidForInput(GameObject subject, ICommandContext target);

    public abstract ICommand CreateCommand(GameObject subject, ICommandContext target);
}
