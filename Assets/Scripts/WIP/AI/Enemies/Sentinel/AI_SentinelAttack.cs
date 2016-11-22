using UnityEngine;
using System.Collections;

public class AI_SentinelAttack : MonoBehaviour {

	#region SENTINEL_ATTACK_PARAMETERS
	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH!")]
	public float angle;


	[Header ("Structs")]

	[Tooltip ("DO NOT TOUCH!")]
	public Vector3 distance;


	[Header ("Scripts")]

	[Tooltip ("DO NOT TOUCH!")]
	public AI_SentinelComponent sentinelComponents;
	#endregion


	#region SENTINEL_ATTACK_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.sentinelComponents = this.GetComponentInParent <AI_SentinelComponent> ();

	}


	public void Update () {

		if (this.sentinelComponents.inputCheckingCoroutine != null) {

			this.distance = this.sentinelComponents.player.transform.position - this.sentinelComponents.transform.position;
			this.angle = Vector3.Angle (this.distance, this.sentinelComponents.transform.forward);

			if (this.angle < this.sentinelComponents.fieldOfViewAngle * 0.5f) {

				if (this.distance.sqrMagnitude < Mathf.Pow (this.sentinelComponents.attackDistance, 2f)) {

					this.sentinelComponents.StopAgent ();

				} else if (this.sentinelComponents.agentHasBeenStopped)
					this.sentinelComponents.ResumeAgent ();

				if (this.sentinelComponents.playerInSight) {

					Debug.LogWarning ("Shooting!");

				}

			}

		}

	}
	#endregion

}