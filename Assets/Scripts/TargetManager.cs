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

        Debug.Log("Spawning targets on selected plane.");

        for (int i = 0; i < targetCount; i++)
        {
            Vector3 randomPosition = GetRandomPositionOnPlane(plane);
            GameObject target = Instantiate(targetPrefab, randomPosition, Quaternion.identity, plane.transform);

            if (target == null)
            {
                Debug.LogError("Target instantiation failed.");
                continue;
            }

            TargetBehavior targetBehavior = target.GetComponent<TargetBehavior>();
            if (targetBehavior != null)
            {
                float randomSpeed = Random.Range(0.1f, 0.3f); // Adjust speed range as needed
                targetBehavior.Initialize(randomSpeed, plane);
                Debug.Log("Target spawned at position: " + randomPosition);
            }
            else
            {
                Debug.LogError("Target prefab is missing TargetBehavior script.");
            }
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
