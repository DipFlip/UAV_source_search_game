using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabBelowPlayer : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public LayerMask groundLayers;
    public float maxRaycastDistance = 100f;
    private string carTag = "Car";

    public void SpawnPrefab()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxRaycastDistance, groundLayers))
        {
            Vector3 spawnPosition = hit.point;
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            
            if (hit.collider.gameObject.CompareTag(carTag))
            {
                spawnedPrefab.transform.SetParent(hit.collider.transform);
            }
        }
        else
        {
            Debug.LogWarning("No surface found below player within maxRaycastDistance.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnPrefab();
        }
    }
}
