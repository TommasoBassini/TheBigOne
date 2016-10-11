using UnityEngine;
using System.Collections;

public class ReloadOxygen : ActionObj
{
    public override void DoStuff()
    {
        FindObjectOfType<OxygenScript>().FullOxygen();
    }
}
