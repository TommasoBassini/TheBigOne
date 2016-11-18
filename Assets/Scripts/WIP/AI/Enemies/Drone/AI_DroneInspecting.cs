using UnityEngine;
using System.Collections;

public class AI_DroneInspecting : MonoBehaviour, IAI_ImplementedStrategy {

	public Coroutine waitingCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public AI_DroneComponent droneComponents;


	#region DRONE_DELEGATES
	public EnemyStateDelegate <AI_DroneInspecting> DelegatedMethod = delegate (AI_DroneInspecting droneInspectingReference) {

		droneInspectingReference.droneComponents.droneIsInspecting = false;
		
	};
	#endregion


	#region DRONE_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.droneComponents = this.GetComponent <AI_DroneComponent> ();

	}


	public void Start () {

		this.waitingCoroutine = this.KillPreviousCoroutine (this.waitingCoroutine);

	}
	#endregion


	#region DRONE_METHODS
	public Coroutine KillPreviousCoroutine (Coroutine coroutine) {

		if (coroutine != null)
			this.StopCoroutine (coroutine);

		return null;

	}


	public Coroutine ExitingStationaryInspectingState (ref bool droneIsInspecting, NavMeshAgent agent, Coroutine coroutine) {

		droneIsInspecting = false;
		agent.autoBraking = false;
		return this.KillPreviousCoroutine (coroutine);

	}
	#endregion


	#region DRONE_COROUTINES
	public IEnumerator CO_WaitingCoroutine (float inspectingCheckingTime, EnemyStateDelegate <AI_DroneInspecting> DelegatedMethod) {

		yield return new WaitForSeconds (inspectingCheckingTime);
		DelegatedMethod (this);

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		if (!this.droneComponents.enemyHasBeenStunned) {

			Debug.Log ("Drone is in <<Inspecting>>");

			if (this.waitingCoroutine == null && this.droneComponents.agent.remainingDistance < 0.5f) {

				this.droneComponents.droneIsInspecting = true;
				this.droneComponents.agent.autoBraking = true;
				this.waitingCoroutine = this.StartCoroutine_Auto (this.CO_WaitingCoroutine (this.droneComponents.inspectingCheckingTime, this.DelegatedMethod));

			}


			if (this.droneComponents.playerInSight) {

				Debug.Log ("Drone switches from <<Inspecting>> to <<Defending>>");
				this.waitingCoroutine = this.ExitingStationaryInspectingState (ref this.droneComponents.droneIsInspecting, this.droneComponents.agent, this.waitingCoroutine);
				return StrategyState.Defending;

			} else if (this.waitingCoroutine != null && !this.droneComponents.droneIsInspecting) {

				Debug.Log ("Drone switches from <<Inspecting>> to <<Falling Into Line>>");
				this.waitingCoroutine = this.ExitingStationaryInspectingState (ref this.droneComponents.droneIsInspecting, this.droneComponents.agent, this.waitingCoroutine);
				return StrategyState.FallingIntoLine;

			} else {

				Debug.Log ("Drone does not change strategy");
				return StrategyState.NoStrategyChanging;

			}

		} else {

			Debug.Log ("Drone has been stunned");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}