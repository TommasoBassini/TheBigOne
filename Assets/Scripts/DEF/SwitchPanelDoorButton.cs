using UnityEngine;
using System.Collections;

//Da mettere in ogni button per le porte del terminale di switch energia
public class SwitchPanelDoorButton : MonoBehaviour
{
    [Tooltip("Aggiungere il gameobject della porta")]
    public DoorBaseScript door;
    [Tooltip("Aggiungere il gameobject dove c'è lo script SwitchEnergyTerminal del terminale")]
    public GameObject terminalMain;
    private SwitchEnergyTerminal terminalSwitch;

    //indice del bool della porta
    private int nInList = 0;

    void Start()
    {
        //ricerca lo script del delegato
        terminalSwitch = terminalMain.GetComponent<SwitchEnergyTerminal>();
        terminalSwitch.terminalSwitch += DoorOn_Off;
        //bool fasullo da aggiungere alla lista di bool 
        bool door = false;
        terminalSwitch.isDoors.Add(door);
        //Indice del button 
        nInList = terminalSwitch.isDoors.Count -1;
    }

    //Metodo che viene chiamato alla pressione del button 
    public void SwitchDoorButton()
    {
        //Resetta i bool del terminale e mette true quello voluto e poi chiama il delegato
        terminalSwitch.ResetAllBool();
        terminalSwitch.isDoors[nInList] = true;
        terminalSwitch.terminalSwitch();
    }

    // metodo chiamato dal delegato
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
