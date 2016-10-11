using UnityEngine;
using System.Collections;

public class SwitchPanelDoorButton : MonoBehaviour
{
    [Tooltip("Aggiungere il gameobject della porta")]
    public DoorBaseScript door;
    [Tooltip("Aggiungere il gameobject dove c'è lo script SwitchEnergyTerminal del terminale")]
    public GameObject terminalMain;
    private SwitchEnergyTerminal terminalSwitch;

    void Start()
    {
        terminalSwitch = terminalMain.GetComponent<SwitchEnergyTerminal>();
        terminalSwitch.terminalSwitch += DoorOn_Off;
    }

    public void SwitchDoorButton()
    {
        terminalSwitch.ResetAllBool();
        terminalSwitch.isDoor = true;
        terminalSwitch.terminalSwitch();
    }

    void DoorOn_Off()
    {
        switch (terminalSwitch.isDoor)
        {
            case (true):
                {
                    door.doorIsUnLocked = true;
                    door.UnlockDoor();
                    break;
                }
            case (false):
                {
                    door.doorIsUnLocked = false;
                    door.UnlockDoor();
                    break;
                }
        }
    }
}
