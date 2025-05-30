using NUnit.Framework;
using System.Data;
using UnityEngine;

public class TargetingSystem : Singleton<TargetingSystem>
{
    //[SerializeField] private bool _debugInput = false;

    //private GameObject targetedObject;

    //private void Update()
    //{
    //    TryTargetHoveredObject();
    //}

    public void TryTargetHoveredObject() //Check what object is being hovered
    {
        //Input validitiy check

        GameObject selectedObject = SelectionSystem.Instance.SelectedObject;
        if (selectedObject == null) return; //Return if no object is SELECTED - since there is no one to give the order to

        CommandQueue commandQueue = selectedObject.GetComponentInChildren<CommandQueue>();
        if (commandQueue == null) return; //Return if the subject doesn't have a command queue - why am I giving a command to something that cannot recieve them?

        GameObject hoveredObject = MouseInputSystem.Instance.GetHoveredObject();
        if (hoveredObject && hoveredObject.GetComponentInChildren<ITargetable>() == null) hoveredObject = null; //Sets the target to 'null' if the currently hovered object isn't targetable

        ICommand command = CommandResolver.Instance.TryResolveCommand(selectedObject, hoveredObject); //Can be overhauled to be on Fixed Update, and used to display UI

        if (command != null) commandQueue.ExecuteCommandImmidiately(command);
    }
}
