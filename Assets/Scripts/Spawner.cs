using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public EnergyBar energyBar;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject attackerPrefab;
    [SerializeField] private GameObject defenderPrefab;
    [SerializeField] private LayerMask fieldLayerMask;
    public bool isPlayerAttacker = true;

    private Camera mainCamera;

    // Lists to track spawned attackers and defenders
    public List<GameObject> attackerList = new List<GameObject>();
    public List<GameObject> defenderList = new List<GameObject>();

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && energyBar.CanUseEnergy(isPlayerAttacker) && GameManager.Instance.isMatchActive)
        {
            // 🟢 Player Spawns Soldier at Click Position
            SpawnSoldierAtClick(isPlayerAttacker);
            energyBar.UseEnergy(isPlayerAttacker);

            // 🔴 Enemy Spawns Soldier at Random Position (Simultaneously)
            Vector3 enemySpawnPosition = isPlayerAttacker ? GetRandomDefenderSpawn() : GetRandomAttackerSpawn();
            SpawnSoldierAtPosition(!isPlayerAttacker, enemySpawnPosition);
            energyBar.UseEnergy(!isPlayerAttacker);

            // sfx
            AudioManager.AudioInstance.PlayButtonSfx();
        }
    }

    /// <summary>
    /// Spawns a soldier at the clicked position (for player only).
    /// </summary>
    private void SpawnSoldierAtClick(bool isAttacker)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, fieldLayerMask))
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.y = 0;

            SpawnSoldierAtPosition(isAttacker, spawnPosition);
        }
    }

    /// <summary>
    /// Spawns a soldier at a specific position (for both player and enemy).
    /// </summary>
    private void SpawnSoldierAtPosition(bool isAttacker, Vector3 spawnPosition)
    {
        GameObject spawnedSoldier;

        if (isAttacker)
        {
            spawnedSoldier = Instantiate(attackerPrefab, spawnPosition, Quaternion.identity);
            attackerList.Add(spawnedSoldier);
        }
        else
        {
            spawnedSoldier = Instantiate(defenderPrefab, spawnPosition, Quaternion.identity);
            defenderList.Add(spawnedSoldier);
        }

        Debug.Log($"Spawned {(isAttacker ? "Attacker" : "Defender")} at {spawnPosition}");
    }

    /// <summary>
    /// Clears all spawned attackers and defenders from the game.
    /// </summary>
    public void ClearAllSoldiers()
    {
        foreach (var attacker in attackerList)
        {
            Destroy(attacker);
        }
        attackerList.Clear();

        foreach (var defender in defenderList)
        {
            Destroy(defender);
        }
        defenderList.Clear();
    }

    /// <summary>
    /// Resets the energy bar for both player and enemy.
    /// </summary>
    public void ResetEnergy()
    {
        energyBar.ResetEnergy();
    }

    /// <summary>
    /// Spawns the ball at a random position.
    /// </summary>
    public void SpawnBall()
    {
        GameObject spawnedBall = Instantiate(ball, Vector3.zero, Quaternion.identity);
        Ball ballScript = spawnedBall.GetComponent<Ball>();
        ballScript.RandomizedSpawnBallPosition();
    }

    // Destroy the ball
    public void DestroyBall()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Destroy(ball);
        }
    }

    /// <summary>
    /// Returns a random spawn position for enemy defenders.
    /// </summary>
    public Vector3 GetRandomDefenderSpawn()
    {
        return new Vector3(Random.Range(-36, -3), 0, Random.Range(-20, 20));
    }

    /// <summary>
    /// Returns a random spawn position for enemy attackers.
    /// </summary>
    public Vector3 GetRandomAttackerSpawn()
    {
        return new Vector3(Random.Range(3, 36), 0, Random.Range(-20, 20));
    }
}
