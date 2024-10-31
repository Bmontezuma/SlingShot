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
            Debug.Log("Input got");
            Touch touch = Input.GetTouch(0);
            if (TryGetPlaneFromTouch(touch.position, out ARPlane plane))
            {
                SelectPlane(plane);
            }
        }
    }

    bool TryGetPlaneFromTouch(Vector2 touchPosition, out ARPlane plane)
    {
        Debug.Log("touch pos: " + touchPosition);
        plane = null;
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // Get the ARPlane using the trackableId from the hit result
            var hitTrackableId = hits[0].trackableId;
            plane = arPlaneManager.GetPlane(hitTrackableId);
            Debug.Log("Plane detected and selected.");
            return plane != null;
        }
        Debug.Log("No plane detected at touch position.");
        return false;
    }

    void SelectPlane(ARPlane plane)
    {
        // Save the selected plane
        selectedPlane = plane;

        // Disable all other planes
        foreach (ARPlane p in arPlaneManager.trackables)
        {
            if (p != selectedPlane)
            {
                p.gameObject.SetActive(false);
            }
        }

        // Show the Start button if it exists
        if (startButton != null)
        {
            startButton.SetActive(true);
            Debug.Log("Start button activated on plane selection.");
        }

        // Invoke the onPlaneSelected event
        onPlaneSelected?.Invoke(selectedPlane);
    }

    public static ARPlane GetSelectedPlane()
    {
        return selectedPlane;
    }
}
