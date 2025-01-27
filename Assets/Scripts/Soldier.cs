using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Soldier : MonoBehaviour
{
    public float speed;
    public float spawnTime;
    public float reactivateTime;
    public bool isActive = false;

    protected Vector3 startPosition;

    public virtual void Activate()
    {
        isActive = true;
        // Logic for activation (e.g., enable movement, reset visuals)
    }

    public virtual void Deactivate()
    {
        isActive = false;
        // Logic for deactivation (e.g., change visuals to greyscale, stop movement)
        Invoke(nameof(Reactivate), reactivateTime);
    }

    public virtual void SpawnTime()
    {
        Invoke(nameof(Activate), spawnTime);
    }

    private void Reactivate()
    {
        Activate();
    }

    public abstract void PerformBehavior(); // Override in derived classes
}
