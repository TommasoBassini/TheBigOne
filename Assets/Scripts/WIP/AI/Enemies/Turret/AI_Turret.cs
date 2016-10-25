using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Turret : FiniteStateMachine <IAI_ImplementedStrategy> {

	public EnemyReference <IAI_ImplementedStrategy> turretReference;


	#region TURRET_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.turretReference = new EnemyReference <IAI_ImplementedStrategy> ();

		this.turretReference.enemyStrategyList = new List <IAI_ImplementedStrategy> ();

		this.turretReference.enemyStrategyList.Add (this.GetComponent <AI_TurretGuarding> ());
		this.turretReference.enemyStrategyList.Add (this.GetComponent <AI_TurretDefending> ());
		this.turretReference.enemyStrategyList.Add (this.GetComponent <AI_TurretFallingIntoLine> ());
		this.turretReference.enemyStrategyList.Add (this.GetComponent <AI_TurretScanning> ());

	}

	public void Update () {

		this.FiniteStateMachineMethod (this.turretReference);

		this.turretReference.enemyStrategyState = this.turretReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}