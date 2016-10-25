using UnityEngine;
using System.Collections;

public class AI_TurretFallingIntoLine : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Turret is in <<Falling Into Line>>");

		if (Input.GetKeyDown (KeyCode.C)) {

			Debug.Log ("Turret switches from <<Falling Into Line>> to <<Defending>>");
			return StrategyState.Defending;

		} else if (Input.GetKeyDown (KeyCode.D)) {

			Debug.Log ("Turret switches from <<Falling Into Line>> to <<Guarding>>");
			return StrategyState.Guarding;

		} else {

			Debug.Log ("Turret does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}