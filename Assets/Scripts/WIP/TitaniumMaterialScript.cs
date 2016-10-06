using UnityEngine;
using System.Collections;

public class TitaniumMaterialScript : AbsorbableMaterialsScript {

	#region TITANIUM_MATERIAL_PARAMETERS
	[Header ("Flag Booleani del Titanio")]
	public bool characterIsNearTitaniumCase;
	public bool characterIsNearTitaniumMendable;
	#endregion


	#region TITANIUM_MATERIAL_MONOBEHAVIOUR_METHODS
	public override void Start () {

		base.Start ();
		this.characterIsNearTitaniumCase = false;
		this.characterIsNearTitaniumMendable = false;

	}

	public override void Update () {

		//Due IF di Debug; simulo la vicinanza al contenitore od all'oggetto riparabile
		//Sarebbero da porsi delle condizioni (suppongo IF-ESLE) in merito alla vicinanza fisica rispetto al personaggio
		if (Input.GetKeyDown (KeyCode.O))
			this.characterIsNearTitaniumCase = !this.characterIsNearTitaniumCase;

		if (Input.GetKeyDown (KeyCode.P))
			this.characterIsNearTitaniumMendable = !this.characterIsNearTitaniumMendable;

		//Se nessun materiale è stato assorbito && si è vicini ad un contenitore di Titanio, valutazione condizioni del padre
		//Altrimenti, se almeno un materiale è stato assorbito && si è vicini ad un oggetto riparabile in Titanio, valutazione condizioni del padre
		if (!TitaniumMaterialScript.characterHasAbsorbedOneMaterial) {

			if (this.characterIsNearTitaniumCase)
				base.Update ();

		} else {

			if (this.characterIsNearTitaniumMendable)
				base.Update ();

		}

	}
	#endregion

}