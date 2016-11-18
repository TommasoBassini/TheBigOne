using UnityEngine;
using System.Collections;

public class AI_SentinelInspecting : MonoBehaviour, IAI_ImplementedStrategy {

	public Coroutine waitingCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public AI_SentinelComponent sentinelComponents;


	#region SENTINEL_DELEGATES
	public EnemyStateDelegate <AI_SentinelInspecting> DelegatedMethod = delegate (AI_SentinelInspecting sentinelInspectingReference) {

		sentinelInspectingReference.sentinelComponents.sentinelIsInspecting = false;

	};
	#endregion


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelComponents = this.GetComponent <AI_SentinelComponent> ();

	}


	public void Start () {

		this.waitingCoroutine = this.KillPreviousCoroutine (this.waitingCoroutine);

	}
	#endregion


	#region SENTINEL_METHODS
	public Coroutine KillPreviousCoroutine (Coroutine coroutine) {

		if (coroutine != null)
			this.StopCoroutine (coroutine);

		return null;

	}


	public Coroutine ExitingStationaryInspectingState (ref bool sentinelIsInspecting, NavMeshAgent agent, Coroutine coroutine) {

		sentinelIsInspecting = false;
		agent.autoBraking = false;
		return this.KillPreviousCoroutine (coroutine);

	}


	public void CheckPlace () {

		this.sentinelComponents.agent.destination = this.sentinelComponents.player.transform.position;

	}
	#endregion


	#region SENTINEL_COROUTINES
	public IEnumerator CO_WaitingCoroutine (float inspectingCheckingTime, EnemyStateDelegate <AI_SentinelInspecting> DelegatedMethod) {

		yield return new WaitForSeconds (inspectingCheckingTime);
		DelegatedMethod (this);

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		if (!this.sentinelComponents.enemyHasBeenStunned) {

			Debug.Log ("Sentinel is in <<Inspecting>>");
      
			if (this.sentinelComponents.playerHasBeenHeard) {

				this.waitingCoroutine = this.ExitingStationaryInspectingState (ref this.sentinelComponents.sentinelIsInspecting, this.sentinelComponents.agent, this.waitingCoroutine);
				this.CheckPlace ();

			}

			if (!this.sentinelComponents.sentinelHasEnlargedItsHearingColliders && this.waitingCoroutine == null && this.sentinelComponents.agent.remainingDistance < 0.5f) {

				this.sentinelComponents.sentinelIsInspecting = true;
				this.sentinelComponents.agent.autoBraking = true;
				this.waitingCoroutine = this.StartCoroutine_Auto (this.CO_WaitingCoroutine (this.sentinelComponents.inspectingCheckingTime, this.DelegatedMethod));

			}


			if (this.sentinelComponents.playerInSight) {

				Debug.Log ("Sentinel switches from <<Inspecting>> to <<Defending>>");
				this.waitingCoroutine = this.ExitingStationaryInspectingState (ref this.sentinelComponents.sentinelIsInspecting, this.sentinelComponents.agent, this.waitingCoroutine);
				return StrategyState.Defending;

			} else if (this.sentinelComponents.sentinelHasEnlargedItsHearingColliders && this.sentinelComponents.agent.remainingDistance < 0.5f) {

				Debug.Log ("Sentinel switches from <<Inspecting>> to <<Scanning>>");
				return StrategyState.Scanning;

			} else if (this.waitingCoroutine != null && !this.sentinelComponents.sentinelIsInspecting) {

				Debug.Log ("Sentinel switches from <<Inspecting>> to <<Falling Into Line>>");
				this.waitingCoroutine = this.ExitingStationaryInspectingState (ref this.sentinelComponents.sentinelIsInspecting, this.sentinelComponents.agent, this.waitingCoroutine);
				return StrategyState.FallingIntoLine;

			} else {

				Debug.Log ("Sentinel does not change strategy");
				return StrategyState.NoStrategyChanging;

			}

		} else {
			
			Debug.Log ("Sentinel has been stunned");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}