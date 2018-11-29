using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IBindUnit {

    [SerializeField] private int id = 0;
    public int Id { get { return id; } }

    [SerializeField] private string itemName = null;
    public string Name { get { return itemName; } }

    [SerializeField] private Sprite icon = null;
    public Sprite Icon { get { return icon; } }

    [SerializeField]
    [TextArea]
    private string descTemplate = null;

    [SerializeField] private ItemTooltip tooltipPrefab = null;
    public ItemTooltip TooltipPrefab { get { return tooltipPrefab; } }

	[SerializeField] protected Unit owner = null;
    public Unit Owner { get { return owner; } set { owner = value; } }

	public abstract void Equip();
	public abstract void Unequip();

    public virtual void Bind(Unit unit) {
        owner = unit;
    }

    public virtual Dictionary<string,string> GetTokens() {
        return null;
    }

    public string Description {
        get {
            Dictionary<string, string> tokens = GetTokens();
            string desc = StringUtil.Replace(descTemplate, tokens);
            return desc;
        }
    }

}
