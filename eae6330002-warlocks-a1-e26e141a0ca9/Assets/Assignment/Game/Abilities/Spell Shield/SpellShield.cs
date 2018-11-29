using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShield : MonoBehaviour {

    [SerializeField] private Unit caster = null;
    public Unit Caster { get { return caster; } set { caster = value; } }

    private void OnTriggerEnter(Collider other) {
        Projectile projectile = other.GetComponentInParent<Projectile>();
        if (projectile == null) return;
        if (projectile.Owner == caster) return;

        // reflect projectile
        Vector3 normal = (projectile.transform.position - transform.position).normalized;
        projectile.transform.forward = Vector3.Reflect(projectile.transform.forward, normal);
    }
}
