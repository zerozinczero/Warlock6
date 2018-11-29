using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AreaDamageOverTime : MonoBehaviour, IDamageSource {

    [SerializeField]
    private float damagePerSecond = 10f;

    [SerializeField]
    private DamageType damageType = DamageType.Normal;

    [SerializeField]
    private List<Health> healthsInArea = null;

    public void Clear() {
        healthsInArea.Clear();
    }

    private void OnTriggerEnter(Collider other) {
        Health health = other.GetComponent<Health>();
        if(health != null && !healthsInArea.Contains(health)) {
            healthsInArea.Add(health);
        }
    }

    private void OnTriggerStay(Collider other) {
        Health health = other.GetComponent<Health>();
        if (health != null && !healthsInArea.Contains(health)) {
            healthsInArea.Add(health);
        }
    }

    private void OnTriggerExit(Collider other) {
        Health health = other.GetComponent<Health>();
        if (health != null && healthsInArea.Contains(health)) {
            healthsInArea.Remove(health);
        }
    }

    void Update() {
        foreach(Health health in healthsInArea) {
            TryDamage(health, Time.deltaTime);
        }
    }

    void TryDamage(IHealth health, float deltaTime) {
        if (health != null) {
            float damageAmount = damagePerSecond * deltaTime;
            DamageInfo damageInfo = new DamageInfo(this, damageAmount, damageType);
            health.Damage(damageInfo);
        }
    }

    public Unit DamagingUnit { get { return null; } }
}
