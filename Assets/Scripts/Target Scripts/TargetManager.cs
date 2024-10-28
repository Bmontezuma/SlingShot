using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    public GameObject targetPrefab;
    public int targetCount = 5; // Number of targets to instantiate
    public float targetMoveSpeed = 0.5f; // Speed at which targets move

    private List<GameObject> targets = new List<GameObject>();
    private ARPlane selectedPlane;

    private void Start()
    {
        // Only allow targets to spawn after a plane has been selected
        PlaneSelectionManager.onPlaneSelected += InitializeTargets;
    }

    private void OnDestroy()
    {
        PlaneSelectionManager.onPlaneSelected -= InitializeTargets;
    }

    private void InitializeTargets(ARPlane plane)
    {
        selectedPlane = plane;

        // Instantiate targets and place them on the plane
        for (int i = 0; i < targetCount; i++)
        {
            Vector3 position = GetRandomPositionOnPlane(selectedPlane);
            GameObject target = Instantiate(targetPrefab, position, Quaternion.identity, selectedPlane.transform);
            target.AddComponent<TargetBehavior>().Initialize(targetMoveSpeed, selectedPlane);
            targets.Add(target);

            // Scale based on distance to camera
            AdjustScaleByDistance(target);
        }
    }

    private Vector3 GetRandomPositionOnPlane(ARPlane plane)
    {
        // Get random point within the plane boundary
        Vector2 randomInBoundary = Random.insideUnitCircle * plane.size.x / 2;
        Vector3 position = new Vector3(randomInBoundary.x, 0, randomInBoundary.y);
        return plane.center + position; // Offset by the plane center
    }

    private void AdjustScaleByDistance(GameObject target)
    {
        float distanceToCamera = Vector3.Distance(Camera.main.transform.position, target.transform.position);
        float scaleFactor = Mathf.Clamp(1f / distanceToCamera, 0.05f, 0.2f);
        target.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
