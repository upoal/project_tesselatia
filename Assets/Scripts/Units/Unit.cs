using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
    public Team Team;
    public UnitDefinition Def;
    public Vector2Int Coord;         // tile coordinate
    public float HP;
    public bool IsMinion;

    Queue<Vector2Int> _plannedMoves = new();

    public void Init(UnitDefinition def, Team team, Vector2Int start) {
        Def = def; Team = team; Coord = start; HP = def.MaxHP;
        transform.position = new Vector3(start.x, 0, start.y);
        GridManager.I.GetTile(start).Occupant = this;
    }

    public void QueueMove(Vector2Int c) => _plannedMoves.Enqueue(c);
    public void ClearMoves() => _plannedMoves.Clear();

    // Planning rule: minions can move forward or sideways, not backward
    public static bool IsLegalMinionStep(Team team, Vector2Int from, Vector2Int to) {
        var delta = to - from;
        if (Mathf.Abs(delta.x) + Mathf.Abs(delta.y) != 1) return false; // 4-neighbour
        if (team == Team.A) {
            if (delta.x < 0) return false;  // no backward
        } else {
            if (delta.x > 0) return false;
        }
        return true;
    }

    // Executed by Action phase (one step max / tick)
    public void TickMovement() {
        if (_plannedMoves.Count == 0) return;
        var next = _plannedMoves.Peek();

        if (!GridManager.I.InBounds(next)) { _plannedMoves.Dequeue(); return; }
        var toTile = GridManager.I.GetTile(next);
        if (!toTile.Walkable || toTile.Occupant != null) return; // blocked

        // move 1 tile
        var fromTile = GridManager.I.GetTile(Coord);
        fromTile.Occupant = null;
        Coord = next;
        toTile.Occupant = this;
        transform.position = new Vector3(Coord.x, 0, Coord.y);
        _plannedMoves.Dequeue();
    }

    public bool IsAlive => HP > 0;
}
