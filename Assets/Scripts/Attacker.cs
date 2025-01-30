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

    private Animator animator;

    private void Start()
    {
        // Get Animator component
        animator = GetComponent<Animator>();

        // Get the ball object
        GameObject ballObject = GameObject.FindGameObjectWithTag("Ball");
        if (ballObject != null)
        {
            ball = ballObject.transform;
            ballScript = ballObject.GetComponent<Ball>();
        }

        // Get the enemy gate object
        enemyFence = GameObject.FindGameObjectWithTag("DefenderFence").transform;
        enemyGate = GameObject.FindGameObjectWithTag("DefenderGate").transform;

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
        bool isMoving = false; // Track movement
        Vector3 moveDirection = Vector3.zero; // Track movement direction

        // If another attacker has the ball, just move forward
        if (ballScript.isHeld && ballScript.attacker != transform)
        {
            moveDirection = Vector3.left * speed * Time.deltaTime;
            isMoving = true;
        }
        // If no ball in the field, move forward to the enemy gate
        else if (ball == null)
        {
            moveDirection = Vector3.left * speed * Time.deltaTime;
            isMoving = true;
        }
        else if (hasBall) // Holding the ball
        {
            moveDirection = (enemyGate.position - transform.position).normalized * speed * 0.5f * Time.deltaTime;
            isMoving = true;
        }
        else
        {
            // Chase the ball if it's not being held
            if (!ballScript.isHeld)
            {
                moveDirection = (ball.position - transform.position).normalized * speed * Time.deltaTime;
                isMoving = true;
            }
            else // Move toward the enemy gate
            {
                moveDirection = (enemyGate.position - transform.position).normalized * speed * 0.5f * Time.deltaTime;
                isMoving = true;
            }
        }

        // **Apply movement**
        transform.position += moveDirection;

        // **Rotate to face movement direction**
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // **Set animation state**
        animator.SetBool("isWalking", isMoving);

        // Check if attacker gets the ball
        if (Vector3.Distance(transform.position, ball.position) < 1.0f)
        {
            hasBall = true;
            ballScript.isHeld = true;
            ballScript.GetAttackerPosition(transform);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DefenderFence") || other.CompareTag("DefenderGate"))
        {
            Debug.Log("Attacker has been destroyed!");
            Destroy(gameObject);
        }
    }

    public void PassBallToNearestAttacker()
    {
        Attacker[] attackers = FindObjectsOfType<Attacker>();
        Attacker nearestAttacker = null;
        float minDistance = float.MaxValue;
        foreach (var attacker in attackers)
        {
            if (attacker.isActive && attacker != this)
            {
                float distance = Vector3.Distance(transform.position, attacker.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestAttacker = attacker;
                }
            }
        }

        if (nearestAttacker != null)
        {
            Debug.Log("Passing the ball to the nearest attacker");
            hasBall = false;
            ballScript.isHeld = false;
            ballScript.UpdateAttacker(nearestAttacker.transform);
            ballScript.MoveBallToNearestAttacker(nearestAttacker.transform);
        }
    }
}
