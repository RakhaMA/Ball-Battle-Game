using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Soldier
{
    public Transform ball; // Reference to the ball
    public Transform enemyGate;
    private bool hasBall = false;

    private void Update()
    {
        if (isActive)
        {
            PerformBehavior();
        }
    }

    public override void PerformBehavior()
    {
        if (hasBall)
        {
            // Move towards the enemy gate
            transform.position = Vector3.MoveTowards(transform.position, enemyGate.position, speed * Time.deltaTime);
        }
        else
        {
            // Chase the ball
            transform.position = Vector3.MoveTowards(transform.position, ball.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && !hasBall)
        {
            hasBall = true;
            // Logic to "hold" the ball
        }
        else if (other.CompareTag("Defender"))
        {
            if (hasBall)
            {
                // Pass the ball to another active attacker
                PassBallToNearestAttacker();
                Deactivate();
            }
        }
    }

    private void PassBallToNearestAttacker()
    {
        // Logic to find the nearest attacker and pass the ball
    }
}

