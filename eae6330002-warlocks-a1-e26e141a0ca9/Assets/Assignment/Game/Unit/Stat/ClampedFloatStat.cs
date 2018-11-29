using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ClampedFloat {
    public float value;
    public float min;
    public float max;
}

public class ClampedFloatStat : GameStat<ClampedFloat, IModifier<ClampedFloat>> {

    public float Percentage {
        get {
            float p = (cachedCurrent.value - cachedCurrent.min) / (cachedCurrent.max - cachedCurrent.min);
            return p;
        }
    }

}
