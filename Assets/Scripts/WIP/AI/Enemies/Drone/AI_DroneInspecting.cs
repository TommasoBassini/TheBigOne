using UnityEngine;
using System.Collections;

public class AI_DroneInspecting : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_DroneComponent droneComponents;


	#region DRONE_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.droneComponents = this.GetComponent <AI_DroneComponent> ();

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		if (!this.droneComponents.enemyHasBeenStunned) {

			Debug.Log ("Drone is in <<Inspecting>>");


			if (this.droneComponents.playerInSight) {

				Debug.Log ("Drone switches from <<Inspecting>> to <<Defending>>");
				return StrategyState.Defending;

			} else if (this.droneComponents.agent.remainingDistance < 0.5f) {

				Debug.Log ("Drone switches from <<Inspecting>> to <<Falling Into Line>>");
				return StrategyState.FallingIntoLine;

			} else {

				Debug.Log ("Drone does not change strategy");
				return StrategyState.NoStrategyChanging;

			}

		} else {

			Debug.Log ("Drone has been stunned");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}