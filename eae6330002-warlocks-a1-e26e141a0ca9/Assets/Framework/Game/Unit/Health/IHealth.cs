public interface IHealth {

    float Current { get; }
    float Max { get; }
    float Percent { get; }  /* 0 to 1 */

    FloatStat Regen { get; }

    bool IsAlive { get; }
    bool IsDead { get; }

    void SetStats(IHealthStatsInfo stats);

    float Damage(IDamageInfo damageInfo);

    void AddDamageModifier(IModifier<DamageEvent> modifier);
    void RemoveDamageModifier(IModifier<DamageEvent> modifier);

}
