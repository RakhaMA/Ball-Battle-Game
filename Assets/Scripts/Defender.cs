using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Soldier
{
    public float detectionRange;
    private Transform target;

    private void Start()
    {
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
        if (target != null)
        {
            // Chase the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

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

    // draw gizmos for detection range
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

