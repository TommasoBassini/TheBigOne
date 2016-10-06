using UnityEngine;
using System.Collections;

public abstract class AbsorbableMaterialsScript : TimerScript {

	#region ABSORBABLE_MATERIALS_SUBCLASS
	public class AbsorbableMaterialsSaveAndLoadData {

		public bool materialIsAbsorbed;

	}
	#endregion


	#region ABSORBABLE_MATERIALS_PARAMETERS
	public static bool characterHasAbsorbedOneMaterial;

	[Header ("Flag Booleani")]
	public bool characterHasAbsorbedMaterial;
	public bool characterIsAbsorbingMaterial;
	public bool characterIsReleasingMaterial;

	[Header ("Materiali Assorbibili - (NON TOCCARE) Parametro di Feedback - Da 0f a 100f")]
	[Range (0f, 100f)] public float absorbingMaterialCurrentlyElapsed = 0f;

	[Header ("Materiali Assorbibili - Parametri Configurabili - Da 0f a 100f")]
	[Range (0f, 100f)] public float absorbingMaterialAbsorbingSpeed = 0.04f;
	[Range (0f, 100f)] public float absorbingMaterialAbsorbingAmount = 4f;
	[Range (0f, 100f)] public float absorbingMaterialMaxAbsorbableAmount = 100f;

	public Coroutine absorbableMaterialCoroutine;
	public AbsorbableMaterialsSaveAndLoadData absorbedMaterialReference;
	#endregion


	#region ABSORBABLE_MATERIALS_PROPERTIES
	public bool CharacterHasAbsorbedMaterial {

		set {

			this.characterHasAbsorbedMaterial = value;
			this.absorbedMaterialReference.materialIsAbsorbed = value;

		}

	}

	public float AbsorbingMaterialCurrentlyElapsed {

		set {

			if (value > this.absorbingMaterialMaxAbsorbableAmount)
				this.absorbingMaterialCurrentlyElapsed = this.absorbingMaterialMaxAbsorbableAmount;
			else
				this.absorbingMaterialCurrentlyElapsed = value;

		}

		get {

			return this.absorbingMaterialCurrentlyElapsed;

		}

	}
	#endregion


	#region ABSORBABLE_MATERIALS_DELEGATES
	public TimedDelegatedMethod[] DelegatedMethod = new TimedDelegatedMethod[] {
		
		delegate (TimerScript timerScriptReference, float changeAmount) {
			
			AbsorbableMaterialsScript absorbableMaterialsScriptRecast;
			
			if (timerScriptReference is AbsorbableMaterialsScript) {
				
				absorbableMaterialsScriptRecast = (timerScriptReference as AbsorbableMaterialsScript);

				absorbableMaterialsScriptRecast.AbsorbingMaterialCurrentlyElapsed += changeAmount;
				
				if (absorbableMaterialsScriptRecast.AbsorbingMaterialCurrentlyElapsed == absorbableMaterialsScriptRecast.absorbingMaterialMaxAbsorbableAmount) {
					
					AbsorbableMaterialsScript.characterHasAbsorbedOneMaterial = true;
					
					absorbableMaterialsScriptRecast.CharacterHasAbsorbedMaterial = true;
					absorbableMaterialsScriptRecast.characterIsAbsorbingMaterial = false;
					absorbableMaterialsScriptRecast.AbsorbingMaterialCurrentlyElapsed = 0f;
					absorbableMaterialsScriptRecast.StopCoroutine (absorbableMaterialsScriptRecast.absorbableMaterialCoroutine);
					
				}
				
			} else
				Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 0, MATERIALE ASSORBIBILE");
			
		},

		delegate (TimerScript timerScriptReference, float changeAmount) {

			AbsorbableMaterialsScript absorbableMaterialsScriptRecast;

			if (timerScriptReference is AbsorbableMaterialsScript) {

				absorbableMaterialsScriptRecast = (timerScriptReference as AbsorbableMaterialsScript);

				absorbableMaterialsScriptRecast.AbsorbingMaterialCurrentlyElapsed += changeAmount;

				if (absorbableMaterialsScriptRecast.AbsorbingMaterialCurrentlyElapsed == absorbableMaterialsScriptRecast.absorbingMaterialMaxAbsorbableAmount) {

					AbsorbableMaterialsScript.characterHasAbsorbedOneMaterial = false;
					
					absorbableMaterialsScriptRecast.CharacterHasAbsorbedMaterial = false;
					absorbableMaterialsScriptRecast.characterIsReleasingMaterial = false;
					absorbableMaterialsScriptRecast.AbsorbingMaterialCurrentlyElapsed = 0f;
					absorbableMaterialsScriptRecast.StopCoroutine (absorbableMaterialsScriptRecast.absorbableMaterialCoroutine);

				}

			} else
				Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 1, MATERIALE ASSORBIBILE");
			
		}
		
	};
	#endregion


	#region ABSORBABLE_MATERIALS_MONOBEHAVIOUR_METHODS
	public void Awake () {

		if (this.absorbedMaterialReference == null)
			this.absorbedMaterialReference = new AbsorbableMaterialsSaveAndLoadData ();

	}

	public virtual void Start () {

		if (this.absorbedMaterialReference.materialIsAbsorbed) {
			
			AbsorbableMaterialsScript.characterHasAbsorbedOneMaterial = true;
			this.characterHasAbsorbedMaterial = true;

		} else
			this.characterHasAbsorbedMaterial = false;
		
		this.characterIsAbsorbingMaterial = false;
		this.characterIsReleasingMaterial = false;

	}

	public virtual void Update () {

		//Se nessun materiale è stato assorbito && esso non è in corso di assorbimento, valutazione Input di solo assorbimento
		if (!AbsorbableMaterialsScript.characterHasAbsorbedOneMaterial && !this.characterIsAbsorbingMaterial) {

			if (Input.GetKeyDown (KeyCode.K)) {

				this.characterIsAbsorbingMaterial = true;
				this.absorbableMaterialCoroutine = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.absorbingMaterialAbsorbingSpeed, this.absorbingMaterialAbsorbingAmount, this.DelegatedMethod [0]));

			}

			//Altrimenti, se il materiale specifco è stato assorbito && esso non è in corso di rilascio, valutazione Input di solo rilascio
		} else if (this.characterHasAbsorbedMaterial && !this.characterIsReleasingMaterial) {

			if (Input.GetKeyDown (KeyCode.L)) {

				this.characterIsReleasingMaterial = true;
				this.absorbableMaterialCoroutine = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.absorbingMaterialAbsorbingSpeed, this.absorbingMaterialAbsorbingAmount, this.DelegatedMethod [1]));

			}
				
		}

	}
	#endregion

}