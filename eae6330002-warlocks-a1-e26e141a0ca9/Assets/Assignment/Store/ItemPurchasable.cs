using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Purchasable", menuName = "Warlocks/Item Purchasable")]
public class ItemPurchasable : Purchasable, IUpgradePurchasable {

    [SerializeField] private Unit target = null;
    public override Unit Target {
        get { return target; }
        set {
            target = value;
            UpdateCurrentUpgrade();
        }
    }

    [HideInInspector][SerializeField] private int maxItems = 4;

    [SerializeField] private Game game = null;
    public override Game Game { get { return game; } set { game = value; } }

    [SerializeField] private UpgradeCell cellPrefab = null;
    public override PurchasableCell CellPrefab { get { return cellPrefab; } }

    [SerializeField] private Item itemPrefab = null;
    [SerializeField] private List<int> upgradeCosts = new List<int>();

    public int UpgradeCount { get { return upgradeCosts.Count; } }

    [SerializeField] private int currentUpgrade = -1;
    public int CurrentUpgrade { get { return currentUpgrade; } }

    public override string Name { get { return itemPrefab.Name; } }
    public override Sprite Icon { get { return itemPrefab.Icon; } }
    public override int Cost { get { return upgradeCosts[Mathf.Clamp(currentUpgrade, 0, UpgradeCount - 1)]; } }

    public override bool CanPurchase() {
        return currentUpgrade < UpgradeCount && target.Items.Count < maxItems;
    }

    public override void OnPurchased() {
        currentUpgrade++;

        // upgrade unit item
        Item targetCurrentItem = target.Items.Find(i => i.Id == itemPrefab.Id);
        if (targetCurrentItem == null) {
            // unit doesn't have this item yet, add it
            targetCurrentItem = Instantiate<Item>(itemPrefab, target.ItemsParent);
            target.Items.Add(targetCurrentItem);
            Bind(targetCurrentItem);
        }

        Level level = targetCurrentItem.GetComponent<Level>();
        level.Current = currentUpgrade;

        targetCurrentItem.Equip();

        game.InGameUi.AbilitiesHud.UpdateUnit(target);  // TODO: target.Modified(); will trigger event that UI is listening for, remove UI dependency from this class
    }

    private void UpdateCurrentUpgrade() {
        // update currentUpgrade to match what unit has
        Item targetCurrentItem = target.Items.Find(i => i.Id == itemPrefab.Id);
        if (targetCurrentItem == null) {
            currentUpgrade = 0;
            return;
        }

        Level level = targetCurrentItem.GetComponent<Level>();
        currentUpgrade = level.Current;
    }

    private void Bind(Item item) {
        IBindGame[] gameBinds = item.GetComponentsInChildren<IBindGame>();
        foreach (IBindGame gameBind in gameBinds)
            gameBind.Bind(game);

        IBindUnit[] unitBinds = item.GetComponentsInChildren<IBindUnit>();
        foreach (IBindUnit unitBind in unitBinds)
            unitBind.Bind(target);
    }

}
