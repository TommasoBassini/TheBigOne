using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Turret : FiniteStateMachine {

	public EnemyReference turretReference;


	#region TURRET_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.turretReference = new EnemyReference ();

		this.turretReference.enemyStrategyList = new List <IAI_ImplementedStrategy> ();

		this.turretReference.enemyStrategyList.Add (this.GetComponent <AI_TurretGuarding> ());
		this.turretReference.enemyStrategyList.Add (this.GetComponent <AI_TurretDefending> ());

	}

	public void Update () {

		this.FiniteStateMachineMethod (this.turretReference);

		this.turretReference.enemyStrategyState = this.turretReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}