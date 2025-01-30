using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Soldier
{
    public float detectionRange;
    private Transform target;
    private Animator animator;
    public GameObject detectionCircle;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get Animator component
        SpawnTime();

        // Scale the detection circle to the detection range
        if (detectionCircle != null)
        {
            detectionCircle.transform.localScale = new Vector3(detectionRange, 0.1f, detectionRange);
            detectionCircle.SetActive(true); // Always visible
        }
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

        if (target != null)
        {
            // Chase the target
            moveDirection = (target.position - transform.position).normalized * speed * Time.deltaTime;
            transform.position += moveDirection;
            isMoving = true;

            // Check if the target is within reach
            if (Vector3.Distance(transform.position, target.position) < 1.0f)
            {
                Debug.Log("Defender caught target: " + target.name);
                Attacker attacker = target.GetComponentInParent<Attacker>();
                if (attacker != null)
                {
                    attacker.PassBallToNearestAttacker();
                    attacker.Deactivate();
                    Deactivate();
                    target = null; // Clear the target after catching
                }
                else
                {
                    Debug.LogWarning("Attacker component is missing from target!");
                    target = null; // Clear the target to avoid further processing
                }
            }
        }
        else
        {
            // Stand by
            LookForTarget();
        }

        // **Rotate to face movement direction**
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // **Set animation state**
        animator.SetBool("isWalking", isMoving);
    }

    private void LookForTarget()
    {
        // Detect attackers within range
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Attacker") && hit.GetComponentInParent<Attacker>().hasBall)
            {
                target = hit.transform;
                Debug.Log("target: " + target);
                break;
            }
        }
    }

    // Draw gizmos for detection range
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
