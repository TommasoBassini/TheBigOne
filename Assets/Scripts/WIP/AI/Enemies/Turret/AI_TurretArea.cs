using UnityEngine;
using System.Collections;

public class AI_TurretArea : MonoBehaviour {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_TurretComponent turretComponents;


	#region TURRET_AREA_MONOBEHAVIOUR_METHODS
	public void Awake () {
		
		this.turretComponents = GetComponentInParent <AI_TurretComponent> ();

	}


	public void OnTriggerStay (Collider other) {

		// If the player has entered the trigger box...
		if (other.gameObject == this.turretComponents.player) {

			if (this.turretComponents.playerHasBeenDetected) {

				// By default the player is not in sight.
				this.turretComponents.playerInSight = false;

				// Compute a vector from the enemy to the player...
				this.turretComponents.direction = other.transform.position - this.turretComponents.transform.position;

				// ... and if a raycast towards the player hits something...
				if (Physics.Raycast (this.turretComponents.transform.position, this.turretComponents.direction.normalized, out this.turretComponents.hit)) {

					// ... and if the raycast hits the player...
					if (this.turretComponents.hit.collider.gameObject == this.turretComponents.player) {

						// ... the player is in sight...
						this.turretComponents.playerInSight = true;

						if (!this.turretComponents.turretIsShoothing) {

							this.turretComponents.SwitchCoroutine (out this.turretComponents.slipOutCoroutine, out this.turretComponents.attackCoroutine,
								this.turretComponents.slipOutCoroutine, this.turretComponents.attackCoroutine,
								this.turretComponents.delegates.CO_TurretCoroutine, this.turretComponents.scanningTime, this.turretComponents.delegates.TurretAttackPostDelay);

						} else {

							// ... and may be attacked.
							Debug.LogWarning ("Shooting!");

						}

					} else {

						this.turretComponents.turretIsShoothing = false;

						this.turretComponents.SwitchCoroutine (out this.turretComponents.attackCoroutine, out this.turretComponents.slipOutCoroutine,
							this.turretComponents.attackCoroutine, this.turretComponents.slipOutCoroutine,
							this.turretComponents.delegates.CO_TurretCoroutine, this.turretComponents.slipOutTime, this.turretComponents.delegates.TurretSlipOutPostDelay);

					}

				}

			}

		}

	}


	public void OnTriggerExit (Collider other) {

		// If the player leaves the trigger zone...
		if (other.gameObject == this.turretComponents.player) {

			// ... the player is not in sight.
			this.turretComponents.playerInSight = false;
			this.turretComponents.turretIsShoothing = false;

			this.turretComponents.SwitchCoroutine (out this.turretComponents.attackCoroutine, out this.turretComponents.slipOutCoroutine,
				this.turretComponents.attackCoroutine, this.turretComponents.slipOutCoroutine,
				this.turretComponents.delegates.CO_TurretCoroutine, this.turretComponents.slipOutTime, this.turretComponents.delegates.TurretSlipOutPostDelay);

		}

	}
	#endregion

}