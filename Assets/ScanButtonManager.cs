using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScanButtonManager : MonoBehaviour
{
    public int colonna = 0;
    public int righe = 0;
    public List<GameObject[]> scans = new List<GameObject[]>();
   
    public GameObject buttonPrefab;

    void Start()
    {
        scans.Add(new GameObject[6]);
    }

    void Update()
    {

    }

    public void SetNewButton()
    {
        if (scans[righe].Length < 6)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(this.transform);
            scans[righe][colonna] = buttonPrefab;
            colonna++;
        }
        else 
        {

            scans.Add(new GameObject[6]);
            righe++;
        }
    }


}
