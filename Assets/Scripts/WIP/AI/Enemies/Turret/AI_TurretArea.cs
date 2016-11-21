using UnityEngine;
using System.Collections;

public class AI_TurretArea : MonoBehaviour {

	#region TURRET_AREA_SUBCLASSES
	public class Delegates {

		public EnemyStateDelegate <AI_TurretArea> TurretAttackPostDelay = delegate (AI_TurretArea turretAreaReference) {

			turretAreaReference.turretComponents.turretIsShoothing = true;
			turretAreaReference.attackCoroutine = turretAreaReference.KillPreviousCoroutine (turretAreaReference.attackCoroutine);

		};

		public EnemyStateDelegate <AI_TurretArea> TurretSlipOutPostDelay = delegate (AI_TurretArea turretAreaReference) {

			turretAreaReference.turretComponents.playerHasBeenDetected = false;
			turretAreaReference.turretComponents.turretScanner.EnableTurretScanner (true);	//Might be moved elsewhere
			turretAreaReference.slipOutCoroutine = turretAreaReference.KillPreviousCoroutine (turretAreaReference.slipOutCoroutine);

		};

		public CO_EnemyCoroutineDelegate <AI_TurretArea> CO_TurretCoroutine;

	}
	#endregion


	#region TURRET_AREA_PARAMETERS
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
	public Delegates delegates;


	[Header ("Scripts")]

	[Tooltip ("DO NOT TOUCH!")]
	public AI_TurretComponent turretComponents;
	#endregion


	#region TURRET_AREA_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.delegates = new Delegates ();
		this.turretComponents = this.GetComponentInChildren <AI_TurretComponent> ();

	}


	public void Start () {
		
		this.delegates.CO_TurretCoroutine = this.CO_TurretDelayedTime;

		this.attackCoroutine = this.KillPreviousCoroutine (this.attackCoroutine);
		this.slipOutCoroutine = this.KillPreviousCoroutine (this.slipOutCoroutine);

	}


	public void OnTriggerStay (Collider other) {

		// If the player has entered the trigger box...
		if (other.gameObject == this.turretComponents.player) {

			if (this.turretComponents.playerHasBeenDetected) {

				// By default the player is not in sight.
				this.turretComponents.playerInSight = false;

				// Compute a vector from the enemy to the player...
				this.direction = other.transform.position - this.turretComponents.transform.position;

				// ... and if a raycast towards the player hits something...
				if (Physics.Raycast (this.turretComponents.transform.position, this.direction.normalized, out this.hit)) {

					// ... and if the raycast hits the player...
					if (this.hit.collider.gameObject == this.turretComponents.player) {

						// ... the player is in sight...
						this.turretComponents.playerInSight = true;

						if (!this.turretComponents.turretIsShoothing) {

							this.SwitchCoroutine (out this.slipOutCoroutine, out this.attackCoroutine, ref this.turretComponents.turretIsShoothing,
								this.slipOutCoroutine, this.attackCoroutine, this.delegates.CO_TurretCoroutine, this.turretComponents.scanningTime, this.delegates.TurretAttackPostDelay);

						} else {

							// ... and may be attacked.
							Debug.LogWarning ("Shooting!");

						}

					} else {

						this.SwitchCoroutine (out this.attackCoroutine, out this.slipOutCoroutine, ref this.turretComponents.turretIsShoothing,
							this.attackCoroutine, this.slipOutCoroutine, this.delegates.CO_TurretCoroutine, this.turretComponents.slipOutTime, this.delegates.TurretSlipOutPostDelay);

					}

				} else {

					this.SwitchCoroutine (out this.attackCoroutine, out this.slipOutCoroutine, ref this.turretComponents.turretIsShoothing,
						this.attackCoroutine, this.slipOutCoroutine, this.delegates.CO_TurretCoroutine, this.turretComponents.slipOutTime, this.delegates.TurretSlipOutPostDelay);
					
				}

			}

		}

	}


	public void OnTriggerExit (Collider other) {

		// If the player leaves the trigger zone...
		if (other.gameObject == this.turretComponents.player) {

			// ... the player is not in sight.
			this.turretComponents.playerInSight = false;

			this.SwitchCoroutine (out this.attackCoroutine, out this.slipOutCoroutine, ref this.turretComponents.turretIsShoothing,
				this.attackCoroutine, this.slipOutCoroutine, this.delegates.CO_TurretCoroutine, this.turretComponents.slipOutTime, this.delegates.TurretSlipOutPostDelay);
			
		}

	}
	#endregion


	#region TURRET_AREA_METHODS
	public void SwitchCoroutine (out Coroutine stoppedCoroutine, out Coroutine initializedCoroutine, ref bool turretIsShoothing, Coroutine toStopCoroutine, Coroutine toStartCoroutine,
		CO_EnemyCoroutineDelegate <AI_TurretArea> CO_DelegatedMethod, float waitingTime, EnemyStateDelegate <AI_TurretArea> DelegatedMethod) {

		stoppedCoroutine = this.KillPreviousCoroutine (toStopCoroutine);

		if (turretIsShoothing)
			turretIsShoothing = false;

		if (toStartCoroutine == null)
			initializedCoroutine = this.StartCoroutine_Auto (CO_DelegatedMethod (waitingTime, DelegatedMethod));
		else
			initializedCoroutine = toStartCoroutine;

	}


	public Coroutine KillPreviousCoroutine (Coroutine coroutine) {

		if (coroutine != null)
			this.StopCoroutine (coroutine);

		return null;

	}
	#endregion


	#region TURRET_AREA_COROUTINE_METHODS
	public IEnumerator CO_TurretDelayedTime (float waitingTime, EnemyStateDelegate <AI_TurretArea> DelegatedMethod) {

		yield return new WaitForSeconds (waitingTime);
		DelegatedMethod (this);

	}
	#endregion

}