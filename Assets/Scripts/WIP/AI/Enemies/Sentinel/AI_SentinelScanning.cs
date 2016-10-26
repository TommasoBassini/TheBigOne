using UnityEngine;
using System.Collections;

public class AI_SentinelScanning : MonoBehaviour, IAI_ImplementedStrategy {

	[Tooltip ("DO NOT TOUCH!")]
	public Coroutine scanningCoroutine;
	public AI_SentinelComponent sentinelComponents;


	#region SENTINEL_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelComponents = this.GetComponent <AI_SentinelComponent> ();

	}
	#endregion


	#region SENTINEL_METHODS
	public bool EnlargeHearingCollidersRadius () {

		this.sentinelComponents.sentinelHasEnlargedItsHearingColliders = true;

		this.sentinelComponents.runCol.radius *= 2f;
		this.sentinelComponents.walkCol.radius *= 2f;
		this.sentinelComponents.crouchCol.radius *= 2f;

		return true;

	}


	public void StopScanning () {

		if (this.scanningCoroutine != null)
			this.StopCoroutine (this.scanningCoroutine);

		this.sentinelComponents.sentinelEndsScanning = false;
		this.sentinelComponents.sentinelIsScanning = false;

	}
	#endregion


	#region SENTINEL_COROUTINES
	public IEnumerator CO_ScanningCoroutine () {

		yield return new WaitForSeconds (this.sentinelComponents.scanningTime);
		this.sentinelComponents.sentinelEndsScanning = true;

	}
	#endregion


	#region IMPLEMENTED_STRATEGY_METHOD
	public StrategyState ExecuteImplementedStrategy () {

		Debug.Log ("Sentinel is in <<Scanning>>");


		if (!this.sentinelComponents.sentinelIsScanning) {
			
			this.sentinelComponents.sentinelIsScanning = this.EnlargeHearingCollidersRadius ();
			this.scanningCoroutine = this.StartCoroutine_Auto (this.CO_ScanningCoroutine ());

		}

		this.transform.localRotation = Quaternion.AngleAxis ((720f / this.sentinelComponents.scanningTime) * Time.deltaTime, Vector3.up);


		if (this.sentinelComponents.playerInSight) {

			Debug.Log ("Sentinel switches from <<Scanning>> to <<Defending>>");
			this.StopScanning ();
			return StrategyState.Defending;

		} else if (this.sentinelComponents.playerHasBeenHeard) {
			
			Debug.Log ("Sentinel switches from <<Scanning>> to <<Inspecting>>");
			this.StopScanning ();
			return StrategyState.Inspecting;

		} else if (this.sentinelComponents.sentinelEndsScanning) {

			Debug.Log ("Sentinel switches from <<Scanning>> to <<Falling Into Line>>");
			this.StopScanning ();
			return StrategyState.FallingIntoLine;

		} else {

			Debug.Log ("Sentinel does not change strategy");
			return StrategyState.NoStrategyChanging;

		}

	}
	#endregion

}