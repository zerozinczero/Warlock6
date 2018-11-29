using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbilityTargeting<T,U> : MonoBehaviour, ITargeting<T,U> {
    
    public abstract void StartTargeting();
    public abstract void CancelTargeting();

    [SerializeField]
    protected T onTargeted = default(T);
    public T OnTargeted { get { return onTargeted; } }

    [SerializeField]
    protected U onCancelled = default(U);
    public U OnCancelled { get { return onCancelled; } }
	
}
