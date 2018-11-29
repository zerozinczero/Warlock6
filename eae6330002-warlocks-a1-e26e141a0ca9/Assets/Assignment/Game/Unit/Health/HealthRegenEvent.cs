using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HealthRegenEvent : IHealthEvent {

    public IHealth Target { get; set; }

    public float Amount { get; set; }

    public float StartHealth { get; set; }
    public float EndHealth { get; set; }
    public float HealthChange { get; set; }

    public float StartMaxHealth { get; set; }
    public float EndMaxHealth { get; set; }

    public IHealthSource Source { get; set; }

    public HealthRegenEvent(IHealth target, IHealthSource source, float amount) {
        Target = target;
        Source = source;
        Amount = amount;
        StartHealth = target.Current;
        EndHealth = Mathf.Clamp(target.Current + amount, 0, target.Max);
        HealthChange = EndHealth - StartHealth;
        StartMaxHealth = target.Max;
        EndMaxHealth = target.Max;
    }

}