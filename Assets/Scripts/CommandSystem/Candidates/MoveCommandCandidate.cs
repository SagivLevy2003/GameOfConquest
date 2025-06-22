using UnityEngine;

[CreateAssetMenu(menuName = "Command Candidates/Move To Position")]
public class MoveCommandCandidate : CommandCandidate
{
    public override bool IsValidForInput(GameObject subject, ICommandContext target)
    {
        Vector2 position = target is PositionCommandContext context ? context.Position : Vector2.zero;

        return new MoveCommand(subject, position).IsValidForInput();
    }

    public override ICommand CreateCommand(GameObject subject, ICommandContext target)
    {
        Vector2 position = target is PositionCommandContext context ? context.Position : Vector2.zero;

        return new MoveCommand(subject, position);
    }
}
