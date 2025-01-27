using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform fieldPlane; // for checking if the ball is within the field
    [SerializeField] private Transform attackerField; // the field where the ball is spawned
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private Transform attacker;

    public bool isHeld = false;


    private void Awake()
    {
        // get the field area
        fieldPlane = GameObject.FindGameObjectWithTag("Field").transform;

        // get the attacking field
        attackerField = GameObject.FindGameObjectWithTag("AttackerField").transform;

    }

    private void Update()
    {
        // check if the ball is being held
        if (isHeld)
        {
            // move the ball with the attacker
            MoveBallWithAttacker(attacker);
        }
        // else
        // {
        //     // check if the ball is within the field
        //     if (!fieldPlane.GetComponent<MeshCollider>().bounds.Contains(transform.position))
        //     {
        //         // if the ball is out of the field, spawn it again
        //         SpawnBall();
        //     }
        // }

        
    }

    public void GetAttackerPosition(Transform attacker)
    {
        // get the attacker's position
        this.attacker = attacker;
    }

    public void MoveBallWithAttacker(Transform attacker)
    {
        // move the ball with the attacker
        transform.position = attacker.position;
    }

    private void Start()
    {
        SpawnBall();
    }

    public void SpawnBall()
    {
        // set the ball's position random in the attacker's field
        transform.position = new Vector3(Random.Range(attackerField.position.x - 30, attackerField.position.x + 30), attackerField.position.y + 5, Random.Range(attackerField.position.z - 25, attackerField.position.z + 25));
    }

}

