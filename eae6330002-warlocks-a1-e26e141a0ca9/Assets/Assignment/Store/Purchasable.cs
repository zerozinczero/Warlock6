using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Purchasable : ScriptableObject, IPurchasable {

    public abstract string Name { get; }
    public abstract Sprite Icon { get; }
    public abstract int Cost { get; }

    public abstract PurchasableCell CellPrefab { get; }

    public abstract bool CanPurchase();
    public abstract void OnPurchased();

    public abstract Unit Target { get; set; }
    public abstract Game Game { get; set; }

}
