using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityStandardAssets.Characters.FirstPerson;

public class AI_SentinelComponent : MonoBehaviour {

	#region SENTINEL_DELEGATES
	public EnemyDelegate <AI_SentinelComponent> DelegatedMethod = delegate (AI_SentinelComponent sentinelReference, Collider other) {
		
		if (!sentinelReference.enemyHasBeenStunned) {
			
			// By default the player is not in sight.
			sentinelReference.playerInSight = false;
			sentinelReference.playerHasBeenHeard = false;
			
			// Compute a vector from the enemy to the player and store the angle between it and forward.
			sentinelReference.direction = other.transform.position - sentinelReference.transform.position;
			sentinelReference.angle = Vector3.Angle (sentinelReference.direction, sentinelReference.transform.forward);
			
			// If the angle between forward and where the player is, is less than half the angle of view...
			if (sentinelReference.angle < sentinelReference.fieldOfViewAngle * 0.5f) {
				
				// ... and if a raycast towards the player hits something...
				if (Physics.Raycast (sentinelReference.transform.position, sentinelReference.direction.normalized, out sentinelReference.hit, sentinelReference.viewCol.radius)) {
					
					// ... and if the raycast hits the player...
					if (sentinelReference.hit.collider.gameObject == sentinelReference.player.gameObject) {
						
						// ... the player is in sight...
						sentinelReference.playerInSight = true;
						
						if (sentinelReference.direction.sqrMagnitude < Mathf.Pow (sentinelReference.attackDistance, 2f)) {
							
							// ... and may be attacked.
							sentinelReference.StopAgent ();
							Debug.LogWarning ("Shooting!");
							
						} else if (sentinelReference.agentHasBeenStopped)
							sentinelReference.ResumeAgent ();
						
					} else if (sentinelReference.agentHasBeenStopped)
						sentinelReference.ResumeAgent ();
					
				} else if (sentinelReference.agentHasBeenStopped)
					sentinelReference.ResumeAgent ();
				
			} else if (sentinelReference.agentHasBeenStopped)
				sentinelReference.ResumeAgent ();
			
			if ((sentinelReference.HearingCollision (sentinelReference.player.run, sentinelReference.runCol, other)) ||
				(sentinelReference.HearingCollision (sentinelReference.player.walking, sentinelReference.walkCol, other)) ||
				(sentinelReference.HearingCollision (sentinelReference.player.isCrouched, sentinelReference.crouchCol, other)))
				sentinelReference.playerHasBeenHeard = true;
			
			/*this.HearingCollision (this.player.run, this.runCol, other);

				if (!this.playerHasBeenHeard && !this.player.isCrouched)
					this.HearingCollision (this.player.walking, this.walkCol, other);

				if (!this.playerHasBeenHeard)
					this.HearingCollision (this.player.isCrouched, this.crouchCol, other);*/
			
		} else {
			
			sentinelReference.StopAgent ();
			
		}
		
	};
	#endregion


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
	[Tooltip ("DO NOT TOUCH!")]
	public bool enemyHasBeenStunned;


	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH! Ask programmers for utilization")]
	public int destPoint;

	[Tooltip ("Determines the plane angle in wich the enemy could spot the player (from 0.1f to 360f)")]
	[Range (0.1f, 360f)] public float fieldOfViewAngle = 110f;               // Number of degrees, centred on forward, for the enemy see
	[Tooltip ("Determines the attack distance of the enemy (from 0.1f to 10f)")]
	[Range (0.1f, 10f)] public float attackDistance = 5f;
	[Tooltip ("Determines the time wich the Sentinel scans around it if lost the player (from 0.1f to 10f)")]
	[Range (0.1f, 10f)] public float scanningTime = 5f;
	[Tooltip ("Determines the stunning time of the enemy if hit by an EMI (from 0.1f to 10f)")]
	[Range (0.1f, 10f)] public float stunnedTime = 5f;
	[Tooltip ("Determines the Input checking time of the enemy (from 0.1f to 10f)")]
	[Range (0.1f, 10f)] public float inputCheckingTime = 0.5f;
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
	public Coroutine inputCheckingCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public Coroutine enemyStunnedCoroutine;
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
		this.enemyHasBeenStunned = false;
		this.agent.autoBraking = false;

		this.destPoint = 0;

		this.inputCheckingCoroutine = this.KillPreviousCoroutine (this.inputCheckingCoroutine);
		this.enemyStunnedCoroutine = this.KillPreviousCoroutine (this.enemyStunnedCoroutine);

	}


	public void OnTriggerEnter (Collider other) {

		if (other.gameObject == this.player.gameObject) {

			if (this.inputCheckingCoroutine == null)
				this.inputCheckingCoroutine = this.StartCoroutine_Auto (this.CO_InputChecking (this.inputCheckingTime, this.DelegatedMethod, other));

		}

	}


	/*public void OnTriggerStay (Collider other) {

		if (!this.enemyHasBeenStunned) {
		
			// If the player has entered the trigger sphere...
			if (other.gameObject == this.player.gameObject) {
			
				// By default the player is not in sight.
				this.playerInSight = false;
				this.playerHasBeenHeard = false;
			
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
						
							if (this.direction.sqrMagnitude < Mathf.Pow (this.attackDistance, 2f)) {

								this.StopAgent ();

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

				if ((this.HearingCollision (this.player.run, this.runCol, other)) ||
				    (this.HearingCollision (this.player.walking, this.walkCol, other)) ||
				    (this.HearingCollision (this.player.isCrouched, this.crouchCol, other)))
					this.playerHasBeenHeard = true;
			
				this.HearingCollision (this.player.run, this.runCol, other);

				if (!this.playerHasBeenHeard && !this.player.isCrouched)
					this.HearingCollision (this.player.walking, this.walkCol, other);

				if (!this.playerHasBeenHeard)
					this.HearingCollision (this.player.isCrouched, this.crouchCol, other);
	
			}

		} else {

			this.StopAgent ();

		}

	}*/


	public void OnTriggerExit (Collider other) {

		// If the player leaves the trigger zone...
		if (other.gameObject == this.player.gameObject) {

			if ((this.HearingExit (this.player.isCrouched, this.crouchCol, other)) &&
				(this.HearingExit (this.player.walking, this.walkCol, other)) && 
				(this.HearingExit (this.player.run, this.runCol, other)))
				this.playerHasBeenHeard = false;

			if ((other.transform.position - this.transform.position).sqrMagnitude > Mathf.Pow (this.viewCol.radius, 2f)) {

				// ... the player is not in sight.
				this.playerInSight = false;

				this.inputCheckingCoroutine = this.KillPreviousCoroutine (this.inputCheckingCoroutine);

				if (!this.enemyHasBeenStunned && this.agentHasBeenStopped)
					this.ResumeAgent ();

			}

		}

	}


	public void OnCollisionEnter (Collision collision) {

		if (collision.gameObject.CompareTag ("IEM")) {

			this.enemyStunnedCoroutine = KillPreviousCoroutine (this.enemyStunnedCoroutine);
			this.enemyHasBeenStunned = true;
			this.enemyStunnedCoroutine = this.StartCoroutine_Auto (this.CO_EnemyStunnedTime ());

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


	/*public void HearingCollision (bool playerMightBeHeard, SphereCollider col, Collider other) {

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
					
					// ... the player has been heard...
					this.playerHasBeenHeard = true;
					
				}
				
			}
			
		}

	}*/


	public bool HearingCollision (bool playerMightBeHeard, SphereCollider col, Collider other) {

		// ... and is emitting noises.
		if (playerMightBeHeard && this.player.transform.hasChanged) {

			// Compute a vector from the enemy to the player...
			this.direction = other.transform.position - this.transform.position;

			// ... and if a raycast towards the player hits something...
			if (Physics.Raycast (this.transform.position, this.direction.normalized, out this.hit, col.radius)) {

				// ... and if the raycast hits the player...
				if (this.hit.collider.gameObject == this.player.gameObject) {

					// ... the player has been heard...
					return true;

				} else
					return false;

			} else
				return false;

		} else
			return false;

	}


	public bool HearingExit (bool playerIsInRightState, SphereCollider col, Collider other) {
		
		if (playerIsInRightState && ((other.transform.position - this.transform.position).sqrMagnitude > Mathf.Pow (col.radius, 2f)))
			return false;
		else
			return true;
		
	}


	public void StopAgent () {

		this.agent.Stop ();
		this.agentHasBeenStopped = true;

	}


	public void ResumeAgent () {

		this.agent.Resume ();
		this.agentHasBeenStopped = false;

	}


	public Coroutine KillPreviousCoroutine (Coroutine coroutine) {

		if (coroutine != null)
			this.StopCoroutine (coroutine);

		return null;

	}
	#endregion


	#region SENTINEL_COROUTINES
	public IEnumerator CO_InputChecking (float inputCheckingTime, EnemyDelegate <AI_SentinelComponent> DelegatedMethod, Collider other) {
		
		while (true) {
			
			yield return new WaitForSeconds (inputCheckingTime);
			DelegatedMethod (this, other);
			
		}
		
	}


	public IEnumerator CO_EnemyStunnedTime () {

		yield return new WaitForSeconds (this.stunnedTime);
		this.ResumeAgent ();
		this.enemyHasBeenStunned = false;
		this.enemyStunnedCoroutine = this.KillPreviousCoroutine (this.enemyStunnedCoroutine);

	}
	#endregion

}