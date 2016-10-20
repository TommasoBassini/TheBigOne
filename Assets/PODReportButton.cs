using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PODReportButton : MonoBehaviour
{

    public TextAsset text;
    public Text textBox;
    public string Titolo;
    public Scrollbar scrool;
    public RectTransform content;

    public GameObject panelReports;
    public GameObject panelReport;

    public void ShowReport()
    {
        panelReports.SetActive(false);
        panelReport.SetActive(true);

        textBox.text = text.text;
    }
}
