using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUi : MonoBehaviour {

    [SerializeField]
    private Text nameLabel = null;

    [SerializeField]
    private Slider healthBarSlider = null;

    public void OnHealthChanged(IHealthEvent healthEvent) {
        healthBarSlider.value = healthEvent.Target.Percent;
    }

    public void SetName(string name, Color color) {
        nameLabel.text = name;
        nameLabel.color = color;
    }
	
}
