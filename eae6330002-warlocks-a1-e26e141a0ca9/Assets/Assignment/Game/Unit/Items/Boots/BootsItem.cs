using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsItem : Item, IModifier<float> {

    [SerializeField] private float moveSpeedIncrease = 5f;
    public float MoveSpeedIncrease { get { return moveSpeedIncrease; } set { moveSpeedIncrease = value; } }
    [SerializeField] private FloatStat moveSpeed = null;

    public override void Equip() {
        moveSpeed.AddModifier(this);
    }

    public override void Unequip() {
        moveSpeed.RemoveModifier(this);
    }

    public float Modify(float speed, object context = null) {
        return speed + moveSpeedIncrease;
    }

    public int Priority { get { return 0; } }

    public override void Bind(Unit unit) {
        base.Bind(unit);
        moveSpeed = owner.Movement.MoveSpeed;
    }

    public override Dictionary<string, string> GetTokens() {
        return new Dictionary<string, string>() {
            { "{SPEED_INCREASE}", moveSpeedIncrease.ToString() }
        };
    }
}
