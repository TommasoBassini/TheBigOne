using UnityEngine;
using System.Collections;

public class AI_Sentinel : FourStateMachine <AI_SentinelGuarding, AI_SentinelDefending, AI_SentinelFallingIntoLine, AI_SentinelScanning> {

	public EnemyReference <AI_SentinelGuarding, AI_SentinelDefending, AI_SentinelFallingIntoLine, AI_SentinelScanning> sentinelReference;


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelReference = new EnemyReference <AI_SentinelGuarding, AI_SentinelDefending, AI_SentinelFallingIntoLine, AI_SentinelScanning> ();

		this.sentinelReference.enemyGuarding = this.GetComponent <AI_SentinelGuarding> ();
		this.sentinelReference.enemyDefending = this.GetComponent <AI_SentinelDefending> ();
		this.sentinelReference.enemyFallingIntoLine = this.GetComponent <AI_SentinelFallingIntoLine> ();
		this.sentinelReference.enemyScanning = this.GetComponent <AI_SentinelScanning> ();

	}

	public void Update () {

		this.FourStateMachineMethod (this.sentinelReference);

		this.sentinelReference.enemyStrategyState = this.sentinelReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}