using UnityEngine;

public enum TileTeam { Neutral, TeamA, TeamB }

public class Tile : MonoBehaviour {
    public Vector2Int Coord;
    public bool Walkable = true;
    public TileTeam LaneOwner = TileTeam.Neutral; // use to color center lane
    public Unit Occupant; // optional: single-occupant simplification
    
    [SerializeField] Renderer rend;
    Color _baseColor;

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.01f, new Vector3(1f, 0.02f, 1f));
    }
    #endif


    void Awake() {
        if (!rend) rend = GetComponentInChildren<Renderer>();
        _baseColor = rend ? rend.material.color : Color.white;
    }

    public void SetHighlight(bool on, Color c) {
        if (!rend) return;
        rend.material.color = on ? c : _baseColor;
    }
}
