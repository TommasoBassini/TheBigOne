using UnityEngine;
using System.Collections;

public class AI_SentinelFallingIntoLine : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Falling Into Line>>");

		if (Input.GetKeyDown (KeyCode.C)) {

			Debug.Log ("Sentinel switches from <<Falling Into Line>> to <<Defending>>");
			return StrategyState.Defending;

		} else if (Input.GetKeyDown (KeyCode.Z)) {

			Debug.Log ("Sentinel switches from <<Falling Into Line>> to <<Inspecting>>");
			return StrategyState.Inspecting;

		} else if (Input.GetKeyDown (KeyCode.D)) {

			Debug.Log ("Sentinel switches from <<Falling Into Line>> to <<Guarding>>");
			return StrategyState.Guarding;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}