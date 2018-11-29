using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof(Unit))]
public class UnitController : MonoBehaviour, IUnitController {

    [SerializeField]
    private Unit unit = null;

    [SerializeField]
    private UnitMovement movement = null;
    public bool HasReachedDestination { get { return movement.HasReachedDestination; } }
    public void MoveTo(Vector3 target) { StopAll(); movement.MoveTo(target); }

    void Start() {
        UnitWorldUi ui = GetComponentInChildren<UnitWorldUi>();
        ui.SetName(unit.Owner.Name, unit.Owner.Color);
    }

    public void StopAll() {
        movement.Stop();
        StopAbilities();
    }

    public void StopAbilities() {
        foreach(Ability ability in unit.Abilities) {
            ability.StopCast();
        }
    }

    public virtual void Reset(IHealthSource healthSource, Transform spawnPoint) {
        HealthStatsInfo healthStatsInfo = new HealthStatsInfo(healthSource, unit.Health.Max, unit.Health.Max);
        unit.Health.SetStats(healthStatsInfo);

        movement.Reset(spawnPoint);

        foreach(Ability ability in unit.Abilities) {
            ability.Reset();
        }

        enabled = true;
        StopAll();

        unit.Ragdoll.Hide();

        unit.gameObject.SetActive(true);
    }
	
}
