using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Collider fieldPlane; // the field area
    [SerializeField] private Transform attackerField; // the field where the ball is spawned
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private Transform attacker;

    public bool isHeld = false;


    private void Awake()
    {
        // get the field area
        fieldPlane = GameObject.FindGameObjectWithTag("FieldArea").GetComponent<Collider>();

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
        else
        {
            if (attacker != null)
            {
                // move the ball to the nearest attacker
                MoveBallToNearestAttacker(attacker);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check if the ball is out of the field
        if (other.CompareTag("FieldArea"))
        {
            Debug.Log("Ball is out of the field!");
            // respawn the ball
            SpawnBall();
        }
    }

    public void GetAttackerPosition(Transform attacker)
    {
        // get the attacker's position
        this.attacker = attacker;
    }

    public void UpdateAttacker(Transform attacker)
    {
        // update the attacker
        this.attacker = attacker;
    }

    public void MoveBallWithAttacker(Transform attacker)
    {
        // move the ball with the attacker
        transform.position = attacker.position;
    }

    // move ball to the nearest attacker with the ball speed
    public void MoveBallToNearestAttacker(Transform attacker)
    {
        // move the ball to the attacker
        transform.position = Vector3.MoveTowards(transform.position, attacker.position, speed * Time.deltaTime);
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

