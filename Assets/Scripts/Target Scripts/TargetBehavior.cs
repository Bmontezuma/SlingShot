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
        planeBounds = new Vector2(plane.size.x, plane.size.y);
        deviceCamera = Camera.main?.transform;

        Debug.Log("Target initialized with speed: " + moveSpeed + ", direction: " + moveDirection + ", bounds: " + planeBounds);
        AdjustScaleBasedOnDistance();
    }

    private void Start()
    {
        // Ensure deviceCamera is set if Initialize is not called immediately
        if (deviceCamera == null)
        {
            deviceCamera = Camera.main?.transform;
            Debug.Log("Device camera assigned in Start: " + deviceCamera);
        }
    }

    private void Update()
    {
        // Move within the X and Z bounds of the plane
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Boundary check with direction reflection to stay within plane bounds
        if (Mathf.Abs(transform.localPosition.x) > planeBounds.x / 2)
        {
            moveDirection.x = -moveDirection.x;
            Debug.Log("Direction reversed on X-boundary hit. New direction: " + moveDirection);
        }

        if (Mathf.Abs(transform.localPosition.z) > planeBounds.y / 2)
        {
            moveDirection.z = -moveDirection.z;
            Debug.Log("Direction reversed on Z-boundary hit. New direction: " + moveDirection);
        }

        // Adjust scale based on distance from the camera
        AdjustScaleBasedOnDistance();
    }

    private Vector3 GetRandomDirection()
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        Debug.Log("Generated random direction: " + direction);
        return direction;
    }

    private void AdjustScaleBasedOnDistance()
    {
        if (deviceCamera == null) return;

        float distance = Vector3.Distance(deviceCamera.position, transform.position);
        float scaleFactor = Mathf.Clamp(1 / distance, 0.1f, 1f);
        transform.localScale = Vector3.one * scaleFactor;

        Debug.Log("Adjusted scale based on distance: " + distance + ", scaleFactor: " + scaleFactor + ", new scale: " + transform.localScale);
    }
}
