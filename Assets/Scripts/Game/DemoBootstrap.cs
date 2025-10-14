using UnityEngine;
using System.Collections;

public class DemoBootstrap : MonoBehaviour {
    IEnumerator Start() {
        // Wait until GridManager exists *and* has built the Tiles array
        yield return new WaitUntil(() => GridManager.I != null && GridManager.I.Tiles != null);

        var gm = GridManager.I;

        // Team A heroes
        Spawner.I.SpawnUnit(Spawner.I.OffensiveHeroDef, Team.A, new Vector2Int(1, gm.LaneY - 1));
        Spawner.I.SpawnUnit(Spawner.I.DefensiveHeroDef, Team.A, new Vector2Int(1, gm.LaneY));
        Spawner.I.SpawnUnit(Spawner.I.JunglerHeroDef,  Team.A, new Vector2Int(1, gm.LaneY + 1));

        // Team B heroes
        Spawner.I.SpawnUnit(Spawner.I.OffensiveHeroDef, Team.B, new Vector2Int(gm.Width - 2, gm.LaneY - 1));
        Spawner.I.SpawnUnit(Spawner.I.DefensiveHeroDef, Team.B, new Vector2Int(gm.Width - 2, gm.LaneY));
        Spawner.I.SpawnUnit(Spawner.I.JunglerHeroDef,  Team.B, new Vector2Int(gm.Width - 2, gm.LaneY + 1));
    }
}
