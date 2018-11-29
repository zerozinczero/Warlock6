using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour {

    [SerializeField] private Text nameLabel = null;

    [SerializeField] private Text descLabel = null;

    public void SetItem(Item item) {
        nameLabel.text = item.Name;
        descLabel.text = item.Description;
    }
	
}
