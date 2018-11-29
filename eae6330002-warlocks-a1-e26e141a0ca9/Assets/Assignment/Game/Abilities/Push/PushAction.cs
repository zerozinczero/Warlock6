using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushAction : StandardAbilityAction {
    
    [SerializeField]
    private Vector3 force = Vector3.zero;

    [SerializeField]
    private float radius = 1f;

    [SerializeField]
    private float range = 1f;

    [SerializeField]
    private float height = 2f;

    [SerializeField]
    private GameObject castVfxPrefab = null;

    [SerializeField]
    private GameObject impactVfxPrefab = null;

    protected override IEnumerator PerformAction() {

        // cast VFX
        GameObject castVfx = Instantiate(castVfxPrefab);
        castVfx.transform.position = actor.ChestTransform.position + actor.transform.forward;

        Vector3 bottom = actor.transform.position;
        Vector3 top = bottom + actor.transform.up * height;
        Vector3 dir = actor.transform.forward;
        RaycastHit[] hits = Physics.CapsuleCastAll(bottom, top, radius, dir, range);
        foreach(RaycastHit raycastHit in hits) {
            if(raycastHit.rigidbody != null) {
                Unit impactedUnit = raycastHit.rigidbody.GetComponent<Unit>();
                if(impactedUnit == actor) {
                    continue;   // don't target self
                }
                Vector3 worldForce = transform.rotation * force;
                raycastHit.rigidbody.AddForce(worldForce);

                GameObject impactVfx = Instantiate(impactVfxPrefab);
                impactVfx.transform.position = raycastHit.point;
            }
        }

        yield break;
    }
}
