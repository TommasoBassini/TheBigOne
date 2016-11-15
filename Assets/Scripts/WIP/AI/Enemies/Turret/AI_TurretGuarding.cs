using UnityEngine;
using System.Collections;

public class AI_TurretGuarding : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_TurretComponent turretComponents;


	#region TURRET_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.turretComponents = this.GetComponent <AI_TurretComponent> ();

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Turret is in <<Guarding>>");
		
		//this.transform.LookAt (this.turretComponents.turretScanner.transform.position);
		
		if (this.turretComponents.playerInSight) {
			
			Debug.Log ("Turret switches from <<Guarding>> to <<Defending>>");
			return StrategyState.Defending;
			
		} else {
			
			Debug.Log ("Turret does not change strategy");
			return StrategyState.NoStrategyChanging;
			
		}

	}
	#endregion

}