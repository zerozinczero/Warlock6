using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {

    [SerializeField] private Transform aliveParent = null;
    public Transform AliveParent { get { return aliveParent; } }

    [SerializeField] private Transform ragdollParent = null;
    public Transform RagdollParent { get { return ragdollParent; } }

    public void Show() {
        AlignSubtree(aliveParent, ragdollParent);
        aliveParent.gameObject.SetActive(false);
        ragdollParent.gameObject.SetActive(true);
    }

    public void Hide() {
        ragdollParent.gameObject.SetActive(false);
        aliveParent.gameObject.SetActive(true);
    }

    private void AlignSubtree(Transform a, Transform b) {
        for (int i = 0; i < a.childCount; i++) {
            Transform aChild = a.GetChild(i);
            Transform bChild = b.Find(aChild.name);

            if (bChild != null) {
                bChild.position = aChild.position;
                bChild.rotation = aChild.rotation;

                AlignSubtree(aChild, bChild);
            }

        }
    }
	
}
