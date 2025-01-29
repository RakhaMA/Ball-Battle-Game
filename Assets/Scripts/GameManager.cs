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

    private Spawner spawner;

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

        StartCoroutine(CountdownToMatch());
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

    private void StartMatch()
    {
        attackerWinPanel.SetActive(false);
        defenderWinPanel.SetActive(false);
        timeRemaining = matchDuration;
        spawner.ResetEnergy();
        spawner.energyBar.StartEnergyRecharge();

        isMatchActive = true;
        spawner.isAttacker = isAttacker;
    }

    public void EndMatch()
    {
        isMatchActive = false;
        timeRemaining = matchDuration;
        spawner.ResetEnergy();
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
        StartCoroutine(CountdownToMatch());
    }

    public void OnAttackerWin()
    {
        attackerWinPanel.SetActive(true);
        EndMatch();
    }

    public void OnDefenderWin()
    {
        defenderWinPanel.SetActive(true);
        EndMatch();
    }
}
