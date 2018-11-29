using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItem : Item, IModifier<DamageEvent> {

	[SerializeField] private DamageType damageType = DamageType.Normal;
	[SerializeField] private float damageMultiplier = 1f;
	public float DamageMultiplier { get { return damageMultiplier; } set { damageMultiplier = value; } }
   
    public override void Equip() {
        owner.Health.AddDamageModifier(this);
    }

    public override void Unequip() {
        owner.Health.RemoveDamageModifier(this);
    }

    public DamageEvent Modify(DamageEvent damageEvent, object context = null) {
        if (damageEvent.DamageType != damageType)
            return damageEvent;
        damageEvent.Amount *= damageMultiplier;
        return damageEvent;
    }

    public int Priority { get { return 0; } }
    
}
