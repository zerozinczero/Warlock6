using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScepterItem : Item, IModifier<float> {

    [SerializeField] private float rangeMultiplier = 1f;
    public float RangeMultiplier { get { return rangeMultiplier; } set { rangeMultiplier = value; } }

    public override void Equip() {
        owner.AbilityRangeMultiplier.AddModifier(this);
    }

    public override void Unequip() {
        owner.AbilityRangeMultiplier.RemoveModifier(this);
    }

    public float Modify(float range, object context = null) {
        range *= rangeMultiplier;
        return range;
    }

    public int Priority { get { return 0; } }

}