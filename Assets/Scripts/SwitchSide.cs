using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchSide : MonoBehaviour
{
    private Camera mainCamera;
    private bool isPlayerAttacker = true;

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI enemyText;
    public GameObject fieldArea;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // debug test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchPlayerSide();
        }
    }

    public void SwitchPlayerSide()
    {
        isPlayerAttacker = !isPlayerAttacker;
        playerText.text = isPlayerAttacker ? "Attacker - Player" : "Defender - Player";
        // change player text color to red if player is defender and blue if player is attacker
        playerText.color = isPlayerAttacker ? Color.blue : Color.red;

        enemyText.text = isPlayerAttacker ? "Defender - AI" : "Attacker - AI";
        // change enemy text color to red if enemy is attacker and blue if enemy is defender
        enemyText.color = isPlayerAttacker ? Color.red : Color.blue;

        // change the camera rotation to (90, 90, 0) but make it reversable again
        mainCamera.transform.Rotate(0, 0, 180);
    }
}
