using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

    [SerializeField] private GameObject damageSource = null;
    public IDamageSource DamageSource {
        get { return damageSource != null ? damageSource.GetComponent<IDamageSource>() : null; }
        set { damageSource = value != null ? (value as Component).gameObject : null; }
    }

    [SerializeField] private float amount = 10f;
    public float Amount { get { return amount; } set { amount = value; } }

    [SerializeField] private DamageType damageType = DamageType.Normal;
    public DamageType DamageType { get { return damageType; } set { damageType = value; } }

    public float DoDamage(Health target) {
        return target.Damage(new DamageInfo(DamageSource, amount, damageType));
    }

    public float DoDamage(Health target, float multiplier) {
        return target.Damage(new DamageInfo(DamageSource, amount * multiplier, damageType));
    }
	
}
