using FishNet.Object;
using UnityEngine;

[System.Serializable]
public class CommandContextResolver
{ 
    public ICommandContext Resolve(NetworkObject subject, NetworkObject target)
    {
        if (subject == null)
        {
            Debug.LogWarning($"CommandContextResolver was sent a null subject!");
            return null;
        }

        if (target == null) //If not hovering over an object, set the context to a position
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new PositionCommandContext()
            {
                SubjectId = subject.ObjectId,
                Position = mousePos
            };
        }
        else //otherwise set the context to the object
        {
            return new GameObjectCommandContext()
            {
                SubjectId = subject.ObjectId,
                TargetId = target.ObjectId
            };
        }
    }
}
