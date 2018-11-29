using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorAction : StandardAbilityAction, IBindGame, IDamageSource {

    [SerializeField] private Meteor meteorPrefab = null;

    [SerializeField] private Vector3 meteorFallOffset = new Vector3(0, 10f, 0);

    [SerializeField] private float meteorFallTargetOffset = 2f;

    [SerializeField] private Vector3 direction;
    public Vector3 Direction { get { return direction; } set { direction = value; } }

    [SerializeField] private Damage impactDamage;

    [SerializeField] private float fallDuration = 1.5f;
    public float FallDuration { get { return fallDuration; } set { fallDuration = value; } }

    [SerializeField] private float impactForce = 300f;
    public float ImpactForce { get { return impactForce; } set { impactForce = value; } }

    [SerializeField] private float impactRange = 5f;
    public float ImpactRange { get { return impactRange; }  set { impactRange = value; } }

    [SerializeField] private GameObject impactVfxPrefab = null;

    [SerializeField] private Damage rollDamage;

    [SerializeField] private float rollDuration = 3f;
    public float RollDuration { get { return rollDuration; } set { rollDuration = value; } }

    [SerializeField] private float rollSpeed = 1f;
    public float RollSpeed { get { return rollSpeed; } set { rollSpeed = value; } }

    [SerializeField] private float rotationalSpeed = 360f;

    [SerializeField] private float meteorRadius = 1f;

    [SerializeField]
    private Map map = null;

    protected override IEnumerator PerformAction() {

        Transform origin = actor.transform;
        Meteor meteor = Instantiate<Meteor>(meteorPrefab);
        meteor.RollDamage.DamageSource = this;

        // fall
        Vector3 fallStart = origin.position + meteorFallOffset;
        Vector3 fallEnd = origin.position + direction * meteorFallTargetOffset + Vector3.up * meteorRadius;
        float fallTime = 0;
        while(fallTime < fallDuration) {
            float t = Mathf.Clamp01(fallTime / fallDuration);

            meteor.transform.position = Vector3.Lerp(fallStart, fallEnd, t);
            meteor.transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * rotationalSpeed, meteor.transform.right);

            yield return new WaitForEndOfFrame();
            fallTime += Time.deltaTime;
        }
        meteor.transform.position = fallEnd;

        // impact
        if (impactVfxPrefab != null) {
            GameObject impactVfx = Instantiate<GameObject>(impactVfxPrefab);
            impactVfx.transform.position = fallEnd;
        }
        Collider[] colliders = Physics.OverlapSphere(fallEnd, impactRange);
        List<Health> impacted = new List<Collider>(colliders)
            .ConvertAll<Health>(c => c.GetComponentInParent<Health>())
            .FindAll(h => h != null && h != (actor.Health as Health));
        foreach(Health impactedHealth in impacted) {
            impactDamage.DoDamage(impactedHealth);
        }
        List<Rigidbody> impactedRbs = new List<Collider>(colliders)
            .ConvertAll<Rigidbody>(c => c.GetComponentInParent<Rigidbody>())
            .FindAll(rb => rb != null && rb != actor.GetComponent<Rigidbody>());
        foreach (Rigidbody impactedRb in impactedRbs) {
            UnitMovement movement = impactedRb.GetComponent<UnitMovement>();
            Vector3 force = impactForce * (impactedRb.position - fallEnd).normalized;
            if(movement != null) {
                movement.AddForce(actor, force);
            } else {
                impactedRb.AddForce(force);
            }
        }

        // roll
        float adjRollDuration = rollDuration * actor.AbilityRangeMultiplier.Current;
        Vector3 rollStart = fallEnd;
        Vector3 rollEnd = rollStart + direction * adjRollDuration * rollSpeed;
        float rollTime = 0;
        while (rollTime < adjRollDuration) {
            float t = Mathf.Clamp01(rollTime / adjRollDuration);

            Vector3 pos = Vector3.Lerp(rollStart, rollEnd, t);
            Vector3 mapPos, normal;
            map.GetMapPointFromWorldPoint(pos, out mapPos, out normal); // hug map floor
            meteor.transform.position = mapPos + normal * meteorRadius;
            meteor.transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * rotationalSpeed, meteor.transform.right);

            yield return new WaitForEndOfFrame();
            rollTime += Time.deltaTime;
        }

        Destroy(meteor.gameObject);

        yield break;
    }

    public Unit DamagingUnit { get { return actor; } }

    public virtual void Bind(Game game) {
        map = game.Map;
    }
}
