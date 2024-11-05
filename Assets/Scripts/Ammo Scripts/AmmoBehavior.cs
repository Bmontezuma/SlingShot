using UnityEngine;
using System;

public class AmmoBehavior : MonoBehaviour
{
    public float launchForceMultiplier = 10f;
    public LineRenderer lineRenderer;
    public Action OnAmmoDepleted; // Callback to notify manager when ammo is used

    private Vector3 initialPosition;
    private Vector3 dragStartPosition;
    private Vector3 dragEndPosition;
    private Rigidbody rb;
    private bool isDragging = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + 1f));
        transform.position = initialPosition;
        rb.useGravity = false;
        lineRenderer.enabled = false;
        gameObject.SetActive(false); // Hide Ammo initially
    }

    private void Update()
    {
        if (Input.touchCount > 0 && gameObject.activeSelf) // Only respond to input if Ammo is active
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && rb.linearVelocity == Vector3.zero)
            {
                isDragging = true;
                dragStartPosition = GetTouchWorldPosition(touch.position);
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
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
        return Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane + 1f));
    }

    private void DragAmmo(Vector2 touchPosition)
    {
        Vector3 currentTouchPosition = GetTouchWorldPosition(touchPosition);
        Vector3 dragOffset = currentTouchPosition - dragStartPosition;
        transform.position = initialPosition + dragOffset;
        lineRenderer.SetPosition(1, transform.position);
    }

    private void LaunchAmmo()
    {
        Vector3 launchDirection = (dragStartPosition - dragEndPosition).normalized;
        float dragDistance = Vector3.Distance(dragStartPosition, dragEndPosition);
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(launchDirection * dragDistance * launchForceMultiplier, ForceMode.Impulse);
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target") || collision.gameObject.CompareTag("Plane") || IsOutOfBounds())
        {
            OnAmmoDepleted?.Invoke(); // Notify manager when ammo needs to be replaced
            Destroy(gameObject); // Destroy current Ammo instance
        }
    }

    private bool IsOutOfBounds()
    {
        return transform.position.y < -5f;
    }

    public void ActivateAmmo()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
