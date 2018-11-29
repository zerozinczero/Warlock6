using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAction : StandardAbilityAction {

    [SerializeField] private float maxDistance = 5f;
    public float MaxDistance { get { return maxDistance; } set { maxDistance = value; } }

    [SerializeField] private float collisionCheckRadius = 0.5f;

    [SerializeField] private Vector3 target;
    public Vector3 Target { get { return target; }  set { target = value; } }

    [SerializeField] private GameObject poofPrefab = null;

    protected override IEnumerator PerformAction() {

        Rigidbody actorRb = actor.GetComponent<Rigidbody>();
        float dist = target.magnitude;
        Transform origin = transform;
        Vector3 destination = target;
        float adjMaxDistance = maxDistance * actor.AbilityRangeMultiplier.Current;
        Vector3 dir = Vector3.Normalize(target - origin.position);
        if (dist > adjMaxDistance) {
            destination = dir * adjMaxDistance + origin.position;
        }

        // collision detection
        bool validDestinationFound = false;
        do {
            Collider[] colliders = Physics.OverlapSphere(destination, collisionCheckRadius);
            List<Rigidbody> rbs = new List<Collider>(colliders)
                                    .ConvertAll<Rigidbody>((c) => c.GetComponentInParent<Rigidbody>())
                                    .FindAll((rb) => rb != null);
            if (rbs.Count > 0) {
                Vector3 newDestination = destination - dir * 2f * collisionCheckRadius;
                if(Vector3.Dot(destination-origin.position, newDestination-origin.position) <= 0) {
                    destination = origin.position;
                    validDestinationFound = true;
                } else {
                    destination = newDestination;
                }
            } else {
                validDestinationFound = true;
            }
        } while (!validDestinationFound);

        if (poofPrefab != null) {
            GameObject startPoof = Instantiate(poofPrefab);
            startPoof.transform.position = actor.transform.position;
            GameObject destPoof = Instantiate(poofPrefab);
            destPoof.transform.position = destination;
        }

        actorRb.MovePosition(destination);

        yield break;
    }

}
