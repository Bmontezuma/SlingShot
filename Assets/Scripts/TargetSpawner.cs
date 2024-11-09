using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int targetCount = 5;
    private ARPlane selectedPlane;
    private bool hasSpawned = false;
    private List<GameObject> activeTargets = new List<GameObject>();

    public void SetSelectedPlane(ARPlane plane)
    {
        selectedPlane = plane;
        hasSpawned = false;
    }

    public void SpawnTargets()
    {
        if (selectedPlane == null || hasSpawned) return;

        hasSpawned = true;
        float x_dim = selectedPlane.size.x / 4;
        float z_dim = selectedPlane.size.y / 4;

        for (int i = 0; i < targetCount; i++)
        {
            float x_rand = Random.Range(-x_dim, x_dim);
            float z_rand = Random.Range(-z_dim, z_dim);
            Vector3 spawnPosition = selectedPlane.transform.TransformPoint(new Vector3(x_rand, 0, z_rand));

            GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity, selectedPlane.transform);
            
            // Apply custom scale
            target.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            target.transform.parent = null;

            TargetBehavior targetBehavior = target.GetComponent<TargetBehavior>();
            if (targetBehavior != null)
            {
                targetBehavior.Initialize(Random.Range(0.1f, 0.5f), selectedPlane);
            }

            activeTargets.Add(target);
        }
    }

    public List<GameObject> GetActiveTargets()
    {
        return activeTargets;
    }
}
