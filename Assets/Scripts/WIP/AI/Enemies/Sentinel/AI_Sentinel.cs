using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Sentinel : FiniteStateMachine <IAI_ImplementedStrategy> {

	public EnemyReference <IAI_ImplementedStrategy> sentinelReference;


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelReference = new EnemyReference <IAI_ImplementedStrategy> ();

		this.sentinelReference.enemyStrategyList = new List <IAI_ImplementedStrategy> ();

		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelGuarding> ());
		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelDefending> ());
		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelFallingIntoLine> ());
		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelScanning> ());

	}

	public void Update () {

		this.FiniteStateMachineMethod (this.sentinelReference);

		this.sentinelReference.enemyStrategyState = this.sentinelReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}