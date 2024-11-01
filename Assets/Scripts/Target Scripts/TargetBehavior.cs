using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TargetBehavior : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveDirection;
    private Vector2 planeBounds;
    private Transform deviceCamera;

    public void Initialize(float speed, ARPlane plane)
    {
        moveSpeed = speed;
        moveDirection = GetRandomDirection();
        planeBounds = new Vector2(plane.size.x, plane.size.y); // Get the plane's width and height
        deviceCamera = Camera.main.transform; // Reference to the camera
        AdjustScaleBasedOnDistance(); // Initial scale adjustment
    }

    private void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Boundary check and change direction if needed
        if (Mathf.Abs(transform.localPosition.x) > planeBounds.x / 2 ||
            Mathf.Abs(transform.localPosition.z) > planeBounds.y / 2)
        {
            moveDirection = GetRandomDirection();
        }

        AdjustScaleBasedOnDistance();
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private void AdjustScaleBasedOnDistance()
    {
        float distance = Vector3.Distance(deviceCamera.position, transform.position);
        float scaleFactor = Mathf.Clamp(1 / distance, 0.1f, 1f); // Adjust scale range as necessary
        transform.localScale = Vector3.one * scaleFactor;
    }
}
