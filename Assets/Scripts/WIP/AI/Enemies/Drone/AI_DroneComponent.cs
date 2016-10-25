using UnityEngine;
using System.Collections;

public class AI_DroneComponent : MonoBehaviour {

    public NavMeshAgent agent;
    public float fieldOfViewAngle = 110f;               // Number of degrees, centred on forward, for the enemy see.
    public bool playerInSight;							// Whether or not the player is currently sighted.
    public SphereCollider col;                         // Reference to the sphere collider trigger component.
    public GameObject player;							// Reference to the player.
    public int destPoint = 0;
    public NavMeshPath path;
    public Transform[] points;
    public bool isPathRandomized = false;
    public bool droneIsFallingIntoLine = false;


    void Awake()
    {
        this.col = this.GetComponent<SphereCollider>();
        this.agent = this.GetComponent<NavMeshAgent>();
        this.agent.autoBraking = false;
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.path = new NavMeshPath();
    }

}
