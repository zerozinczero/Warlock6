using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    [SerializeField] private Damage rollDamage;
    public Damage RollDamage { get { return rollDamage; } }

    private void OnCollisionEnter(Collision collision) {
        HandleCollision(collision, Time.fixedDeltaTime);
    }

    private void OnCollisionStay(Collision collision) {
        HandleCollision(collision, Time.fixedDeltaTime);
    }

    private void HandleCollision(Collision collision, float elapsedTime) {
        Health health = collision.collider.GetComponentInParent<Health>();
        if(health != null) {
            rollDamage.DoDamage(health, elapsedTime);
        }
    }
}
