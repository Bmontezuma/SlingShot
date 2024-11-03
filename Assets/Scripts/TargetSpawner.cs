using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int targetCount = 5;
    private ARPlane selectedPlane;

    public void SetSelectedPlane(ARPlane plane)
    {
        selectedPlane = plane;
        Debug.Log("Plane selected for target spawning: " + selectedPlane.trackableId);
    }

    public void SpawnTargets()
    {
        if (selectedPlane == null)
        {
            Debug.LogError("No plane selected for spawning targets.");
            return;
        }

        // Get the plane dimensions
        float x_dim = selectedPlane.size.x / 2; // Half the width to stay within bounds
        float z_dim = selectedPlane.size.y / 2; // Half the length to stay within bounds

        Debug.Log("Spawning targets...");

        for (int i = 0; i < targetCount; i++)
        {
            // Random x and z positions within the bounds of the plane
            float x_rand = Random.Range(-x_dim, x_dim);
            float z_rand = Random.Range(-z_dim, z_dim);
            Vector3 spawnPosition = selectedPlane.transform.TransformPoint(new Vector3(x_rand, 0, z_rand));

            // Instantiate the target at the calculated position as a child of the plane
            GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity, selectedPlane.transform);
            target.transform.localScale = Vector3.one * 0.2f; // Adjust for AR visibility

            // Detach from the plane to make it independent
            target.transform.parent = null;
            Debug.Log("Target spawned at: " + spawnPosition);
        }
    }
}
