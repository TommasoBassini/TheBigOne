using UnityEngine;
using System.Collections;

public class AI_SentinelFallingIntoLine : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_SentinelComponent sentinelComponents;


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelComponents = this.GetComponent <AI_SentinelComponent> ();

	}
	#endregion


	#region SENTINEL_METHODS
	public void ResetHearingCollidersRadius () {

		this.sentinelComponents.sentinelHasEnlargedItsHearingColliders = false;

		this.sentinelComponents.runCol.radius *= 0.5f;
		this.sentinelComponents.walkCol.radius *= 0.5f;
		this.sentinelComponents.crouchCol.radius *= 0.5f;

	}

	public bool ReturnToPatrol () {

		if (this.sentinelComponents.destPoint == 0) {
			
			this.sentinelComponents.destPoint = this.sentinelComponents.points.Length - 1;
			
		} else {
			
			this.sentinelComponents.destPoint--;
			
		}

		this.sentinelComponents.agent.destination = this.sentinelComponents.points [this.sentinelComponents.destPoint].position;
		return true;

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Falling Into Line>>");


		if (this.sentinelComponents.sentinelHasEnlargedItsHearingColliders)
			this.ResetHearingCollidersRadius ();

		if (!this.sentinelComponents.sentinelIsFallingIntoLine)
			this.sentinelComponents.sentinelIsFallingIntoLine = this.ReturnToPatrol ();


		if (this.sentinelComponents.playerInSight) {

			Debug.Log ("Sentinel switches from <<Falling Into Line>> to <<Defending>>");
			this.sentinelComponents.sentinelIsFallingIntoLine = false;
			return StrategyState.Defending;

		} else if (this.sentinelComponents.playerHasBeenHeard) {

			Debug.Log ("Sentinel switches from <<Falling Into Line>> to <<Inspecting>>");
			this.sentinelComponents.sentinelIsFallingIntoLine = false;
			return StrategyState.Inspecting;

		} else if (this.sentinelComponents.agent.remainingDistance < 1f) {

			Debug.Log ("Sentinel switches from <<Falling Into Line>> to <<Guarding>>");
			this.sentinelComponents.sentinelIsFallingIntoLine = false;
			return StrategyState.Guarding;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}