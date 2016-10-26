using UnityEngine;
using System.Collections;

public class AI_SentinelGuarding : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_SentinelComponent sentinelComponents;


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelComponents = this.GetComponent <AI_SentinelComponent> ();

	}
	#endregion


	#region SENTINEL_METHODS
	public void GoToNextPoint () {

		// Returns if no points have been set up
		if (this.sentinelComponents.points.Length == 0) {

			Debug.LogWarning ("WARNING! " + this.ToString() + " Has the transform's array length equal to zero!");
			return;

		}

		// Set the agent to go to the currently selected destination.
		this.sentinelComponents.agent.destination = this.sentinelComponents.points [this.sentinelComponents.destPoint].position;

		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		this.sentinelComponents.destPoint = (this.sentinelComponents.destPoint + 1) % this.sentinelComponents.points.Length;

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Guarding>>");


		if (this.sentinelComponents.agent.remainingDistance < 0.5f)
			this.GoToNextPoint ();
		

		if (this.sentinelComponents.playerInSight) {

			Debug.Log ("Sentinel switches from <<Guarding>> to <<Defending>>");
			return StrategyState.Defending;

		} else if (this.sentinelComponents.playerHasBeenHeard) {

			Debug.Log ("Sentinel switches from <<Guarding>> to <<Inspecting>>");
			return StrategyState.Inspecting;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}