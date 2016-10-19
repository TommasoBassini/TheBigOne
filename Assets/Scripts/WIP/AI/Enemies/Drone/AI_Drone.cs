using UnityEngine;
using System.Collections;

public class AI_Drone : ThreeStateMachine <AI_DroneGuarding, AI_DroneDefending, AI_DroneFallingIntoLine> {

	public EnemyReference <AI_DroneGuarding, AI_DroneDefending, AI_DroneFallingIntoLine> droneReference;


	#region DRONE_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.droneReference = new EnemyReference <AI_DroneGuarding, AI_DroneDefending, AI_DroneFallingIntoLine> ();

		this.droneReference.enemyGuarding = this.GetComponent <AI_DroneGuarding> ();
		this.droneReference.enemyDefending = this.GetComponent <AI_DroneDefending> ();
		this.droneReference.enemyFallingIntoLine = this.GetComponent <AI_DroneFallingIntoLine> ();

	}

	public void Update () {

		this.ThreeStateMachineMethod (this.droneReference);

		this.droneReference.enemyStrategyState = this.droneReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}