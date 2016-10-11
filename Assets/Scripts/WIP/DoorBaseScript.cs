using UnityEngine;
using System.Collections;

public class DoorBaseScript : MonoBehaviour
{

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

    public GameObject[] buttons;
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
	public void Awake ()
    {
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

	#endregion

    public void UnlockDoor()
    {
        UpdateButtons();
        doorIsUnLocked = true;
    }

    public void UpdateButtons()
    {
        foreach (var item in buttons)
        {
            MeshRenderer mr = item.GetComponent<MeshRenderer>();
            Material mat = mr.material;
            if (!doorIsUnLocked)
            {
                mat.SetColor("_Color", Color.red);
            }
            if (doorIsUnLocked)
            {
                mat.SetColor("_Color", Color.yellow);
            }
            if (doorIsUnLocked && doorIsOpen)
            {
                mat.SetColor("_Color", Color.green);
            }
        }
    }
}