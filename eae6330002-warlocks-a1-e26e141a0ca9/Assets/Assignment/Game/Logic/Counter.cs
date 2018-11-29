using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour {

    [SerializeField]
    private int defaultValue = 0;

    [SerializeField]
    private List<string> keys = new List<string>();

    [SerializeField]
    private List<int> counts = new List<int>();

    public void Set(string key, int count) {
        int index = keys.IndexOf(key);
        if(index >= 0) {
            keys[index] = key;
            counts[index] = count;
        } else {
            keys.Add(key);
            counts.Add(count);
        }
    }

    public int Get(string key) {
        int index = keys.IndexOf(key);
        if (index >= 0) {
            return counts[index];
        } else {
            return defaultValue;
        }
    }

    public int Increment(string key) {
        int value = Get(key);
        value++;
        Set(key, value);
        return value;
    }

    public int Decrement(string key, int min = 0) {
        int value = Get(key);
        value = Mathf.Max(min, value - 1);
        Set(key, value);
        return value;
    }
	
}
