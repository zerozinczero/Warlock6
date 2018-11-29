using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour, IRound<RoundPhase,RoundEvent>, IHealthSource {
    
    [SerializeField]
    private RoundPhase phase = RoundPhase.PreRound;
    public RoundPhase Phase { get { return phase; } }

    [SerializeField]
    private RoundEvent onPhaseChanged = null;
    public RoundEvent OnPhaseChanged { get { return onPhaseChanged; } }

    [SerializeField]
    private Player winner = null;
    public IPlayer Winner { get { return winner; } }

    [SerializeField]
    private List<Unit> allUnits = null;

    public List<IUnit> AliveUnits {
        get {
            return allUnits.FindAll((u) => u.Health.IsAlive).ConvertAll<IUnit>((u) => u as IUnit);
        }
    }

    public List<IPlayer> AlivePlayers {
        get {
            return allUnits.FindAll((u) => u.Health.IsAlive).ConvertAll<IPlayer>((u) => u.Owner);
        }
    }

    [SerializeField]
    private Map map = null;

    [SerializeField]
    private float timePerDecay = 10f;

    [SerializeField]
    private float amountPerDecay = 5f;

    [SerializeField]
    private float celebrationTime = 3f;

    public void Initialize(Map map, List<Unit> units) {
        allUnits = new List<Unit>(units);
        this.map = map;

        phase = RoundPhase.PreRound;
        OnPhaseChanged.Invoke(this);

        map.SetIslandRadius(map.Island.MaxRadius);
        PrepUnits();
        map.Lava.Clear();

    }

    public void StartRound() {
        
        phase = RoundPhase.InProgress;
        OnPhaseChanged.Invoke(this);

        StartCoroutine(DecayIsland(map.Island));
    }

    void PrepUnits() {
        foreach(Unit unit in allUnits) {
            int i = allUnits.IndexOf(unit);
            Transform spawnPoint = map.GetSpawnPoint(i);
            UnitController unitController = unit.Controller as UnitController;
            unitController.Reset(this, spawnPoint);
        }
    }

    IEnumerator DecayIsland(Island island) {

        while (island.Radius > 0) {

            yield return new WaitForSeconds(timePerDecay);

            float newRadius = Mathf.Max(0f, island.Radius - amountPerDecay);
            map.SetIslandRadius(newRadius);

        }

    }



    void Update() {
        switch(phase) {
            case RoundPhase.InProgress:
                Update_InProgress();
                break;
        }
    }

    void Update_InProgress() {
        List<IPlayer> alivePlayers = AlivePlayers;
        if(alivePlayers.Count <= 1) {
            winner = alivePlayers.Count > 0 ? alivePlayers[0] as Player : null;
            StartCelebration();
        }
    }

    void StartCelebration() {
        phase = RoundPhase.Celebration;
        onPhaseChanged.Invoke(this);
        StopAllCoroutines();
        StartCoroutine(Celebration_Coroutine());
    }

    void EndRound() {
        phase = RoundPhase.Completed;
        onPhaseChanged.Invoke(this);
    }

    IEnumerator Celebration_Coroutine() {
        yield return new WaitForSeconds(celebrationTime);
        EndRound();
    }
	
}
