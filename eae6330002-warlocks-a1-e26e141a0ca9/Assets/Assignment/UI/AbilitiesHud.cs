using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesHud : MonoBehaviour {

    [SerializeField]
    private Unit currentUnit = null;

    [SerializeField]
    private List<AbilityButton> abilityButtons = new List<AbilityButton>();

    [SerializeField]
    private AbilityEvent abilityPressedAction = null;

    [SerializeField]
    private Text unitNameLabel = null;

    [SerializeField]
    private Slider healthBarSlider = null;

    [SerializeField]
    private Text healthLabel = null;

    [SerializeField] private Text healthRegenLabel = null;

    [SerializeField]
    private Transform previewCamTransform = null;

    [SerializeField]
    private Vector3 previewOffset = new Vector3(0, 5f, 1.5f);

    [SerializeField]
    private RawImage selectedUnitPreview = null;

    private KeyCode[] hotkeys = new KeyCode[] {
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R,
        KeyCode.T,
        KeyCode.Y
    };

    public void SetUnit(Unit unit) {
        SetUnit(unit, false);
    }

    public void SetUnit(Unit unit, bool forceRefresh) {
        if(unit == currentUnit && !forceRefresh) {
            return;
        }

        if(currentUnit != null) {
            (currentUnit.Health as Health).OnHealthEvent.RemoveListener(OnUnitHealthEvent);
        }
        currentUnit = unit;

        int i = 0;
        if (unit != null) {
            (currentUnit.Health as Health).OnHealthEvent.AddListener(OnUnitHealthEvent);
            UpdateHealth(currentUnit.Health);
            healthBarSlider.gameObject.SetActive(true);

            foreach (Ability ability in unit.Abilities) {
                if (i > abilityButtons.Count) {
                    Debug.LogWarning("Too many abilities for UI to display");
                    break;
                }
                AbilityButton abilityButton = abilityButtons[i];
                abilityButton.SetAbility(ability, hotkeys[i].ToString());

                i++;
            }
        } else {
            healthBarSlider.gameObject.SetActive(false);
        }

        for (; i < abilityButtons.Count; i++) {
            abilityButtons[i].SetAbility(null, null);
        }

        unitNameLabel.text = unit != null ? unit.Name : "";

        UpdateItemsUi(unit);

    }

    public void UpdateUnit(Unit unit) {
        if(unit == currentUnit) {
            SetUnit(unit, true);
        }
    }

    public void OnAbilityButtonPressed(AbilityButton abilityButton) {
        if (abilityPressedAction != null) {
            abilityPressedAction.Invoke(abilityButton.Ability);
        }
    }

    private void Update() {
        for (int i = 0; i < hotkeys.Length && i < abilityButtons.Count; i++) {
            if (Input.GetKeyDown(hotkeys[i])) {
                OnAbilityButtonPressed(abilityButtons[i]);
            }
        }
    }

    private void OnUnitHealthEvent(IHealthEvent healthEvent) {
        UpdateHealth(currentUnit.Health);
    }

    void UpdateHealth(IHealth health) {
        healthBarSlider.value = health.Percent;
        healthLabel.text = string.Format("{0}/{1}", (int)health.Current, (int)health.Max);

        if(healthRegenLabel != null) {
            if(health.Regen != null && health.Regen.Current > 0) {
                healthRegenLabel.text = string.Format("+{0}", health.Regen.Current);
                healthRegenLabel.gameObject.SetActive(true);
            } else {
                healthRegenLabel.gameObject.SetActive(false);
            }
        }
    }

    private void LateUpdate() {
        UpdateSelectedUnitPreview();
    }

    private void UpdateSelectedUnitPreview() {
        Unit selectedUnit = currentUnit;
        if (selectedUnit != null) {
            Vector3 offset = selectedUnit.transform.rotation * previewOffset;
            previewCamTransform.position = selectedUnit.transform.position + offset;
            previewCamTransform.LookAt(selectedUnit.ChestTransform, selectedUnit.transform.up);
            selectedUnitPreview.gameObject.SetActive(true);
        } else {
            selectedUnitPreview.gameObject.SetActive(false);
        }
    }

    [SerializeField] private List<ItemSlot> itemSlots = null;
    private void UpdateItemsUi(Unit unit) {
        for (int i = 0; i < itemSlots.Count; i++) {
            Item item = unit != null && i < unit.Items.Count ? unit.Items[i] : null;
            ItemSlot itemSlot = itemSlots[i];
            itemSlot.SetItem(item);
        }
    }
}
