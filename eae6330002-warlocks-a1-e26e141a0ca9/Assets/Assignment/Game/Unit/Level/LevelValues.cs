using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelValues<T, U> : MonoBehaviour where U : UnityEvent<T> {

    [SerializeField] private string desc = null;
    [SerializeField] private Level level = null;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private List<T> values = null;

    public U OnValueChanged = null;

    private void Awake() {
        if(level != null) {
            level.OnLevelChanged.AddListener(SetLevel);
            SetLevel(level.Current);
        }
    }

    public void SetLevel(int newLevel) {
        currentLevel = newLevel;

        T value = GetValue(newLevel);

        if(OnValueChanged != null) {
            OnValueChanged.Invoke(value);
        }
    }

    public T GetValue(int lvl) {
        T value = default(T);
        if (values != null || values.Count > 0) {
            value = values[Mathf.Clamp(lvl, 0, values.Count - 1)];
        }
        return value;
    }
	
    public T Current {
        get {
            return GetValue(currentLevel);
        }
    }
}
