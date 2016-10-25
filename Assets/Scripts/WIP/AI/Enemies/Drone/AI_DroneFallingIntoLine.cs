using UnityEngine;
using System.Collections;

public class AI_DroneFallingIntoLine : MonoBehaviour, IAI_ImplementedStrategy {

    public AI_DroneComponent droneComponents;


	#region DRONE_MONOBEHAVIOUR_METHODS
    public void Awake () {
		
        this.droneComponents = this.GetComponent <AI_DroneComponent> ();

    }
	#endregion


	#region DRONE_METHODS
    public bool ReturnToPatrol () {

		if (this.droneComponents.destPoint == 0) {
			
			this.droneComponents.destPoint = this.droneComponents.points.Length - 1;

		} else {

			this.droneComponents.destPoint--;

		}

        this.droneComponents.agent.destination = this.droneComponents.points [this.droneComponents.destPoint].position;
        return true;

    }
	#endregion


    #region IMPLEMENTED_STRATEGY_METHOD
    public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Drone is in <<Falling Into Line>>");


        if (!this.droneComponents.droneIsFallingIntoLine) {
			
            this.droneComponents.droneIsFallingIntoLine = this.ReturnToPatrol ();

        }


		if (this.droneComponents.playerInSight) {

			Debug.Log ("Drone switches from <<Falling Into Line>> to <<Defending>>");
			this.droneComponents.droneIsFallingIntoLine = false;
			return StrategyState.Defending;

		} else if (this.droneComponents.agent.remainingDistance < 1f) {

			Debug.Log ("Drone switches from <<Falling Into Line>> to <<Guarding>>");
            this.droneComponents.droneIsFallingIntoLine = false;
            return StrategyState.Guarding;

		} else {

			Debug.Log ("Drone does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}