using UnityEngine;
using System.Collections;

public class AI_DroneFallingIntoLine : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Drone is in <<Falling Into Line>>");

		if (Input.GetKeyDown (KeyCode.C)) {

			Debug.Log ("Drone switches from <<Falling Into Line>> to <<Defending>>");
			return StrategyState.Defending;

		} else if (Input.GetKeyDown (KeyCode.D)) {

			Debug.Log ("Drone switches from <<Falling Into Line>> to <<Guarding>>");
			return StrategyState.Guarding;

		} else {

			Debug.Log ("Drone does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}