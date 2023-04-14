using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabOnProps : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int minSpawns = 1;
    public int maxSpawns = 5;

    private void Start()
    {
        SpawnPrefabOnRandomProps();
    }

    public void SpawnPrefabOnRandomProps()
    {
        GameObject[] props = GameObject.FindGameObjectsWithTag("Prop");
        int totalSpawns = Random.Range(minSpawns, maxSpawns + 1);

        if (totalSpawns > props.Length)
        {
            totalSpawns = props.Length;
        }

        List<GameObject> selectedProps = new List<GameObject>();

        for (int i = 0; i < totalSpawns; i++)
        {
            GameObject randomProp = props[Random.Range(0, props.Length)];

            while (selectedProps.Contains(randomProp))
            {
                randomProp = props[Random.Range(0, props.Length)];
            }

            selectedProps.Add(randomProp);
            Instantiate(prefabToSpawn, randomProp.transform.position, Quaternion.identity, randomProp.transform);
        }
    }
}
