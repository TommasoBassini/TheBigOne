using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;

public class MenuControl : MonoBehaviour
{
    public bool isMenu = false;
    public GameObject pnlMenu;
    public GameObject[] panels;
    public GameObject panelShowScan;
    public GameObject panelShowReport;
    public Button[] buttons;
    public int nMenu = 0;
    public int nButtonReport = 0;
    public bool isSubMenu = false;
    public bool isShowReport = false;
    public bool isShowScan = false;
    public GameObject reportsPanel;
    private List<GameObject> reportButtons = new List<GameObject>();

    private ObjectInteract player;

    public GameObject mirino;
    public GameObject scansPanel;
    public GameObject objActive;
    public Button scanButton;

    void Start()
    {
        foreach (Transform item in reportsPanel.transform)
        {
            reportButtons.Add(item.gameObject);
        }
        player = FindObjectOfType<ObjectInteract>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (!player.isInteracting)
            {
                if (!isMenu)
                {
                    isMenu = true;
                    FindObjectOfType<FirstPersonController>().enabled = false;
                    mirino.SetActive(false);
                    pnlMenu.SetActive(true);
                    panels[0].SetActive(true);
                    buttons[1].Select();
                    buttons[0].Select();
                    return;
                }
                else
                {
                    isMenu = false;
                    FindObjectOfType<FirstPersonController>().enabled = true;
                    mirino.SetActive(true);
                    pnlMenu.SetActive(false);
                    panels[nMenu].SetActive(false);
                    nMenu = 0;
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1) && isMenu)
        {
            if (isSubMenu)
            {
                if (isShowReport)
                {
                    isShowReport = false;
                    panelShowReport.SetActive(false);
                    panels[nMenu].SetActive(true);
                    reportButtons[nButtonReport].GetComponent<Button>().Select();
                    return;
                }

                if (isShowScan)
                {
                    ObjectInteract player = FindObjectOfType<ObjectInteract>();
                    player.isInspecting = false;
                    player.isInteracting = false;
                    isShowScan = false;
                    panelShowScan.SetActive(false);
                    panels[nMenu].SetActive(true);
                    scanButton.Select();
                    Destroy(objActive);
                    return;
                }

                if (!isShowReport && !isShowScan)
                {
                    buttons[nMenu].Select();
                    isSubMenu = false;
                    return;
                }
                return;
            }
            else
            {
                isMenu = false;
                FindObjectOfType<FirstPersonController>().enabled = true;
                mirino.SetActive(true);
                pnlMenu.SetActive(false);
                panels[nMenu].SetActive(false);
                nMenu = 0;
                return;
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
                podReportButton.titolo = ReportManager.reportState[i].titolo;
                podReportButton.autore = ReportManager.reportState[i].autore;
                podReportButton.luogo = ReportManager.reportState[i].luogo;
                podReportButton.text = ReportManager.reportState[i].text;
                podReportButton.isUnlocked = true;
            }
        }
    }

    public void SelectScanButton()
    {
        if (scansPanel.transform.GetChild(0) != null)
        {
            isSubMenu = true;
            Button buttonToSelect = scansPanel.transform.GetChild(0).GetComponent<Button>();
            buttonToSelect.Select();
        }
    }

    public void CheckPodStatus()
    {

        PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
        //Controllo livello medico
        switch (playerStatus.medicLvl)
        {
            case 1:
                {
                    this.transform.Find("pnlStatus/pnlPermissions/pnlMedicPermissions/txtPermission").GetComponent<Text>().color = Color.green;
                    this.transform.Find("pnlStatus/pnlPermissions/pnlMedicPermissions/txtPermission1").GetComponent<Text>().color = Color.green;
                    break;
                }
            case 2:
                {
                    this.transform.Find("pnlStatus/pnlPermissions/pnlMedicPermissions/txtPermission2").GetComponent<Text>().color = Color.green;
                    break;
                }
        }

        switch (playerStatus.engineerLvl)
        {
            case 1:
                {
                    this.transform.Find("pnlStatus/pnlPermissions/pnlEngineerPermissions/txtPermission").GetComponent<Text>().color = Color.green;
                    this.transform.Find("pnlStatus/pnlPermissions/pnlEngineerPermissions/txtPermission1").GetComponent<Text>().color = Color.green;
                    break;
                }
            case 2:
                {
                    this.transform.Find("pnlStatus/pnlPermissions/pnlEngineerPermissions/txtPermission2").GetComponent<Text>().color = Color.green;
                    break;
                }
        }

        switch (playerStatus.guardLvl)
        {
            case 1:
                {
                    this.transform.Find("pnlStatus/pnlPermissions/pnlSecurityPermissions/txtPermission").GetComponent<Text>().color = Color.green;
                    this.transform.Find("pnlStatus/pnlPermissions/pnlSecurityPermissions/txtPermission1").GetComponent<Text>().color = Color.green;
                    break;
                }
            case 2:
                {
                    this.transform.Find("pnlStatus/pnlPermissions/pnlSecurityPermissions/txtPermission2").GetComponent<Text>().color = Color.green;
                    break;
                }
        }
        this.transform.Find("pnlStatus/pnlPermissions/pnlMedicPermissions/txtMedic").GetComponent<Text>().text = "Medico: " + playerStatus.medicLvl.ToString();
        this.transform.Find("pnlStatus/pnlPermissions/pnlEngineerPermissions/txtEngineer").GetComponent<Text>().text = "Ingegnere: " + playerStatus.medicLvl.ToString();
        this.transform.Find("pnlStatus/pnlPermissions/pnlSecurityPermissions/txtSecurity").GetComponent<Text>().text = "Guardia: " + playerStatus.medicLvl.ToString();

    }
}
