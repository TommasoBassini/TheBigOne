using UnityEngine;
using System.Collections;

public abstract class ThreeStateMachine <TAI0, TAI1, TAI2> : MonoBehaviour
	where TAI0 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI1 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI2 : MonoBehaviour, IAI_ImplementedStrategy {

	public void ThreeStateMachineMethod (EnemyReference <TAI0, TAI1, TAI2> enemyReference) {

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

		default:

			Debug.LogError (this.ToString () + " does not understand wich strategy it should use!");
			break;

		}

	}

}