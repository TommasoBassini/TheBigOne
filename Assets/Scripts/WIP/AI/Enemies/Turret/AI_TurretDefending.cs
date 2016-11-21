using UnityEngine;
using System.Collections;

public class AI_TurretDefending : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_TurretComponent turretComponents;


	#region TURRET_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.turretComponents = this.GetComponent <AI_TurretComponent> ();

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Turret is in <<Defending>>");

		if (this.turretComponents.playerInSight)
			this.transform.LookAt (this.turretComponents.player.transform.position);

		
		if (!this.turretComponents.playerHasBeenDetected) {
			
			Debug.Log ("Turret switches from <<Defending>> to <<Guarding>>");
			return StrategyState.Guarding;
			
		} else {
			
			Debug.Log ("Turret does not change strategy");
			return StrategyState.NoStrategyChanging;
			
		}

	}
	#endregion

}