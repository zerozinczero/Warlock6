using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public struct UpgradeTransfer {

    [System.Serializable]
    public struct TransferData {
        public string component;
        public List<string> properties;
    }

    public List<TransferData> transferData;
	

    public void Transfer(GameObject from, GameObject to) {
        foreach(TransferData data in transferData) {
            Component fromc = from.GetComponent(data.component);
            Component toc = to.GetComponent(data.component);
            System.Type type = System.Type.GetType(data.component);
            //FieldInfo[] fields = fromc.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (string fieldName in data.properties) {
                FieldInfo field = type.GetField(fieldName,
                   BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null) {
                    Debug.LogWarningFormat("Data transfer unable to find property {0}.{1}", data.component, fieldName);
                    continue;
                }
                object fromv = field.GetValue(fromc);
                field.SetValue(toc, fromv);
            }
        }
    }
}
