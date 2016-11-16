using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnergyManage : MonoBehaviour
{

    public Light[] lights;
    public DoorBaseScript door;
    public GameObject luci;

    void Start()
    {
        lights = luci.GetComponentsInChildren<Light>();
        
    }

    public void Ligth()
    {
        //door.doorIsUnLocked = false;
        //door.UnlockDoor();
        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }

    public void UnlockDoor()
    {
        //door.doorIsUnLocked = true;
        //door.UnlockDoor();
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }
}
