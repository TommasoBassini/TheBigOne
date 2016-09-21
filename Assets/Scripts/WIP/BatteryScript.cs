using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BatteryScript : MonoBehaviour {

	#region BATTERY_PARAMETERS
	[Header ("Riferimenti")]
	public Text uiBatteryText;
	public TorchLightScript torchLightScript;

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


	#region BATTERY_PROPERTIES
	public float BatteryEnergyAmount {

		set {

			if (value > this.maxBatteryEnergyAmount)
				this.batteryEnergyAmount = this.maxBatteryEnergyAmount;
			else if (value < this.minBatteryEnergyAmount)
				this.batteryEnergyAmount = this.minBatteryEnergyAmount;
			else
				this.batteryEnergyAmount = value;

			this.torchLightScript.TorchLightIntensityAmount = this.batteryEnergyAmount;
			this.uiBatteryText.text = (this.batteryEnergyAmount * 100).ToString ("000");

		}

		get {

			return this.batteryEnergyAmount;

		}

	}
	#endregion


	#region BATTERY_MONOBEHAVIOUR_METHODS
	public void Start () {

		//fermo tutte le coroutine ad ogni riavvio di scena (si sa mai)
		this.StopAllCoroutines ();

		//Si può assegnare qualsiasi altro valore in caso di salvataggi od altra occasione
		this.BatteryEnergyAmount = this.maxBatteryEnergyAmount;

	}

	public void Update () {

		if (Input.GetKeyDown (KeyCode.R))
			this.BatteryEnergyAmount += this.batteryEnergyRegenerationAmount;

	}
	#endregion


	#region BATTERY_COROUTINES
	public IEnumerator CO_BatteryEnergyDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.batteryEnergyDecadenceSpeed);
			this.BatteryEnergyAmount -= this.batteryEnergyDecadenceAmount;

		}

	}
	#endregion


	#region BATTERY_METHODS
	public void StartBatteryEnergyDecadence () {

		this.StartCoroutine_Auto (this.CO_BatteryEnergyDecadence ());

	}

	public void StopBatteryEnergyDecadence () {

		this.StopAllCoroutines ();

	}
	#endregion

}