using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BatteryScript : TimerScript {

	#region BATTERY_SUBCLASS
	public class BatteryEnergySaveAndLoadData {

		public float energy;

	}
	#endregion


	#region BATTERY_PARAMETERS
	[Header ("Riferimenti")]
	public Text uiBatteryText;
	public TorchLightScript torchLightScript;

	[Header ("Batteria Parametri Base - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyAmount = 1f;
	[Range (0f, 8f)] public float minBatteryEnergyAmount = 0f;
	[Range (0f, 8f)] public float maxBatteryEnergyAmount = 1f;

	[Header ("Batteria Parametri di Utilizzo - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyDecadenceSpeed = 2f;
	[Range (0f, 8f)] public float batteryEnergyDecadenceAmount = 0.02f;

	[Header ("Batteria Parametri Rigenerazione - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyRegenerationAmount = 0.2f;

	public Coroutine batteryEnergyDecadenceCoroutineRef;
	public BatteryEnergySaveAndLoadData energyRef;
	#endregion


	#region BATTERY_PROPERTIES
	public float BatteryEnergyAmount {

		set {

			if (value > this.maxBatteryEnergyAmount)
				this.energyRef.energy = this.maxBatteryEnergyAmount;
			else if (value < this.minBatteryEnergyAmount)
				this.energyRef.energy = this.minBatteryEnergyAmount;
			else
				this.energyRef.energy = value;

			this.batteryEnergyAmount = this.energyRef.energy;
			this.torchLightScript.TorchLightIntensityAmount = this.energyRef.energy;
			this.uiBatteryText.text = (this.energyRef.energy * 100).ToString ("000");

		}

		get {

			return this.energyRef.energy;

		}

	}
	#endregion


	#region BATTERY_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.energyRef = new BatteryEnergySaveAndLoadData ();

	}

	public void Start () {

		//Si può assegnare qualsiasi altro valore in caso di salvataggi od altra occasione
		this.BatteryEnergyAmount = this.batteryEnergyAmount;

	}

	public void Update () {

		this.batteryEnergyAmount = this.BatteryEnergyAmount;

		if (Input.GetKeyDown (KeyCode.R))
			this.BatteryEnergyAmount += this.batteryEnergyRegenerationAmount;

	}
	#endregion


	#region BATTERY_METHODS
	public void StartBatteryEnergyDecadence () {

		this.batteryEnergyDecadenceCoroutineRef = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.batteryEnergyDecadenceSpeed, this.batteryEnergyDecadenceAmount, this.DelegatedMethod [0]));

	}

	public void StopBatteryEnergyDecadence () {

		if (this.batteryEnergyDecadenceCoroutineRef != null)
			this.StopCoroutine (this.batteryEnergyDecadenceCoroutineRef);

	}
	#endregion

}