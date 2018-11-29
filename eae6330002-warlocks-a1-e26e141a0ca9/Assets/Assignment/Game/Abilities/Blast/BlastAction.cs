using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastAction : StandardAbilityAction, IDamageSource {

    [SerializeField] private GameObject explosionPrefab = null;

    [SerializeField] private float innerRadius = 1f;
    public float InnerRadius { get { return innerRadius; } set { innerRadius = value; } }

    [SerializeField] private float outerRadius = 3f;
    public float OuterRadius { get { return outerRadius; } set { outerRadius = value; } }

    [SerializeField] private float innerDamage = 20f;
    public float InnerDamage { get { return innerDamage; } set { innerDamage = value; } }

    [SerializeField] private float outerDamage = 5f;
    public float OuterDamage { get { return outerDamage; } set { outerDamage = value; } }

    [SerializeField] private float selfDamage = 5f;

    [SerializeField] private DamageType damageType = DamageType.Normal;

    [SerializeField] private float innerForce = 500f;
    public float InnerForce { get { return innerForce; } set { innerForce = value; } }

    [SerializeField] private float outerForce = 100f;
    public float OuterForce { get { return outerForce; } set { outerForce = value; } }

    [SerializeField] private AnimationCurve powerCurve = null;

    protected override IEnumerator PerformAction() {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = actor.transform.position;

        float adjOuterRadius = outerRadius * actor.AbilityRangeMultiplier.Current;
        Collider[] colliders = Physics.OverlapSphere(actor.transform.position, adjOuterRadius);
        foreach(Collider coll in colliders) {
            Vector3 delta = coll.transform.position - actor.transform.position;
            float dist = delta.magnitude;
            float t = Mathf.Clamp01((dist - innerRadius) / (adjOuterRadius - innerRadius));
            float power = powerCurve.Evaluate(t);//Mathf.Clamp01((dist - innerRadius) / (adjOuterRadius - innerRadius));


            Rigidbody rb = coll.GetComponentInParent<Rigidbody>();
            Unit target = coll.GetComponentInParent<Unit>();
            if(rb != null && target != actor) {
                Vector3 force = delta.normalized * (power * (innerForce - outerForce) + outerForce);
                if(target != null) {
                    UnitMovement movement = target.GetComponent<UnitMovement>();
                    movement.AddForce(actor, force);
                } else {
                    rb.AddForce(force);
                }
            }

            Health health = coll.GetComponentInParent<Health>();
            if(health != null && target != actor) {
                float damageAmount = power * (innerDamage - outerDamage) + outerDamage;
                DamageInfo damageInfo = new DamageInfo(this, damageAmount, damageType);
                health.Damage(damageInfo);
            }
        }

        DamageInfo selfDamageInfo = new DamageInfo(this, selfDamage, damageType);
        actor.Health.Damage(selfDamageInfo);
        yield break;
    }

    public Unit DamagingUnit { get { return actor; } }

    public override Dictionary<string, string> GetPropertyTokens() {
        return new Dictionary<string, string>() {
            {"{ENEMY_DAMAGE}", innerDamage.ToString()},
            {"{SELF_DAMAGE}", selfDamage.ToString()},
            {"{FORCE}", innerForce.ToString()},
            {"{OUTER_RADIUS}", outerRadius.ToString()}
        };
    }

}
