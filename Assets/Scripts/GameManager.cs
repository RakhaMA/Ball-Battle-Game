using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private float matchDuration = 60.0f;
    [SerializeField] private float timeRemaining;
    public bool isMatchActive = false;
    [SerializeField] private bool isAttacker = true;

    public TextMeshProUGUI timerText;
    public GameObject countdownPanel;
    public TextMeshProUGUI countdownText;

    public GameObject attackerWinPanel;
    public GameObject defenderWinPanel;
    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI roundText;

    public int roundCount = 0; // there are 5 rounds in total
    public int playerScore = 0;
    public int enemyScore = 0;

    private Spawner spawner;
    private SwitchSide switchSide;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spawner = GetComponent<Spawner>();
        switchSide = GetComponent<SwitchSide>();

        mainMenuPanel.SetActive(true);

        //StartCoroutine(CountdownToMatch());
    }

    private void Update()
    {
        if (isMatchActive)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = timeRemaining.ToString("F1");
            if (timeRemaining <= 0)
            {
                // defender wins if time runs out
                OnDefenderWin();
            }
        }
    }

    public void NextRound()
    {
        roundCount++;
        roundText.text = $"Round {roundCount + 1}/5";
        if (roundCount >= 5) // if all rounds are finished
        {
            roundCount = 0; // reset round count
            OnGameOver();
        }
        else
        {
            isAttacker = !isAttacker;
            switchSide.SwitchPlayerSide();
            StartCountdownMatch();
        }
    }

    private void StartMatch()
    {
        spawner.SpawnBall();
        timeRemaining = matchDuration;
        spawner.ResetEnergy();
        spawner.energyBar.StartEnergyRecharge();

        isMatchActive = true;
        spawner.isPlayerAttacker = isAttacker;
    }

    public void EndMatch()
    {
        isMatchActive = false;
        timeRemaining = matchDuration;
        
        spawner.energyBar.StopEnergyRecharge();
        spawner.ClearAllSoldiers();
    }

    IEnumerator CountdownToMatch()
    {
        countdownPanel.SetActive(true);
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
        }
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1);
        countdownPanel.SetActive(false);
        StartMatch();
    }

    public void StartCountdownMatch()
    {
        attackerWinPanel.SetActive(false);
        defenderWinPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        StartCoroutine(CountdownToMatch());
    }

    public void OnAttackerWin()
    {
        // add player score if player is attacker
        if (isAttacker)
        {
            playerScore++;
        }
        else
        {
            enemyScore++;
        }

        attackerWinPanel.SetActive(true);
        EndMatch();
    }

    public void OnDefenderWin()
    {
        // add player score if player is defender
        if (!isAttacker)
        {
            playerScore++;
        }
        else
        {
            enemyScore++;
        }

        spawner.DestroyBall();

        defenderWinPanel.SetActive(true);
        EndMatch();
    }

    public void OnGameOver()
    {
        attackerWinPanel.SetActive(false);
        defenderWinPanel.SetActive(false);

        finalScoreText.text = $"Player: {playerScore} - Enemy: {enemyScore}";
        gameOverPanel.SetActive(true);

    }

    public void OnMainMenu()
    {
        mainMenuPanel.SetActive(true);
        EndMatch();
    }

    public void ResetRoundCount()
    {
        roundCount = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
