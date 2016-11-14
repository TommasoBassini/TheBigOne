using UnityEngine;
using System.Collections;

public class AI_TurretScanner : MonoBehaviour {

	#region TURRET_SCANNER_PARAMETERS
	[Header ("Boolean Flags")]

	[Tooltip ("DO NOT TOUCH!")]
	public bool isScanningForward;


	[Header ("Structs")]

	[Tooltip ("DO NOT TOUCH!")]
	public Vector3 distance;


	[Header ("Classes")]

	[Tooltip ("DO NOT TOUCH!")]
	public AI_TurretComponent turretComponents;
	#endregion


	#region TURRET_SCANNER_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.turretComponents = this.GetComponentInParent <AI_TurretComponent> ();

	}


	public void Start () {

		this.BooleanCleaning ();
		this.ResetScanner ();

	}


	public void Update () {

		if (!this.turretComponents.enemyHasBeenStunned) {
		
			if (this.isScanningForward) {
			
				this.distance = this.turretComponents.points [1].position - this.transform.position;
			
				if (this.distance.sqrMagnitude < 0.01f)
					this.isScanningForward = false;
				else
					this.transform.Translate (this.distance.normalized * Time.deltaTime);
			
			} else {
			
				this.distance = this.turretComponents.points [0].position - this.transform.position;
			
				if (this.distance.sqrMagnitude < 0.01f)
					this.isScanningForward = true;
				else
					this.transform.Translate (this.distance.normalized * Time.deltaTime);
			
			}

		}
		
	}


	public void OnTriggerEnter (Collider other) {

		if (!this.turretComponents.enemyHasBeenStunned) {

			if (other.gameObject == this.turretComponents.player) {

				this.turretComponents.playerHasBeenDetected = true;
				this.transform.localPosition = Vector3.zero;
				this.EnableTurretScanner (false);

			}

		}

	}
	#endregion


	#region TURRET_SCANNER_METHODS
	public void EnableTurretScanner (bool state) {

		if (state) {
			
			this.BooleanCleaning ();
			this.ResetScanner ();

		}
		
		this.enabled = state;

	}


	public void BooleanCleaning () {

		this.isScanningForward = true;

	}


	public void ResetScanner () {

		if (this.turretComponents.points.Length == 0) {

			Debug.LogWarning ("WARNING! " + this.ToString() + " Has the transform's array length equal to zero!");
			return;

		}

		this.transform.position = this.turretComponents.points [0].position;
			
	}
	#endregion

}