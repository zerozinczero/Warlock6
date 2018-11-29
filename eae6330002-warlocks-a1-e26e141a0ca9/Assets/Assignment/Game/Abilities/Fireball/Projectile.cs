using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour {

    public class CollisionEvent : UnityEvent<Projectile, Collider> {}
    public class ProjectileEvent : UnityEvent<Projectile> {}

    [SerializeField] private float speed = 5f;
    public float Speed { get { return speed; } }

    [SerializeField] private float maxLifetime = 5f;
    public float MaxLifetime { get { return maxLifetime; } set { maxLifetime = value; } }

    [SerializeField] private float collisionForce = 100f;    // Newtons
    public float CollisionForce { get { return collisionForce; } }

    [SerializeField] private Unit owner = null;
    public Unit Owner { get { return owner; } set { owner = value; } }

    public UnityEvent OnCollision = null;

    private float lifetime = 0f;
    private bool isAlive = true;
    public bool IsAlive { get { return isAlive; } }

	void Update () {
        if (!isAlive)
            return;

        lifetime += Time.deltaTime;
        if(lifetime >= maxLifetime) {
            isAlive = false;
        }

        transform.position += transform.forward * (speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider coll) {

        UnitMovement movement = coll.GetComponentInParent<UnitMovement>();
        if (movement != null && !movement.Pushable.Current)
            return;

        bool validCollision = false;
        Health health = coll.GetComponentInParent<Health>();
        if(health != null) {
            Damage damage = GetComponent<Damage>();
            float actualDamage = damage.DoDamage(health);
            validCollision = true;
        }

        Rigidbody rb = coll.GetComponentInParent<Rigidbody>();
        if(rb != null) {
            Vector3 delta = coll.transform.position - transform.position;
            Vector3 dir = delta.normalized;
            Vector3 force = collisionForce * dir;
            rb.AddForce(force);
            //validCollision = true;
        }

        if(validCollision) {
            isAlive = false;
        }
    }

    public void Reset() {
        isAlive = true;
        lifetime = 0;
    }
}
