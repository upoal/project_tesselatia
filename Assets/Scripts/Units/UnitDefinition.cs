using UnityEngine;

[CreateAssetMenu(menuName="AutoChess/UnitDefinition")]
public class UnitDefinition : ScriptableObject {
    public string UnitName;
    public float MaxHP = 100;
    public float DPS = 8;
    public float AttackRange = 1f;  // tiles (Manhattan)
    public float MovePerTick = 1f;  // tiles per tick (we’ll clamp to 1 step)
}
