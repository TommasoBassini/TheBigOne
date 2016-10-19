using UnityEngine;
using System.Collections;

public abstract class FourStateMachine <TAI0, TAI1, TAI2, TAI3> : MonoBehaviour
	where TAI0 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI1 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI2 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI3 : MonoBehaviour, IAI_ImplementedStrategy {

	public void FourStateMachineMethod (EnemyReference <TAI0, TAI1, TAI2, TAI3> enemyReference) {

		switch (enemyReference.enemyStrategyState) {

		case StrategyState.NoStrategyChanging:

			break;

		case StrategyState.Guarding:

			enemyReference.enemyImplementedStrategy = enemyReference.enemyGuarding;
			break;

		case StrategyState.Defending:

			enemyReference.enemyImplementedStrategy = enemyReference.enemyDefending;
			break;

		case StrategyState.FallingIntoLine:

			enemyReference.enemyImplementedStrategy = enemyReference.enemyFallingIntoLine;
			break;

		case StrategyState.Scanning:

			enemyReference.enemyImplementedStrategy = enemyReference.enemyScanning;
			break;

		default:

			Debug.LogError (this.ToString () + " does not understand wich strategy it should use!");
			break;

		}

	}

}