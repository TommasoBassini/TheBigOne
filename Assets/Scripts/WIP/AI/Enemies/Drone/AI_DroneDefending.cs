using UnityEngine;
using System.Collections;

public class AI_DroneDefending : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Drone is in <<Defending>>");

		if (Input.GetKeyDown (KeyCode.B)) {

			Debug.Log ("Drone switches from <<Defending>> to <<Falling Into Line>>");
			return StrategyState.FallingIntoLine;

		} else {

			Debug.Log ("Drone does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}