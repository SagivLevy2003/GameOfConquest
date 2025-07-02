using FishNet.Object;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CommandSystemManager : NetworkSingleton<CommandSystemManager>
{
    [Header("System References")]
    [SerializeField] private CommandContextResolver _contextResolver = new();
    [SerializeField] private CommandResolver _commandResolver = new();

    [Header("Debug")]
    [SerializeField] private bool _logInteractions;

    private ICommandContext _currentCommandContext = null; //Used to check if the command context was changed
    public event Action<ICommand> OnPotentialCommandChanged;

    //OVERHAUL - ADD A HOVER SYSTEM, MAYBE STATIC COMMAND DATA INSTANCES FOR ICONS N STUFF, MAKE SURE IT RUNS ONCE PER FRAME
    //BUILD IT SO THAT RESOLVING ONLY HAPPENS WHEN AN INTERACTION ACTUALLY OCCURS!

    public void DetermineCurrentCommand()
    {
        NetworkObject selectedObject = SelectionSystem.Instance.SelectedObject;
        if (!IsSelectedObjectValid(selectedObject)) return; //Returns if the subject is invalid

        GameObject hoveredObject = MouseInputSystem.Instance.GetHoveredObject();
        NetworkObject hoveredObjectNetObj = hoveredObject ? hoveredObject.transform.root.GetComponent<NetworkObject>() : null; //Get the NetObj of the hovered object

        ICommandContext commandContext = _contextResolver.Resolve(selectedObject, hoveredObjectNetObj); //Get a commandContext according to what is being hovered / targeted

        if (IsSameContext(_currentCommandContext, commandContext)) //Returns if the command context wasn't changed.
        {
            return;
        }

        _currentCommandContext = commandContext; //Update the current commandContext

        if (commandContext == null) //Issue no command if there's no fitting context
        {
            if (_logInteractions) Debug.Log($"No valid command context for: " +
                $"Subject: <color=cyan>{selectedObject.name}</color> | <color=cyan>{hoveredObjectNetObj.name}</color>");

            OnPotentialCommandChanged?.Invoke(null);
            return;
        }

        ICommand command = _commandResolver.TryResolveCommand(commandContext); //Can be overhauled to be on Fixed Update, and used to display UI

        if (_logInteractions) //Log the command creation
        {
            if (command == null)
                Debug.Log($"No command found for: [Subject: <color=cyan>{selectedObject.name}</color>]");
            else
                Debug.Log($"Creating command: <color=cyan>{command.Name}</color>. " +
                    $"[Subject: <color=cyan>{commandContext.SubjectId}</color> | Command Context: <color=cyan>{commandContext}</color>]");
        }

        OnPotentialCommandChanged?.Invoke(command);
    }

    private bool IsSelectedObjectValid(NetworkObject subject)
    {
        return subject != null  
            && subject.GetComponentInChildren<CommandQueue>() != null; //Can't give a command to something that can't recieve them, can you? :D
    }

    private bool IsSameContext(ICommandContext a, ICommandContext b) //Manually ompares the context, cause of obnoxious struct limitations
    {
        if (a is PositionCommandContext pa && b is PositionCommandContext pb)
            return pa.Equals(pb);
        if (a is GameObjectCommandContext ga && b is GameObjectCommandContext gb)
            return ga.Equals(gb);

        return false;
    }
}