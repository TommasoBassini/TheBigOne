using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FiniteStateMachine : MonoBehaviour {

	private int state;

	public void FiniteStateMachineMethod (EnemyReference enemyReference) {
		
	 	this.state = 1;

		if (enemyReference.enemyStrategyState == StrategyState.NoStrategyChanging)
			return;

		do {

			if (enemyReference.enemyStrategyState == (StrategyState) ((int) StrategyState.NoStrategyChanging + this.state)) {
				
				enemyReference.enemyImplementedStrategy = enemyReference.enemyStrategyList [this.state - 1];
				return;

			}

		} while (++(this.state) <= enemyReference.enemyStrategyList.Count);

		Debug.LogError (this.ToString () + " does not understand wich strategy it should use!");
		return;

	}

}