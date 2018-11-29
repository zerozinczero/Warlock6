using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    [SerializeField]
    private Transform target = null;

	void Start () {
        if(target == null) {
            target = Camera.main.transform;
        }
	}

	void Update () {
        transform.LookAt(target);
	}
}
