using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour, IAppAware {

    [SerializeField]
    private int requiredRoundWins = 2;
    public int RequiredRoundWins { get { return requiredRoundWins; } }

    [SerializeField] private InGameUi inGameUi = null;
    public InGameUi InGameUi { get { return inGameUi; } }

    [SerializeField] private Camera cam = null;
    public Camera Camera { get { return cam; } }

    [SerializeField]
    private List<Unit> units = null;

    [SerializeField]
    private List<Player> players = null;
    public List<Player> Players { get { return players; } }

    [SerializeField]
    private Map map = null;
    public Map Map { get { return map; } }

    [SerializeField]
    private Transform roundsParent = null;

    [SerializeField]
    private Round roundPrefab = null;

    [SerializeField]
    private List<Round> rounds = null;

    [SerializeField]
    private Obstacles obstacles = null;

    [SerializeField]
    private GameStatsManager gameStats = null;

    [SerializeField]
    private float roundStartDelay = 20f;

    [SerializeField]
    private float gameEndDelay = 5f;


    [SerializeField]
    private App app = null;
    public App App { get { return app; } set { app = value; } }

    public void StartGame() {
        StartNewRound();
    }

    void StartNewRound() {
        int roundNum = rounds.Count + 1;
        Round round = Instantiate<Round>(roundPrefab, roundsParent);
        round.name = string.Format("Round {0}", roundNum);
        rounds.Add(round);

        round.OnPhaseChanged.AddListener(OnRoundPhaseChange);
        round.Initialize(map, units);
        round.Invoke("StartRound", roundStartDelay);

        obstacles.Clear();
        obstacles.Spawn();

        inGameUi.RoundResultsPanel.Hide();
    }

    void OnRoundPhaseChange(Round round) {
        switch(round.Phase) {
            case RoundPhase.PreRound:
                OnPreRound(round);
                break;
            case RoundPhase.InProgress:
                OnRoundStart(round);
                break;
            case RoundPhase.Celebration:
                OnRoundOutcome(round);
                break;
            case RoundPhase.Completed:
                OnRoundCompleted(round);
                break;
        }
    }

    void OnPreRound(Round round) {
        inGameUi.ShowStore();
        inGameUi.ShowMessage(round.name, Color.yellow);
    }

    void OnRoundStart(Round round) {
        inGameUi.HideStore();
        inGameUi.HideMessage();
    }

    void OnRoundOutcome(Round round) {
        Player winner = round.Winner as Player;
        gameStats.PlayerWon(winner);
        ShowRoundOutcome(winner);
    }

    void OnRoundCompleted(Round round) {

        round.OnPhaseChanged.RemoveListener(OnRoundPhaseChange);

        Player winner = round.Winner as Player;

        if (winner != null) {
            int roundWins = gameStats.GetRoundsWonCount(winner);
            if (roundWins >= requiredRoundWins) {
                OnGameOver(winner);
            } else {
                StartNewRound();
            }
        } else {
            StartNewRound();
        }
    }

    void ShowRoundOutcome(Player winner) {
        if(winner != null) {
            if (winner.IsLocal) {
                inGameUi.RoundResultsPanel.SetMessage(inGameUi.winRoundMessage, Color.yellow);
            } else {
                inGameUi.RoundResultsPanel.SetMessage(string.Format(inGameUi.loseRoundMessage, winner.Name), Color.red);
            }
        } else {
            inGameUi.RoundResultsPanel.SetMessage(inGameUi.tieMessage, Color.white);
        }

        inGameUi.RoundResultsPanel.SetData(players);
        inGameUi.RoundResultsPanel.Show();
    }

    void OnGameOver(Player winner) {
        ShowGameOutcome(winner);

        StartCoroutine(GameOver_Coroutine());
    }

    void ShowGameOutcome(Player winner) {
        if(winner != null) {
            if(winner.IsLocal) {
                inGameUi.RoundResultsPanel.SetMessage(inGameUi.winGameMessage, Color.yellow);
            } else {
                inGameUi.RoundResultsPanel.SetMessage(string.Format(inGameUi.loseGameMessage, winner.Name), Color.red);
            }
        } else {
            inGameUi.RoundResultsPanel.SetMessage(inGameUi.tieMessage, Color.white);
        }

        inGameUi.RoundResultsPanel.SetData(players);
        inGameUi.RoundResultsPanel.Show();
    }

    IEnumerator GameOver_Coroutine() {
        
        yield return new WaitForSeconds(gameEndDelay);

        if(app != null) {
            app.Scenes.GoToMainMenu();
        }
    }


}
