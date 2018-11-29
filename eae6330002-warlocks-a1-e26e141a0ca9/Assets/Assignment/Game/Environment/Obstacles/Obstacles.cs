using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {

    [SerializeField]
    private List<GameObject> prefabs = new List<GameObject>();

    [SerializeField]
    private List<GameObject> spawned = new List<GameObject>();

    [SerializeField]
    private float spawnAreaPercentage = 0.75f;

    [SerializeField]
    private int spawnMin = 2;

    [SerializeField]
    private int spawnMax = 6;

    [SerializeField]
    private Map map = null;

    public void Clear() {
        foreach(GameObject go in spawned) {
            Destroy(go);
        }
        spawned.Clear();
    }

    public void Spawn() {
        Spawn(Random.Range(spawnMin, spawnMax + 1));
    }

    public void Spawn(int count) {
        for (int i = 0; i < count; i++) {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
            GameObject go = Instantiate(prefab);
            Vector3 randomPoint = map.Island.GetRandomPosition();
            randomPoint.x *= spawnAreaPercentage;
            randomPoint.z *= spawnAreaPercentage;
            go.transform.position = randomPoint;
            go.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);

            spawned.Add(go);
        }
    }
}
