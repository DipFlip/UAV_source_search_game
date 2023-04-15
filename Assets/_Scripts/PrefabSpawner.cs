using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawnGood;
    public GameObject prefabToSpawnBad;
    public LayerMask flagableLayers;
    public int numberOfRays = 12;
    public float raycastDistance = 10f;
    public float sourceDetectionDistance = 5f;
    public SourceDetector sourceDetector;

    public void SpawnPrefabClosest()
    {
        Vector3 dronePosition = transform.position;
        float closestHitDistance = Mathf.Infinity;
        Vector3 spawnPosition = new Vector3(0,0,0);
        int verticalDivisions = numberOfRays / 4; // Adjust this value according to the desired vertical resolution
        // Additional raycast straight down
        RaycastHit straightDownHit;
        if (Physics.Raycast(dronePosition, Vector3.down, out straightDownHit, raycastDistance, flagableLayers))
        {
            closestHitDistance = straightDownHit.distance;
            spawnPosition = straightDownHit.point;

        }
        {
            closestHitDistance = straightDownHit.distance;
            spawnPosition = straightDownHit.point;

        }
        for (int i = 0; i < numberOfRays; i++)
        {
            float horizontalAngle = i * 360f / numberOfRays;

            for (int j = -verticalDivisions / 2; j < verticalDivisions / 2; j++)
            {
                float verticalAngle = j * 180f / verticalDivisions;

                Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
                Vector3 direction = rotation * Vector3.forward;

                RaycastHit hit;
                if (Physics.Raycast(dronePosition, direction, out hit, raycastDistance, flagableLayers))
                {
                    if (hit.distance < closestHitDistance)
                    {
                        closestHitDistance = hit.distance;
                        spawnPosition = hit.point;
                    }
                }
            }
        }

        if (spawnPosition != new Vector3(0,0,0))
        {
            GameObject prefabToSpawn;
            GameObject closestSource = sourceDetector.GetClosestSource();

            if (closestSource != null && Vector3.Distance(dronePosition, closestSource.transform.position) <= sourceDetectionDistance)
            {
                prefabToSpawn = prefabToSpawnGood;
                spawnPosition = closestSource.transform.position;
            }
            else
            {
                prefabToSpawn = prefabToSpawnBad;
            }

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
