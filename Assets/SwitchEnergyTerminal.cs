using UnityEngine;
using System.Collections;

public class SwitchEnergyTerminal : MonoBehaviour
{
    public delegate void De_TerminalSwitch();
    public De_TerminalSwitch terminalSwitch;


    public bool isLight;
    public bool isDoor;


    public void ResetAllBool()
    {
        isLight = false;
        isDoor = false;
    }
}
