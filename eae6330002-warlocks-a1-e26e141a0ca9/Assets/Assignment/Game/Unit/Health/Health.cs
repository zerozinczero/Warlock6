using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHealth, IHealthSource {

    public UnityEvent OnDeath;

    #region IDamageable

    [SerializeField]
    private float current = 100f;
    public float Current {
        get { return current; }
    }

    [SerializeField]
    private float max = 100f;
    public float Max { get { return max; } set { max = value; } }

    [SerializeField] private FloatStat regen = null;
    public FloatStat Regen { get { return regen; } }

    [SerializeField] private List<IModifier<DamageEvent>> damageModifiers = new List<IModifier<DamageEvent>>();

    public float Percent { get { return current / max; } }

    public bool IsAlive { get { return current > 0f; } }
    public bool IsDead { get { return current <= 0f; } }


    public float Damage(IDamageInfo damageInfo) {

        if (IsDead) {
            return 0f;
        }

        DamageEvent damageEvent = new DamageEvent(this, damageInfo);
        DamageEvent adjustedDamageEvent = damageEvent;

        // run damage through damage modification pipeline
        foreach(IModifier<DamageEvent> modifier in damageModifiers) {
            adjustedDamageEvent = modifier.Modify(adjustedDamageEvent, this);
        }

        float actualDamage = Mathf.Min(current, adjustedDamageEvent.Amount);
        float endHealth = current - actualDamage;
        if (Equals(actualDamage, 0f)) {
            return 0f;
        }

        damageEvent.HealthChange = -actualDamage;
        damageEvent.EndHealth = endHealth;

        ApplyHealthChange(damageEvent);

        if (IsDead && OnDeath != null) {
            OnDeath.Invoke();
        }

        return actualDamage;
    }

    public void SetStats(IHealthStatsInfo stats) {

        HealthStatsEvent healthStatsEvent = new HealthStatsEvent(this, stats);
        healthStatsEvent.EndHealth = Mathf.Min(stats.Current, stats.Max);   // enforce that health cannot be greater than max health

        ApplyHealthChange(healthStatsEvent);

    }


    #endregion

    [System.Serializable]
    public class HealthEventCallback : UnityEvent<IHealthEvent> {}

    [SerializeField]
    private HealthEventCallback onHealthEvent = null;
    public HealthEventCallback OnHealthEvent { get { return onHealthEvent; } }

    void Update() {
        if (regen != null && IsAlive) {
            float regenAmount = regen.Current * Time.deltaTime;
            float actualRegen = Mathf.Min(max - current, regenAmount);
            if (regenAmount > 0) {
                HealthRegenEvent healthRegenEvent = new HealthRegenEvent(this, this, regenAmount);
                ApplyHealthChange(healthRegenEvent);
            }
        }
    }

    #region Helpers

    private void ApplyHealthChange(IHealthEvent healthEvent) {
        
        max = healthEvent.EndMaxHealth;
        current = healthEvent.EndHealth;

        if(onHealthEvent != null) {
            onHealthEvent.Invoke(healthEvent);
        }

    }

    #endregion

    public void AddDamageModifier(IModifier<DamageEvent> modifier) {
        if (modifier == null) return;

        damageModifiers.Add(modifier);
        damageModifiers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
    }

    public void RemoveDamageModifier(IModifier<DamageEvent> modifier) {
        damageModifiers.Remove(modifier);
    }
}
