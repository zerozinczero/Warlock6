using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {

    [SerializeField]
    private Camera minimapCamera = null;

    [SerializeField]
    private Camera mainCamera = null;

    [SerializeField]
    private RawImage miniMapImage = null;

    [SerializeField]
    private string[] raycastLayers = new string[] { "Terrain" };

    [SerializeField]
    private float maxRaycastDistance = 100f;

    private int layerMask;

    private void Start() {
        layerMask = LayerMask.GetMask(raycastLayers);
    }

    private void ScrollCamera(Vector3 viewportPosition) {
        Ray minimapToWorldRay = minimapCamera.ViewportPointToRay(viewportPosition);
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(minimapToWorldRay, out raycastHit, maxRaycastDistance, layerMask);
        if (hit) {
            Vector3 cameraTarget = raycastHit.point;

            Ray mainCameraToWorldRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

            hit = Physics.Raycast(mainCameraToWorldRay, out raycastHit, maxRaycastDistance, layerMask);
            if (hit) {
                Vector3 mainCameraCurrentTarget = raycastHit.point;
                Vector3 delta = cameraTarget - mainCameraCurrentTarget;
                delta.y = 0;    // only tranlsate in the x-y plane
                mainCamera.transform.position += delta;
            }
        }
    }

    public void OnPointerDown(BaseEventData eventData) {
        UpdatePointerEvent(eventData as PointerEventData);
    }

    public void OnPointerDrag(BaseEventData eventData) {
        UpdatePointerEvent(eventData as PointerEventData);
    }

    void UpdatePointerEvent(PointerEventData pointerEventData) {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(miniMapImage.rectTransform, pointerEventData.position, null, out localPoint);
        Vector2 normalized = Rect.PointToNormalized(miniMapImage.rectTransform.rect, localPoint);
        Vector3 viewportPoint = new Vector3(normalized.x, normalized.y, 0);
        ScrollCamera(viewportPoint);
    }
}
