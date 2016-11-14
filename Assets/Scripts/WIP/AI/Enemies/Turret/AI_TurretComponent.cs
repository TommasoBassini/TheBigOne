using UnityEngine;
using System.Collections;

public delegate void TurretDelegate (AI_TurretComponent turretComponentReference);

public class AI_TurretComponent : MonoBehaviour {

	#region TURRET_CLASSES
	public class Delegates {

		public TurretDelegate TurretAttackPostDelay = delegate (AI_TurretComponent turretComponentReference) {
			
			turretComponentReference.turretIsShoothing = true;
			turretComponentReference.attackCoroutine = null;
			
		};

		public TurretDelegate TurretSlipOutPostDelay = delegate (AI_TurretComponent turretComponentReference) {
			
			turretComponentReference.playerHasBeenDetected = false;
			turretComponentReference.turretScanner.EnableTurretScanner (true);	//Might be moved elsewhere
			turretComponentReference.slipOutCoroutine = null;
			
		};

	}
	#endregion


	#region TURRET_PARAMETERS
	[Header ("Boolean Flags")]

	[Tooltip ("DO NOT TOUCH!")]
	public bool playerHasBeenDetected;
	[Tooltip ("DO NOT TOUCH!")]
	public bool playerInSight;							// Whether or not the player is currently sighted
	[Tooltip ("DO NOT TOUCH!")]
	public bool turretIsShoothing;
	[Tooltip ("DO NOT TOUCH!")]
	public bool enemyHasBeenStunned;


	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH! Ask programmers for utilization")]
	public int destPoint;

	[Tooltip ("Scanning time used to recognize the player - from 0f to 10f")]
	[Range (0f, 10f)] public float scanningTime = 2f;
	[Tooltip ("Waiting time used to return in guarding state - from 0f to 10f")]
	[Range (0f, 10f)] public float waitingTime = 5f;
	[Tooltip ("Determines the stunning time of the enemy if hit by an EMI (from 0f to 10f)")]
	[Range (0f, 10f)] public float stunnedTime = 5f;


	[Header ("Structs")]

	[Tooltip ("DO NOT TOUCH!")]
	public Vector3 direction;
	[Tooltip ("DO NOT TOUCH!")]
	public RaycastHit hit;


	[Header ("Classes")]

	[Tooltip ("DO NOT TOUCH!")]
	public Coroutine attackCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public Coroutine slipOutCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public Coroutine enemyStunnedCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public Delegates delegates;
	[Tooltip ("DO NOT TOUCH!")]
	public BoxCollider col;                         	// Reference to the box collider trigger component
	[Tooltip ("DO NOT TOUCH!")]
	public LineRenderer attackRay;
	[Tooltip ("DO NOT TOUCH!")]
	public AI_TurretScanner turretScanner;


	[Header ("GameObjects")]

	[Tooltip ("DO NOT TOUCH!")]
	public GameObject player;							// Reference to the player


	[Header ("Patrolling Points")]

	[Tooltip ("First, set the number; second, manually assign any GameObject desidered to be a patrolling point (Transforms will be automatically taken)")]
	public Transform[] points;
	#endregion


	#region TURRET_MONOBEHAVIOUR_METHODS
	public void Awake() {

		this.delegates = new Delegates ();
		this.col = this.GetComponent <BoxCollider> ();
		this.attackRay = this.GetComponentInChildren <LineRenderer> (true);
		this.turretScanner = this.GetComponentInChildren <AI_TurretScanner> (true);

		this.player = GameObject.FindGameObjectWithTag ("Player");

	}


	public void Start () {

		this.playerHasBeenDetected = false;
		this.playerInSight = false;
		this.turretIsShoothing = false;
		this.enemyHasBeenStunned = false;

		this.destPoint = 0;

		this.attackCoroutine = null;
		this.slipOutCoroutine = null;
		this.enemyStunnedCoroutine = null;

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

					Debug.DrawLine (this.transform.position, this.hit.point, Color.green);
					
					// ... and if the raycast hits the player...
					if (this.hit.collider.gameObject == this.player) {
						
						// ... the player is in sight...
						this.playerInSight = true;

						if (!this.turretIsShoothing) {

							if (this.slipOutCoroutine != null)
								this.slipOutCoroutine = this.KillPreviousCoroutine (this.slipOutCoroutine);

							if (this.attackCoroutine == null)
								this.attackCoroutine = this.StartCoroutine_Auto (this.CO_TurretDelayedTime (this.scanningTime, this.delegates.TurretAttackPostDelay));

						} else {

							this.attackRay.enabled = true;
							this.attackRay.SetPosition (0, this.attackRay.transform.position);
							this.attackRay.SetPosition (1, other.transform.position);

							// ... and may be attacked.
							Debug.LogWarning ("Shooting!");

						}
							
					} else {

						this.turretIsShoothing = false;
						this.attackRay.enabled = false;

						if (this.attackCoroutine != null)
							this.attackCoroutine = this.KillPreviousCoroutine (this.attackCoroutine);

						if (this.slipOutCoroutine == null)
							this.slipOutCoroutine = this.StartCoroutine_Auto (this.CO_TurretDelayedTime (this.waitingTime, this.delegates.TurretSlipOutPostDelay));

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

			this.turretIsShoothing = false;
			this.attackRay.enabled = false;

			if (this.attackCoroutine != null)
				this.attackCoroutine = this.KillPreviousCoroutine (this.attackCoroutine);

			if (this.slipOutCoroutine == null)
				this.slipOutCoroutine = this.StartCoroutine_Auto (this.CO_TurretDelayedTime (this.waitingTime, this.delegates.TurretSlipOutPostDelay));

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


	#region TURRET_METHODS
	public Coroutine KillPreviousCoroutine (Coroutine coroutine) {

		if (coroutine != null)
			this.StopCoroutine (coroutine);
		
		return null;

	}
	#endregion


	#region TURRET_COROUTINE_METHODS
	public IEnumerator CO_TurretDelayedTime (float waitingTime, TurretDelegate DelegatedMethod) {
		
		yield return new WaitForSeconds (waitingTime);
		DelegatedMethod (this);

	}


	public IEnumerator CO_EnemyStunnedTime () {

		yield return new WaitForSeconds (this.stunnedTime);
		this.enemyHasBeenStunned = false;
		this.enemyStunnedCoroutine = this.KillPreviousCoroutine (this.enemyStunnedCoroutine);

	}
	#endregion

}