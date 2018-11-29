using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : ClampedFloatStat, ICooldown {

    public float Duration { get { return cachedCurrent.max; } set { baseValue.max = value; Changed(); } }

    public float Remaining { get { return cachedCurrent.value; } }
    public float RemainingNormalized { get { return Percentage; } }

    public bool Ready { get { return Remaining <= 0f; } }

    public virtual void Activate() {
        cachedCurrent.value = cachedCurrent.max;
        baseValue.value = Percentage * (baseValue.max - baseValue.min) + baseValue.min;
    }

    public virtual void Reset() {
        cachedCurrent.value = 0f;
        baseValue.value = 0f;
    }

    void Update() {
        cachedCurrent.value -= Mathf.Min(Time.deltaTime, cachedCurrent.value);
        baseValue.value = Percentage * (baseValue.max - baseValue.min) + baseValue.min;
	}

}
