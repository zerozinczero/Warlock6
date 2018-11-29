using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StandardAbility : Ability {

    [SerializeField][TextArea]
    private string propertiesTemplate = null;

    [SerializeField]
    private StandardAbilityAction action = null;

    [SerializeField]
    private UnityEvent startCast = null;

    [SerializeField]
    private Cooldown cooldown = null;

    private void Start() {
        action.OnPerformed.AddListener(OnActionPerformed);
        action.OnCancelled.AddListener(OnActionCancelled);
    }

    public override bool CanCast(out string message) {

        if (cooldown != null && !cooldown.Ready) {
            message = "Spell on cooldown";
            return false;
        }

        message = null;
        return true;
    }

    public override void Cast() {
        if (startCast != null) {
            startCast.Invoke();
        } else {
            action.Perform();
        }
    }

    void OnActionPerformed(GameAction action) {
        if(cooldown != null) {
            cooldown.Activate();
        }
    }

    void OnActionCancelled(GameAction action) {

    }

    public override void StopCast() {
        action.Stop();
    }

    public override void Reset() {
        if(cooldown != null) {
            cooldown.Reset();
        }
    }

    public string PropertiesText {
        get {
            Dictionary<string, string> tokens = action.GetPropertyTokens();
            string propertiesText = StringUtil.Replace(propertiesTemplate, tokens);
            return propertiesText;
        }
    }

}
