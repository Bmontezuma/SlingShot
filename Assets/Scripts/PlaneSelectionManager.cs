using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PlaneSelectionManager : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;
    private ARRaycastManager arRaycastManager;
    private static ARPlane selectedPlane;
    public GameUIManager gameUIManager;
    public TargetSpawner targetSpawner;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool planeSelected = false;

    void Start()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!planeSelected && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            UnityEngine.Touch touch = Input.GetTouch(0);
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

        foreach (ARPlane p in arPlaneManager.trackables)
        {
            if (p != selectedPlane)
            {
                p.gameObject.SetActive(false);
            }
        }

        selectedPlane.gameObject.SetActive(true);

        if (targetSpawner != null)
        {
            targetSpawner.SetSelectedPlane(selectedPlane);
        }
        
        gameUIManager.OnPlaneSelected(); // Notify GameUIManager to handle UI
    }

    public static ARPlane GetSelectedPlane()
    {
        return selectedPlane;
    }
}
