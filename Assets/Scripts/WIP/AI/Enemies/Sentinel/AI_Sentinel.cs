﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Sentinel : FiniteStateMachine {

	[Tooltip ("DO NOT TOUCH!")]
	public EnemyReference sentinelReference;


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelReference = new EnemyReference ();

		this.sentinelReference.enemyStrategyList = new List <IAI_ImplementedStrategy> ();

		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelGuarding> ());
		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelDefending> ());
		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelFallingIntoLine> ());
		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelInspecting> ());
		this.sentinelReference.enemyStrategyList.Add (this.GetComponent <AI_SentinelScanning> ());

	}

	public void Update () {

		this.FiniteStateMachineMethod (this.sentinelReference);

		this.sentinelReference.enemyStrategyState = this.sentinelReference.enemyImplementedStrategy.ExecuteImplementedStrategy ();

	}
	#endregion

}