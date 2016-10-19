using UnityEngine;
using System.Collections;

public class AI_Turret : FourStateMachine <AI_TurretGuarding, AI_TurretDefending, AI_TurretFallingIntoLine, AI_TurretScanning> {

	public EnemyReference <AI_TurretGuarding, AI_TurretDefending, AI_TurretFallingIntoLine, AI_TurretScanning> turretReference;


	#region TURRET_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.turretReference = new EnemyReference <AI_TurretGuarding, AI_TurretDefending, AI_TurretFallingIntoLine, AI_TurretScanning> ();

		this.turretReference.enemyGuarding = this.GetComponent <AI_TurretGuarding> ();
		this.turretReference.enemyDefending = this.GetComponent <AI_TurretDefending> ();
		this.turretReference.enemyFallingIntoLine = this.GetComponent <AI_TurretFallingIntoLine> ();
		this.turretReference.enemyScanning = this.GetComponent <AI_TurretScanning> ();

	}

	public void Update () {

		this.FourStateMachineMethod (this.turretReference);

		this.turretReference.enemyStrategyState = this.turretReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}