using UnityEngine;
using System.Collections;

public class AI_TurretComponent : MonoBehaviour {

	#region TURRET_PARAMETERS
	[Header ("Boolean Flags")]

	[Tooltip ("DO NOT TOUCH!")]
	public bool playerHasBeenDetected;
	[Tooltip ("DO NOT TOUCH!")]
	public bool playerInSight;							// Whether or not the player is currently sighted
	[Tooltip ("DO NOT TOUCH!")]
	public bool turretIsShoothing;


	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH! Ask programmers for utilization")]
	public int destPoint;

	[Tooltip ("Scanning time used to recognize the player - from 0.1f to 10f")]
	[Range (0.1f, 10f)] public float scanningTime = 2f;
	[Tooltip ("Waiting time used to return in guarding state - from 0.1f to 10f")]
	[Range (0.1f, 10f)] public float slipOutTime = 5f;


	[Header ("Classes")]

	[Tooltip ("DO NOT TOUCH!")]
	public LineRenderer attackRay;


	[Header ("Scripts")]

	[Tooltip ("DO NOT TOUCH!")]
	public AI_TurretScanner turretScanner;


	[Header ("GameObjects")]

	[Tooltip ("DO NOT TOUCH!")]
	public GameObject player;							// Reference to the player


	[Header ("Patrolling Points")]

	[Tooltip ("First, set the number; second, manually assign any GameObject desidered to be a patrolling point (Transforms will be automatically taken)")]
	public Transform[] points;
	#endregion


	#region TURRET_MONOBEHAVIOUR_METHODS
	public void Awake() {

		this.turretScanner = this.GetComponentInParent <AI_TurretArea> ().GetComponentInChildren <AI_TurretScanner> ();
		this.attackRay = this.GetComponentInChildren <LineRenderer> (true);
		this.player = GameObject.FindGameObjectWithTag ("Player");

	}


	public void Start () {

		this.playerHasBeenDetected = false;
		this.playerInSight = false;
		this.turretIsShoothing = false;

		this.destPoint = 0;

	}
	#endregion

}