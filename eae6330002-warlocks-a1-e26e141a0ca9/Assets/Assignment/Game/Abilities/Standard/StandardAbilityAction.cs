using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StandardAbilityAction : GameAction, IBindUnit {

    [SerializeField]
    protected Unit actor = null;

    [SerializeField]
    protected float castTime = 1f;
    public float CastTime { get { return castTime; } set { castTime = value; } }

    [SerializeField]
    protected string castTrigger = "Cast 1";

    [SerializeField]
    protected Animator animator = null;

    [SerializeField]
    private TurnToDirectionAction turnAction = null;

    protected Coroutine actionInProgress = null;
    public override bool IsInProgress { get { return actionInProgress != null; } }

    protected bool cancelled = false;

    public override void Perform() {
        cancelled = false;
        actionInProgress = StartCoroutine(Perform_Coroutine());
    }

    protected IEnumerator Perform_Coroutine() {

        yield return StartCoroutine(PreCast());

        if (!string.IsNullOrEmpty(castTrigger)) {
            animator.SetFloat("CastSpeed", 1f/castTime);
            animator.SetTrigger(castTrigger);
            yield return new WaitForCast(animator);
        }

        if(cancelled) {
            if(OnCancelled != null) {
                OnCancelled.Invoke(this);
            }
            yield break;
        }

        if(OnPerformed != null) {
            OnPerformed.Invoke(this);
        }

        yield return StartCoroutine(PerformAction());

        actionInProgress = null;
    }

    protected abstract IEnumerator PerformAction();

    public override void Stop() {
        cancelled = true;
    }

    protected virtual IEnumerator PreCast() {
        yield break;
    }

    protected IEnumerator TurnToDir(Vector3 dir) {
        if (turnAction != null) {
            turnAction.Direction = dir;
            turnAction.Perform();
            yield return new WaitWhile(() => turnAction.IsInProgress);
        }
    }

    public virtual void Bind(Unit unit) {
        actor = unit;
        animator = unit.GetComponent<Animator>();
    }

    public virtual Dictionary<string,string> GetPropertyTokens() {
        return null;
    }

}