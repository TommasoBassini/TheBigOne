﻿using UnityEngine;
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
	[Tooltip ("DO NOT TOUCH!")]
	public bool agentHasBeenStopped;
	[Tooltip ("DO NOT TOUCH!")]
	public bool enemyHasBeenStunned;


	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH! Ask programmers for utilization")]
	public int destPoint;

	[Tooltip ("Determines the plane angle in wich the enemy could spot the player (from 0f to 360f)")]
	[Range (0f, 360f)] public float fieldOfViewAngle = 110f;               // Number of degrees, centred on forward, for the enemy see
	[Tooltip ("Determines the attack distance of the enemy (from 0f to 10f)")]
	[Range (0f, 10f)] public float attackDistance = 5f;
	[Tooltip ("Determines the stunning time of the enemy if hit by an EMI (from 0f to 10f)")]
	[Range (0f, 10f)] public float stunnedTime = 5f;
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
	[Tooltip ("DO NOT TOUCH!")]
	public Coroutine enemyStunnedCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public LineRenderer attackRay;


	[Header ("GameObjects")]

	[Tooltip ("DO NOT TOUCH!")]
    public GameObject player;							// Reference to the player


	[Header ("Patrolling Points")]

	[Tooltip ("First, set the number; second, manually assign any GameObject desidered to be a patrolling point (Transforms will be automatically taken)")]
    public Transform[] points;
	#endregion


	#region DRONE_MONOBEHAVIOUR_METHODS
    public void Awake() {

		this.agent = this.GetComponent <NavMeshAgent> ();
        this.col = this.GetComponent <SphereCollider> ();
		this.attackRay = this.GetComponentInChildren <LineRenderer> (true);
        this.player = GameObject.FindGameObjectWithTag ("Player");

    }


	public void Start () {

		this.droneIsFallingIntoLine = false;
		this.playerInSight = false;
		this.agentHasBeenStopped = false;
		this.enemyHasBeenStunned = false;
		this.agent.autoBraking = false;

		this.destPoint = 0;

		this.enemyStunnedCoroutine = null;

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

						if (this.direction.sqrMagnitude < Mathf.Pow (this.attackDistance, 2f)) {

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

		}

	}


	public void OnTriggerExit (Collider other) {

		// If the player leaves the trigger zone...
		if (other.gameObject == this.player) {

			// ... the player is not in sight.
			this.playerInSight = false;

			if (this.agentHasBeenStopped)
				this.ResumeAgent ();

		}

	}


	public void OnCollisionEnter (Collision collision) {

		if (collision.gameObject.CompareTag ("IEM")) {

			if (this.enemyStunnedCoroutine == null) {
				
				this.enemyHasBeenStunned = true;
				this.enemyStunnedCoroutine = this.StartCoroutine_Auto (this.CO_EnemyStunnedTime ());

			}

		}

	}
	#endregion


	#region DRONE_METHODS
	public void ResumeAgent () {

		this.agent.Resume ();
		this.agentHasBeenStopped = false;
		this.attackRay.enabled = false;

	}


	public Coroutine KillPreviousCoroutine (Coroutine coroutine) {

		if (coroutine != null)
			this.StopCoroutine (coroutine);

		return null;

	}
	#endregion


	#region DRONE_COROUTINES
	public IEnumerator CO_EnemyStunnedTime () {

		yield return new WaitForSeconds (this.stunnedTime);
		this.enemyHasBeenStunned = false;
		this.enemyStunnedCoroutine = this.KillPreviousCoroutine (this.enemyStunnedCoroutine);

	}
	#endregion

}