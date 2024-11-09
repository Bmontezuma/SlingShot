using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // For TextMeshPro UI
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    public int maxAmmo = 7;
    public float maxHoldTime = 2.0f;
    public float forceMultiplier = 500f;
    
    public TextMeshProUGUI ammoCountText;    // Reference for ammo count display
    public TextMeshProUGUI scoreText;        // Reference for score display
    public Button playAgainButton;

    private int currentAmmo;
    private int score;
    private float holdTime;
    private bool isDragging;
    private Vector3 initialPosition;
    private LineRenderer lineRenderer;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        
        initialPosition = transform.position;
        currentAmmo = maxAmmo;
        score = 0;
        UpdateUI();
        playAgainButton.gameObject.SetActive(false);
    }

    void Update()
    {
        // Handle touch input for drag
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && IsTouchingAmmo(touch))
            {
                StartDrag();
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                DragAmmo(touch);
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                Release();
            }
        }
    }

    private bool IsTouchingAmmo(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform;
        }
        return false;
    }

    private void StartDrag()
    {
        isDragging = true;
        holdTime = 0;
        rb.isKinematic = true;
        lineRenderer.enabled = true;
    }

    private void DragAmmo(Touch touch)
    {
        holdTime += Time.deltaTime;
        holdTime = Mathf.Clamp(holdTime, 0, maxHoldTime);

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
        transform.position = Vector3.Lerp(transform.position, touchPosition, 0.1f);

        UpdateLineRenderer();
    }

    private void Release()
    {
        isDragging = false;
        rb.isKinematic = false;
        lineRenderer.enabled = false;

        Vector3 shootDirection = (initialPosition - transform.position).normalized;
        float force = forceMultiplier * (holdTime / maxHoldTime);
        rb.AddForce(shootDirection * force);

        currentAmmo--;
        UpdateUI();

        if (currentAmmo > 0)
        {
            StartCoroutine(ResetAmmo());
        }
        else
        {
            playAgainButton.gameObject.SetActive(true);
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, initialPosition);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void UpdateUI()
    {
        ammoCountText.text = "Ammo: " + currentAmmo;
        scoreText.text = "Score: " + score;
    }

    private IEnumerator ResetAmmo()
    {
        yield return new WaitForSeconds(1f);
        transform.position = initialPosition;
        rb.linearVelocity = Vector3.zero;  // Updated to use linearVelocity
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    public void PlayAgain()
    {
        currentAmmo = maxAmmo;
        score = 0;
        UpdateUI();
        playAgainButton.gameObject.SetActive(false);
        StartCoroutine(ResetAmmo());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            score++;
            UpdateUI();
            Destroy(collision.gameObject);  // Destroy the enemy on hit
        }
    }
}
