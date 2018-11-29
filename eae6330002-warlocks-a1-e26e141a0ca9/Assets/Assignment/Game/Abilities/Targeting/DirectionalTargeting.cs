using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalTargeting : PointTargeting {

    protected override void OnPointTargeted(Vector3 point) {
        Vector3 dir = Vector3.Normalize(point - origin.position);
        base.OnPointTargeted(dir);
    }

}
