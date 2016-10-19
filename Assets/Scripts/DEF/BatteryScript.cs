using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BatteryScript : TimerScript {

	#region BATTERY_SUBCLASS
	public class BatteryEnergySaveAndLoadData
    {
		public float energy;

	}
	#endregion


	#region BATTERY_PARAMETERS
	[Header ("Riferimenti")]
	public Text uiBatteryText;
    public Text menuBatteryText;
    public TorchLightScript torchLightScript;

	[Header ("Batteria Parametri Base - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyAmount = 1f;
	[Range (0f, 8f)] public float minBatteryEnergyAmount = 0f;
	[Range (0f, 8f)] public float maxBatteryEnergyAmount = 1f;

	[Header ("Batteria Parametri di Utilizzo - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyDecadenceSpeed = 2f;
	[Range (0f, 8f)] public float batteryEnergyDecadenceAmount = 0.02f;

	[Header ("Batteria Parametro Step Per Rigenerazione Breve - Da 0 a 100")]
	[Range (0, 100)] public int batteryEnergyRegenerationSteps = 20;

	[Header ("Batteria Parametri Rigenerazione Breve - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergySmallRegenerationSpeed = 0.1f;
	[Range (0f, 8f)] public float batteryEnergySmallRegenerationAmount = 0.01f;

	[Header ("Batteria Parametri Rigenerazione Completa - Da 0f ad 8f")]
	[Range (0f, 8f)] public float batteryEnergyCompleteRegenerationSpeed = 0.1f;
	[Range (0f, 8f)] public float batteryenergyCompleteRegenerationAmount = 0.05f;

	public Coroutine[] batteryEnergyCoroutine;
	public BatteryEnergySaveAndLoadData energyReference;
	#endregion


	#region BATTERY_PROPERTIES
	public float BatteryEnergyAmount {
		
		set {
			
			if (value > this.maxBatteryEnergyAmount) {

				if (this.batteryEnergyCoroutine [1] != null)
					this.StopCoroutine (this.batteryEnergyCoroutine [1]);
				
				this.energyReference.energy = this.maxBatteryEnergyAmount;
				
			} else if (value < this.minBatteryEnergyAmount)
				this.energyReference.energy = this.minBatteryEnergyAmount;
			else
				this.energyReference.energy = value;
			
			this.batteryEnergyAmount = this.energyReference.energy;
			//this.torchLightScript.TorchLightIntensityAmount = this.energyReference.energy;
			this.uiBatteryText.text = (this.energyReference.energy * 100).ToString ("000");
            this.menuBatteryText.text = (this.energyReference.energy * 100).ToString("000");

        }

        get {
			
			return this.energyReference.energy;
			
		}
		
	}
	#endregion


	#region BATTERY_DELEGATES
	public TimedDelegatedMethod[] DelegatedMethod = new TimedDelegatedMethod[] 
    {
		delegate (TimerScript timerScriptReference, float changeAmount) 
        {
			if (timerScriptReference is BatteryScript)
            {

				(timerScriptReference as BatteryScript).BatteryEnergyAmount -= changeAmount;
                
			}
            else Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 0, BATTERIA");
		},

		delegate (TimerScript timerScriptReference, float changeAmount) {

			if (timerScriptReference is BatteryScript) {

				(timerScriptReference as BatteryScript).BatteryEnergyAmount += changeAmount;

			} else Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 1, BATTERIA");

		}

	};
	#endregion


	#region BATTERY_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.batteryEnergyCoroutine = new Coroutine[2];

		if (this.energyReference == null)
			this.energyReference = new BatteryEnergySaveAndLoadData ();

	}

	public void Start () {

		//Si può assegnare qualsiasi altro valore in caso di salvataggi od altra occasione
		this.BatteryEnergyAmount = this.batteryEnergyAmount;

	}

	public void Update () {

		this.BatteryEnergyAmount = this.batteryEnergyAmount;

		if (Input.GetKeyDown (KeyCode.R)) {

			if (this.batteryEnergyCoroutine [1] != null)
				this.StopCoroutine (this.batteryEnergyCoroutine [1]);

			this.batteryEnergyCoroutine [1] = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.batteryEnergyRegenerationSteps, this.batteryEnergySmallRegenerationSpeed, this.batteryEnergySmallRegenerationAmount, this.DelegatedMethod [1]));
			Debug.Log ("Ricarico poca batteria");

		}

		if (Input.GetKeyDown (KeyCode.Y)) {

			if (this.batteryEnergyCoroutine [1] != null)
				this.StopCoroutine (this.batteryEnergyCoroutine [1]);

			this.batteryEnergyCoroutine [1] = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.batteryEnergyCompleteRegenerationSpeed, this.batteryenergyCompleteRegenerationAmount, this.DelegatedMethod [1]));
		}

	}

    public void FullBattery()
    {
        if (this.batteryEnergyCoroutine[1] != null)
            this.StopCoroutine(this.batteryEnergyCoroutine[1]);

        this.batteryEnergyCoroutine[1] = this.StartCoroutine_Auto(this.CO_TimerCoroutine(this.batteryEnergyCompleteRegenerationSpeed, this.batteryenergyCompleteRegenerationAmount, this.DelegatedMethod[1]));
    }
	#endregion


	#region BATTERY_METHODS
	public void StartBatteryEnergyDecadence () {

		this.BatteryEnergyAmount -= 0.01f;
		this.batteryEnergyCoroutine [0] = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.batteryEnergyDecadenceSpeed, this.batteryEnergyDecadenceAmount, this.DelegatedMethod [0]));

	}

	public void StopBatteryEnergyDecadence () {

		this.StopCoroutine (this.batteryEnergyCoroutine [0]);
		this.BatteryEnergyAmount -= 0.01f;

	}
	#endregion

}