using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : MonoBehaviour {

    public void OnUnitHealthEvent(IHealthEvent healthEvent) {
        Unit deadUnit = (healthEvent.Target as Health).GetComponent<Unit>();
        Player player = deadUnit.Owner;
        PlayerController controller = player.GetComponent<PlayerController>();
        int aliveUnitsCount = controller.Units.FindAll(u => u.Health.IsAlive).Count;
        if(aliveUnitsCount == 0) {
            OnPlayerDeath(player, null);    // TODO: attribute death to killing player
        }
    }

    public void OnPlayerDeath(Player player, Player killer) {
        PlayerGameStats stats = player.GetComponent<PlayerGameStats>();
        stats.Deaths++;

        if (killer != null) {
            PlayerGameStats killerStats = player.GetComponent<PlayerGameStats>();
            killerStats.Kills++;
        }
    }
	

    public int GetRoundsWonCount(Player player) {
        int roundWins = 0;
        PlayerGameStats stats = player.GetComponent<PlayerGameStats>();
        if (stats != null) {
            roundWins = stats.RoundWins;
        }
        return roundWins;
    }

    public void PlayerWon(Player winner) {
        if (winner != null) {
            PlayerGameStats stats = winner.GetComponent<PlayerGameStats>();
            if (stats != null) {
                stats.RoundWins++;
            }
        }
    }
}
