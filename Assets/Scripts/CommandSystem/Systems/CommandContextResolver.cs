using FishNet.Object;
using UnityEngine;

[System.Serializable]
public class CommandContextResolver
{ 
    public CommandContext Resolve(NetworkObject subject, NetworkObject target) //Returns an 
    {
        if (subject == null)
        {
            Debug.LogWarning($"CommandContextResolver was sent a null subject!");
            return new()
            {
                SubjectId = -1
            };
        }


        if (target == null) //If not hovering over an object, set the context to a position
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Overhaul for performance if needed
            return new()
            {
                SubjectId = subject.ObjectId,
                TargetId = -1,
                Position = mousePos
            };
        }
        else //otherwise set the context to the object
        {
            return new()
            {
                SubjectId = subject.ObjectId,
                TargetId = target.ObjectId,
                Position = Vector2.zero
            };
        }
    }
}
