using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Soldier
{
    public Transform ball; // Reference to the ball
    public Transform enemyFence;
    public Transform enemyGate;
    public bool hasBall = false;

    public Ball ballScript; // Reference to the Ball object

    private void Start()
    {
        // Get the ball object
        GameObject ballObject = GameObject.FindGameObjectWithTag("Ball");
        if (ballObject != null)
        {
            ball = ballObject.transform;
            ballScript = ballObject.GetComponent<Ball>();
        }

        // Get the enemy gate object
        enemyFence = GameObject.FindGameObjectWithTag("DefenderFence").transform; // needs to be changed to the correct tag later (depends on atk/deff)

        // Get the enemy gate object
        enemyGate = GameObject.FindGameObjectWithTag("DefenderGate").transform; // needs to be changed to the correct tag later (depends on atk/deff)

        SpawnTime();
    }

    private void Update()
    {
        if (isActive)
        {
            PerformBehavior();
        }
    }

    public override void PerformBehavior()
    {
        // if no ball in the field, move forward with half the speed
        if (ball == null)
        {
            transform.position += Vector3.left * speed * 0.5f * Time.deltaTime;
            return;
        }

        if (hasBall)
        {
            // Move towards the enemy gate with half the speed
            transform.position = Vector3.MoveTowards(transform.position, enemyGate.position, speed * 0.5f * Time.deltaTime);
        }
        else
        {
            // check if the ball currently not being held
            if (!ballScript.isHeld)
            {
                // Chase the ball
                transform.position = Vector3.MoveTowards(transform.position, ball.position, speed * Time.deltaTime);
            }
            else // if the ball is being held
            {
                // Move towards the enemy gate with half the speed
                transform.position = Vector3.MoveTowards(transform.position, enemyGate.position, speed * 0.5f * Time.deltaTime);
            }
        }

        // check if the attacker has get the ball
        if (Vector3.Distance(transform.position, ball.position) < 1.0f)
        {
            hasBall = true;
            // Logic to "hold" the ball
            ballScript.isHeld = true;
            ballScript.GetAttackerPosition(transform);
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Ball") && !hasBall)
    //     {
    //         hasBall = true;
    //         // Logic to "hold" the ball
    //     }
    //     else if (other.CompareTag("Defender"))
    //     {
    //         if (hasBall)
    //         {
    //             // Pass the ball to another active attacker
    //             PassBallToNearestAttacker();
    //             Deactivate();
    //         }
    //     }
    // }

    private void PassBallToNearestAttacker()
    {
        // Logic to find the nearest attacker and pass the ball
    }
}

