using UnityEngine;
using System.Collections;

public class AI_SentinelGuarding : MonoBehaviour, IAI_ImplementedStrategy {

	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Guarding>>");

		if (Input.GetKeyDown (KeyCode.A)) {

			Debug.Log ("Sentinel switches from <<Guarding>> to <<Defending>>");
			return StrategyState.Defending;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}