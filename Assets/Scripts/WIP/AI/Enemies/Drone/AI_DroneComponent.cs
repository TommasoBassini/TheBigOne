using UnityEngine;
using System.Collections;

public class AI_DroneComponent : MonoBehaviour {

	#region DRONE_PARAMETERS
	public bool isPathRandomized;
	public bool droneIsFallingIntoLine;
	public bool playerInSight;							// Whether or not the player is currently sighted

	public int destPoint;

	public float fieldOfViewAngle = 110f;               // Number of degrees, centred on forward, for the enemy see
	public float angle;

	public Vector3 direction;

	public RaycastHit hit;

    public NavMeshAgent agent;

    public SphereCollider col;                         // Reference to the sphere collider trigger component

    public GameObject player;							// Reference to the player

    public Transform[] points;
	#endregion


	#region DRONE_MONOBEHAVIOR_METHODS
    public void Awake() {

		this.agent = this.GetComponent <NavMeshAgent> ();

        this.col = this.GetComponent <SphereCollider> ();

        this.player = GameObject.FindGameObjectWithTag ("Player");

    }


	public void Start () {

		this.droneIsFallingIntoLine = false;
		this.playerInSight = false;
		this.agent.autoBraking = false;

		this.destPoint = 0;

	}


	public void OnTriggerStay (Collider other) {
		
		// If the player has entered the trigger sphere...
		if (other.gameObject == this.player) {
			
			// By default the player is not in sight.
			this.playerInSight = false;

			// Compute a vector from the enemy to the player and store the angle between it and forward.
			this.direction = other.transform.position - this.transform.position;
			this.angle = Vector3.Angle (this.direction, this.transform.forward);

			// If the angle between forward and where the player is, is less than half the angle of view...
			if (this.angle < this.fieldOfViewAngle * 0.5f) {

				// ... and if a raycast towards the player hits something...
				if (Physics.Raycast (this.transform.position, this.direction.normalized, out this.hit, this.col.radius)) {
					
					// ... and if the raycast hits the player...
					if (this.hit.collider.gameObject == this.player) {
						
						// ... the player is in sight.
						this.playerInSight = true;

					}

				}

			}

		}

	}
	#endregion

}