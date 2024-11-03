using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int targetCount = 5;
    private ARPlane selectedPlane;
    private bool hasSpawned = false; // Flag to prevent multiple spawns

    // This method is called when a plane is selected
    public void SetSelectedPlane(ARPlane plane)
    {
        selectedPlane = plane;
        hasSpawned = false; // Reset the spawn flag when a new plane is selected
        Debug.Log("Plane selected for target spawning: " + selectedPlane.trackableId);
    }

    // This method spawns targets on the selected plane only once
    public void SpawnTargets()
    {
        if (selectedPlane == null)
        {
            Debug.LogError("No plane selected for spawning targets.");
            return;
        }

        if (hasSpawned)
        {
            Debug.Log("Targets have already been spawned. Skipping additional spawn.");
            return; // Exit if targets have already been spawned
        }

        hasSpawned = true; // Set the flag to prevent further spawns

        float x_dim = selectedPlane.size.x / 2;
        float z_dim = selectedPlane.size.y / 2;

        Debug.Log("Spawning targets within plane bounds...");

        for (int i = 0; i < targetCount; i++)
        {
            float x_rand = Random.Range(-x_dim, x_dim);
            float z_rand = Random.Range(-z_dim, z_dim);
            Vector3 spawnPosition = selectedPlane.transform.TransformPoint(new Vector3(x_rand, 0, z_rand));

            GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity, selectedPlane.transform);
            target.transform.localScale = Vector3.one * 0.2f; // Initial scale for AR visibility
            target.transform.parent = null;

            TargetBehavior targetBehavior = target.GetComponent<TargetBehavior>();
            if (targetBehavior != null)
            {
                targetBehavior.Initialize(Random.Range(0.1f, 0.5f), selectedPlane);
            }

            Debug.Log("Target spawned at position: " + spawnPosition + " with scale: " + target.transform.localScale);
        }
    }
}
