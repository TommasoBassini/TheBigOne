using UnityEngine;
using System.Collections;

public class AI_DroneFallingIntoLine : MonoBehaviour, IAI_ImplementedStrategy {

    public AI_DroneComponent droneComponents;

    void Awake()
    {
        this.droneComponents = this.GetComponent<AI_DroneComponent>();
    }

    void OnTriggerStay(Collider other)
    {
        // If the player has entered the trigger sphere...
        if (other.gameObject == this.droneComponents.player)
        {
            // By default the player is not in sight.
            this.droneComponents.playerInSight = false;

            // Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = other.transform.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            // If the angle between forward and where the player is, is less than half the angle of view...
            if (angle < this.droneComponents.fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                // ... and if a raycast towards the player hits something...
                if (Physics.Raycast(this.transform.position, direction.normalized, out hit, this.droneComponents.col.radius))
                {
                    // ... and if the raycast hits the player...
                    if (hit.collider.gameObject == this.droneComponents.player)
                    {
                        // ... the player is in sight.
                        this.droneComponents.playerInSight = true;

                    }
                }
            }

        }
    }

    public bool ReturnToPatrol()
    {
        this.droneComponents.agent.destination = this.droneComponents.points[this.droneComponents.destPoint].position;
        return true;
    }



    #region IMPLEMENTED_STRATEGY_METHOD
    public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Drone is in <<Falling Into Line>>");

        if (!this.droneComponents.droneIsFallingIntoLine)
        {
            this.droneComponents.droneIsFallingIntoLine = this.ReturnToPatrol();
        }

		if (this.droneComponents.playerInSight) {

			Debug.Log ("Drone switches from <<Falling Into Line>> to <<Defending>>");
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