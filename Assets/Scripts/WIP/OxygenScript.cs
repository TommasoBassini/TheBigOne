using UnityEngine;
using System.Collections;

//Dovrebbe esserci un bug tra rigenerazione ossigeno e cambio camminata/corsa (devo chiedere una cosa a Michele)
//Devo ancora fare in modo che, durante l'ispezione di ogetti/terminali/cose, non vi sia consumo di ossigeno (devo chiedere una cosa a Michele)
//Devo commentare il codice

public class OxygenScript : MonoBehaviour {

	#region PARAMETERS
	public bool isInspecting;

	[Header ("Ossigeno Parametri Base")]
	[Range (0f, 100f)] public float oxygenAmount;
	[Range (0f, 100f)] public float minOxygenAmount = 0f;
	[Range (0f, 100f)] public float maxOxygenAmount = 100f;

	[Header ("Ossigeno Parametri Camminata")]
	[Range (0f, 100f)] public float oxygenWalkingDecadenceSpeed = 5f;
	[Range (0f, 100f)] public float oxygenWalkingDecadenceAmount = 5f;

	[Header ("Ossigeno Parametri Corsa")]
	[Range (0f, 100f)] public float oxygenRunningDecadenceSpeed = 2f;
	[Range (0f, 100f)] public float oxygenRunningDecadenceAmount = 8f;

	[Header ("Ossigeno Parametri Rigenerazione")]
	[Range (0f, 100f)] public float oxygenRegenerationSpeed = 0.5f;
	[Range (0f, 100f)] public float oxygenRegenerationAmount = 10f;
	#endregion


	#region MONOBEHAVIOUR_METHODS
	public void Start () {

		this.OxygenRecharge ();
		this.StartCoroutine_Auto (this.CO_OxygenWalkingDecadence ());

	}

	public void Update () {

		if (this.oxygenAmount <= this.minOxygenAmount) {

			this.StopAllCoroutines ();

		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {

			this.oxygenAmount--;
			this.StopAllCoroutines ();
			this.StartCoroutine_Auto (this.CO_OxygenRunningDecadence ());

		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {

			this.oxygenAmount--;
			this.StopAllCoroutines ();
			this.StartCoroutine_Auto (this.CO_OxygenWalkingDecadence ());

		}

		if (Input.GetKeyDown (KeyCode.Space)) {

			this.StartCoroutine_Auto (this.CO_OxygenRegeneration ());

		}

	}
	#endregion

	#region COROUTINES
	public IEnumerator CO_OxygenWalkingDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.oxygenWalkingDecadenceSpeed);
			this.oxygenAmount -= this.oxygenWalkingDecadenceAmount;

		}

	}

	public IEnumerator CO_OxygenRunningDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.oxygenRunningDecadenceSpeed);
			this.oxygenAmount -= this.oxygenRunningDecadenceAmount;

		}

	}

	public IEnumerator CO_OxygenRegeneration () {

		int i = 4;

		do {

			yield return new WaitForSeconds (this.oxygenRegenerationSpeed);
			this.oxygenAmount += this.oxygenRegenerationAmount;
			if (this.oxygenAmount >= this.maxOxygenAmount)
				this.oxygenAmount = this.maxOxygenAmount;

		} while (--i > 0);

	}
	#endregion

	#region METHODS
	public void OxygenRecharge (float currentOxygen = 100f) {

		this.oxygenAmount = currentOxygen;

	}
	#endregion

}