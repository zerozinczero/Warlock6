using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundResultsPanel : MonoBehaviour {

    [SerializeField]
    private RoundResultsRow rowPrefab = null;

    [SerializeField]
    private Transform tableParent = null;

    [SerializeField]
    private Text titleLabel = null;

    [SerializeField]
    private Text messageLabel = null;

    public void SetData(List<Player> players) {
        Clear();

        // add a header
        Instantiate<RoundResultsRow>(rowPrefab, tableParent);

        // one row per player
        foreach(Player player in players) {
            RoundResultsRow row = Instantiate<RoundResultsRow>(rowPrefab, tableParent);
            PlayerGameStats stats = player.GetComponent<PlayerGameStats>();
            row.SetData(player.Name, stats.Kills, stats.Deaths, stats.RoundWins);
        }
    }

    private void Clear() {
        for (int i = tableParent.childCount - 1; i >= 0; i--) {
            Destroy(tableParent.GetChild(i).gameObject);
        }
    }
	
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void SetMessage(string message, Color color) {
        messageLabel.text = message;
        messageLabel.color = color;
    }
}
