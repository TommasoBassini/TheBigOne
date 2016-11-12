using UnityEngine;
using System.Collections;

//Classe dell'oggetto che ricarica l'ossigeno
public class ReloadOxygen : ActionObj
{
    public override void DoStuff()
    {
        FindObjectOfType<OxygenScript>().FullOxygen();
    }
}
