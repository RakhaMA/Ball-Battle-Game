using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Collider fieldPlane; // the field area
    [SerializeField] private Transform attackerField; // the field where the ball is spawned
    [SerializeField] private float speed = 1.5f;
    public Transform attacker;

    public bool isHeld = false;


    private void Awake()
    {
        // get the field area
        fieldPlane = GameObject.FindGameObjectWithTag("FieldArea").GetComponent<Collider>();

        // get the attacking field
        attackerField = GameObject.FindGameObjectWithTag("AttackerField").transform;

    }

    private void Start()
    {
        //RandomizedSpawnBallPosition(); // NO NEED FOR NOW
    }

    private void Update()
    {
        // check if the ball is being held
        if (isHeld && attacker != null)
        {
            // move the ball with the attacker
            MoveBallWithAttacker(attacker);
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check if the ball is out of the field
        if (other.CompareTag("FieldArea"))
        {
            Debug.Log("Ball is out of the field!");
            // respawn the ball
            RandomizedSpawnBallPosition();
        }
    }

    // make function if the ball collide with Gate (goals)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DefenderGate"))
        {
            Debug.Log("Goal!");
            Destroy(gameObject);
            // attacker wins
            GameManager.Instance.OnAttackerWin();
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
        StartCoroutine(MoveBallParabolically(attacker.position, 2.0f)); // 2.0f is the arc height
    }

    private IEnumerator MoveBallParabolically(Vector3 targetPosition, float arcHeight)
    {
        Vector3 startPosition = transform.position; // Initial position of the ball
        float distance = Vector3.Distance(startPosition, targetPosition);
        float elapsedTime = 0f;
        float duration = distance / speed; // Calculate total time based on speed

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // Normalized time (0 to 1)

            // Interpolate horizontally
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Add parabolic height
            float height = Mathf.Sin(Mathf.PI * t) * arcHeight; // Creates the arc
            transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + height, horizontalPosition.z);

            yield return null; // Wait for the next frame
        }

        // Snap the ball to the target position at the end
        transform.position = targetPosition;

        // Notify the attacker that the ball has arrived
        Debug.Log("Ball reached attacker: " + attacker.name);
    }

    

    public void RandomizedSpawnBallPosition()
    {
        // set the ball's position random in the attacker's field
        transform.position = new Vector3(Random.Range(attackerField.position.x - 30, attackerField.position.x + 30), attackerField.position.y + 5, Random.Range(attackerField.position.z - 25, attackerField.position.z + 25));
    }

}

