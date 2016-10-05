﻿using UnityEngine;
using System.Collections;

public abstract class AbsorbableMaterialsScript : TimerScript {

	#region ABSORBABLE_MATERIALS_PARAMETERS
	public static bool characterHasAbsorbedOneMaterial = false;

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
	#endregion


	#region ABSORBABLE_MATERIALS_PROPERTIES
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
					
					absorbableMaterialsScriptRecast.characterHasAbsorbedMaterial = true;
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

					absorbableMaterialsScriptRecast.characterHasAbsorbedMaterial = false;
					absorbableMaterialsScriptRecast.characterIsReleasingMaterial = false;
					absorbableMaterialsScriptRecast.AbsorbingMaterialCurrentlyElapsed = 0f;

					AbsorbableMaterialsScript.characterHasAbsorbedOneMaterial = false;

					absorbableMaterialsScriptRecast.StopCoroutine (absorbableMaterialsScriptRecast.absorbableMaterialCoroutine);

				}

			} else
				Debug.LogError ("ERRORE RICONOSCIMENTO TIPO SCRIPT, DELEGATO 1, MATERIALE ASSORBIBILE");
			
		}
		
	};
	#endregion


	#region ABSORBABLE_MATERIALS_MONOBEHAVIOUR_METHODS
	public virtual void Start () {

		this.characterHasAbsorbedMaterial = false;
		this.characterIsAbsorbingMaterial = false;
		this.characterIsReleasingMaterial = false;

	}

	public virtual void Update () {

		if (!AbsorbableMaterialsScript.characterHasAbsorbedOneMaterial && !this.characterHasAbsorbedMaterial && !this.characterIsAbsorbingMaterial) {

			if (Input.GetKeyDown (KeyCode.K)) {

				AbsorbableMaterialsScript.characterHasAbsorbedOneMaterial = true;

				this.characterIsAbsorbingMaterial = true;
				this.absorbableMaterialCoroutine = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.absorbingMaterialAbsorbingSpeed, this.absorbingMaterialAbsorbingAmount, this.DelegatedMethod [0]));

			}

		} else if (this.characterHasAbsorbedMaterial && !this.characterIsReleasingMaterial) {

			if (Input.GetKeyDown (KeyCode.L)) {

				this.characterIsReleasingMaterial = true;
				this.absorbableMaterialCoroutine = this.StartCoroutine_Auto (this.CO_TimerCoroutine (this.absorbingMaterialAbsorbingSpeed, this.absorbingMaterialAbsorbingAmount, this.DelegatedMethod [1]));

			}
				
		}

	}
	#endregion

}