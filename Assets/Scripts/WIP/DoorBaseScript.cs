using UnityEngine;
using System.Collections;

public class DoorBaseScript : MonoBehaviour {

	#region DOOR_BASE_SUBCLASS
	public class DoorBaseSaveAndLoadData {

		public bool[] doorStatuses;

	}
	#endregion


	#region DOOR_BASE_PARAMETERS
	[Header ("Flag Booleani")]
	public bool doorIsPowered;
	public bool doorIsUnLocked;
	public bool doorIsWorking;
	public bool doorIsOpen;

	public DoorBaseSaveAndLoadData doorBaseReference;
	#endregion


	#region DOOR_BASE_PROPERTIES
	public bool[] DoorStatuses {

		set {

			this.doorBaseReference.doorStatuses = value;

		}

		get {

			return this.doorBaseReference.doorStatuses;

		}

	}
	#endregion


	#region DOOR_BASE_MONOBEHAVIOUR_METHODS
	public void Awake () {

		if (this.doorBaseReference == null)
			this.doorBaseReference = new DoorBaseSaveAndLoadData ();

	}

	public void Start () {

		if (this.DoorStatuses == null)
			this.DoorStatuses = new bool [] {
				this.doorIsOpen,				//0
				this.doorIsWorking,				//1
				this.doorIsUnLocked,			//2
				this.doorIsPowered				//3
			};

	}

	public void Update () {

		this.doorIsPowered = this.DoorStatuses [3];
		this.doorIsUnLocked = this.DoorStatuses [2];
		this.doorIsWorking = this.DoorStatuses [1];
		this.doorIsOpen = this.DoorStatuses [0];

		if (Input.GetKeyDown (KeyCode.Alpha1))
			this.DoorStatuses [3] = !this.DoorStatuses [3];

		if (Input.GetKeyDown (KeyCode.Alpha2))
			this.DoorStatuses [2] = !this.DoorStatuses [2];

		if (Input.GetKeyDown (KeyCode.Alpha3))
			this.DoorStatuses [1] = !this.DoorStatuses [1];


		if (this.DoorStatuses [3]) {

			if (this.DoorStatuses [2]) {

				if (this.DoorStatuses [1]) {
					
					if (Input.GetKeyDown (KeyCode.Alpha4))
						this.DoorStatuses [0] = !this.DoorStatuses [0];

					if (this.DoorStatuses [0]) {

						Debug.Log ("Sono aperta");

					} else {

						Debug.Log ("Non sono aperta");

					}

				} else {

					Debug.Log ("Non sono funzionante");

				}

			} else {

				Debug.Log ("Non sono sbloccata");

			}

		} else {

			Debug.Log ("Non sono alimentata");

		}

	}
	#endregion

}