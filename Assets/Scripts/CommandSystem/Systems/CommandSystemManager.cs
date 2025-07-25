using FishNet;
using FishNet.Object;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CommandSystemManager : NetworkSingleton<CommandSystemManager>
{
    [Header("System References")]
    [SerializeField] private CommandContextResolver _contextResolver = new();
    [field: SerializeField] public CommandResolver CommandResolver { get; private set; } = new();

    [Header("General")]
    [SerializeField] private float _hoveredObjectCommandPoolingDelay = 1;

    [Header("Debug")]
    [SerializeField] private bool _logInteractions;

    //Used to check if the command context was changed
    [SerializeField] private CommandContext _currentCommandContext = new(); //The previous command context that was assigned
    [SerializeField] private CommandCandidate _currentCommandCandidate = null;
    //------------------------------------------------

    public UnityEvent<CommandCandidate> OnCommandCandidateChanged; 

    private void Start()
    {
        //It's not good that this is here but.. I've given up on SOLID at this point
        OnCommandCandidateChanged.AddListener(CursorManager.Instance.OnCommandCandidateChanged);

        StartCoroutine(PoolForCurrentCommand());
    }

    public void DetermineCurrentCommand()
    {
        NetworkObject selectedObject = SelectionSystem.Instance.SelectedObject;
        if (!IsSelectedObjectValid(selectedObject))
        {
            _currentCommandCandidate = null;
            OnCommandCandidateChanged?.Invoke(null);

            return; //Returns if the subject is invalid
        }

        GameObject hoveredObject = MouseInputSystem.Instance.GetHoveredObject();

        CommandContext frameCommandContext = GetContext(selectedObject, hoveredObject); //Get a commandContext according to what is being hovered / targeted

        if (frameCommandContext.Equals(_currentCommandContext)) //Returns if the command context wasn't changed.
        {
            return;
        }

        _currentCommandContext = frameCommandContext; //Update the current commandContext


        if (_logInteractions) LogContext("Command context changed: ",_currentCommandContext);

        if (!TryGetValidCommandCandidate(_currentCommandContext, out CommandCandidate candidate))
        {
            if (_logInteractions) Debug.Log($"No valid command candidate for: " +
                $"Subject: <color=cyan>{(selectedObject ? selectedObject.name : "null")}</color> | " +
                $"Target: <color=cyan>{(hoveredObject ? hoveredObject.name : _currentCommandContext.Position)}</color>");

            _currentCommandCandidate = null;
            OnCommandCandidateChanged?.Invoke(null);
            return;
        }

        // Candidate is valid and new
        if (_currentCommandCandidate != candidate)
        {
            Debug.Log($"Command candidate changed to: <color=yellow>{candidate.GetType().Name}</color>");
            _currentCommandCandidate = candidate;
            OnCommandCandidateChanged?.Invoke(candidate);
        }
    }

    public void TryExecuteCurrentCommandOnServer()
    {
        if (_currentCommandContext.SubjectId == -1) return; //Return if the subject is invalid
        if (_logInteractions) LogContext("Sending command context to server:", _currentCommandContext);
        TryExecuteCommand(_currentCommandContext);
    }

    [ServerRpc(RequireOwnership=false)]
    public void TryExecuteCommand(CommandContext context) //Called from MouseInputSystem - that's probably not great, but it's enough within the project's scope
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogWarning($"Attempted to run ExecuteCurrentCommand from a client.");
            return;
        }

        if (!TryGetValidCommandCandidate(context, out CommandCandidate candidate)) //Get an appropriate command candidate for the context
        {
            Debug.LogWarning("Invalid command context received from client.");
            return;
        }

        NetworkObject subjectNetworkObject = NetworkSystemManager.Instance.NetworkObjectManager.GetNetworkObjectById(context.SubjectId);

        if (!subjectNetworkObject) return; //Return if there is no network object associated with the subject id

        CommandQueue commandQueue = subjectNetworkObject.GetComponentInChildren<CommandQueue>();

        if (!commandQueue) return; //Return if the subject doesn't have a command queue

        commandQueue.ExecuteCommandImmidiately(candidate.CreateCommand(context));
    }

    IEnumerator PoolForCurrentCommand()
    {
        while (true)
        {
            DetermineCurrentCommand();
            yield return new WaitForSeconds(_hoveredObjectCommandPoolingDelay);
        }
    }

    private bool TryGetValidCommandCandidate(CommandContext context, out CommandCandidate candidate)
    {
        candidate = null;

        if (context.SubjectId == -1)
            return false;

        candidate = CommandResolver.GetCommandCandidate(context);
        return candidate != null && candidate.IsValidForContext(context);
    }

    private bool IsSelectedObjectValid(NetworkObject subject)
    {
        return subject != null
            && subject.GetComponentInChildren<CommandQueue>() != null //Can't give a command to something that can't recieve them, can you? :D
            && subject.IsOwner;
    }

    private CommandContext GetContext(NetworkObject selected, GameObject hovered)
    {
        var hoveredNetObj = hovered ? hovered.transform.root.GetComponent<NetworkObject>() : null;
        return _contextResolver.Resolve(selected, hoveredNetObj);
    }

    private void LogContext(string body,CommandContext context)
    {
        Debug.Log($"{body}\n" +
                  $"SubjectId: <color=cyan>{context.SubjectId}</color> | <color=cyan>{context.TargetId}</color> | <color=cyan>{context.Position}</color> ");
    }
}