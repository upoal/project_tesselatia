using UnityEngine;

public class GridManager : MonoBehaviour {
    public static GridManager I;
    public int Width = 14;
    public int Height = 10;
    public int LaneY = 5;              // single lane row
    public GameObject TilePrefab;
    public Tile[,] Tiles;

    void Awake() { I = this; }

    void Start() {
        if (TilePrefab == null) {
            Debug.LogError("[GridManager] TilePrefab is not assigned.");
            return;
        }

        Tiles = new Tile[Width, Height];

        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                var go = Instantiate(TilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                if (go == null) { Debug.LogError($"[GridManager] Instantiate returned null at {x},{y}"); continue; }

                // Try find Tile on root or children
                var tile = go.GetComponent<Tile>() ?? go.GetComponentInChildren<Tile>();
                if (tile == null) {
                    Debug.LogError($"[GridManager] Tile component missing on TilePrefab at {x},{y}. Add Tile.cs to prefab.");
                    continue;
                }

                tile.Coord = new Vector2Int(x, y);
                if (y == LaneY)
                    tile.LaneOwner = (x < Width / 2) ? TileTeam.TeamA : TileTeam.TeamB;

                Tiles[x, y] = tile;
            }
        }

        if (Camera.main != null) {
            Camera.main.transform.position = new Vector3(Width * 0.5f, 14f, -3f);
            Camera.main.transform.rotation = Quaternion.Euler(60, 0, 0);
        } else {
            Debug.LogWarning("[GridManager] No Main Camera found (tag MainCamera).");
        }
    }


    public bool InBounds(Vector2Int c) => c.x >= 0 && c.y >= 0 && c.x < Width && c.y < Height;
    public Tile GetTile(Vector2Int c) {
        if (Tiles == null) return null;
        return InBounds(c) ? Tiles[c.x, c.y] : null;
    }

}
