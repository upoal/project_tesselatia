using UnityEngine;

public class Structure : MonoBehaviour {
    public Team Team;
    public bool IsBase;
    public float MaxHP = 500;
    public float HP;
    public float DPS = 10;
    public int AttackRangeTiles = 4;
    public Vector2Int Coord;

    void Awake() { HP = MaxHP; }

    public void Init(Team t, Vector2Int coord, bool isBase) {
        Team = t; Coord = coord; IsBase = isBase;
        transform.position = new Vector3(coord.x, 0, coord.y);
    }
}
