using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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

    public List<Panels> orderOfLastPanel = new List<Panels>();

    void Start()
    {
        orderOfLastPanel.Add(panels[0]);
    }

    public IEnumerator FillImageOverTime(int n, int valueDiff)
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

    public IEnumerator BlockTerminal(float nSecondi)
    {
        Button[] terminalButtons = this.gameObject.GetComponentsInChildren<Button>();
        GameObject selectedButton = FindObjectOfType<EventSystem>().currentSelectedGameObject; ;
        foreach (var button in terminalButtons)
        {
            button.interactable = false;
        }

        yield return new WaitForSeconds(nSecondi);

        foreach (var button in terminalButtons)
        {
            button.interactable = true;
        }

        if (selectedButton != null)
        {
            selectedButton.GetComponent<Button>().Select();
        }
    }
}
