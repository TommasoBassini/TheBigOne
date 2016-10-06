using UnityEngine;
using System.Collections;

public class SilicaMaterialScript : AbsorbableMaterialsScript {

	#region SILICA_MATERIAL_PARAMETERS
	[Header ("Flag Booleani della Silice")]
	public bool characterIsNearSilicaCase;
	public bool characterIsNearSilicaMendable;
	#endregion


	#region SILICA_MATERIAL_MONOBEHAVIOUR_METHODS
	public override void Start () {

		base.Start ();
		this.characterIsNearSilicaCase = false;
		this.characterIsNearSilicaMendable = false;

	}

	public override void Update () {

		//Due IF di Debug; simulo la vicinanza al contenitore od all'oggetto riparabile
		//Sarebbero da porsi delle condizioni (suppongo IF-ESLE) in merito alla vicinanza fisica rispetto al personaggio
		if (Input.GetKeyDown (KeyCode.U))
			this.characterIsNearSilicaCase = !this.characterIsNearSilicaCase;

		if (Input.GetKeyDown (KeyCode.I))
			this.characterIsNearSilicaMendable = !this.characterIsNearSilicaMendable;
		
		//Se nessun materiale è stato assorbito && si è vicini ad un contenitore di Silice, valutazione condizioni del padre
		//Altrimenti, se almeno un materiale è stato assorbito && si è vicini ad un oggetto riparabile in Silice, valutazione condizioni del padre
		if (!SilicaMaterialScript.characterHasAbsorbedOneMaterial) {

			if (this.characterIsNearSilicaCase)
				base.Update ();

		} else {

			if (this.characterIsNearSilicaMendable)
				base.Update ();

		}

	}
	#endregion

}