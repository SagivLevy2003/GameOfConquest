using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class CommandQueue : MonoBehaviour
{
    [SerializeField] private bool _logInteractions = false;

    public readonly List<ICommand> Queue = new();

    public Entity AttachedEntity { get; private set; }

    [Header("Debug")]
    [field: SerializeField] private List<string> _debugQueueDisplay = new();

    private void Awake()
    {
        AttachedEntity = GetComponent<Entity>();

        if (AttachedEntity == null)
        {
            Debug.LogWarning($"CommandQueue component without an Entity component detected on <color=cyan>{gameObject}</color>!");
        }
    }

    public void ExecuteCommandImmidiately(ICommand command) //clears the queue and executes a command immidiately
    {
        Clear();
        EnqueueCommand(command);
    }

    public void EnqueueCommand(ICommand command) //Adds a command to the command execution queue
    {
        if (_logInteractions) Debug.Log($"Adding command <color=cyan>{command}</color> to Queue for <color=cyan>{AttachedEntity.gameObject}</color>");

        if (command == null)
        {
            Debug.LogWarning($"Attempted to enqueue a null command on <color=cyan>{gameObject}</color>!");
            return;
        }

        Queue.Add(command); //Adds the command to the queue
        _debugQueueDisplay.Add(command.Name); //Adds the command to the debug display

        if (Queue.Count == 1) ExecuteCommand(command); //If the queue is empty and this is the only command, execute it immidiately
    }

    private void HandleCommandCompletion() //Exists the current command and executes the next one
    {
        if (_logInteractions) Debug.Log($"Command queue updated of finished command execution: <color=cyan>{Queue[0]}</color>");

        if (Queue.Count <= 0)//Returns if the queue is, for some reason, empty
        {
            Debug.LogWarning($"ExecuteNextCommand was called on an empty CommandQueue on <color=cyan>{gameObject}</color>!");
            return;
        }

        Queue.RemoveAt(0); //Remove the command that just finished from the queue
        _debugQueueDisplay.RemoveAt(0); //Updates the debug display

        if (Queue.Count > 0) ExecuteCommand(Queue[0]); //Execute the next command, if any
    }

    private void ExecuteCommand(ICommand command) //Executes a given command
    {
        command.OnExecutionFinished -= HandleCommandCompletion; // prevent double-subscribe
        command.OnExecutionFinished += HandleCommandCompletion;
        command.Execute();
    }

    public void Clear()
    {
        if (_logInteractions) Debug.Log($"Cleared command queue for <color=cyan>{AttachedEntity.gameObject}</color>");
        
        foreach (ICommand command in Queue) //Executes the exit logic on 
        {
            command.CommandExecutionFinished(true);
        }

        Queue.Clear();
        _debugQueueDisplay.Clear(); 
    }
}
