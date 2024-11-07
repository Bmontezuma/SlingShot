using UnityEngine;

public class MoveRandomly : MonoBehaviour
{
    public float speed = 1f;                  // Speed of movement
    public float directionChangeInterval = 2f; // Time in seconds to change direction
    public float movementRadius = 5f;         // Maximum movement distance from starting point

    private Vector3 startPosition;
    private Vector3 destination;

    private void Start()
    {
        startPosition = transform.position;
        SetRandomDestination();
        InvokeRepeating("SetRandomDestination", directionChangeInterval, directionChangeInterval);
    }

    private void Update()
    {
        // Move the capsule toward the destination
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        // If the capsule is close to the destination, set a new one
        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            SetRandomDestination();
        }
    }

    private void SetRandomDestination()
    {
        // Set a new random destination within the movement radius
        float randomX = Random.Range(-movementRadius, movementRadius);
        float randomZ = Random.Range(-movementRadius, movementRadius);
        destination = startPosition + new Vector3(randomX, 0, randomZ);

        Debug.Log($"New destination set to: {destination}");
    }
}
