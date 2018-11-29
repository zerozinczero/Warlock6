using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAttribution : MonoBehaviour {

    [SerializeField] private Unit unit = null;
    [SerializeField] private UnitMovement movement = null;
    [SerializeField] private DamageEvent lastDamageEvent;
    [SerializeField] private Unit killedBy = null;

    public void OnDeath() {
        Unit damagingUnit = lastDamageEvent.DamageSource != null ? lastDamageEvent.DamageSource.DamagingUnit : null;
        if(damagingUnit != null) {
            killedBy = damagingUnit;
        } else {
            // assume lava kill, by rule whoever pushed this unit last gets the kill
            killedBy = movement.LastForceApplier;
        }

        if(killedBy != null && killedBy != unit) {
            Player player = killedBy.Owner;
            PlayerGameStats playerGameStats = player.GetComponent<PlayerGameStats>();
            playerGameStats.Kills++;
            Debug.LogFormat("{0} killed {1}", killedBy, unit);
        }
    }

    public void OnHealthChange(IHealthEvent healthEvent) {
        if (healthEvent is DamageEvent) {
            lastDamageEvent = (DamageEvent)healthEvent;
        }
    }
	
}
