using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherAction : StandardAbilityAction {

    [SerializeField] private Vector3 castPoint;
    public Vector3 CastPoint { get { return castPoint; } set { castPoint = value; } }

    [SerializeField] private float castRadius = 5;
    public float CastRadius { get { return castRadius; } set { castRadius = value; } }

    [SerializeField] private float bindRadius = 4;
    public float BindRadius { get { return bindRadius; } set { bindRadius = value; } }

    [SerializeField] private float maxTetherDist = 4;
    public float MaxTetherDist { get { return maxTetherDist; } set { maxTetherDist = value; } }

    [SerializeField] private float duration = 3;
    public float Duration { get { return duration; } set { duration = value; } }

    [SerializeField] private ParticleSystem vfxPrefab = null;

    protected override IEnumerator PerformAction() {

        // randomly select 2 units in the bind area
        float adjBindRadius = bindRadius * actor.AbilityRangeMultiplier.Current;
        Collider[] colliders = Physics.OverlapSphere(castPoint, adjBindRadius);
        List<Unit> units = new List<Collider>(colliders).ConvertAll<Unit>(c => c.GetComponentInParent<Unit>()).FindAll(u => u != null && u.Health.IsAlive);
        // TODO: shuffle
        if (units.Count > 2)
            units.RemoveRange(2, units.Count - 2);
        else if (units.Count < 2) {
            Debug.Log("Not enough targets to tether");
            yield break;
        }

        // bind the 2 units together
        List<Rigidbody> rbs = units.ConvertAll<Rigidbody>(u => u.GetComponent<Rigidbody>());
        SpringJoint springJoint = rbs[0].gameObject.AddComponent<SpringJoint>();
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedBody = rbs[1];
        springJoint.connectedAnchor = Vector3.zero;
        springJoint.spring = 2;
        springJoint.enableCollision = true;

        // add a visual effect
        ParticleSystem vfx = Instantiate<ParticleSystem>(vfxPrefab);

        float time = 0;
        while (time < duration) {
            vfx.transform.position = units[0].ChestTransform.position;
            vfx.transform.LookAt(units[1].ChestTransform);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        // tether ended, clean up
        Destroy(vfx.gameObject);
        Destroy(springJoint);

        yield break;
    }

}
