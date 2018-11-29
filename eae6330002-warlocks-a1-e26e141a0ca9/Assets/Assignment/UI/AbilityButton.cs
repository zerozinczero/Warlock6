using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityButton : MonoBehaviour {

    [SerializeField]
    private Ability ability = null;
    public Ability Ability { get { return ability; } }

    private Cooldown cooldown = null;

    [SerializeField]
    private Text abilityNameLabel = null;

    [SerializeField]
    private Text hotkeyLabel = null;

    [SerializeField]
    private Button button = null;

    [SerializeField]
    private Image cooldownOverlay = null;

    [SerializeField]
    private Image abilityIcon = null;

    [SerializeField] private AbilityTooltip tooltip = null;

    public void SetAbility(Ability ability, string hotkey) {
        this.ability = ability;
        cooldown = ability != null ? ability.GetComponent<Cooldown>() : null;
        abilityNameLabel.text = ability != null ? ability.Name : "";
        button.interactable = ability != null;
        hotkeyLabel.text = hotkey;
        Sprite icon = ability != null ? ability.Icon : null;
        abilityIcon.overrideSprite = icon;
        abilityIcon.color = icon != null ? Color.white : Color.clear;
    }

    private void Update() {
        float cd = cooldown != null ? cooldown.RemainingNormalized : 0f;
        cooldownOverlay.fillAmount = cd;
    }

    public void OnHoverEnter(BaseEventData baseEventData) {
        if(ability != null && ability.TooltipPrefab != null) {
            tooltip = Instantiate<AbilityTooltip>(ability.TooltipPrefab, transform);
            tooltip.SetAbility(ability);
        }
    }

    public void OnHoverExit(BaseEventData baseEventData) {
        if(tooltip != null) {
            Destroy(tooltip.gameObject);
        }
    }

}
