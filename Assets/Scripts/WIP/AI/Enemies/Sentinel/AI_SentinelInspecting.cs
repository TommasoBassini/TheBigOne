using UnityEngine;
using System.Collections;

public class AI_SentinelInspecting : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Inspecting>>");

		if (Input.GetKeyDown (KeyCode.X)) {

			Debug.Log ("Sentinel switches from <<Inspecting>> to <<Defending>>");
			return StrategyState.Defending;

		} else if (Input.GetKeyDown (KeyCode.J)) {

			Debug.Log ("Sentinel switches from <<Inspecting>> to <<Scanning>>");
			return StrategyState.Scanning;

		} else if (Input.GetKeyDown (KeyCode.Y)) {

			Debug.Log ("Sentinel switches from <<Inspecting>> to <<Falling Into Line>>");
			return StrategyState.FallingIntoLine;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}