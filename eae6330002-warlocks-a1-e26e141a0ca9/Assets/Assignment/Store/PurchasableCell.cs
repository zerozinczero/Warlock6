using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PurchasableCell : MonoBehaviour {

    [SerializeField]
    private IPurchasable purchasable = null;

    [SerializeField]
    private Image icon = null;

    [SerializeField]
    private Text nameLabel = null;

    [SerializeField]
    private Text costLabel = null;

    [SerializeField]
    private Button buyButton = null;

    public PurchasableEvent OnPurchase = null;

    public virtual void SetPurchasable(IPurchasable purchasable) {
        this.purchasable = purchasable;
        icon.sprite = purchasable.Icon;
        nameLabel.text = purchasable.Name;
        costLabel.text = purchasable.Cost.ToString();
        buyButton.interactable = purchasable.CanPurchase();
    }

    public void OnPurchasableClicked() {
        if(OnPurchase != null) {
            OnPurchase.Invoke(purchasable);
        }
    }
	
}
