using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthAction : StandardAbilityAction {

    [SerializeField] private float duration = 5f;
    public float Duration { get { return duration; } set { duration = value; } }

    [SerializeField] private Transform renderersParent = null;

    [SerializeField] private bool invisible = false;

    [SerializeField] private float invisibleAlphaLocal = 0.5f;

    [SerializeField] private float invisibleAlphaHidden = 0f;

    private Dictionary<Renderer, Color> rendererAlphas = new Dictionary<Renderer, Color>();

    protected override IEnumerator PerformAction() {
        yield return StartCoroutine(GoInvisible());
    }

    private void SaveRenderers() {
        Renderer[] renderers = renderersParent.GetComponentsInChildren<Renderer>();
        rendererAlphas.Clear();
        foreach(Renderer r in renderers) {
            rendererAlphas.Add(r, r.material.color);
        }
    }

    IEnumerator Fade() {
        SaveRenderers();
        float time = 0;
        float targetAlpha = actor.Owner.IsLocal ? invisibleAlphaLocal : invisibleAlphaHidden;
        while (time < castTime) {
            float t = Mathf.Clamp01(time / castTime);
            foreach (Renderer r in rendererAlphas.Keys) {
                Color start = rendererAlphas[r];
                Color end = start;
                end.a = targetAlpha;
                r.material.color = Color.Lerp(start, end, t);
            }

            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        foreach (Renderer r in rendererAlphas.Keys) {
            Color end = rendererAlphas[r];
            end.a = targetAlpha;
            r.material.color = end;
        }
    }

    IEnumerator GoInvisible() {
        yield return StartCoroutine(Fade());
        invisible = true;

        float invisTime = 0;
        while(invisible && invisTime < duration) {
            yield return new WaitForEndOfFrame();
            invisTime += Time.deltaTime;
        }

        RestoreRenderers();
    }

    private void RestoreRenderers() {
        foreach (Renderer r in rendererAlphas.Keys) {
            r.material.color = rendererAlphas[r];
        }
    }

    public void OnActionPerformed() {
        invisible = false;
    }

    public void OnRevealInvisibleUnit() {
        invisible = false;
    }

    public override void Bind(Unit unit) {
        base.Bind(unit);
        renderersParent = unit.Ragdoll.AliveParent;
    }
}