using NUnit.Framework;
using System.Data;
using UnityEngine;

public class TargetingSystem : Singleton<TargetingSystem>
{
    public void TryTargetHoveredObject() //Check what object is being hovered
    {
        //Input validitiy check

        GameObject selectedObject = SelectionSystem.Instance.SelectedObject;
        if (selectedObject == null) return; //Return if no object is SELECTED - since there is no one to give the order to

        CommandQueue commandQueue = selectedObject.GetComponentInChildren<CommandQueue>();
        if (commandQueue == null) return; //Return if the subject doesn't have a command queue - why am I giving a command to something that cannot recieve them?

        ICommandContext commandContext = GetContext(MouseInputSystem.Instance.GetHoveredObject()); //Parses the hovered object into a context object

        ICommand command = CommandResolver.Instance.TryResolveCommand(selectedObject, commandContext); //Can be overhauled to be on Fixed Update, and used to display UI

        if (command != null) commandQueue.ExecuteCommandImmidiately(command);
    }

    ICommandContext GetContext(GameObject hoveredObject)
    {
        if (hoveredObject == null) //If not hovering over an object, set the context to a position
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new PositionCommandContext(mousePos);
        }
        else //otherwise set the context to the object
        {
            return new GameObjectCommandContext(hoveredObject);
        }
    }
}
