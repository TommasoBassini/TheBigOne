using UnityEngine;
using System.Collections;

public class DoorBaseScript : MonoBehaviour
{
    public bool isOpen;
    public bool isUnlocked;
    public bool isPowered;

    public GameObject[] buttons;

    public void UpdateButtons()
    {
        foreach (var item in buttons)
        {
            MeshRenderer mr = item.GetComponent<MeshRenderer>();
            Material mat = mr.material;

            if (isPowered)
            {

                if (!isUnlocked)
                    mat.SetColor("_Color", Color.red);
                else
                {
                    if (isOpen)
                        mat.SetColor("_Color", Color.green);
                    else
                        mat.SetColor("_Color", Color.yellow);
                }
            }
            else
                mat.SetColor("_Color", Color.grey);
        }
    }

    public void InvertLockStatus()
    {
        if (isUnlocked && isOpen)
        {
            Debug.Log("hujgyudasvdfaskbhdsabddffdsfdsgvyjdfs");
            OpenClosedDoor();
        }
        isUnlocked = !isUnlocked;
        UpdateButtons();
    }

    public void InvertPowerStatus()
    {
        if (isPowered && isOpen)
        {
            OpenClosedDoor();
        }

        isPowered = !isPowered;
        UpdateButtons();
    }

    public void ForceLockStatus(bool status)
    {
        if (!status && isOpen)
        {
            OpenClosedDoor();
        }
        isUnlocked = status;
        UpdateButtons();
    }

    public void ForcePowerStatus(bool status)
    {
        if (!status && isOpen)
        {
            OpenClosedDoor();
        }
        isPowered = status;
        UpdateButtons();
    }

    public void OpenClosedDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetTrigger("Chiudi");
            UpdateButtons();
            return;
        }
        else
        {
            isOpen = true;
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetTrigger("Chiudi");
            UpdateButtons();
            return;
        }
    }
}