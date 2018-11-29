using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ObjectPool<T,E> : MonoBehaviour where T : Object where E : UnityEvent<T> {

    [SerializeField]
    private List<T> pool = new List<T>();

    [SerializeField]
    private T prefab = null;
    public T Prefab {
        get { return prefab; }
        set {
            prefab = value;
            Resize(0);
            Resize(initialSize);
        }
    }

    [SerializeField]
    private bool autoCreate = true;

    [SerializeField]
    private int initialSize = 1;

    [SerializeField]
    private E prepareAction = default(E);

    [SerializeField]
    private E returnAction = default(E);

    [SerializeField]
    private E freeAction = default(E);

    private void Start() {
        Resize(initialSize);
    }

    public void Resize(int count) {
        // if pool is too small, create new instances
        for (int i = pool.Count; i < count; i++) {
            Return(Create());
        }

        // if pool is too big, free excess
        for (int i = pool.Count - 1; i >= count; i--) {
            Free(pool[i]);
        }
    }

    private T Create() {
        T obj = Instantiate<T>(prefab, transform);
        return obj;
    }

    protected virtual void OnFree(T obj) {}

    private void Free(T obj) {
        pool.Remove(obj);
        OnFree(obj);
        if(freeAction != null) {
            freeAction.Invoke(obj);
        }
    }

    protected virtual void OnGet(T obj) {}

    public T Get() {
        T obj = null;
        if(pool.Count > 0) {
            obj = pool[0];
            pool.RemoveAt(0);
        } else if(autoCreate) {
            obj = Create();
        }

        if (obj != null) {
            OnGet(obj);

            if (prepareAction != null) {
                prepareAction.Invoke(obj);
            }
        }

        return obj;
    }

    public T Get(System.Predicate<T> predicate) {
        T obj = pool.Find(predicate);
        if(obj != null) {
            pool.Remove(obj);
            OnGet(obj);
            if (prepareAction != null) {
                prepareAction.Invoke(obj);
            }
        }
        return obj;
    }

    protected virtual void OnReturn(T obj) {}

    public void Return(T obj) {
        OnReturn(obj);
        if(returnAction != null) {
            returnAction.Invoke(obj);
        }
        pool.Add(obj);
    }
	
}
