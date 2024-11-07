using UnityEngine;
using System;

public class AmmoBehavior : MonoBehaviour
{
    public float launchForceMultiplier = 10f;
    public LineRenderer lineRenderer;
    public Action OnAmmoDepleted;

    private Vector3 initialPosition;
    private Vector3 dragStartPosition;
    private Vector3 dragEndPosition;
    private Rigidbody rb;
    private bool isDragging = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Set Rigidbody to kinematic initially for stable dragging
        rb.isKinematic = true;
        
        // Set initial position a bit in front of the camera for visibility
        initialPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 1f));
        transform.position = initialPosition;
        
        // Disable gravity and ensure LineRenderer is set up
        rb.useGravity = false;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
        else
        {
            Debug.LogWarning("LineRenderer is not assigned.");
        }

        Debug.Log($"Ammo initialized at position: {transform.position}");
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && rb.linearVelocity == Vector3.zero)
            {
                isDragging = true;
                dragStartPosition = GetTouchWorldPosition(touch.position);
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                Debug.Log("Touch began, starting drag.");
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                DragAmmo(touch.position);
                ShowTrajectory();
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                isDragging = false;
                dragEndPosition = GetTouchWorldPosition(touch.position);
                LaunchAmmo();
                lineRenderer.enabled = false;
            }
        }
    }

    private Vector3 GetTouchWorldPosition(Vector2 touchPosition)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 1f));
    }

    private void DragAmmo(Vector2 touchPosition)
    {
        Vector3 currentTouchPosition = GetTouchWorldPosition(touchPosition);
        Vector3 dragOffset = currentTouchPosition - dragStartPosition;
        transform.position = initialPosition + dragOffset;
        lineRenderer.SetPosition(1, transform.position);
        Debug.Log($"Dragging Ammo. Current Position: {transform.position}");
    }

    private void LaunchAmmo()
    {
        Vector3 launchDirection = (dragStartPosition - dragEndPosition).normalized;
        float dragDistance = Vector3.Distance(dragStartPosition, dragEndPosition);

        // Switch Rigidbody to dynamic to allow movement and enable gravity
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Apply force to launch
        rb.AddForce(launchDirection * dragDistance * launchForceMultiplier, ForceMode.Impulse);
        Debug.Log($"Ammo launched with force: {launchDirection * dragDistance * launchForceMultiplier}");
    }

    // ShowTrajectory method to visualize the launch path
    private void ShowTrajectory()
    {
        Vector3 launchDirection = (dragStartPosition - transform.position).normalized;
        float dragDistance = Vector3.Distance(dragStartPosition, transform.position);
        Vector3 launchVelocity = launchDirection * dragDistance * launchForceMultiplier;

        lineRenderer.positionCount = 10;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float time = i * 0.1f;
            Vector3 point = CalculateTrajectoryPoint(transform.position, launchVelocity, time);
            lineRenderer.SetPosition(i, point);
        }
    }

    private Vector3 CalculateTrajectoryPoint(Vector3 startPosition, Vector3 startVelocity, float time)
    {
        return startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;
    }

    public void ResetAmmo()
    {
        transform.position = initialPosition;
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log($"Ammo reset to initial position: {transform.position}");
    }
}
