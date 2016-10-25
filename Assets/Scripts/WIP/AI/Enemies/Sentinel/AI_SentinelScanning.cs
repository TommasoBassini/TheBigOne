using UnityEngine;
using System.Collections;

public class AI_SentinelScanning : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Scanning>>");

		if (Input.GetKeyDown (KeyCode.F)) {

			Debug.Log ("Sentinel switches from <<Scanning>> to <<Defending>>");
			return StrategyState.Defending;

		} else if (Input.GetKeyDown (KeyCode.K)) {
			
			Debug.Log ("Sentinel switches from <<Scanning>> to <<Inspecting>>");
			return StrategyState.Inspecting;

		} else if (Input.GetKeyDown (KeyCode.G)) {

			Debug.Log ("Sentinel switches from <<Scanning>> to <<Falling Into Line>>");
			return StrategyState.FallingIntoLine;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}