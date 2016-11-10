using UnityEngine;
using System.Collections;

public class DoorButton : ActionObj
{
    public DoorBaseScript door;
    private bool isInteractable = true;

    public override void DoStuff()
    {
        if (door.doorIsUnLocked && isInteractable)
        {
            isInteractable = false;
            Invoke("EnableInteraction", 1);
            if (!door.doorIsOpen)
            {
                Debug.Log("Ho aperto la porta perchè è sbloccata");
                door.doorIsOpen = true;
                Animator anim = door.gameObject.GetComponent<Animator>();
                anim.SetTrigger("Apri");
                door.UpdateButtons();
                return;
            }
            else
            {
                Debug.Log("Ho aperto la porta perchè è sbloccata");
                door.doorIsOpen = false;
                Animator anim = door.gameObject.GetComponent<Animator>();
                anim.SetTrigger("Chiudi");
                door.UpdateButtons();
                return;
            }
        }
    }

    void EnableInteraction()
    {
        isInteractable = true;
    }
}
