using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TorchLightScript : MonoBehaviour {

	#region TORCHLIGHT_AND_BATTERY_PARAMETERS
	[Header ("Riferimenti")]
	public Light torchLight;
	public Text uiTorchLightText;

	[Header ("Flag Booleani")]
	public bool torchLightHasBeenTriggered;

	[Header ("Batteria Parametri Base - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyAmount;
	[Range (0f, 8f)] public float minBatteryEnergyAmount = 0f;
	[Range (0f, 8f)] public float maxBatteryEnergyAmount = 1f;

	[Header ("Batteria Parametri di Utilizzo - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyDecadenceSpeed = 2f;
	[Range (0f, 8f)] public float batteryEnergyDecadenceAmount = 0.02f;

	[Header ("Batteria Parametri Rigenerazione - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyRegenerationAmount = 0.2f;
	#endregion


	#region TORCHLIGHT_AND_BATTERY_PROPERTIES
	public float BatteryEnergyAmount {

		set {

			if (value > this.maxBatteryEnergyAmount)
				this.batteryEnergyAmount = this.maxBatteryEnergyAmount;
			else if (value < this.minBatteryEnergyAmount)
				this.batteryEnergyAmount = this.minBatteryEnergyAmount;
			else
				this.batteryEnergyAmount = value;

			this.torchLight.intensity = this.batteryEnergyAmount;
			this.torchLight.bounceIntensity = this.batteryEnergyAmount;
			this.uiTorchLightText.text = (this.batteryEnergyAmount * 100).ToString ("000");

		}

		get {

			return this.batteryEnergyAmount;

		}

	}
	#endregion


	#region TORCHLIGHT_AND_BATTERY_MONOBEHAVIOUR_METHODS
	public void Start () {

		//Solita pulizia dei booleani
		this.torchLightHasBeenTriggered = false;
		this.torchLight.enabled = false;

		//fermo tutte le coroutine ad ogni riavvio di scena (si sa mai)
		this.StopAllCoroutines ();

		//Si può assegnare qualsiasi altro valore in caso di salvataggi od altra occasione
		this.BatteryEnergyAmount = this.maxBatteryEnergyAmount;

	}

	public void Update () {

		if (Input.GetKeyDown (KeyCode.T)) {
			//Attivazione-disattivazione torcia; qualora venisse abilitata, si memorizza un booleano di trigger

			if (this.torchLight.enabled = !this.torchLight.enabled)
				this.torchLightHasBeenTriggered = true;

		}

		if (this.torchLightHasBeenTriggered) {
			
			this.StartCoroutine_Auto (this.CO_BatteryEnergyDecadence ());
			this.torchLightHasBeenTriggered = false;

		} else if (!this.torchLight.enabled)
			StopAllCoroutines ();

		if (Input.GetKeyDown (KeyCode.R))
			this.BatteryEnergyAmount += this.batteryEnergyRegenerationAmount;

	}
	#endregion


	#region TORCHLIGHT_AND_BATTERY_COROUTINES
	public IEnumerator CO_BatteryEnergyDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.batteryEnergyDecadenceSpeed);
			this.BatteryEnergyAmount -= this.batteryEnergyDecadenceAmount;

		}

	}
	#endregion

}