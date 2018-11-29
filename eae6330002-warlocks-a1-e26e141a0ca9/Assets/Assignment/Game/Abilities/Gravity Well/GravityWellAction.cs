using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWellAction : StandardAbilityAction {

    [SerializeField] private GameObject vfxPrefab = null;

    [SerializeField] private Vector3 vfxOffset = new Vector3(0, 0.5f, 0);

    [SerializeField] private Vector3 location;
    public Vector3 Location { get { return location; } set { location = value; } }

    [SerializeField] private float duration = 5f;
    public float Duration { get { return duration; } set { duration = value; } }

    [SerializeField] private float setupTime = 1f;
    public float SetupTime { get { return setupTime; } set { setupTime = value; } }

    [SerializeField] private float innerRadius = 1f;
    public float InnerRadius { get { return innerRadius; } set { innerRadius = value; } }

    [SerializeField] private float innerForce = 300f;
    public float InnerForce { get { return innerForce; } set { innerForce = value; } }

    [SerializeField] private float outerRadius = 5f;
    public float OuterRadius { get { return outerRadius; } set { outerRadius = value; } }

    [SerializeField] private float outerForce = 100f;
    public float OuterForce { get { return outerForce; } set { outerForce = value; } }

    [SerializeField] private AnimationCurve forceCurve = AnimationCurve.Linear(0, 0, 1, 1);

    protected override IEnumerator PerformAction() {

        yield return new WaitForSeconds(setupTime);

        float adjOuterRadius = outerRadius * actor.AbilityRangeMultiplier.Current;
        GameObject vfx = Instantiate<GameObject>(vfxPrefab);
        vfx.transform.position = location + vfxOffset;
        vfx.transform.localScale = adjOuterRadius * Vector3.one;

        float time = 0;
        while(time < duration) {
            Collider[] colliders = Physics.OverlapSphere(location, adjOuterRadius);
            List<Rigidbody> rbs = new List<Collider>(colliders)
                                    .ConvertAll<Rigidbody>((c) => c.GetComponentInParent<Rigidbody>())
                                    .FindAll((rb) => rb != null);
            foreach(Rigidbody rb in rbs) {
                Vector3 delta = rb.position - location;
                Vector3 dir = delta.normalized;
                float dist = delta.magnitude;
                float t = Mathf.Clamp01(dist / (adjOuterRadius - innerRadius));
                float newtons = forceCurve.Evaluate(t) * (innerForce - outerForce) + innerForce;
                Vector3 force = -dir * newtons;
                rb.AddForce(force * Time.deltaTime);
            }

            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        Destroy(vfx);

        yield break;
    }
}
