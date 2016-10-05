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

		if (Input.GetKeyDown (KeyCode.O))
			this.characterIsNearTitaniumCase = !this.characterIsNearTitaniumCase;

		if (Input.GetKeyDown (KeyCode.P))
			this.characterIsNearTitaniumMendable = !this.characterIsNearTitaniumMendable;

		if (!TitaniumMaterialScript.characterHasAbsorbedOneMaterial && this.characterIsNearTitaniumCase)
			base.Update ();
		else if (TitaniumMaterialScript.characterHasAbsorbedOneMaterial && this.characterIsNearTitaniumMendable)
			base.Update ();

	}
	#endregion

}