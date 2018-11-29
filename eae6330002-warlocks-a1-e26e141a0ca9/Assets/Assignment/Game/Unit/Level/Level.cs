using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    [SerializeField] private int current = 0;
    public int Current {
        get { return current; }
        set {
            current = Mathf.Min(value, max);
            if (OnLevelChanged != null) {
                OnLevelChanged.Invoke(current);
            }
        }
    }

    [SerializeField] private int max = 3;
    public int Max {
        get { return max; }
        set { max = value; }
    }

    public IntEvent OnLevelChanged = null;

    private void Awake() {
        if(OnLevelChanged != null) {
            OnLevelChanged.Invoke(current);
        }
    }
}
