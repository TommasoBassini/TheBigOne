using UnityEngine;
using System.Collections;

[System.Serializable]
public class Permessi
{
    [Header ("Permessi medici")]
    public bool hasScannerMedic = false;
    public bool hasAemOssigeno = false;
    public bool hasIemMedico = false;

    [Header("Permessi ingegnere")]
    public bool hasScannerEngineer = false;
    public bool hasAemMaterial = false;
    public bool hasIemEngineer = false;

    [Header("Permessi Guardia")]
    public bool hasScanner = false;
    public bool hasIemSicurezza = false;
}

public class PlayerStatus : MonoBehaviour
{
    public int medicLvl;
    public int engineerLvl;
    public int guardLvl;
    public Permessi permessi;

    public int life;

    public AbrsorbType storageMaterial;

    public void MedicLvlUp(int n)
    {
        medicLvl = n;
        switch (medicLvl)
        {
            case 1:
                {
                    permessi.hasAemOssigeno = true;
                    permessi.hasScannerMedic = true;
                    break;
                }
            case 2:
                {
                    permessi.hasIemMedico = true;
                    break;
                }
        }
    }

    public void EngineerLvlUp(int n)
    {
        engineerLvl = n;
        switch (engineerLvl)
        {
            case 1:
                {
                    permessi.hasScannerEngineer = true;
                    permessi.hasIemSicurezza = true;
                    break;
                }
        }
    }

    public void GuardLvlUp(int n)
    {
        guardLvl = n;
        switch (guardLvl)
        {
            case 1:
                {
                    permessi.hasScanner = true;
                    permessi.hasAemMaterial = true;
                    break;
                }
            case 2:
                {
                    permessi.hasIemEngineer = true;
                    break;
                }
        }
    }
}
