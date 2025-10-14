using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Phase { Planning, Action, Cleanup }

public class PhaseController : MonoBehaviour {
    public static PhaseController I;
    public Phase Current = Phase.Planning;
    public int TurnIndex = 1;

    [Header("Timings")]
    public float PlanningSeconds = 15f;      // simultaneous planning
    public float ActionSeconds = 6f;         // resolves in fixed ticks
    public float TickHz = 10f;               // deterministic ticks

    WaveManager wave;
    CombatSystem combat;

    void Awake(){ I = this; }
    void Start() {
        wave = FindObjectOfType<WaveManager>();
        combat = FindObjectOfType<CombatSystem>();
        StartCoroutine(Loop());
    }

    IEnumerator Loop() {
        while (true) {
            // ---- Planning
            Current = Phase.Planning;
            UIEvents.OnPlanningStarted?.Invoke(TurnIndex);
            float t = PlanningSeconds;
            while (t>0f) { t -= Time.deltaTime; yield return null; }

            // spawn waves periodically
            wave.MaybeSpawn(TurnIndex);

            // ---- Action
            Current = Phase.Action;
            UIEvents.OnActionStarted?.Invoke(TurnIndex);
            float tickDt = 1f / TickHz;
            int ticks = Mathf.CeilToInt(ActionSeconds * TickHz);

            // seed combat for determinism
            combat.BeginActionPhase(TurnIndex);

            for (int i=0; i<ticks; i++) {
                // 1) movement: each unit executes at most one queued step
                foreach (var u in GameWorld.AllUnits) if (u.IsAlive) u.TickMovement();

                // 2) combat
                combat.Tick();

                yield return new WaitForSeconds(tickDt);
            }

            // ---- Cleanup
            Current = Phase.Cleanup;
            UIEvents.OnCleanupStarted?.Invoke(TurnIndex);
            combat.CleanupDead();
            // TODO: rewards, heal windows, etc.

            TurnIndex++;
        }
    }
}

public static class UIEvents {
    public static System.Action<int> OnPlanningStarted;
    public static System.Action<int> OnActionStarted;
    public static System.Action<int> OnCleanupStarted;
}

public static class GameWorld {
    public static readonly List<Unit> AllUnits = new();
    // You can add registration in Unit.Init if you prefer
}
