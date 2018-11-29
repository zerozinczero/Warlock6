using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour {

    [SerializeField] private int id = 0;
    public int Id { get { return id; } }

    [SerializeField] private string abilityName = null;
    public string Name { get { return abilityName; } }

    [SerializeField] private Sprite icon = null;
    public Sprite Icon { get { return icon; } }

    [SerializeField]
    [TextArea]
    private string descTemplate = null;

    [SerializeField] private AbilityTooltip tooltipPrefab = null;
    public AbilityTooltip TooltipPrefab { get { return tooltipPrefab; } }

    public abstract void Cast();
    public abstract void StopCast();
    public abstract bool CanCast(out string message);

    public virtual void Reset() {}

    protected Dictionary<string,string> GetTokens() {
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
