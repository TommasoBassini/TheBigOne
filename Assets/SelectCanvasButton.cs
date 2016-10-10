using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectCanvasButton : MonoBehaviour
{
    public Button firstSelected;
    public GameObject activePanel;
    public GameObject firstActivePanel;

    public GameObject[] panels;

    public void ResetCanvas()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
        firstActivePanel.SetActive(true);
        activePanel = firstActivePanel;
    }
}
