using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Map : MonoBehaviour {

    [SerializeField]
    private Terrain terrain = null;
    public Terrain Terrain { get { return terrain; } }

    [SerializeField]
	private Island island = null;
	public Island Island { get { return island; } }

    [SerializeField]
    private AreaDamageOverTime lava = null;
    public AreaDamageOverTime Lava { get { return lava; } }

    [SerializeField]
	private Transform spawnPointsParent = null;
	public Transform GetSpawnPoint(int index) {
		return spawnPointsParent.GetChild(index);
	}

    [SerializeField]
    private Collider mapCollider = null;

    private const float MAX_RAY_DIST = 1000f;
    private Vector3 HIGH_RAY = Vector3.up * MAX_RAY_DIST / 2f;

    public void SetIslandRadius(float radius) {
        island.Radius = radius;
        lava.gameObject.SetActive(true);
    }

    public bool GetMapPointFromScreenPoint(Vector3 screenPos, out Vector3 mapPos, out Vector3 mapNormal) {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit raycastHit;
        bool hit = mapCollider.Raycast(ray, out raycastHit, MAX_RAY_DIST);
        if(hit) {
            mapPos = raycastHit.point;
            mapNormal = raycastHit.normal;
        } else {
            mapPos = Vector3.zero;
            mapNormal = Vector3.up;
        }
        return hit;
    }

    public bool GetMapPointFromWorldPoint(Vector3 worldPos, out Vector3 mapPos, out Vector3 mapNormal) {
        Ray ray = new Ray(worldPos + HIGH_RAY, Vector3.down);
        RaycastHit raycastHit;
        bool hit = mapCollider.Raycast(ray, out raycastHit, MAX_RAY_DIST);
        if (hit) {
            mapPos = raycastHit.point;
            mapNormal = raycastHit.normal;
        } else {
            mapPos = Vector3.zero;
            mapNormal = Vector3.up;
        }
        return hit;
    }

}

	