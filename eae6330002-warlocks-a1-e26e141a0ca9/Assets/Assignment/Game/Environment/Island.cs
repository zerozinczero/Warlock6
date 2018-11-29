using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Island : MonoBehaviour {

    [System.Serializable]
    public class IslandEvent : UnityEvent<Island> {}

    [SerializeField]
    private Terrain terrain = null;

    [SerializeField]
    private Map map = null;

    [SerializeField]
    private float height = 0.1f;

    [SerializeField]
    private float maxRadius = 50;
    public float MaxRadius { get { return maxRadius; } }

    [SerializeField]
    private float falloffDist = 3f;

    [SerializeField]
    private AnimationCurve falloffCurve = null;

    [SerializeField]
    [HideInInspector]
    private float radius = 20f;
    public float Radius {
        get { return radius; }
        set {
            if (!Equals(radius, value)) {
                radius = value;
                ChangeIslandSize(radius);

            }
        }
    }

    [SerializeField]
    private IslandEvent onIslandSizeChanged = null;
    public IslandEvent OnIslandSizeChanged { get { return onIslandSizeChanged; } }

    private void Start() {
        CreateRuntimeTerrain();
    }

    private void Reset() {
        ChangeIslandSize(radius);
    }

    private void ChangeIslandSize(float size) {
        float worldToSample = (terrain.terrainData.heightmapResolution / terrain.terrainData.size.x);
        int samples = Mathf.CeilToInt(2*(maxRadius + falloffDist) * worldToSample);
        int offsetX = terrain.terrainData.heightmapWidth / 2 - samples / 2;
        int offsetY = terrain.terrainData.heightmapHeight / 2 - samples / 2;

        float[,] heights = terrain.terrainData.GetHeights(offsetX, offsetY, samples, samples);
        for (int x = 0; x < samples; x++) {
            for (int y = 0; y < samples; y++) {
                float dist = (new Vector2(x - samples/2, y - samples/2) / worldToSample).magnitude;
                //heights[x, y] = dist <= size ? height : 0;
                float t = Mathf.Clamp01((size - dist) / falloffDist);
                float h = falloffCurve.Evaluate(t) * height;
                heights[x, y] = h;
            }
        }

        terrain.terrainData.SetHeights(offsetX, offsetY, heights);

        //navMeshSurface.BuildNavMesh();

        if(onIslandSizeChanged != null) {
            onIslandSizeChanged.Invoke(this);
        }
    }

    private void CreateRuntimeTerrain() {
        TerrainData newTerrainData = Instantiate<TerrainData>(terrain.terrainData);
        Terrain.activeTerrain.terrainData = newTerrainData;
        terrain.terrainData = newTerrainData;
        TerrainCollider terrainCollider = terrain.GetComponent<TerrainCollider>();
        terrainCollider.terrainData = newTerrainData;
    }

    public Vector3 GetRandomPosition() {
        Vector2 rand = Random.insideUnitCircle * radius;
        Vector3 pos = new Vector3(rand.x, 0, rand.y);
        Vector3 mapPos, mapNormal;
        map.GetMapPointFromWorldPoint(pos, out mapPos, out mapNormal);
        return mapPos;
    }

}
