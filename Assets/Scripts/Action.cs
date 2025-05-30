using UnityEngine;

public abstract class Command
{
    public GameObject Target;

    public abstract void Execute(GameObject Executer);
}