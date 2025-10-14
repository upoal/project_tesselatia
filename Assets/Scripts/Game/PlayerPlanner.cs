using UnityEngine;
using UnityEngine.InputSystem; // ← new input

public class PlayerPlanner : MonoBehaviour {
    public Camera Cam;
    public Team PlayerTeam = Team.A;
    public Color HoverColor = Color.yellow;
    private Unit _selected;

    void Update() {
        if (PhaseController.I.Current != Phase.Planning) return;

        // Handle mouse (desktop). Add touch/controller later if you like.
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.wasPressedThisFrame) {
            var tile = RayToTile(mouse.position.ReadValue());
            if (tile == null) return;

            if (_selected == null) {
                if (tile.Occupant != null && tile.Occupant.Team == PlayerTeam) {
                    _selected = tile.Occupant;
                    tile.SetHighlight(true, HoverColor);
                }
            } else {
                var to = tile.Coord;
                if (_selected.IsMinion) {
                    if (Unit.IsLegalMinionStep(_selected.Team, _selected.Coord, to) &&
                        tile.Occupant == null) {
                        _selected.QueueMove(to);
                    }
                } else {
                    // heroes: allow 4-neighbour step
                    if (Mathf.Abs((_selected.Coord - to).x) + Mathf.Abs((_selected.Coord - to).y) == 1 &&
                        tile.Occupant == null) {
                        _selected.QueueMove(to);
                    }
                }
            }
        }

        if (mouse.rightButton.wasPressedThisFrame) {
            Deselect();
        }
    }

    private void Deselect() {
        if (_selected != null) {
            var t = GridManager.I.GetTile(_selected.Coord);
            if (t) t.SetHighlight(false, Color.white);
            _selected = null;
        }
    }

    private Tile RayToTile(Vector2 screenPos) {
        var r = Cam.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y, 0));
        if (Physics.Raycast(r, out var hit, 100f)) {
            return hit.collider.GetComponentInParent<Tile>();
        }
        return null;
    }
}
