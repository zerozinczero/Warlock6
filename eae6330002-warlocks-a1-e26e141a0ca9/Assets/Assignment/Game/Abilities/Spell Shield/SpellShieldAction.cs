using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShieldAction : StandardAbilityAction, IModifier<DamageEvent>, IModifier<bool> {

    [SerializeField] private float duration = 5f;
    public float Duration { get { return duration; } set { duration = value; } }

    [SerializeField] private SpellShield shield = null;

    [SerializeField] private bool activated = false;

    protected override IEnumerator PerformAction() {

        activated = true;
        shield.Caster = actor;
        shield.gameObject.SetActive(true);
        actor.Health.AddDamageModifier(this);
        actor.Movement.Pushable.AddModifier(this);

        yield return new WaitForSeconds(duration);

        activated = false;
        actor.Health.RemoveDamageModifier(this);
        actor.Movement.Pushable.RemoveModifier(this);
        shield.gameObject.SetActive(false);

    }

    public DamageEvent Modify(DamageEvent damageEvent, object context = null) {
        if (damageEvent.DamageType != DamageType.Normal)
            return damageEvent;

        damageEvent.Amount = 0;
        return damageEvent;
    }

    public bool Modify(bool pushable, object context = null) {
        return false;
    }

    public int Priority { get { return -10; } }
}
