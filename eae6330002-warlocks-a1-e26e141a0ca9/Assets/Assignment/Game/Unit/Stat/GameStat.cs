using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStat<T,M> : MonoBehaviour  where M : IModifier<T> {

    [SerializeField] private string statName = null;
    public string Name { get { return statName; } set { statName = value; } }

    [SerializeField] protected T baseValue = default(T);
    public T BaseValue { get { return baseValue; } set { baseValue = value; Changed(); } }

    public virtual T Current {
        get {
            return cachedCurrent;
        }
    }
    [SerializeField] protected T cachedCurrent = default(T);

    [SerializeField] private List<M> modifiers = new List<M>();

    private void Start() {
        Changed();
    }

    protected T CalcValue(T value, List<M> modifiers) {
        if (modifiers != null) {
            foreach (M modifier in modifiers) {
                value = modifier.Modify(value);
            }
        }

        return value;
    }

    public void AddModifier(M modifier) {
        if (!modifiers.Contains(modifier)) {
            modifiers.Add(modifier);
        }
        modifiers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        Changed();
    }

    public void RemoveModifier(M modifier) {
        modifiers.Remove(modifier);
        Changed();
    }

    public void Refresh() {
        Changed();
    }

    protected virtual void Changed() {
        cachedCurrent = CalcValue(baseValue, modifiers);
        if (OnChanged != null)
            OnChanged.Invoke(this);
    }

    public UnityEvent<GameStat<T,M>> OnChanged = null;
	
}
