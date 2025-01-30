using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    public GameObject gamePrefab; // Assign your game world prefab here
    private GameObject spawnedGame;
    private ARRaycastManager raycastManager;

    private void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                if (spawnedGame == null)
                {
                    // Spawn the game world
                    spawnedGame = Instantiate(gamePrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    // Move the game world to the new position
                    spawnedGame.transform.position = hitPose.position;
                }
            }
        }
    }
}
