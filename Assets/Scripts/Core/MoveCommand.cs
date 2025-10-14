using UnityEngine;

public class MoveCommand : ICommand {
    public Vector2Int Target;
    public MoveCommand(Vector2Int target) { Target = target; }
    public void Apply(Unit u) { u.QueueMove(Target); }
}
