using UnityEngine;
using System.Collections;

public class AI_DroneAttack : MonoBehaviour {

	#region DRONE_ATTACK_PARAMETERS
	[Header ("Variables")]

	[Tooltip ("DO NOT TOUCH!")]
	public float angle;


	[Header ("Structs")]

	[Tooltip ("DO NOT TOUCH!")]
	public Vector3 distance;


	[Header ("Scripts")]

	[Tooltip ("DO NOT TOUCH!")]
	public AI_DroneComponent droneComponents;
	#endregion


	#region DRONE_ATTACK_MONOBEHAVIOUR_METHODS
	public void Awake () {

		this.droneComponents = this.GetComponentInParent <AI_DroneComponent> ();

	}


	public void Update () {

		if (this.droneComponents.inputCheckingCoroutine != null) {

			this.distance = this.droneComponents.player.transform.position - this.droneComponents.transform.position;
			this.angle = Vector3.Angle (this.distance, this.droneComponents.transform.forward);

			if (this.angle < this.droneComponents.fieldOfViewAngle * 0.5f) {

				if (this.distance.sqrMagnitude < Mathf.Pow (this.droneComponents.attackDistance, 2f)) {

					this.droneComponents.StopAgent ();

				} else if (this.droneComponents.agentHasBeenStopped)
					this.droneComponents.ResumeAgent ();

				if (this.droneComponents.playerInSight) {

					Debug.LogWarning ("Shooting!");

				}

			}

		}

	}
	#endregion

}