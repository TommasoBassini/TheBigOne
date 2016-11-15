using UnityEngine;
using System.Collections;

public class SwitchPanelLightButton : MonoBehaviour
{
    [Tooltip("Aggiungere il gameobject dove ci sono le luci da spegnere/accendere")]
    public GameObject LightOnOff;
    private Light[] lights;

    [Tooltip("Aggiungere il gameobject dove c'è lo script SwitchEnergyTerminal del terminale")]
    public GameObject terminalMain;
    private SwitchEnergyTerminal terminalSwitch;

    void Start ()
    {
        lights = LightOnOff.GetComponentsInChildren<Light>();
        terminalSwitch = terminalMain.GetComponent<SwitchEnergyTerminal>();
        terminalSwitch.terminalSwitch += LightOn_Off;
    }

    public void SwitchLightButton()
    {
        terminalSwitch.ResetAllBool();
        terminalSwitch.isLight = true;
        terminalSwitch.terminalSwitch();
    }

    void LightOn_Off()
    {
        switch (terminalSwitch.isLight)
        {
            case (true):
                {
                    foreach (Light light in lights)
                    {
                        light.enabled = true;
                    }
                    break;
                }
            case (false):
                {
                    foreach (Light light in lights)
                    {
                        light.enabled = false;
                    }
                    break;
                }
        }
    }
}
