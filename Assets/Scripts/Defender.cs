using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Soldier
{
    public float detectionRange;
    private Transform target;

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
                // Catch the attacker
                target.GetComponent<Attacker>().Deactivate();
                Deactivate();
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
            if (hit.CompareTag("Attacker") && hit.GetComponent<Attacker>().isActive)
            {
                target = hit.transform;
                break;
            }
        }
    }
}

