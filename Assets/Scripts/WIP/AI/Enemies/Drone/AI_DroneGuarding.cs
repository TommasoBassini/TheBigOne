using UnityEngine;
using System.Collections;

public class AI_DroneGuarding : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public AI_DroneComponent droneComponents;
	
	
	#region DRONE_MONOBEHAVIOUR_METHODS
	public void Awake () {
		
		this.droneComponents = this.GetComponent <AI_DroneComponent> ();
		
	}
	#endregion
	
	
	#region DRONE_METHODS
	public void GoToNextPointOrdered () {
		
		// Returns if no points have been set up
		if (this.droneComponents.points.Length == 0) {
			
			Debug.LogWarning ("WARNING! " + this.ToString() + " Has the transform's array length equal to zero!");
			return;
			
		}
		
		// Set the agent to go to the currently selected destination.
		this.droneComponents.agent.destination = this.droneComponents.points [this.droneComponents.destPoint].position;
		
		// Choose the next point in the array as the destination,
		// cycling to the start if necessary.
		this.droneComponents.destPoint = (this.droneComponents.destPoint + 1) % this.droneComponents.points.Length;
		
	}
	
	
	public void GoToNextPointRandomized () {
		
		// Returns if no points have been set up
		if (this.droneComponents.points.Length == 0) {
			
			Debug.LogWarning ("WARNING! " + this.ToString() + " Has the transform's array length equal to zero!");
			return;
			
		}
		
		// Set the agent to go to the currently selected destination.
		this.droneComponents.agent.destination = this.droneComponents.points [this.droneComponents.destPoint].position;
		
		// Choose the next point in the array as the destination,
		// randomly.
		this.droneComponents.destPoint = Random.Range (0, this.droneComponents.points.Length);
		
	}
	#endregion
	
	
	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		if (!this.droneComponents.enemyHasBeenStunned) {
		
			Debug.Log ("Drone is in <<Guarding>>");
		
		
			if (this.droneComponents.agent.remainingDistance < 0.5f) {
			
				if (this.droneComponents.isPathRandomized)
					this.GoToNextPointRandomized ();
				else
					this.GoToNextPointOrdered ();

			}
		
		
			if (this.droneComponents.playerInSight) {
			
				Debug.Log ("Drone switches from <<Guarding>> to <<Defending>>");
				return StrategyState.Defending;
			
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