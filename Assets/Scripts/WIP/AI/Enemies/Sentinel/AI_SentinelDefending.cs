using UnityEngine;
using System.Collections;

public class AI_SentinelDefending : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_SentinelComponent sentinelComponents;


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelComponents = this.GetComponent <AI_SentinelComponent> ();

	}


	public void OnTriggerExit (Collider other) {

		// If the player leaves the trigger zone...
		if (other.gameObject == this.sentinelComponents.player.gameObject) {

			if ((other.transform.position - this.transform.position).sqrMagnitude > Mathf.Pow (this.sentinelComponents.viewCol.radius, 2f)) {

				// ... the player is not in sight.
				this.sentinelComponents.playerInSight = false;

			}

		}

	}
	#endregion


	#region SENTINEL_METHODS
	public void ResetHearingCollidersRadius () {

		this.sentinelComponents.sentinelHasEnlargedItsHearingColliders = false;

		this.sentinelComponents.runCol.radius *= 0.5f;
		this.sentinelComponents.walkCol.radius *= 0.5f;
		this.sentinelComponents.crouchCol.radius *= 0.5f;

	}

	public void ChasePlayer () {

		this.sentinelComponents.agent.destination = this.sentinelComponents.player.transform.position;

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Defending>>");


		if (this.sentinelComponents.sentinelHasEnlargedItsHearingColliders)
			this.ResetHearingCollidersRadius ();

		this.ChasePlayer ();


		if (!this.sentinelComponents.playerInSight) {

			Debug.Log ("Sentinel switches from <<Defending>> to <<Scanning>>");
			return StrategyState.Scanning;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}