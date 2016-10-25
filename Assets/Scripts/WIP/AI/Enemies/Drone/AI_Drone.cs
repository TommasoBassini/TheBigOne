using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Drone : FiniteStateMachine <IAI_ImplementedStrategy> {

	public EnemyReference <IAI_ImplementedStrategy> droneReference;


	#region DRONE_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.droneReference = new EnemyReference <IAI_ImplementedStrategy> ();

		this.droneReference.enemyStrategyList = new List <IAI_ImplementedStrategy> ();

		this.droneReference.enemyStrategyList.Add (this.GetComponent <AI_DroneGuarding> ());
		this.droneReference.enemyStrategyList.Add (this.GetComponent <AI_DroneDefending> ());
		this.droneReference.enemyStrategyList.Add (this.GetComponent <AI_DroneFallingIntoLine> ());

	}

	public void Update () {

		this.FiniteStateMachineMethod (this.droneReference);

		this.droneReference.enemyStrategyState = this.droneReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}