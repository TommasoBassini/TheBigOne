using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityStandardAssets.Characters.FirstPerson;

public class AI_SentinelComponent : MonoBehaviour {

	#region SENTINEL_PARAMETERS
	[Header ("Boolean Flags")]

	[Tooltip ("DO NOT TOUCH!")]
	public bool sentinelHasEnlargedItsHearingColliders;
	[Tooltip ("DO NOT TOUCH!")]
	public bool sentinelIsScanning;
	[Tooltip ("DO NOT TOUCH!")]
	public bool sentinelEndsScanning;
	[Tooltip ("DO NOT TOUCH!")]
	public bool sentinelIsFallingIntoLine;
	[Tooltip ("DO NOT TOUCH!")]
	public bool playerHasBeenHeard;
	[Tooltip ("DO NOT TOUCH!")]
	public bool playerInSight;							// Whether or not the player is currently sighted
	[Tooltip ("DO NOT TOUCH!")]
	public bool agentHasBeenStopped;


	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH! Ask programmers for utilization")]
	public int destPoint;

	[Tooltip ("Determines the plane angle in wich the enemy could spot the player (from 0f to 360f)")]
	[Range (0f, 360f)] public float fieldOfViewAngle = 110f;               // Number of degrees, centred on forward, for the enemy see
	[Tooltip ("Determines the SQUARED attack distance of the enemy (from 0f to 100f)")]
	[Range (0f, 100f)] public float sqrAttackDistance = 25f;
	[Tooltip ("Determines the time wich the Sentinel scans around it if lost the player (from 0f to 10f)")]
	[Range (0f, 10f)] public float scanningTime = 5f;
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
	public SphereCollider viewCol;                     // Reference to the "View" sphere collider trigger component
	[Tooltip ("DO NOT TOUCH!")]
	public SphereCollider runCol;                      // Reference to the "Run" sphere collider trigger component
	[Tooltip ("DO NOT TOUCH!")]
	public SphereCollider walkCol;                     // Reference to the "Walk" sphere collider trigger component
	[Tooltip ("DO NOT TOUCH!")]
	public SphereCollider crouchCol;                   // Reference to the "Crouch" sphere collider trigger component
	[Tooltip ("DO NOT TOUCH!")]
	public LineRenderer attackRay;


	[Header ("Scripts")]

	[Tooltip ("DO NOT TOUCH!")]
	public FirstPersonController player;			   // Reference to the player


	[Header ("Patrolling Points")]

	[Tooltip ("First, set the number; second, manually assign any GameObject desidered to be a patrolling point (Transforms will be automatically taken)")]
	public Transform[] points;
	#endregion


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake() {

		this.agent = this.GetComponent <NavMeshAgent> ();
		this.InitializeSphereColliders (out this.viewCol, out this.runCol, out this.walkCol, out this.crouchCol);
		this.attackRay = this.GetComponentInChildren <LineRenderer> (true);
		this.player = GameObject.FindGameObjectWithTag ("Player").GetComponent <FirstPersonController> ();

	}


	public void Start () {

		this.sentinelHasEnlargedItsHearingColliders = false;
		this.sentinelIsScanning = false;
		this.sentinelEndsScanning = false;
		this.sentinelIsFallingIntoLine = false;
		this.playerHasBeenHeard = false;
		this.playerInSight = false;
		this.agentHasBeenStopped = false;
		this.agent.autoBraking = false;

		this.destPoint = 0;

	}


	public void OnTriggerStay (Collider other) {
		
		// If the player has entered the trigger sphere...
		if (other.gameObject == this.player.gameObject) {
			
			// By default the player is not in sight.
			this.playerInSight = false;
			
			// Compute a vector from the enemy to the player and store the angle between it and forward.
			this.direction = other.transform.position - this.transform.position;
			this.angle = Vector3.Angle (this.direction, this.transform.forward);
			
			// If the angle between forward and where the player is, is less than half the angle of view...
			if (this.angle < this.fieldOfViewAngle * 0.5f) {
				
				// ... and if a raycast towards the player hits something...
				if (Physics.Raycast (this.transform.position, this.direction.normalized, out this.hit, this.viewCol.radius)) {
					
					// ... and if the raycast hits the player...
					if (this.hit.collider.gameObject == this.player.gameObject) {
						
						// ... the player is in sight...
						this.playerInSight = true;
						
						if (this.direction.sqrMagnitude < this.sqrAttackDistance) {

							this.agent.Stop ();
							this.agentHasBeenStopped = true;
							this.attackRay.enabled = true;
							this.attackRay.SetPosition (0, this.attackRay.transform.position);
							this.attackRay.SetPosition (1, other.transform.position);

							// ... and may be attacked.
							Debug.LogWarning ("Shooting!");
							
						} else if (this.agentHasBeenStopped)
							this.ResumeAgent ();
						
					} else if (this.agentHasBeenStopped)
						this.ResumeAgent ();
					
				} else if (this.agentHasBeenStopped)
					this.ResumeAgent ();
				
			} else if (this.agentHasBeenStopped)
				this.ResumeAgent ();
			
			this.HearingCollision (this.player.run, this.runCol, other);

			if (!this.playerHasBeenHeard && !this.player.isCrouched)
				this.HearingCollision (this.player.walking, this.walkCol, other);

			if (!this.playerHasBeenHeard)
				this.HearingCollision (this.player.isCrouched, this.crouchCol, other);

		}

	}


	public void OnTriggerExit (Collider other) {

		// If the player leaves the trigger zone...
		if (other.gameObject == this.player.gameObject) {

			/*this.HearingExit (this.player.isCrouched, this.crouchCol, other);

			if (!this.playerHasBeenHeard && !this.player.isCrouched)
			    this.HearingExit (this.player.walking, this.walkCol, other);

			if (!this.playerHasBeenHeard)
				this.HearingExit (this.player.run, this.runCol, other);*/

			if ((other.transform.position - this.transform.position).sqrMagnitude > Mathf.Pow (this.viewCol.radius, 2f)) {

				// ... the player is not in sight.
				this.playerInSight = false;

				if (this.agentHasBeenStopped)
					this.ResumeAgent ();

			}

		}

	}
	#endregion


	#region SENTINEL_METHODS
	public void InitializeSphereColliders (out SphereCollider viewCol, out SphereCollider runCol, out SphereCollider walkCol, out SphereCollider crouchCol) {

		List <SphereCollider> sphereColliders = new List <SphereCollider> ();

		this.GetComponents <SphereCollider> (sphereColliders);

		viewCol = this.InitializeSphereCollider (sphereColliders);
		runCol = this.InitializeSphereCollider (sphereColliders);
		walkCol = this.InitializeSphereCollider (sphereColliders);
		crouchCol = this.InitializeSphereCollider (sphereColliders);

	}


	public SphereCollider InitializeSphereCollider (List <SphereCollider> sphereColliders) {

		int i = sphereColliders.Count - 1;

		SphereCollider spCol = sphereColliders [0];


		foreach (SphereCollider sphereCollider in sphereColliders) {
			
			if (sphereCollider.radius > spCol.radius)
				spCol = sphereCollider;
			
		}

		do {

			if (sphereColliders [i].radius == spCol.radius) {

				sphereColliders.RemoveAt (i);
				break;

			}

		} while (--i >= 0);

		return spCol;
			
	}


	public void HearingCollision (bool playerMightBeHeard, SphereCollider col, Collider other) {

		// ... and is emitting noises.
		if (playerMightBeHeard && this.player.transform.hasChanged) {
			
			// By default the player has not been heard.
			this.playerHasBeenHeard = false;
			
			// Compute a vector from the enemy to the player...
			this.direction = other.transform.position - this.transform.position;
			
			// ... and if a raycast towards the player hits something...
			if (Physics.Raycast (this.transform.position, this.direction.normalized, out this.hit, col.radius)) {
				
				// ... and if the raycast hits the player...
				if (this.hit.collider.gameObject == this.player.gameObject) {
					
					// ... the player has been heard running...
					this.playerHasBeenHeard = true;
					
				}
				
			}
			
		}

	}


	/*public void HearingExit (bool playerIsInRightState, SphereCollider col, Collider other) {

		if (playerIsInRightState) {

			if ((other.transform.position - this.transform.position).sqrMagnitude > Mathf.Pow (col.radius, 2f)) {

				// ... the player cannot be heard.
				this.playerHasBeenHeard = false;

            }
            else
            {
                this.playerHasBeenHeard = true;
            }

        }
        else
        {
            this.playerHasBeenHeard = false;
        }

	}*/


	public void ResumeAgent () {

		this.agent.Resume ();
		this.agentHasBeenStopped = false;
		this.attackRay.enabled = false;

	}
	#endregion

}