using UnityEngine;
using System.Collections;

public class AI_TurretGuarding : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Turret is in <<Guarding>>");

		if (Input.GetKeyDown (KeyCode.A)) {

			Debug.Log ("Turret switches from <<Guarding>> to <<Defending>>");
			return StrategyState.Defending;

		} else {

			Debug.Log ("Turret does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}