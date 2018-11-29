using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectPool : ObjectPool<GameObject,GameObjectEvent> {

    [SerializeField]
    private bool destroyOnFree = true;

    [SerializeField]
    private bool activate = true;

    protected override void OnGet(GameObject obj) {
        if(activate) {
            obj.SetActive(true);
        }
        obj.transform.SetParent(null);
    }

    protected override void OnReturn(GameObject obj) {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
    }

    protected override void OnFree(GameObject obj) {
        if(destroyOnFree) {
            Destroy(obj);
        }
    }

    public U Get<U>() where U : MonoBehaviour {
        GameObject obj = Get();
        U objU = null;
        if (obj != null) {
            objU = obj.GetComponent<U>();
        }
        return objU;
    }

    public void Return(MonoBehaviour component) {
        Return(component.gameObject);
    }

}
