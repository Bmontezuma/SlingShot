using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    public GameObject targetPrefab; // Assign target prefab in Inspector
    public int targetCount = 5; // Number of targets to spawn

    private void OnEnable()
    {
        PlaneSelectionManager.onPlaneSelected += SpawnTargetsOnPlane;
    }

    private void OnDisable()
    {
        PlaneSelectionManager.onPlaneSelected -= SpawnTargetsOnPlane;
    }

    private void SpawnTargetsOnPlane(ARPlane plane)
    {
        if (plane == null)
        {
            Debug.LogError("No plane selected for spawning targets.");
            return;
        }

        // Spawn targets on the selected plane
        for (int i = 0; i < targetCount; i++)
        {
            Vector3 randomPosition = GetRandomPositionOnPlane(plane);
            Instantiate(targetPrefab, randomPosition, Quaternion.identity, plane.transform); // Attach to plane
        }
    }

    private Vector3 GetRandomPositionOnPlane(ARPlane plane)
    {
        // Generate a random position within the bounds of the selected plane
        Vector3 center = plane.transform.position;
        Vector2 planeSize = plane.size;

        float randomX = UnityEngine.Random.Range(-planeSize.x / 2, planeSize.x / 2);
        float randomZ = UnityEngine.Random.Range(-planeSize.y / 2, planeSize.y / 2);

        return center + new Vector3(randomX, 0, randomZ);
    }
}
