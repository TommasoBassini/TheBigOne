using UnityEngine;
using System.Collections.Generic;

//Enum used to establish the AIs' states
public enum StrategyState : byte {NoStrategyChanging, Guarding, Defending, FallingIntoLine, Inspecting, Scanning};


//Interface used for AIs' Strategy Pattern Implementation
public interface IAI_ImplementedStrategy {

	StrategyState ExecuteImplementedStrategy ();

}


//Class used for common initialization puproses
public class EnemyReference {
	
	//Enum
	public StrategyState enemyStrategyState = StrategyState.Guarding;
	
	//Interface
	public IAI_ImplementedStrategy enemyImplementedStrategy;

	//List of Classes
	public List <IAI_ImplementedStrategy> enemyStrategyList;

}