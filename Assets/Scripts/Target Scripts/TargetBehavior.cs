using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TargetBehavior : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveDirection;
    private Vector2 planeBounds;

    public void Initialize(float speed, ARPlane plane)
    {
        moveSpeed = speed;
        moveDirection = GetRandomDirection();
        planeBounds = new Vector2(plane.size.x, plane.size.y); // Use x and y instead of z
    }

    private void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.localPosition.x) > planeBounds.x / 2 ||
            Mathf.Abs(transform.localPosition.z) > planeBounds.y / 2)
        {
            moveDirection = GetRandomDirection();
        }
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
}
