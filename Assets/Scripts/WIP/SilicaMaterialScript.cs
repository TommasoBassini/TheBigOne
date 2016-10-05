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

		if (Input.GetKeyDown (KeyCode.U))
			this.characterIsNearSilicaCase = !this.characterIsNearSilicaCase;

		if (Input.GetKeyDown (KeyCode.I))
			this.characterIsNearSilicaMendable = !this.characterIsNearSilicaMendable;

		if (!SilicaMaterialScript.characterHasAbsorbedOneMaterial && this.characterIsNearSilicaCase)
			base.Update ();
		else if (SilicaMaterialScript.characterHasAbsorbedOneMaterial && this.characterIsNearSilicaMendable)
			base.Update ();

	}
	#endregion

}