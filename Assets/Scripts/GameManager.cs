using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalMatches = 5;
    public float matchTime = 140f;
    private int currentMatch = 1;
    private float timer;

    public int playerScore = 0;
    public int enemyScore = 0;

    public Ball ball; // Reference to the Ball object
    public Transform playerField; // Reference to the player's field
    public Transform enemyField; // Reference to the enemy's field

    private bool isPlayerAttacking = true;

    void Start()
    {
        StartMatch();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            EndMatch();
        }
    }

    private void StartMatch()
    {
        Debug.Log("Starting Match " + currentMatch);
        timer = matchTime;

        // Alternate roles
        isPlayerAttacking = currentMatch % 2 != 0;

        // Reset ball position
        //ball.attackingField = isPlayerAttacking ? playerField : enemyField;
        //ball.SpawnBall();

        // Reset soldiers (optional: add reset logic for soldiers)
    }

    private void EndMatch()
    {
        Debug.Log("Match " + currentMatch + " ended!");
        currentMatch++;

        if (currentMatch > totalMatches)
        {
            EndGame();
        }
        else
        {
            StartMatch();
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over!");
        if (playerScore > enemyScore)
        {
            Debug.Log("Player Wins!");
        }
        else if (playerScore < enemyScore)
        {
            Debug.Log("Enemy Wins!");
        }
        else
        {
            Debug.Log("It's a Draw!");
            // Optional: Trigger Penalty Maze Mode
        }
    }

    public void OnAttackerWin()
    {
        playerScore++;
        Debug.Log("Player Scored! Current Score: " + playerScore);
        EndMatch();
    }

    public void OnDefenderWin()
    {
        enemyScore++;
        Debug.Log("Enemy Scored! Current Score: " + enemyScore);
        EndMatch();
    }
}
