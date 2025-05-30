using UnityEngine;

public abstract class CommandCandidate : ScriptableObject
{
    public abstract bool IsValidForInput(GameObject subject, GameObject target);

    public abstract ICommand CreateCommand(GameObject subject, GameObject target);
}
