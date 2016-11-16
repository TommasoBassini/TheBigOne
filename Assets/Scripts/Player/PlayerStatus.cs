using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public int maxLife;
    public int life;

    public AbrsorbType storageMaterial;

    public float timeForRestore;
    public float speedOfRestore;

    public Image damageEffectImage;

    void Start()
    {
        life = maxLife;
    }

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

    public void ReceiveDamege(int damage)
    {
        StopCoroutine("RestoreLife");
        life -= damage;
        Debug.Log(life);
        if (life <= 0)
        {
            Debug.Log("Morto");
            //Mettere qui logica per morte
        }
        else
        {
            StartCoroutine("RestoreLife");
        }

        if (life <= (50))
        {
            StartCoroutine(DamageEffect(life));

        }
    }
    
    private IEnumerator DamageEffect(int life)
    {
        float elapsedTime = 0.0f;
        float startAlpha = damageEffectImage.color.a;
        float step = 200 /50;

        float finalAlpha = (200 - (step * life))/255;
        while (elapsedTime < 0.3f)
        {
            damageEffectImage.color = new Color(damageEffectImage.color.r, damageEffectImage.color.g, damageEffectImage.color.b, Mathf.Lerp(startAlpha, finalAlpha, (elapsedTime / 0.3f)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RestoreLife()
    {
        yield return new WaitForSeconds(timeForRestore);
        float elapsedTime = 0.0f;
        float startLife = life;
        while (elapsedTime < speedOfRestore)
        {
            life = Mathf.RoundToInt(Mathf.Lerp(startLife, maxLife, (elapsedTime / speedOfRestore)));
            if(life<50)
                StartCoroutine(DamageEffect(life));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
