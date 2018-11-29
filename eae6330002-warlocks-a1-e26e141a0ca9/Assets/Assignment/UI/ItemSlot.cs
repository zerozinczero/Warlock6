using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour {

    [SerializeField] private Item item = null;

    [SerializeField] private Image icon = null;

    [SerializeField] private ItemTooltip tooltip = null;

    public void SetItem(Item item) {
        this.item = item;

        icon.sprite = item != null ? item.Icon : null;
    }

    public void OnHoverEnter(BaseEventData baseEventData) {
        if(item != null && item.TooltipPrefab != null) {
            tooltip = Instantiate<ItemTooltip>(item.TooltipPrefab, transform);
            tooltip.SetItem(item);
        }
    }

    public void OnHoverExit(BaseEventData baseEventData) {
        if(tooltip != null) {
            Destroy(tooltip.gameObject);
        }
    }

}
