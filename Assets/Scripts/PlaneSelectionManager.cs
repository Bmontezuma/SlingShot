using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PlaneSelectionManager : MonoBehaviour
{
    public static ARPlane selectedPlane;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager; // Use ARRaycastManager instead of ARPlaneManager for raycasting
    public GameObject startButton;

    public static event Action<ARPlane> onPlaneSelected;

    private void Start()
    {
        startButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    ARPlane plane = planeManager.GetPlane(hits[0].trackableId);
                    if (plane != null && selectedPlane == null)
                    {
                        SelectPlane(plane);
                    }
                }
            }
        }
    }

    private void SelectPlane(ARPlane plane)
    {
        selectedPlane = plane;

        foreach (var detectedPlane in planeManager.trackables)
        {
            if (detectedPlane != selectedPlane)
                detectedPlane.gameObject.SetActive(false);
        }

        onPlaneSelected?.Invoke(selectedPlane);

        startButton.SetActive(true);
    }
}
