using UnityEngine;
using System.Collections;

public class TorchLightScript : MonoBehaviour {

	#region TORCHLIGHT_PARAMETERS
	[Header ("Riferimenti")]
	public Light torchLight;
	public BatteryScript batteryScript;

	[Header ("Flag Booleani")]
	public bool torchLightHasBeenTriggered;
	#endregion


	#region TORCHLIGHT_PROPERTIES
	public float TorchLightIntensityAmount {

		set {

			this.torchLight.intensity = this.batteryScript.BatteryEnergyAmount;
			this.torchLight.bounceIntensity = this.batteryScript.BatteryEnergyAmount;

		}

		get {

			return this.torchLight.intensity;

		}

	}
	#endregion


	#region TORCHLIGHT_MONOBEHAVIOUR_METHODS
	public void Start () {

		//Solita pulizia dei booleani
		this.torchLightHasBeenTriggered = false;
		this.torchLight.enabled = false;

		//fermo tutte le coroutine ad ogni riavvio di scena (si sa mai)
		this.StopAllCoroutines ();

	}

	public void Update () {

		if (Input.GetKeyDown (KeyCode.T)) {
			//Attivazione-disattivazione torcia; qualora venisse abilitata, si memorizza un booleano di trigger

			if (this.torchLight.enabled = !this.torchLight.enabled)
				this.torchLightHasBeenTriggered = true;

		}

		if (this.torchLightHasBeenTriggered) {
			
			this.batteryScript.StartBatteryEnergyDecadence ();
			this.torchLightHasBeenTriggered = false;

		} else if (!this.torchLight.enabled)
			this.batteryScript.StopBatteryEnergyDecadence ();

	}
	#endregion

}