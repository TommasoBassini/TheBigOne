using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class OxygenScript : TimerScript {

	#region OXYGEN_SUBCLASS
	public class OxygenSaveAndLoadData {
		
		public float oxygen;
		
	}
	#endregion


	#region OXYGEN_PARAMETERS
	[Header ("Riferimento a UI Text")]
	public Text uiOxygenText;

	[Header ("Flag Booleani")]
	public bool leftShiftHasBeenPressed;
	public bool characterIsRunning;

	[Header ("Ossigeno Parametri Base - Da 0f a 10f")]
	[Range (0f, 1000f)] public float oxygenAmount = 100f;
	[Range (0f, 1000f)] public float minOxygenAmount = 0f;
	[Range (0f, 1000f)] public float maxOxygenAmount = 100f;

	[Header ("Ossigeno Parametri da Fermo - Da 0f a 10f")]
	[Range (0f, 10f)] public float oxygenStandingDecadenceSpeed = 10f;
	[Range (0f, 3f)] public float oxygenStandingDecadenceAmount = 1f;

	[Header ("Ossigeno Parametri Camminata - Da 0f a 10f")]
	[Range (0f, 10f)] public float oxygenWalkingDecadenceSpeed = 5f;
	[Range (0f, 3f)] public float oxygenWalkingDecadenceAmount = 5f;

	[Header ("Ossigeno Parametri Corsa - Da 0f a 10f")]
	[Range (0f, 10f)] public float oxygenRunningDecadenceSpeed = 2f;
	[Range (0f, 3f)] public float oxygenRunningDecadenceAmount = 8f;

	[Header ("Ossigeno Parametro Step Per Rigenerazione Breve - Da 0 a 100")]
	[Range (0, 100)] public int oxygenRegenerationSteps = 20;

	[Header ("Ossigeno Parametri Rigenerazione Breve - Da 0f a 10f")]
	[Range (0f, 10f)] public float oxygenSmallRegenerationSpeed = 0.1f;
	[Range (0f, 10f)] public float oxygenSmallRegenerationAmount = 1f;

	[Header ("Ossigeno Parametri Rigenerazione Completa - Da 0f a 10f")]
	[Range (0f, 10f)] public float oxygenCompleteRegenerationSpeed = 0.1f;
	[Range (0f, 10f)] public float oxygenCompleteRegenerationAmount = 5f;

	public Coroutine[] oxygenCoroutine;
	public OxygenSaveAndLoadData OxygenReference;
    private FirstPersonController fpsController;
	#endregion


	#region OXYGEN_PROPERTIES
	public float OxygenAmount {
		
		set {
			
			if (value > this.maxOxygenAmount) {

				if (this.oxygenCoroutine [1] != null)
					this.StopCoroutine (this.oxygenCoroutine [1]);
				
				this.OxygenReference.oxygen = this.maxOxygenAmount;
				
			} else if (value < this.minOxygenAmount)
				this.OxygenReference.oxygen = this.minOxygenAmount;
			else
				this.OxygenReference.oxygen = value;
			
			this.oxygenAmount = this.OxygenReference.oxygen;
			this.uiOxygenText.text = this.OxygenReference.oxygen.ToString ("000");
			
		}
		
		get {
			
			return this.OxygenReference.oxygen;
			
		}
		
	}
	#endregion


	#region OXYGEN_DELEGATES
	public TimedDelegatedMethod[] DelegatedMethod = new TimedDelegatedMethod[] {

		delegate (TimerScript timerScriptReference, float changeAmount) {

			if (timerScriptReference is OxygenScript) {

				(timerScriptReference as OxygenScript).OxygenAmount -= changeAmount;

			} else Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 0, OSSIGENO");

		},

		delegate (TimerScript timerScriptReference, float changeAmount) {

			if (timerScriptReference is OxygenScript) {

				(timerScriptReference as OxygenScript).OxygenAmount += changeAmount;

			} else Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 1, OSSIGENO");

		}

	};
	#endregion


	#region OXYGEN_MONOBEHAVIOUR_METHODS
	public void Awake ()
    {
        fpsController = GetComponent<FirstPersonController>();

        this.oxygenCoroutine = new Coroutine[2];

		if (this.OxygenReference == null)
			this.OxygenReference = new OxygenSaveAndLoadData ();

	}

	public void Start ()
    {


        //Qualora ci sia un restart della scena, è sempre buona regola pulire i flag booleani
        this.leftShiftHasBeenPressed = false;
		this.characterIsRunning = false;

		//La meccanica dell'ossigeno viene inizializzata con un ammontare di ossigeno (sarebbe possibile metterne uno a piacere, in caso di salvataggi)
		//con decadenza da fermo
		this.OxygenAmount = this.oxygenAmount;
		this.oxygenCoroutine [0] = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.oxygenStandingDecadenceSpeed, this.oxygenStandingDecadenceAmount, this.DelegatedMethod [0]));
		
	}
	
	public void Update ()
    {

        this.OxygenAmount = this.oxygenAmount;
		
		if (this.OxygenAmount == this.minOxygenAmount)
        {
			//Se il personaggio è morto, vengono fermate tutte le coroutine (è possibile, per finezza, spegnere il MonoBehaviour qui dentro)
			this.StopAllCoroutines ();
		}
        else
        {
			//Se il personaggio è vivo, viene eseguito il seguente codice; si: ogni qualvolta la persona giocatrice dovesse cambiare la propria camminata, corsa o ritornare fermo,
			//verrebbe punita con la sottrazione di un punto ossigeno; la motivazione è da ricercarsi nell'uso delle coroutine, meccanica utile e semplice, ma che permetterebbe
			//di exploitare il gioco, evitando qualsivoglia consumo di ossigeno

			//Il tasto di cambio camminata/corsa è l'unico booleano che abbia senso memorizzare per ogni Update
			this.leftShiftHasBeenPressed = Input.GetKeyDown (KeyCode.Joystick1Button9);

            /*if ((Input.GetKey (KeyCode.W) ^ Input.GetKey (KeyCode.S)) || (Input.GetKey (KeyCode.A) ^ Input.GetKey (KeyCode.D)))*/
            if (fpsController.walking)
            {
                //Se il personaggio dovesse muoversi, verrebbe eseguito il seguente codice di if --> Bool? isMov != null
                /*if (this.leftShiftHasBeenPressed || ((Input.GetKeyDown (KeyCode.W) || Input.GetKeyUp (KeyCode.W)) ^ (Input.GetKeyDown (KeyCode.S) || Input.GetKeyUp (KeyCode.S))) ||
					((Input.GetKeyDown (KeyCode.A) || Input.GetKeyUp (KeyCode.A)) ^ (Input.GetKeyDown (KeyCode.D) || Input.GetKeyUp (KeyCode.D))))*/
                if (this.leftShiftHasBeenPressed || fpsController.isChangeWalking)
                {
					//All'infuori del cambio camminata/corsa, tutte le valutazioni sulle chiavi sono utili per simulare (qui dentro) il cambio movimento del pesonaggio --> Valutazione trigger cambio corsa/camminata - trigger inizio movimento

					if (this.leftShiftHasBeenPressed)
                    {
						//Seconda valutazione seriale del cambio camminata/corsa, motivazione della memorizzazione; il cambio di modalità viene memorizzato in un secondo booleano
						
						this.characterIsRunning = !this.characterIsRunning;
					}
					
					if (this.characterIsRunning)
                    {
						//Bool? isMov == true

						this.StopCoroutine (this.oxygenCoroutine [0]);
                        this.OxygenAmount -= 0.25f;
                        this.oxygenCoroutine [0] = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.oxygenRunningDecadenceSpeed, this.oxygenRunningDecadenceAmount, this.DelegatedMethod [0]));
					}
                    else
                    {
						//Bool? isMov == false

						this.StopCoroutine (this.oxygenCoroutine [0]);
                        this.OxygenAmount -= 0.25f;
                        this.oxygenCoroutine [0] = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.oxygenWalkingDecadenceSpeed, this.oxygenWalkingDecadenceAmount, this.DelegatedMethod [0]));
					}
				}
			}
            /*else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S) || Input.GetKeyUp(KeyCode.S)) ||
                (Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.D)))*/
            else if (fpsController.isChangeWalking)
            {
                //Se il personaggio non dovesse più muoversi (FERMANDOSI), o dovesse avere degli input, fra loro contrastanti, smetterebbe di muoversi, resettando il proprio stato di camminata/corsa --> Valutazione trigger della fermata

                this.StopCoroutine(this.oxygenCoroutine[0]);
                this.characterIsRunning = false;
                this.OxygenAmount-= 0.25f;
                this.oxygenCoroutine[0] = this.StartCoroutine_Auto(this.CO_TimerCoroutine(this.oxygenStandingDecadenceSpeed, this.oxygenStandingDecadenceAmount, this.DelegatedMethod[0]));
            }


            if (Input.GetKeyDown (KeyCode.Space))
            {
				//Prima della percezione della chiave utile, andrebbe messa la valutazione della "corretta vicinanza" della postazione al personaggio

				if (this.oxygenCoroutine [1] != null)
					this.StopCoroutine (this.oxygenCoroutine [1]);

				this.oxygenCoroutine [1] = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.oxygenRegenerationSteps, this.oxygenSmallRegenerationSpeed, this.oxygenSmallRegenerationAmount, this.DelegatedMethod [1]));
				Debug.Log ("Ricarico poco ossigeno");
			}

			if (Input.GetKeyDown (KeyCode.LeftControl))
            {

				if (this.oxygenCoroutine [1] != null)
					this.StopCoroutine (this.oxygenCoroutine [1]);

				this.oxygenCoroutine [1] = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.oxygenCompleteRegenerationSpeed, this.oxygenCompleteRegenerationAmount, this.DelegatedMethod [1]));
				Debug.Log ("Ricarico tutto l'ossigeno");
			}
		}
		
	}
	#endregion

}