using UnityEngine;
using System.Collections;

public class AI_DroneDefending : MonoBehaviour, IAI_ImplementedStrategy {

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


    void OnTriggerExit(Collider other)
    {
        // If the player leaves the trigger zone...
        if (other.gameObject == this.droneComponents.player)
            // ... the player is not in sight.
            this.droneComponents.playerInSight = false;
    }
   
    public void ChasePlayer()
    {
        this.droneComponents.agent.destination = this.droneComponents.player.transform.position;
    }
    
    
    #region IMPLEMENTED_STRATEGY_METHOD
    public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Drone is in <<Defending>>");

        this.ChasePlayer();
                
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