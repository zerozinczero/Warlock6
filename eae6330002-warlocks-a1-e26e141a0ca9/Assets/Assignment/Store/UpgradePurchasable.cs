using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Purchasable", menuName = "Warlocks/Upgrade Purchasable")]
public class UpgradePurchasable : Purchasable, IUpgradePurchasable {

    [SerializeField] private Unit target = null;
    public override Unit Target {
        get { return target; }
        set {
            target = value;
            UpdateCurrentUpgrade();
        }
    }

    [SerializeField] private Game game = null;
    public override Game Game { get { return game; } set { game = value; } }

    [SerializeField] private UpgradeCell cellPrefab = null;
    public override PurchasableCell CellPrefab { get { return cellPrefab; } }

    [SerializeField] private Ability abilityPrefab = null;
    [SerializeField] private List<int> upgradeCosts = new List<int>();

    public int UpgradeCount { get { return upgradeCosts.Count; } }

    [SerializeField] private int currentUpgrade = 0;
    public int CurrentUpgrade { get { return currentUpgrade; } }

    public override string Name { get { return abilityPrefab.Name; } }
    public override Sprite Icon { get { return abilityPrefab.Icon; } }
    public override int Cost { get { return upgradeCosts[Mathf.Clamp(currentUpgrade, 0, UpgradeCount-1)]; } }

    public override bool CanPurchase() {
        return currentUpgrade < UpgradeCount;
    }

    public override void OnPurchased() {
        currentUpgrade++;

        // upgrade unit ability
        Ability targetCurrentAbility = target.Abilities.Find(a => a.Id == abilityPrefab.Id);
        if(targetCurrentAbility == null) {
            // unit doesn't have this ability yet, add it
            targetCurrentAbility = Instantiate<Ability>(abilityPrefab, target.AbilitiesParent);
            target.Abilities.Add(targetCurrentAbility);
            Bind(targetCurrentAbility);
        }
        Level level = targetCurrentAbility.GetComponent<Level>();
        level.Current = currentUpgrade;

        game.InGameUi.AbilitiesHud.UpdateUnit(target);  // TODO: target.Modified(); will trigger event that UI is listening for, remove UI dependency from this class
    }

    private void UpdateCurrentUpgrade() {
        // update currentUpgrade to match what unit has
        Ability targetCurrentAbility = target.Abilities.Find(a => a.Name == Name);
        if(targetCurrentAbility == null) {
            currentUpgrade = 0;
            return;
        }

        Level level = targetCurrentAbility.GetComponent<Level>();
        currentUpgrade = level.Current;
    }

    private void Bind(Ability ability) {
        IBindGame[] gameBinds = ability.GetComponentsInChildren<IBindGame>();
        foreach (IBindGame gameBind in gameBinds)
            gameBind.Bind(game);

        IBindUnit[] unitBinds = ability.GetComponentsInChildren<IBindUnit>();
        foreach (IBindUnit unitBind in unitBinds)
            unitBind.Bind(target);
    }
	
}
