using UnityEngine;

public interface ICommandContext { }

public class GameObjectCommandContext : ICommandContext
{
    public GameObject Object;

    public GameObjectCommandContext(GameObject targetObject)
    {
        Object = targetObject;
    }
}

public class PositionCommandContext : ICommandContext
{
    public Vector2 Position;

    public PositionCommandContext(Vector2 position)
    {
        Position = position;
    }
}