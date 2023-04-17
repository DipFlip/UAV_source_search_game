using System.Collections.Generic;
using UnityEngine;

public class SourceDetector : MonoBehaviour
{
    private List<GameObject> sourcesInRange;

    private void Start()
    {
        sourcesInRange = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Source"))
        {
            sourcesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Source"))
        {
            sourcesInRange.Remove(other.gameObject);
        }
    }
    public void RemoveSource(GameObject source)
    {
        sourcesInRange.Remove(source);
    }

    public GameObject GetClosestSource()
    {
        GameObject closestSource = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject source in sourcesInRange)
        {
            float distance = Vector3.Distance(transform.position, source.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestSource = source;
            }
        }

        return closestSource;
    }
}
