using UnityEngine;
using System.Collections;

public class AI_TurretComponent : MonoBehaviour {

	#region TURRET_SUBCLASSES
	public class Delegates {

		public EnemyStateDelegate <AI_TurretComponent> TurretAttackPostDelay = delegate (AI_TurretComponent turretComponentReference) {
			
			turretComponentReference.turretIsShoothing = true;
			turretComponentReference.attackCoroutine = turretComponentReference.KillPreviousCoroutine (turretComponentReference.attackCoroutine);
			
		};

		public EnemyStateDelegate <AI_TurretComponent> TurretSlipOutPostDelay = delegate (AI_TurretComponent turretComponentReference) {
			
			turretComponentReference.playerHasBeenDetected = false;
			turretComponentReference.turretScanner.EnableTurretScanner (true);	//Might be moved elsewhere
			turretComponentReference.slipOutCoroutine = turretComponentReference.KillPreviousCoroutine (turretComponentReference.slipOutCoroutine);
			
		};

		public CO_EnemyCoroutineDelegate <AI_TurretComponent> CO_TurretCoroutine;

	}
	#endregion


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


	[Header ("Structs")]

	[Tooltip ("DO NOT TOUCH!")]
	public Vector3 direction;
	[Tooltip ("DO NOT TOUCH!")]
	public RaycastHit hit;


	[Header ("Classes")]

	[Tooltip ("DO NOT TOUCH!")]
	public Coroutine attackCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public Coroutine slipOutCoroutine;
	[Tooltip ("DO NOT TOUCH!")]
	public Delegates delegates;
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

		this.delegates = new Delegates ();
		this.delegates.CO_TurretCoroutine = this.CO_TurretDelayedTime;

		this.attackRay = this.GetComponentInChildren <LineRenderer> (true);
		this.turretScanner = this.GetComponentInChildren <AI_TurretScanner> (true);
		this.player = GameObject.FindGameObjectWithTag ("Player");

	}


	public void Start () {

		this.playerHasBeenDetected = false;
		this.playerInSight = false;
		this.turretIsShoothing = false;

		this.destPoint = 0;

		this.attackCoroutine = this.KillPreviousCoroutine (this.attackCoroutine);
		this.slipOutCoroutine = this.KillPreviousCoroutine (this.slipOutCoroutine);

	}
	#endregion


	#region TURRET_METHODS
	public void SwitchCoroutine (out Coroutine stoppedCoroutine, out Coroutine initializedCoroutine, Coroutine firstCoroutine, Coroutine secondCoroutine,
		CO_EnemyCoroutineDelegate <AI_TurretComponent> CO_DelegatedMethod, float waitingTime, EnemyStateDelegate <AI_TurretComponent> DelegatedMethod) {
		
		stoppedCoroutine = this.KillPreviousCoroutine (firstCoroutine);
		
		if (secondCoroutine == null)
			initializedCoroutine = this.StartCoroutine_Auto (CO_DelegatedMethod (waitingTime, DelegatedMethod));
		else
			initializedCoroutine = secondCoroutine;
		
	}
	
	
	public Coroutine KillPreviousCoroutine (Coroutine coroutine) {

		if (coroutine != null)
			this.StopCoroutine (coroutine);
		
		return null;

	}
	#endregion


	#region TURRET_COROUTINE_METHODS
	public IEnumerator CO_TurretDelayedTime (float waitingTime, EnemyStateDelegate <AI_TurretComponent> DelegatedMethod) {
		
		yield return new WaitForSeconds (waitingTime);
		DelegatedMethod (this);

	}
	#endregion

}