using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchasable {

    string Name { get; }
    int Cost { get; }
    Sprite Icon { get; }

    PurchasableCell CellPrefab { get; }

    bool CanPurchase();
    void OnPurchased();

}
