using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OxygenScript : MonoBehaviour {

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

	[Header ("Ossigeno Parametri Base - Da 0f a 1000f")]
	[Range (0f, 1000f)] public float oxygenAmount = 100f;
	[Range (0f, 1000f)] public float minOxygenAmount = 0f;
	[Range (0f, 1000f)] public float maxOxygenAmount = 100f;

	[Header ("Ossigeno Parametri da Fermo - Da 0f a 1000f")]
	[Range (0f, 1000f)] public float oxygenStandingDecadenceSpeed = 10f;
	[Range (0f, 1000f)] public float oxygenStandingDecadenceAmount = 1f;

	[Header ("Ossigeno Parametri Camminata - Da 0f a 1000f")]
	[Range (0f, 1000f)] public float oxygenWalkingDecadenceSpeed = 5f;
	[Range (0f, 1000f)] public float oxygenWalkingDecadenceAmount = 5f;

	[Header ("Ossigeno Parametri Corsa - Da 0f a 1000f")]
	[Range (0f, 1000f)] public float oxygenRunningDecadenceSpeed = 2f;
	[Range (0f, 1000f)] public float oxygenRunningDecadenceAmount = 8f;

	[Header ("Ossigeno Parametri Rigenerazione - Da 0f a 1000f")]
	[Range (0f, 1000f)] public float oxygenRegenerationAmount = 20f;

	public OxygenSaveAndLoadData OxygenRef;
	#endregion


	#region OXYGEN_PROPERTIES
	public float OxygenAmount {

		set {

			if (value > this.maxOxygenAmount)
				this.OxygenRef.oxygen = this.maxOxygenAmount;
			else if (value < this.minOxygenAmount)
				this.OxygenRef.oxygen = this.minOxygenAmount;
			else
				this.OxygenRef.oxygen = value;

			this.uiOxygenText.text = this.OxygenRef.oxygen.ToString ("000");

		}

		get {

			return this.OxygenRef.oxygen;

		}

	}
	#endregion


	#region OXYGEN_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.OxygenRef = new OxygenSaveAndLoadData ();

	}

	public void Start () {

		//Qualora ci sia un restart della scena, è sempre buona regola pulire i flag booleani
		this.leftShiftHasBeenPressed = false;
		this.characterIsRunning = false;

		//Non so se, al cambio/restart di una scena, sopravvivano le coroutine; non voglio rischiare
		this.StopAllCoroutines ();

		//La meccanica dell'ossigeno viene inizializzata con un ammontare di ossigeno (sarebbe possibile metterne uno a piacere, in caso di salvataggi)
		//con decadenza da fermo
		this.OxygenAmount = this.oxygenAmount;
		this.StartCoroutine_Auto (this.CO_OxygenStandingDecadence ());
		
	}
	
	public void Update () {
		
		if (this.OxygenAmount == this.minOxygenAmount) {

			//Se il personaggio è morto, vengono fermate tutte le coroutine (è possibile, per finezza, spegnere il MonoBehaviour qui dentro)
			this.StopAllCoroutines ();
			Debug.Log ("Sono morto");
			
		} else {

			//Se il personaggio è vivo, viene eseguito il seguente codice; si: ogni qualvolta la persona giocatrice dovesse cambiare la propria camminata, corsa o ritornare fermo,
			//verrebbe punita con la sottrazione di un punto ossigeno; la motivazione è da ricercarsi nell'uso delle coroutine, meccanica utile e semplice, ma che permetterebbe
			//di exploitare il gioco, evitando qualsivoglia consumo di ossigeno

			//Il tasto di cambio camminata/corsa è l'unico booleano che abbia senso memorizzare per ogni Update
			this.leftShiftHasBeenPressed = Input.GetKeyDown (KeyCode.LeftShift);

			if ((Input.GetKey (KeyCode.W) ^ Input.GetKey (KeyCode.S)) || (Input.GetKey (KeyCode.A) ^ Input.GetKey (KeyCode.D))) {
				//Se il personaggio dovesse muoversi, verrebbe eseguito il seguente codice di if

				if (this.leftShiftHasBeenPressed || ((Input.GetKeyDown (KeyCode.W) || Input.GetKeyUp (KeyCode.W)) ^ (Input.GetKeyDown (KeyCode.S) || Input.GetKeyUp (KeyCode.S))) ||
					((Input.GetKeyDown (KeyCode.A) || Input.GetKeyUp (KeyCode.A)) ^ (Input.GetKeyDown (KeyCode.D) || Input.GetKeyUp (KeyCode.D)))) {
					//All'infuori del cambio camminata/corsa, tutte le valutazioni sulle chiavi sono utili per simulare (qui dentro) il cambio movimento del pesonaggio

					if (this.leftShiftHasBeenPressed) {
						//Seconda valutazione seriale del cambio camminata/corsa, motivazione della memorizzazione; il cambio di modalità viene memorizzato in un secondo booleano
						
						this.characterIsRunning = !this.characterIsRunning;
						
					}
					
					if (this.characterIsRunning) {
						
						this.StopAllCoroutines ();
						this.OxygenAmount--;
						this.StartCoroutine_Auto (this.CO_OxygenRunningDecadence ());
						Debug.Log ("Sto correndo");
						
					} else {
						
						this.StopAllCoroutines ();
						this.OxygenAmount--;
						this.StartCoroutine_Auto (this.CO_OxygenWalkingDecadence ());
						Debug.Log ("Sto camminando");
						
					}
					
				}
				
			} else if ((Input.GetKeyDown (KeyCode.W) || Input.GetKeyUp (KeyCode.W)) || (Input.GetKeyDown (KeyCode.S) || Input.GetKeyUp (KeyCode.S)) ||
				(Input.GetKeyDown (KeyCode.A) || Input.GetKeyUp (KeyCode.A)) || (Input.GetKeyDown (KeyCode.D) || Input.GetKeyUp (KeyCode.D))) {
				//Se il personaggio non dovesse più muoversi (FERMANDOSI), o dovesse avere degli input, fra loro contrastanti, smetterebbe di muoversi, resettando il proprio stato di camminata/corsa
				
				this.StopAllCoroutines ();
				this.characterIsRunning = false;
				this.OxygenAmount--;
				this.StartCoroutine_Auto (this.CO_OxygenStandingDecadence ());
				Debug.Log ("Sono fermo");
				
			}
			
			if (Input.GetKeyDown (KeyCode.Space)) {
				//Prima della percezione della chiave utile, andrebbe messa la valutazione della "corretta vicinanza" della postazione al personaggio

				this.OxygenAmount += this.oxygenRegenerationAmount;
				Debug.Log ("Ricarico ossigeno");
				
			}
			
		}
		
	}
	#endregion


	#region OXYGEN_COROUTINES
	public IEnumerator CO_OxygenStandingDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.oxygenStandingDecadenceSpeed);
			this.OxygenAmount -= this.oxygenStandingDecadenceAmount;

		}

	}

	public IEnumerator CO_OxygenWalkingDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.oxygenWalkingDecadenceSpeed);
			this.OxygenAmount -= this.oxygenWalkingDecadenceAmount;

		}

	}

	public IEnumerator CO_OxygenRunningDecadence () {

		while (true) {

			yield return new WaitForSeconds (this.oxygenRunningDecadenceSpeed);
			this.OxygenAmount -= this.oxygenRunningDecadenceAmount;

		}

	}
	#endregion

}