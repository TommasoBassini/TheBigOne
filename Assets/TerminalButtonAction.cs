using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TerminalButtonAction : MonoBehaviour
{

    public void UnlockDoor (DoorBaseScript door)
    {
        Debug.Log("Porta Sbloccata");
        door.doorIsUnLocked = true;
        door.UnlockDoor();
    }
}
