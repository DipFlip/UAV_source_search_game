using UnityEngine;
using UnityEngine.AI;
using System.Collections;
 
public class WanderingAI : MonoBehaviour {
 
    public float wanderRadius = 1000f;
    public float wanderTimer;
 
    private Transform target;
    private NavMeshAgent agent;
    private float timer;
 
    // Use this for initialization
    void OnEnable () {
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;
    }
    void Start(){
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
        timer = 0;
    }
 
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        // check if reached destination
        if (timer > 5){

        if (agent.remainingDistance <= agent.stoppingDistance) {
            // if reached destination, set new destination
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
        }
        }
        // if (timer >= wanderTimer) {
        //     Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        //     agent.SetDestination(newPos);
        //     timer = 0;
        // }
    }
 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
}