using UnityEngine;
using System.Collections;

public class AI_DroneGuarding : MonoBehaviour, IAI_ImplementedStrategy {

   
    public AI_DroneComponent droneComponents;
    
    void Awake()
    {
        this.droneComponents = this.GetComponent<AI_DroneComponent>();
    }

     public void GotoNextPointOrdered()
    {
        // Returns if no points have been set up
        if (this.droneComponents.points.Length == 0)
        {
            Debug.LogWarning("WARNING! " + this.ToString() + " Has the transform's array leght equal to zero");
            return;
        }

        // Set the agent to go to the currently selected destination.
        this.droneComponents.agent.destination = this.droneComponents.points[this.droneComponents.destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        this.droneComponents.destPoint = (this.droneComponents.destPoint + 1) % this.droneComponents.points.Length;
    }

    public void GotoNextPointRandomized()
    {
        // Returns if no points have been set up
        if (this.droneComponents.points.Length == 0)
        {
            Debug.LogWarning("WARNING! " + this.ToString() + " Has the transform's array leght equal to zero");
            return;
        }

        // Set the agent to go to the currently selected destination.
        this.droneComponents.agent.destination = this.droneComponents.points[this.droneComponents.destPoint].position;

        // Choose the next point in the array as the destination,
        // randomly.
        this.droneComponents.destPoint = Random.Range(0, this.droneComponents.points.Length);  
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


    #region IMPLEMENTED_STRATEGY_METHOD
    public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Drone is in <<Guarding>>");


        if (this.droneComponents.agent.remainingDistance < 0.5f)
        {

            if (this.droneComponents.isPathRandomized)
                {
                    this.GotoNextPointRandomized();
                }
            else
                {
                    this.GotoNextPointOrdered();
                }

        }


		if (this.droneComponents.playerInSight)
        {

            Debug.Log ("Drone switches from <<Guarding>> to <<Defending>>");
			return StrategyState.Defending;

		} else {

			Debug.Log ("Drone does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}