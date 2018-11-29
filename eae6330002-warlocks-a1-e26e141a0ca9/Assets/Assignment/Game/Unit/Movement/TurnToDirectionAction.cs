using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToDirectionAction : GameAction {

    [SerializeField]
    private Vector3 direction = Vector3.forward;
    public Vector3 Direction { get { return direction; } set { direction = value; } }

    [SerializeField]
    private Transform target = null;

    [SerializeField]
    private float turnSpeed = 360f;

    protected Coroutine actionInProgress = null;
    public override bool IsInProgress { get { return actionInProgress != null; } }

    public override void Perform() {
        actionInProgress = StartCoroutine(Perform_Coroutine());
    }

    private IEnumerator Perform_Coroutine() {
        Quaternion q = Quaternion.LookRotation(direction, target.up);
        while(Quaternion.Angle(target.rotation, q) > turnSpeed * Time.deltaTime) {
            Quaternion r = Quaternion.RotateTowards(target.rotation, q, turnSpeed * Time.deltaTime);
            target.rotation = r;
            yield return new WaitForEndOfFrame();
        }

        target.rotation = q;
        if(OnPerformed != null) {
            OnPerformed.Invoke(this);
        }

        actionInProgress = null;
    }

    public override void Stop() {
        StopAllCoroutines();
        if(OnCancelled != null) {
            OnCancelled.Invoke(this);
        }
    }


}
