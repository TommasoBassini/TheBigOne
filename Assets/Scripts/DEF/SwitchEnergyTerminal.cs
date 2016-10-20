using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Da mettere nel pannello dello switch di energia
public class SwitchEnergyTerminal : MonoBehaviour
{
    // Delegato del terminale
    public delegate void De_TerminalSwitch();
    public De_TerminalSwitch terminalSwitch;


    public bool isLight;
    public List<bool> isDoors = new List<bool>();

    //Resetta tutti i bool 
    public void ResetAllBool()
    {
        isLight = false;

        for (int i = 0; i < isDoors.Count; i++)
        {
            isDoors[i] = false;
        }
    }

}
