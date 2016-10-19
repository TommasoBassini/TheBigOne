using UnityEngine;

//Enum used to establish the AIs' states
public enum StrategyState : byte {NoStrategyChanging, Guarding, Defending, FallingIntoLine, Scanning};


//Interface used for AIs' Strategy Pattern Implementation
public interface IAI_ImplementedStrategy {

	StrategyState ExecuteImplementedStrategy ();

}


//Classes used for common and uncoupled initialization puproses
#region ENEMY_REFERENCE_CLASSES
public abstract class EnemyStrategyReference {

	//Enum
	public StrategyState enemyStrategyState = StrategyState.Guarding;

	//Interface
	public IAI_ImplementedStrategy enemyImplementedStrategy;

}


public class EnemyReference <TAI0, TAI1, TAI2> : EnemyStrategyReference
	where TAI0 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI1 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI2 : MonoBehaviour, IAI_ImplementedStrategy {

	//Classes
	public TAI0 enemyGuarding;
	public TAI1 enemyDefending;
	public TAI2 enemyFallingIntoLine;

}


public class EnemyReference <TAI0, TAI1, TAI2, TAI3> : EnemyStrategyReference
	where TAI0 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI1 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI2 : MonoBehaviour, IAI_ImplementedStrategy
	where TAI3 : MonoBehaviour, IAI_ImplementedStrategy {

	//Classes
	public TAI0 enemyGuarding;
	public TAI1 enemyDefending;
	public TAI2 enemyFallingIntoLine;
	public TAI3 enemyScanning;

}
#endregion