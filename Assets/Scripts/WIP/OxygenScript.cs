using UnityEngine;
using System.Collections;

//Dovrebbe esserci un bug tra rigenerazione ossigeno e cambio camminata/corsa

public class OxygenScript : MonoBehaviour {

	public bool isInpecting;

	[Header ("Ossigeno Parametri Base")]
	public float oxygenAmount;
	public const float MIN_OXYGEN_AMOUNT = 0f;
	public const float MAX_OXYGEN_AMOUNT = 100f;
	[Header ("Ossigeno Parametri Camminata")]
	public float oxygenWalkingDecadenceSpeed = 5f;
	public float oxygenWalkingDecadenceAmount = 5f;
	[Header ("Ossigeno Parametri Corsa")]
	public float oxygenRunningDecadenceSpeed = 2f;
	public float oxygenRunningDecadenceAmount = 8f;
	[Header ("Ossigeno ParametriRigenerazione")]
	public float oxygenRegenerationSpeed = 0.5f;
	public float oxygenRegenerationAmount = 10f;


	#region MonoBehaviour_Methods
	public void Start () {

		this.OxygenRecharge ();
		this.StartCoroutine_Auto (this.COOxygenWalkingDecadence ());

	}

	public void Update () {

		if (this.oxygenAmount <= MIN_OXYGEN_AMOUNT) {

			this.StopAllCoroutines ();

		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {

			this.oxygenAmount--;
			this.StopAllCoroutines ();
			this.StartCoroutine_Auto (this.COOxygenRunningDecadence ());

		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {

			this.oxygenAmount--;
			this.StopAllCoroutines ();
			this.StartCoroutine_Auto (this.COOxygenWalkingDecadence ());

		}

		if (Input.GetKeyDown (KeyCode.Space)) {

			this.StartCoroutine_Auto (this.COOxygenRegeneration ());

		}

	}
	#endregion

	#region Coroutines
	public IEnumerator COOxygenWalkingDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.oxygenWalkingDecadenceSpeed);
			this.oxygenAmount -= this.oxygenWalkingDecadenceAmount;

		}

	}

	public IEnumerator COOxygenRunningDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.oxygenRunningDecadenceSpeed);
			this.oxygenAmount -= this.oxygenRunningDecadenceAmount;

		}

	}

	public IEnumerator COOxygenRegeneration () {

		int i = 4;

		do {

			yield return new WaitForSeconds (this.oxygenRegenerationSpeed);
			this.oxygenAmount += this.oxygenRegenerationAmount;
			if (this.oxygenAmount >= MAX_OXYGEN_AMOUNT)
				this.oxygenAmount = MAX_OXYGEN_AMOUNT;

		} while (--i > 0);

	}
	#endregion

	#region Methods
	public void OxygenRecharge (float oxygen = MAX_OXYGEN_AMOUNT) {

		this.oxygenAmount = oxygen;

	}
	#endregion

}