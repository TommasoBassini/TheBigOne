using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PODReportButton : MonoBehaviour
{

    public TextAsset text;
    public string Titolo;
    public Text textBox;
    public Scrollbar scrool;
    public RectTransform content;
    public Button buttonPlayAudio;
    public GameObject panelReports;
    public GameObject panelReport;
    public bool isUnlocked = false;



    public void ShowReport()
    {
        if (isUnlocked)
        {
            panelReports.SetActive(false);
            panelReport.SetActive(true);

            textBox.text = text.text;
            content.sizeDelta = new Vector2(11.23f, textBox.preferredHeight + 10);
            scrool.value = 1;
            buttonPlayAudio.Select();
        }
    }

    //Metodo che viene chiamato in UpdateSelected per rilevare lo scrool del testo
    public void DetectScroll()
    {
        float angV = Input.GetAxis("Vertical");
        scrool.value += angV * Time.deltaTime;
    }
}
