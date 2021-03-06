﻿using UnityEngine;
using System.Collections;

public class SwitchPanelDoorButton : MonoBehaviour
{
    [Tooltip("Aggiungere il gameobject della porta")]
    public DoorBaseScript door;
    [Tooltip("Aggiungere il gameobject dove c'è lo script SwitchEnergyTerminal del terminale")]
    public GameObject terminalMain;
    private SwitchEnergyTerminal terminalSwitch;

    
    private int nInList = 0;

    void Start()
    {
        terminalSwitch = terminalMain.GetComponent<SwitchEnergyTerminal>();
        terminalSwitch.terminalSwitch += DoorOn_Off;
        bool door = false;
        terminalSwitch.isDoors.Add(door);
        nInList = terminalSwitch.isDoors.Count -1;
    }

    public void SwitchDoorButton()
    {
        terminalSwitch.ResetAllBool();
        terminalSwitch.isDoors[nInList] = true;
        terminalSwitch.terminalSwitch();
    }

    void DoorOn_Off()
    {
        switch (terminalSwitch.isDoors[nInList])
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
