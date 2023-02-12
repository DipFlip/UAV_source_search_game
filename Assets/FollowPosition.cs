using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField] private GameObject followTarget;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.transform.position;
    }
}
