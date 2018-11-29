using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundResultsRow : MonoBehaviour {

    [SerializeField]
    private Text playerNameLabel = null;

    [SerializeField]
    private Text winsLabel = null;

    [SerializeField]
    private Text deathsLabel = null;

    [SerializeField]
    private Text killsLabel = null;

    public void SetData(string playerName, int kills, int deaths, int wins) {
        playerNameLabel.text = playerName;
        winsLabel.text = wins.ToString();
        deathsLabel.text = deaths.ToString();
        killsLabel.text = kills.ToString();
    }
	
}
