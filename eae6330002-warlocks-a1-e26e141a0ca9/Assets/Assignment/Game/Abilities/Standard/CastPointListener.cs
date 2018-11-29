using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CastPointListener : MonoBehaviour {

    [SerializeField]
    private bool castPointHit = false;
    public bool CastPointHit { get { return castPointHit; } }

    [SerializeField]
    private UnityEvent onCastPointHit = null;
    public void Subscribe(UnityAction callback) {
        onCastPointHit.AddListener(callback);
    }

    void OnCastPoint() {
        if (!castPointHit) {
            castPointHit = true;
            if (onCastPointHit != null) {
                onCastPointHit.Invoke();
            }
        }
    }
	
}
