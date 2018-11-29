using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForCast : CustomYieldInstruction {

    private CastPointListener listener = null;

    public WaitForCast(Animator animator) {
        listener = animator.gameObject.AddComponent<CastPointListener>();
    }

    public override bool keepWaiting {
        get {
            bool wait = !listener.CastPointHit;
            if(!wait) {
                Object.Destroy(listener);
            }
            return wait;
        }
    }
	
}
