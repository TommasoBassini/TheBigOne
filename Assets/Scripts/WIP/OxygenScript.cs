using UnityEngine;
using System.Collections;

//C'è un bug di logica (finezza); ripartire dal camminare (non c'è da riscrivere, solo da verificare la logica) 
//Devo commentare il codice

public class OxygenScript : MonoBehaviour {

	#region PARAMETERS
	//public bool characterIsInspecting;
	public bool characterIsRunning;

	[Header ("Ossigeno Parametri Base")]
	[Range (0f, 100f)] public float oxygenAmount;
	[Range (0f, 100f)] public float minOxygenAmount = 0f;
	[Range (0f, 100f)] public float maxOxygenAmount = 100f;

	[Header ("Ossigeno Parametri da Fermo")]
	[Range (0f, 100f)] public float oxygenStandingDecadenceSpeed = 10f;
	[Range (0f, 100f)] public float oxygenStandingDecadenceAmount = 1f;

	[Header ("Ossigeno Parametri Camminata")]
	[Range (0f, 100f)] public float oxygenWalkingDecadenceSpeed = 5f;
	[Range (0f, 100f)] public float oxygenWalkingDecadenceAmount = 5f;

	[Header ("Ossigeno Parametri Corsa")]
	[Range (0f, 100f)] public float oxygenRunningDecadenceSpeed = 2f;
	[Range (0f, 100f)] public float oxygenRunningDecadenceAmount = 8f;

	[Header ("Ossigeno Parametri Rigenerazione")]
	[Range (0f, 100f)] public float oxygenRegenerationSpeed = 0.5f;
	[Range (0f, 100f)] public float oxygenRegenerationAmount = 40f;
	#endregion


	#region MONOBEHAVIOUR_METHODS
	public void Start () {

		this.characterIsRunning = false;

		this.OxygenRecharge ();
		this.StartCoroutine_Auto (this.CO_OxygenStandingDecadence ());

	}

	public void Update () {

		if (this.oxygenAmount <= this.minOxygenAmount) {

			this.StopAllCoroutines ();
			Debug.Log ("Sono morto");

		} else {
			
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				
				this.characterIsRunning = !this.characterIsRunning;

			}

			if ((Input.GetKey (KeyCode.W) ^ Input.GetKey (KeyCode.S)) || (Input.GetKey (KeyCode.A) ^ Input.GetKey (KeyCode.D))) {
				
				if (this.characterIsRunning) {
					
					if ((Input.GetKeyDown (KeyCode.W) || Input.GetKeyUp (KeyCode.W)) ^ (Input.GetKeyDown (KeyCode.S) || Input.GetKeyUp (KeyCode.S))) {
						
						this.oxygenAmount--;
						this.StopAllCoroutines ();
						this.StartCoroutine_Auto (this.CO_OxygenRunningDecadence ());
						Debug.Log ("Sto correndo in avanti/dietro");
						
					}

					if ((Input.GetKeyDown (KeyCode.A) || Input.GetKeyUp (KeyCode.A)) ^ (Input.GetKeyDown (KeyCode.D) || Input.GetKeyUp (KeyCode.D))) {

						this.oxygenAmount--;
						this.StopAllCoroutines ();
						this.StartCoroutine_Auto (this.CO_OxygenRunningDecadence ());
						Debug.Log ("Sto correndo in sinistra/destra");

					}
					
				} else {
					
					if (((Input.GetKeyDown (KeyCode.W) || Input.GetKeyUp (KeyCode.W)) ^ (Input.GetKeyDown (KeyCode.S) || Input.GetKeyUp (KeyCode.S))) &&
						!(Input.GetKey (KeyCode.A) ^ Input.GetKey (KeyCode.D))) {
						
						this.oxygenAmount--;
						this.StopAllCoroutines ();
						this.StartCoroutine_Auto (this.CO_OxygenWalkingDecadence ());
						Debug.Log ("Sto camminando in avanti/dietro");
						
					}

					if ((Input.GetKeyDown (KeyCode.A) || Input.GetKeyUp (KeyCode.A)) ^ (Input.GetKeyDown (KeyCode.D) || Input.GetKeyUp (KeyCode.D)) &&
						!(Input.GetKey (KeyCode.W) ^ Input.GetKey (KeyCode.S))) {

						this.oxygenAmount--;
						this.StopAllCoroutines ();
						this.StartCoroutine_Auto (this.CO_OxygenWalkingDecadence ());
						Debug.Log ("Sto camminando in sinistra/destra");

					}
					
				}
				
			} else if ((Input.GetKeyDown (KeyCode.W) || Input.GetKeyUp (KeyCode.W)) || (Input.GetKeyDown (KeyCode.S) || Input.GetKeyUp (KeyCode.S)) ||
				(Input.GetKeyDown (KeyCode.A) || Input.GetKeyUp (KeyCode.A)) || (Input.GetKeyDown (KeyCode.D) || Input.GetKeyUp (KeyCode.D))) {

				this.characterIsRunning = false;
				this.oxygenAmount--;
				this.StopAllCoroutines ();
				this.StartCoroutine_Auto (this.CO_OxygenStandingDecadence ());
				Debug.Log ("Sono fermo");
				
			}
			
			if (Input.GetKeyDown (KeyCode.Space)) {
				
				//this.StartCoroutine_Auto (this.CO_OxygenRegeneration ());
				if ((this.oxygenAmount += this.oxygenRegenerationAmount) > this.maxOxygenAmount) {

					this.oxygenAmount = this.maxOxygenAmount;
					Debug.Log ("Ricarico ossigeno");

				}
				
			}

		}

	}
	#endregion


	#region COROUTINES
	public IEnumerator CO_OxygenStandingDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.oxygenStandingDecadenceSpeed);
			this.oxygenAmount -= this.oxygenStandingDecadenceAmount;

		}

	}

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

	/*public IEnumerator CO_OxygenRegeneration () {

		int i = 4;

		do {

			yield return new WaitForSeconds (this.oxygenRegenerationSpeed);
			this.oxygenAmount += this.oxygenRegenerationAmount;
			if (this.oxygenAmount >= this.maxOxygenAmount)
				this.oxygenAmount = this.maxOxygenAmount;

		} while (--i > 0);

	}*/
	#endregion


	#region METHODS
	public void OxygenRecharge (float currentOxygen = 100f) {

		this.oxygenAmount = currentOxygen;

	}
	#endregion

}