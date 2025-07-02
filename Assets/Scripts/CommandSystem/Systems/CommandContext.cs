using UnityEngine;

public interface ICommandContext 
{
    public int SubjectId { get; set; }
}

public struct GameObjectCommandContext : ICommandContext
{
    public int SubjectId { get; set; }
    public int TargetId;
}

public struct PositionCommandContext : ICommandContext
{
    public int SubjectId { get; set; }
    public Vector2 Position;
}