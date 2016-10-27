using UnityEngine;
using System.Collections;

public class AI_TurretComponent : MonoBehaviour {

	#region TURRET_PARAMETERS
	[Header ("Boolean Flags")]

	[Tooltip ("DO NOT TOUCH!")]
	public bool playerHasBeenDetected;
	[Tooltip ("DO NOT TOUCH!")]
	public bool playerInSight;							// Whether or not the player is currently sighted


	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH! Ask programmers for utilization")]
	public int destPoint;


	[Header ("Structs")]

	[Tooltip ("DO NOT TOUCH!")]
	public Vector3 direction;
	[Tooltip ("DO NOT TOUCH!")]
	public RaycastHit hit;


	[Header ("Classes")]

	[Tooltip ("DO NOT TOUCH!")]
	public BoxCollider col;                         	// Reference to the box collider trigger component


	[Header ("GameObjects")]

	[Tooltip ("DO NOT TOUCH!")]
	public GameObject player;							// Reference to the player


	[Header ("Patrolling Points")]

	[Tooltip ("First, set the number; second, manually assign any GameObject desidered to be a patrolling point (Transforms will be automatically taken)")]
	public Transform[] points;
	#endregion


	#region DRONE_MONOBEHAVIOUR_METHODS
	public void Awake() {

		this.col = this.GetComponent <BoxCollider> ();
		this.player = GameObject.FindGameObjectWithTag ("Player");

	}


	public void Start () {

		this.playerHasBeenDetected = false;
		this.playerInSight = false;

		this.destPoint = 0;

	}


	public void OnTriggerStay (Collider other) {
		
		// If the player has entered the trigger box...
		if (other.gameObject == this.player) {

			if (this.playerHasBeenDetected) {

				// By default the player is not in sight.
				this.playerInSight = false;
				
				// Compute a vector from the enemy to the player...
				this.direction = other.transform.position - this.transform.position;
				
				// ... and if a raycast towards the player hits something...
				if (Physics.Raycast (this.transform.position, this.direction.normalized, out this.hit)) {
					
					// ... and if the raycast hits the player...
					if (this.hit.collider.gameObject == this.player) {
						
						// ... the player is in sight...
						this.playerInSight = true;

						// ... and may be attacked.
						Debug.LogWarning ("Shooting!");
							
					}
					
				}

			}

		}
		
	}


	public void OnTriggerExit (Collider other) {

		// If the player leaves the trigger zone...
		if (other.gameObject == this.player) {

			// ... the player is not in sight.
			this.playerInSight = false;

		}

	}
	#endregion

}