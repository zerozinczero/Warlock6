using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour {

    public string winRoundMessage = "You won the round!";
    public string loseRoundMessage = "{0} won the round";
    public string tieMessage = "It's a tie";

    public string winGameMessage = "You won the game!";
    public string loseGameMessage = "{0} won the game";

    public string winsMessage = "{0}/{1} wins";

    [SerializeField]
    private Text messageLabel = null;

    [SerializeField] private AbilitiesHud abilitiesHud = null;
    public AbilitiesHud AbilitiesHud { get { return abilitiesHud; } }

    [SerializeField]
    private StoreController storeController = null;

    [SerializeField]
    private RoundResultsPanel roundResultsPanel = null;
    public RoundResultsPanel RoundResultsPanel { get { return roundResultsPanel; } }

    [SerializeField] private InputManager inputManager = null;
    public InputManager InputManager { get { return inputManager; } }

    [SerializeField] private Transform targetingArrow = null;
    public Transform TargetingArrow { get { return targetingArrow; } }

    public void ShowMessage(string message, Color? color = null) {
        messageLabel.text = message;
        if(color.HasValue) {
            messageLabel.color = color.Value;
        }
        messageLabel.gameObject.SetActive(true);
    }

    public void HideMessage() {
        messageLabel.gameObject.SetActive(false);
    }

    public void ShowStore() {
        storeController.gameObject.SetActive(true);
    }

    public void HideStore() {
        storeController.gameObject.SetActive(false);
    }
}
