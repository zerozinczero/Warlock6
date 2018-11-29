using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Transform target = null;

    [SerializeField]
    private Vector2 zoomRange = new Vector2(10, 100);

    [SerializeField]
    private float zoomRate = 1f;   // meters per wheel degree scrolled

    [SerializeField]
    private Vector3 panMin = new Vector3(-30, 10, -30);

    [SerializeField]
    private Vector3 panMax = new Vector3(30, 100, 30);

    [SerializeField]
    private float panRate = 30f;

    [SerializeField]
    private float panMargin = 10f;

    private void Update() {
        UpdateZoom();
        UpdatePan();
    }

    void UpdateZoom() {
        if(!Equals(Input.mouseScrollDelta.y, 0f)) {
            Vector3 forward = target.forward;
            Vector3 delta = Input.mouseScrollDelta.y * zoomRate * forward;

            Vector3 newPosition = transform.position + delta;
            if(newPosition.y >= zoomRange.x && newPosition.y <= zoomRange.y) {
                transform.position = newPosition;
            }
        }
    }

    void UpdatePan() {
        float width = Screen.width;
        float height = Screen.height;

        Vector3 right = transform.right;
        Vector3 up = Vector3.Cross(Vector3.up, right);
        Vector3 delta = Vector3.zero;

        Vector3 mousePosition = Input.mousePosition;

        if(mousePosition.x >= 0 && mousePosition.x <= panMargin) {
            delta += -panRate * Time.deltaTime * right;
        } else if(mousePosition.x < width && mousePosition.x >= width - panMargin) {
            delta += panRate * Time.deltaTime * right;
        }

        if(mousePosition.y >= 0 && mousePosition.y <= panMargin) {
            delta += panRate * Time.deltaTime * up;
        } else if(mousePosition.y < height && mousePosition.y >= height - panMargin) {
            delta += -panRate * Time.deltaTime * up;
        }

        Vector3 newPosition = transform.position + delta;
        newPosition = new Vector3(
            Mathf.Clamp(newPosition.x, panMin.x, panMax.x),
            Mathf.Clamp(newPosition.y, panMin.y, panMax.y),
            Mathf.Clamp(newPosition.z, panMin.z, panMax.z)
        );
        transform.position = newPosition;
    }

}
