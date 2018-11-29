using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingItem : Item, IModifier<float> {

    [SerializeField] private float regenIncrease = 1f;
    public float RegenIncrease { get { return regenIncrease; } set { regenIncrease = value; } }

    public override void Equip() {
        owner.Health.Regen.AddModifier(this);
    }

    public override void Unequip() {
        owner.Health.Regen.RemoveModifier(this);
    }

    public float Modify(float regenAmount, object context = null) {
        regenAmount += regenIncrease;
        return regenAmount;
    }

    public int Priority { get { return 0; } }

}