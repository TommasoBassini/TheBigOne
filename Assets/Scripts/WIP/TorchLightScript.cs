using UnityEngine;
using System.Collections;

public class TorchLightScript : MonoBehaviour {

	#region TORCHLIGHT_PARAMETERS
	[Header ("Riferimenti")]
	public Light torchLight;
	public BatteryScript batteryScript;

	[Header ("Flag Booleani")]
	public bool torchLightHasBeenTriggeredOn;
	public bool torchLightHasBeenTriggeredOff;
	#endregion


	#region TORCHLIGHT_PROPERTIES
	public float TorchLightIntensityAmount {

		set {

			this.torchLight.intensity = value;
			this.torchLight.bounceIntensity = value;

		}

		get {

			return this.torchLight.intensity;

		}

	}
	#endregion


	#region TORCHLIGHT_MONOBEHAVIOUR_METHODS
	public void Start () {

		//Solita pulizia dei booleani
		this.torchLightHasBeenTriggeredOn = false;
		this.torchLightHasBeenTriggeredOff = false;
		this.torchLight.enabled = false;

	}

	public void Update () {
		
		if (Input.GetKeyDown (KeyCode.T)) {
			//Attivazione-disattivazione torcia; qualora venisse abilitata, si memorizza un booleano di trigger
			
			if (this.torchLight.enabled = !this.torchLight.enabled)
				this.torchLightHasBeenTriggeredOn = true;
			else
				this.torchLightHasBeenTriggeredOff = true;
			
		}
		
		if (this.torchLightHasBeenTriggeredOn) {
			
			this.batteryScript.StartBatteryEnergyDecadence ();
			this.torchLightHasBeenTriggeredOn = false;
			
		}
		
		if (this.torchLightHasBeenTriggeredOff) {
			
			this.batteryScript.StopBatteryEnergyDecadence ();
			this.torchLightHasBeenTriggeredOff = false;
			
		}
		
	}
	#endregion

}