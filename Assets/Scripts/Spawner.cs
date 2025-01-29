using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private EnergyBar energyBar;
    [SerializeField] private GameObject attackerPrefab;
    [SerializeField] private GameObject defenderPrefab;
    [SerializeField] private LayerMask fieldLayerMask;
    [SerializeField] private bool isAttacker = true;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && energyBar.CanUseEnergy(isAttacker))
        {
            SpawnSoldierAtClick();
            energyBar.UseEnergy(isAttacker);
        }
    }

    private void SpawnSoldierAtClick()
    {
        // Raycast from the mouse position to the field
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, fieldLayerMask))
        {
            Vector3 spawnPosition = hit.point; // Get the click position
            spawnPosition.y = 0; // Ensure the soldier spawns at ground level

            if (isAttacker)
            {
                // Spawn attacker
                Instantiate(attackerPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                // Spawn defender
                Instantiate(defenderPrefab, spawnPosition, Quaternion.identity);
            }

            Debug.Log($"Spawned {(isAttacker ? "Attacker" : "Defender")} at {spawnPosition}");
        }
    }
}
