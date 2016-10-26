using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;

public class MenuControl : MonoBehaviour
{
    private bool isMenu = false;
    public GameObject pnlMenu;
    public GameObject[] panels;
    public Button[] buttons;
    public int nMenu = 0;
    public bool isSubMenu = false;

    public GameObject reportsPanel;
    private List<GameObject> reportButtons = new List<GameObject>();

    void Start()
    {
        foreach (Transform item in reportsPanel.transform)
        {
            reportButtons.Add(item.gameObject);
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (!isMenu)
            {
                isMenu = true;
                FindObjectOfType<FirstPersonController>().enabled = false;
                pnlMenu.SetActive(true);
                panels[0].SetActive(true);
                buttons[0].Select();
                return;
            }
            else
            {
                isMenu = false;
                FindObjectOfType<FirstPersonController>().enabled = true;
                pnlMenu.SetActive(false);
                panels[nMenu].SetActive(false);
                nMenu = 0;
                return;
            }
        }

        if (isMenu)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                if (isSubMenu)
                {
                    buttons[nMenu].Select();
                    isSubMenu = false;
                    return;
                }
                else
                {
                    isMenu = false;
                    FindObjectOfType<FirstPersonController>().enabled = true;
                    pnlMenu.SetActive(false);
                    panels[nMenu].SetActive(false);
                    nMenu = 0;
                    return;
                }
            }
        }
    }

    public void ChangePanel(int n)
    {
        panels[nMenu].SetActive(false);
        nMenu = n;
        panels[nMenu].SetActive(true);
    }

    public void SelectPanel(Button buttonToSelect)
    {
        isSubMenu = true;
        buttonToSelect.Select();
    }

    public void RefreshReport()
    {
        for (int i = 0; i < ReportManager.reportState.Count; i++)
        {
            if (ReportManager.reportState[i].discovered)
            {
                reportButtons[ReportManager.reportState[i].indexInList].GetComponentInChildren<Text>().text = ReportManager.reportState[i].titolo;
                PODReportButton podReportButton = reportButtons[ReportManager.reportState[i].indexInList].GetComponent<PODReportButton>();
                podReportButton.Titolo = ReportManager.reportState[i].titolo;
                podReportButton.text = ReportManager.reportState[i].text;
                podReportButton.isUnlocked = true;
            }
        }
    }
}
