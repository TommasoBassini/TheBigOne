﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class FiniteStateMachine <TAI> : MonoBehaviour where TAI : IAI_ImplementedStrategy {

	public void FiniteStateMachineMethod (EnemyReference <TAI> enemyReference) {
		
	 	int state = 1;

		if (enemyReference.enemyStrategyState == StrategyState.NoStrategyChanging)
			return;

		do {

			if (enemyReference.enemyStrategyState == (StrategyState) ((int) StrategyState.NoStrategyChanging + state)) {
				
				enemyReference.enemyImplementedStrategy = enemyReference.enemyStrategyList [state - 1];
				return;

			}

		} while (++state < enemyReference.enemyStrategyList.Count + 1);

		Debug.LogError (this.ToString () + " does not understand wich strategy it should use!");
		return;

	}

}