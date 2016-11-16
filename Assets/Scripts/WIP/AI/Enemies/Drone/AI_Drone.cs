using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Drone : FiniteStateMachine {

	public EnemyReference droneReference;


	#region DRONE_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.droneReference = new EnemyReference ();

		this.droneReference.enemyStrategyList = new List <IAI_ImplementedStrategy> ();

		this.droneReference.enemyStrategyList.Add (this.GetComponent <AI_DroneGuarding> ());
		this.droneReference.enemyStrategyList.Add (this.GetComponent <AI_DroneDefending> ());
		this.droneReference.enemyStrategyList.Add (this.GetComponent <AI_DroneFallingIntoLine> ());
		this.droneReference.enemyStrategyList.Add (this.GetComponent <AI_DroneInspecting> ());

	}

	public void Update () {

		this.FiniteStateMachineMethod (this.droneReference);

		this.droneReference.enemyStrategyState = this.droneReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}