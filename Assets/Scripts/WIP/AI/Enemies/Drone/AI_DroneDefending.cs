using UnityEngine;
using System.Collections;

public class AI_DroneDefending : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
    public AI_DroneComponent droneComponents;


	#region DRONE_MONOBEHAVIOUR_METHODS
    public void Awake () {
		
        this.droneComponents = this.GetComponent <AI_DroneComponent> ();

    }
	#endregion


	#region DRONE_METHODS
    public void ChasePlayer () {
		
        this.droneComponents.agent.destination = this.droneComponents.player.transform.position;

    }
	#endregion
    
    
    #region IMPLEMENTED_STRATEGY_METHOD
    public StrategyState ExecuteImplementedStrategy () {

		if (!this.droneComponents.enemyHasBeenStunned) {

			Debug.Log ("Drone is in <<Defending>>");


			this.ChasePlayer ();

                
			if (!this.droneComponents.playerInSight) {

				Debug.Log ("Drone switches from <<Defending>> to <<Inspecting>>");
				return StrategyState.Inspecting;

			} else {

				Debug.Log ("Drone does not change strategy");
				return StrategyState.NoStrategyChanging;

			}

		} else {
			
			Debug.Log ("Drone has been stunned");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}