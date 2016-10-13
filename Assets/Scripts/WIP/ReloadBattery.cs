using UnityEngine;
using System.Collections;

public class ReloadBattery : ActionObj
{
    public override void DoStuff()
    {
        FindObjectOfType<BatteryScript>().FullBattery();
    }

}
