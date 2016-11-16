using UnityEngine;
using System.Collections;

public class DoorButton : ActionObj
{
    public DoorBaseScript door;
    private bool isInteractable = true;

    public override void DoStuff()
    {
        if (door.isUnlocked && isInteractable)
        {
            isInteractable = false;
            Invoke("EnableInteraction", 1);
            door.OpenClosedDoor();
        }
    }

    void EnableInteraction()
    {
        isInteractable = true;
    }
}
