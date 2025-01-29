using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider[] energyBarAttacker; // Energy bars for attackers
    public Slider[] energyBarDefender; // Energy bars for defenders

    public float rechargeRate = 2.0f; // Time per energy point recharge
    public float maxEnergyPerBar = 1.0f; // Each slider represents 1 energy point
    public float fillSpeed = 0.01f; // Speed of smooth animation

    private void Start()
    {
        InitializeEnergyBars();
        StartCoroutine(RechargeEnergy(energyBarAttacker)); // Start attacker energy recharge
        StartCoroutine(RechargeEnergy(energyBarDefender)); // Start defender energy recharge
    }

    private void Update()
    {
        // Press "F" to use energy from attacker side
        if (Input.GetKeyDown(KeyCode.F))
        {
            UseEnergy(energyBarAttacker);
        }

        // Press "J" to use energy from defender side
        if (Input.GetKeyDown(KeyCode.J))
        {
            UseEnergy(energyBarDefender);
        }
    }

    private void InitializeEnergyBars()
    {
        // Set all sliders to 0 and configure their max value
        foreach (Slider slider in energyBarAttacker)
        {
            slider.value = 0;
            slider.maxValue = maxEnergyPerBar;
        }
        foreach (Slider slider in energyBarDefender)
        {
            slider.value = 0;
            slider.maxValue = maxEnergyPerBar;
        }
    }

    private IEnumerator RechargeEnergy(Slider[] energyBars)
    {
        while (true) // Continuous recharge loop
        {
            for (int i = 0; i < energyBars.Length; i++) // Always start from the first empty slot
            {
                if (energyBars[i].value < maxEnergyPerBar) // If this slot is empty, charge it
                {
                    yield return StartCoroutine(FillSliderSmoothly(energyBars[i])); // Smooth recharge
                }
            }
            yield return null; // Allow coroutine to continue looping
        }
    }

    private IEnumerator FillSliderSmoothly(Slider slider)
    {
        float elapsedTime = 0f;
        float startValue = slider.value; // Start from current value

        while (elapsedTime < rechargeRate) // Fill over `rechargeRate` seconds
        {
            slider.value = Mathf.Lerp(startValue, maxEnergyPerBar, elapsedTime / rechargeRate);
            elapsedTime += fillSpeed;
            yield return new WaitForSeconds(fillSpeed);
        }

        slider.value = maxEnergyPerBar; // Ensure it fully fills
    }

    public void UseEnergy(Slider[] energyBars)
    {
        // Find the **latest fully charged** energy bar and consume it instantly
        for (int i = energyBars.Length - 1; i >= 0; i--) // Iterate **backward** (latest first)
        {
            if (energyBars[i].value == maxEnergyPerBar)
            {
                energyBars[i].value = 0; // Instantly remove energy
                Debug.Log("Used 1 energy from slot " + (i + 1));
                return; // Stop after using 1 energy
            }
        }

        Debug.Log("No energy available!");
    }

    // ✅ NEW METHOD (Easier Usage from Spawner)
    public void UseEnergy(bool isAttacker)
    {
        Slider[] energyBars = isAttacker ? energyBarAttacker : energyBarDefender;
        UseEnergy(energyBars); // Calls the existing function
    }


    public bool CanUseEnergy(bool isAttacker)
    {
        Slider[] energyBars = isAttacker ? energyBarAttacker : energyBarDefender;

        // Loop through energy bars to find any available energy
        foreach (Slider bar in energyBars)
        {
            if (bar.value == maxEnergyPerBar)
            {
                return true; // At least one slot has energy
            }
        }
        
        return false; // No energy available
    }

}
