using UnityEngine;
using System.Collections;

public class AI_SentinelDefending : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Defending>>");

		if (Input.GetKeyDown (KeyCode.E)) {

			Debug.Log ("Sentinel switches from <<Defending>> to <<Scanning>>");
			return StrategyState.Scanning;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}