using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
    public int WavesEveryNTurns = 2;
    public int MinionsPerWave = 3;
    public Vector2Int SpawnA = new(1, 5);
    public Vector2Int SpawnB;   // set in Start dynamically
    GridManager gm;

    void Start() {
        gm = GridManager.I;
        SpawnB = new Vector2Int(gm.Width - 2, gm.LaneY);
    }

    public void MaybeSpawn(int turnIndex) {
        if (turnIndex % WavesEveryNTurns != 0) return;
        for (int i = 0; i < MinionsPerWave; i++) {
            var a = Spawner.I.SpawnUnit(Spawner.I.MinionDef, Team.A,
                new Vector2Int(SpawnA.x, SpawnA.y + Mathf.Clamp(i - 1, -1, 1)), true);
            var b = Spawner.I.SpawnUnit(Spawner.I.MinionDef, Team.B,
                new Vector2Int(SpawnB.x, SpawnB.y + Mathf.Clamp(i - 1, -1, 1)), true);
        }
    }
}
