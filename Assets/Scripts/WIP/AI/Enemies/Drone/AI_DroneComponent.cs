using UnityEngine;
using System.Collections;

public class AI_DroneComponent : MonoBehaviour {

	#region DRONE_PARAMETERS
	[Header ("Boolean Flags")]

	[Tooltip ("Enables/Disables path randomization; by default, is disabled")]
	public bool isPathRandomized;
	[Tooltip ("DO NOT TOUCH!")]
	public bool droneIsFallingIntoLine;
	[Tooltip ("DO NOT TOUCH!")]
	public bool playerInSight;							// Whether or not the player is currently sighted


	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH! Ask programmers for utilization")]
	public int destPoint;

	[Tooltip ("Determines the plane angle in wich the enemy could spot the player (from 0f to 360f)")]
	[Range (0f, 360f)] public float fieldOfViewAngle = 110f;               // Number of degrees, centred on forward, for the enemy see
	[Tooltip ("Determines the SQUARED attack distance of the enemy (from 0f to 100f)")]
	[Range (0f, 100f)] public float attackDistance = 5f;
	[Tooltip ("DO NOT TOUCH!")]
	public float angle;


	[Header ("Structs")]

	[Tooltip ("DO NOT TOUCH!")]
	public Vector3 direction;
	[Tooltip ("DO NOT TOUCH!")]
	public RaycastHit hit;


	[Header ("Classes")]

	[Tooltip ("DO NOT TOUCH!")]
    public NavMeshAgent agent;
	[Tooltip ("DO NOT TOUCH!")]
    public SphereCollider col;                         // Reference to the sphere collider trigger component


	[Header ("GameObjects")]

	[Tooltip ("DO NOT TOUCH!")]
    public GameObject player;							// Reference to the player


	[Header ("Patrolling Points")]

	[Tooltip ("First, set the number; second, manually assign any GameObject desidered to be a patrolling point (Transforms will be automatically taken)")]
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
						
						// ... the player is in sight...
						this.playerInSight = true;

						if (this.direction.sqrMagnitude < this.attackDistance) {

							// ... and may be attacked.
							Debug.LogWarning ("Shooting!");

						}

					}

				}

			}

		}

	}
	#endregion

}