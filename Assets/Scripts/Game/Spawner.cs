using UnityEngine;

public class Spawner : MonoBehaviour {
    public static Spawner I;
    public UnitDefinition MinionDef;
    public UnitDefinition OffensiveHeroDef, DefensiveHeroDef, JunglerHeroDef;
    public GameObject UnitPrefab;

    void Awake(){ I = this; }

    public Unit SpawnUnit(UnitDefinition def, Team team, Vector2Int coord, bool isMinion=false) {
        var go = Instantiate(UnitPrefab);
        var u = go.GetComponent<Unit>();
        u.IsMinion = isMinion;
        u.Init(def, team, coord);
        go.name = $"{def.UnitName}_{team}_{coord}";
        return u;
    }
}
