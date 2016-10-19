using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchEnergyTerminal : MonoBehaviour
{
    public delegate void De_TerminalSwitch();
    public De_TerminalSwitch terminalSwitch;


    public bool isLight;
    public List<bool> isDoors = new List<bool>();


    public void ResetAllBool()
    {
        isLight = false;

        for (int i = 0; i < isDoors.Count; i++)
        {
            isDoors[i] = false;
        }
    }

}
