using UnityEngine;
using System.Collections;

public class AI_TurretDefending : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Turret is in <<Defending>>");

		if (Input.GetKeyDown (KeyCode.B)) {

			Debug.Log ("Turret switches from <<Defending>> to <<Falling Into Line>>");
			return StrategyState.FallingIntoLine;

		} else if (Input.GetKeyDown (KeyCode.E)) {

			Debug.Log ("Turret switches from <<Defending>> to <<Scanning>>");
			return StrategyState.Scanning;

		} else {

			Debug.Log ("Turret does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}