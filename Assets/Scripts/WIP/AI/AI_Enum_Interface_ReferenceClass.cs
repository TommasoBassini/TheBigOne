using UnityEngine;
using System.Collections.Generic;

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


public class EnemyReference <TAI> : EnemyStrategyReference where TAI : IAI_ImplementedStrategy {

	//List of Classes
	public List <TAI> enemyStrategyList;

}
#endregion