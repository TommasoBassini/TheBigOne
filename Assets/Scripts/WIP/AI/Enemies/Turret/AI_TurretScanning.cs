using UnityEngine;
using System.Collections;

public class AI_TurretScanning : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Turret is in <<Scanning>>");

		if (Input.GetKeyDown (KeyCode.F)) {

			Debug.Log ("Turret switches from <<Scanning>> to <<Defending>>");
			return StrategyState.Defending;

		} else if (Input.GetKeyDown (KeyCode.G)) {

			Debug.Log ("Turret switches from <<Scanning>> to <<Falling Into Line>>");
			return StrategyState.FallingIntoLine;

		} else {

			Debug.Log ("Turret does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}