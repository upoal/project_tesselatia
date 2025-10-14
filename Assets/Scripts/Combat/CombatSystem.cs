using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour {
    System.Random rng;
    GridManager gm;
    List<Structure> structures;

    void Start(){
        gm = GridManager.I;
        structures = FindObjectsOfType<Structure>().ToList();
    }

    public void BeginActionPhase(int turn) {
        rng = new System.Random(turn * 9176 + 1337); // deterministic seed
    }

    public void Tick() {
        // Structures fire
        foreach (var s in structures.Where(s=>s.HP>0)) {
            var target = FindNearestEnemyUnit(s.Team, s.Coord, s.AttackRangeTiles);
            if (target != null) DealDamage(target, s.DPS / PhaseController.I.TickHz);
        }

        // Units fight closest enemy in Manhattan range 1 (melee)
        foreach (var u in GameWorld.AllUnits.Where(u=>u.IsAlive)) {
            var enemy = FindNearestEnemyUnit(u.Team, u.Coord, Mathf.RoundToInt(u.Def.AttackRange));
            if (enemy != null) DealDamage(enemy, u.Def.DPS / PhaseController.I.TickHz);
            else {
                // default minion intent: queue a forward move if none planned
                if (PhaseController.I.Current == Phase.Action && u.IsMinion) {
                    Vector2Int forward = u.Team == Team.A ? new Vector2Int(u.Coord.x+1, u.Coord.y) : new Vector2Int(u.Coord.x-1, u.Coord.y);
                    TryAutoQueue(u, forward);
                }
            }
        }

        // Victory checks: base destroyed?
        foreach (var s in structures.Where(s=>s.IsBase && s.HP<=0)) {
            Debug.Log($"Team {(s.Team==Team.A? "B":"A")} WINS!");
        }
    }

    void TryAutoQueue(Unit u, Vector2Int dst) {
        if (!GridManager.I.InBounds(dst)) return;
        var tile = GridManager.I.GetTile(dst);
        if (tile.Occupant == null) u.QueueMove(dst);
    }

    Unit FindNearestEnemyUnit(Team team, Vector2Int from, int rangeTiles) {
        Unit best = null; int bestDist = int.MaxValue;
        foreach (var e in GameWorld.AllUnits) {
            if (!e.IsAlive || e.Team == team) continue;
            int d = Mathf.Abs(e.Coord.x - from.x) + Mathf.Abs(e.Coord.y - from.y);
            if (d <= rangeTiles && d < bestDist) { best = e; bestDist = d; }
        }
        return best;
    }

    void DealDamage(Unit target, float dmg) {
        target.HP -= dmg;
        if (target.HP <= 0) {
            var tile = GridManager.I.GetTile(target.Coord);
            if (tile && tile.Occupant == target) tile.Occupant = null;
        }
    }

    public void CleanupDead() {
        // destroy units with HP<=0
        for (int i = GameWorld.AllUnits.Count - 1; i >= 0; i--) {
            var u = GameWorld.AllUnits[i];
            if (!u.IsAlive) {
                GameWorld.AllUnits.RemoveAt(i);
                Destroy(u.gameObject);
            }
        }
    }
}
