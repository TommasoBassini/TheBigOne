using UnityEngine;
using System.Collections;

public class AI_SentinelInspecting : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_SentinelComponent sentinelComponents;


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelComponents = this.GetComponent <AI_SentinelComponent> ();

	}
	#endregion


	#region SENTINEL_METHODS
	public void CheckPlace () {

		this.sentinelComponents.agent.destination = this.sentinelComponents.player.transform.position;

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Inspecting>>");


		if (this.sentinelComponents.playerHasBeenHeard)
			this.CheckPlace ();


		if (this.sentinelComponents.playerInSight) {

			Debug.Log ("Sentinel switches from <<Inspecting>> to <<Defending>>");
			return StrategyState.Defending;

		} else if (this.sentinelComponents.sentinelHasEnlargedItsHearingColliders && this.sentinelComponents.agent.remainingDistance < 0.5f) {

			Debug.Log ("Sentinel switches from <<Inspecting>> to <<Scanning>>");
			return StrategyState.Scanning;

		} else if (this.sentinelComponents.agent.remainingDistance < 1f) {

			Debug.Log ("Sentinel switches from <<Inspecting>> to <<Falling Into Line>>");
			return StrategyState.FallingIntoLine;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}