using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCell : PurchasableCell {

    [SerializeField]
    private IUpgradePurchasable upgradePurchasable = null;

    [SerializeField]
    private List<Image> upgradeIcons = new List<Image>();

    private Color hasPurchasedColor = Color.white;
    private Color notPurchasedColor = new Color(0.5f, 0.5f, 0.5f);

    public override void SetPurchasable(IPurchasable purchasable) {
        base.SetPurchasable(purchasable);

        upgradePurchasable = purchasable as IUpgradePurchasable;
        int currentLevel = upgradePurchasable.CurrentUpgrade;
        for (int i = 0; i < upgradeIcons.Count; i++) {
            upgradeIcons[i].color = i < currentLevel ? hasPurchasedColor : notPurchasedColor;
        }
    }



}
