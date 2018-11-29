using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandardAbilityTooltip : AbilityTooltip {

    [SerializeField] private Ability ability = null;

    [SerializeField] private Text abilityNameLabel = null;

    [SerializeField] private Text levelLabel = null;

    [SerializeField] private Text descLabel = null;

    [SerializeField] private Text propertiesLabel = null;

    [SerializeField] private Text cooldownLabel = null;

    public override void SetAbility(Ability ability) {
        this.ability = ability;

        abilityNameLabel.text = ability.Name;

        levelLabel.text = ""; //string.Format("lvl {0}", 1);
        descLabel.text = ability.Description;
        if (ability is StandardAbility) {
            StandardAbility standardAbility = ability as StandardAbility;
            propertiesLabel.text = standardAbility.PropertiesText;

        } else {
            propertiesLabel.text = "";
        }

        Cooldown cooldown = ability.GetComponent<Cooldown>();
        if (cooldown != null) {
            cooldownLabel.text = string.Format("CD: {0}", cooldown.Duration);
        } else {
            cooldownLabel.text = "";
        }
    }
	
}
