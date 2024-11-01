using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using System.Collections.Generic;

public class PlaneSelectionManager : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;
    private ARRaycastManager arRaycastManager;
    private static ARPlane selectedPlane;
    public GameObject startButton; // Assign in Inspector
    public GameUIManager gameUIManager; // Reference to GameUIManager
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Event to notify when a plane is selected
    public static event Action<ARPlane> onPlaneSelected;

    void Start()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        arRaycastManager = GetComponent<ARRaycastManager>();
        
        if (startButton != null)
        {
            startButton.SetActive(false); // Hide Start button initially
        }
        else
        {
            Debug.LogError("Start Button not assigned in Inspector!");
        }
    }

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            if (TryGetPlaneFromTouch(touch.position, out ARPlane plane))
            {
                SelectPlane(plane);
            }
        }
    }

    bool TryGetPlaneFromTouch(Vector2 touchPosition, out ARPlane plane)
    {
        plane = null;
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitTrackableId = hits[0].trackableId;
            plane = arPlaneManager.GetPlane(hitTrackableId);
            return plane != null;
        }
        return false;
    }

    void SelectPlane(ARPlane plane)
    {
        selectedPlane = plane;

        // Disable all other planes but keep the selected plane active
        foreach (ARPlane p in arPlaneManager.trackables)
        {
            if (p != selectedPlane)
            {
                p.gameObject.SetActive(false);
            }
        }

        // Ensure the selected plane remains visible
        selectedPlane.gameObject.SetActive(true);

        // Show the Start button if it exists
        if (startButton != null)
        {
            startButton.SetActive(true);
        }

        // Invoke the onPlaneSelected event to notify other scripts
        onPlaneSelected?.Invoke(selectedPlane);
    }

    public void OnStartButtonPressed()
    {
        Debug.Log("Start button pressed; game starting.");
        startButton.SetActive(false); // Hide the Start button
        gameUIManager.StartGame(); // Trigger GameUIManager to take over
    }

    public static ARPlane GetSelectedPlane()
    {
        return selectedPlane;
    }
}
