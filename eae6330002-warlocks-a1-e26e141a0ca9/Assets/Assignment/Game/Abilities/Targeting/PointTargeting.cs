using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PointTargeting : AbilityTargeting<Vector3Event,UnityEvent>, IInputController, IBindGame, IBindUnit {

    [SerializeField]
    private int mouseButton = 0;

    [SerializeField]
    private string[] layers = new string[] { "Terrain" };

    [SerializeField]
    private Camera cam = null;

    [SerializeField]
    private InputManager inputManager = null;

    [SerializeField]
    protected Transform origin = null;
    public Transform Origin { get { return origin; } set { origin = value; } }

    [SerializeField]
    private InGameUi ui = null;

    [SerializeField]
    private string targetingMessage = "Select a target";

    [SerializeField]
    private Color messageColor = Color.yellow;

    [SerializeField]
    private Transform arrow = null;

    private int layerMask = 0;

    private Vector3? target = null;
    public Vector3? Target { get { return target; } set { target = value; } }

    private void Start() {
        layerMask = LayerMask.GetMask(layers);
        if(cam == null) {
            cam = Camera.main;
        }
    }

    public override void StartTargeting() {

        if (target.HasValue) {
            OnPointTargeted(target.Value);
        } else {
            enabled = true;
            inputManager.Add(this);
            if (ui != null) {
                ui.ShowMessage(targetingMessage, messageColor);
            }
            if (arrow != null) {
                arrow.gameObject.SetActive(true);
            }
        }
    }

    public override void CancelTargeting() {
        StopTargeting();
        if(OnCancelled != null) {
            OnCancelled.Invoke();
        }
    }

    private void StopTargeting() {
        if (enabled) {
            enabled = false;
            inputManager.Remove(this);
            if (ui != null) {
                ui.HideMessage();
            }
            if (arrow != null) {
                arrow.gameObject.SetActive(false);
            }
        }
    }

    public bool ProcessInput() {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(ray, out raycastHit, layerMask);

        if (hit && arrow != null) {
            Vector3 delta = raycastHit.point - origin.position;
            Vector3 dir = delta.normalized;
            arrow.forward = dir;
            arrow.position = origin.position;
        }

        if (Input.GetMouseButtonUp(mouseButton)) {

            if(hit) {
                OnPointTargeted(raycastHit.point);
            }
        } else if(Input.anyKeyDown && !Input.GetMouseButton(mouseButton)) {
            CancelTargeting();
        }
        return true;
    }

    protected virtual void OnPointTargeted(Vector3 point) {
        if (OnTargeted != null) {
            OnTargeted.Invoke(point);
            StopTargeting();
        }
    }

    public void Bind(Game game) {
        inputManager = game.InGameUi.InputManager;
        ui = game.InGameUi;
        cam = game.Camera;
        arrow = game.InGameUi.TargetingArrow;
    }

    public void Bind(Unit unit) {
        origin = unit.transform;
    }

}
