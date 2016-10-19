using UnityEngine;
using System.Collections;

[System.Serializable]
class Permessi
{
    [Header ("Permessi medici")]
    public bool isScannerMedic = false;
    public bool isAemOssigeno = false;
    public bool isIemMedico = false;
}

public class PlayerStatus : MonoBehaviour
{
    public int medicLvl;
    public int engineerLvl;
    public int guardLvl;



    public void MedicLvlUp()
    {
        medicLvl++;
        switch (medicLvl)
        {
            default:
                break;
        }
    }
}
