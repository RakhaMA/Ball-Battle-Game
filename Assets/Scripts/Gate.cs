using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gate : MonoBehaviour
{
    public bool isEnemyGate; // True if this is the enemy gate
    public GameManager gameManager; // Reference to the game manager
    public ParticleSystem scoreEffect; // Particle effect for scoring

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log(isEnemyGate ? "Attacker wins!" : "Defender wins!");
            
            // Play particle effect
            if (scoreEffect != null)
            {
                scoreEffect.Play();
            }

            // Notify the game manager about the result
            if (isEnemyGate)
            {
                gameManager.OnAttackerWin();
            }
            else
            {
                gameManager.OnDefenderWin();
            }
        }
    }
}

