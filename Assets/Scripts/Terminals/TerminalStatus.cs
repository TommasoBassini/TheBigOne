using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public struct Panels
{
    public GameObject panel;
    public Button firstSelectButtonInPanel;
}

//Classe da mettere dentro il main panel di un terminale
public class TerminalStatus : MonoBehaviour
{
    public GameObject activePanel;
    public Panels[] panels;

    public ImagesToFillEnergy[] imagesToFill;


    public IEnumerator FillImageOverTime(int n,int valueDiff)
    {
        float time = imagesToFill[n].timerToFill;
        float finishValue = imagesToFill[n].imageToChange.fillAmount + ((float)valueDiff / 100);
        float elapsedTime = 0.0f;
        float startFill = imagesToFill[n].imageToChange.fillAmount;
        while (elapsedTime < time)
        {
            imagesToFill[n].imageToChange.fillAmount = Mathf.Lerp(startFill, finishValue, (elapsedTime / time));
            imagesToFill[n].textValue.text = imagesToFill[n].valuePrefix + (imagesToFill[n].imageToChange.fillAmount * 100).ToString("0") + imagesToFill[n].valueSuffix;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
