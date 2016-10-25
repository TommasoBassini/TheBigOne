using UnityEngine;
using System.Collections;

public class AI_DroneDefending : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
    public AI_DroneComponent droneComponents;


	#region DRONE_MONOBEHAVIOUR_METHODS
    public void Awake () {
		
        this.droneComponents = this.GetComponent <AI_DroneComponent> ();

    }


    public void OnTriggerExit (Collider other) {
		
        // If the player leaves the trigger zone...
		if (other.gameObject == this.droneComponents.player) {
			
			// ... the player is not in sight.
			this.droneComponents.playerInSight = false;

		}

    }
	#endregion


	#region DRONE_METHODS
    public void ChasePlayer () {
		
        this.droneComponents.agent.destination = this.droneComponents.player.transform.position;

    }
	#endregion
    
    
    #region IMPLEMENTED_STRATEGY_METHOD
    public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Drone is in <<Defending>>");


        this.ChasePlayer ();

                
		if (!this.droneComponents.playerInSight) {

			Debug.Log ("Drone switches from <<Defending>> to <<Falling Into Line>>");
			return StrategyState.FallingIntoLine;

		} else {

			Debug.Log ("Drone does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}