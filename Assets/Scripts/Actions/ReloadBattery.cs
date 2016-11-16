using UnityEngine;
using System.Collections;

//Classe dell'oggetto che ricarica la batteria
public class ReloadBattery : ActionObj
{
    public override void DoStuff()
    {
        FindObjectOfType<BatteryScript>().FullBattery();
    }

}
