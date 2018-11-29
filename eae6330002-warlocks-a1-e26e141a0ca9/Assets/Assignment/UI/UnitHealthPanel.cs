using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthPanel : MonoBehaviour {

    [SerializeField]
    private Text unitName = null;

    [SerializeField]
    private Slider healthBar = null;

    [SerializeField]
    private Text statusLabel = null;

    public void Set(string name, float healthPercentage, string status, Color color) {
        unitName.text = name;
        unitName.color = color;
        healthBar.value = healthPercentage;
        statusLabel.text = status;
        statusLabel.color = color;
    }
}
