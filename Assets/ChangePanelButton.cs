using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public enum SecurityType
{
    medic,
    engineer,
    guard
}

public class ChangePanelButton : MonoBehaviour
{
    public SecurityType securityType;
    public int securityLvl;
    public GameObject panelToShow;

    public void ChangePanel(int n)
    {
        bool access = false;
        PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
        switch (securityType)
        {
            case SecurityType.medic:
                {
                    if (playerStatus.medicLvl >= securityLvl)
                    {
                        access = true;
                    }
                    break;
                }
            case SecurityType.engineer:
                {
                    if (playerStatus.engineerLvl >= securityLvl)
                    {
                        access = true;
                    }
                    break;
                }
            case SecurityType.guard:
                {
                    if (playerStatus.guardLvl >= securityLvl)
                    {
                        access = true;
                    }
                    break;
                }
            default:
                {
                    access = false;
                    break;
                }
        }

        if (access)
        {
            GetComponentInParent<TerminalFeedbacks>().feedbackEventPermission.hasPermission[n].Invoke();
            TerminalStatus canvas = GetComponent<TerminalStatus>();
            canvas.activePanel.SetActive(false);
            panelToShow.SetActive(true);
            canvas.activePanel = panelToShow;

            Button[] buttons = panelToShow.GetComponentsInChildren<Button>();

            if (buttons.Length > 0)
            {
                buttons[0].Select();
            }
        }
        else
        {
            GetComponentInParent<TerminalFeedbacks>().feedbackEventPermission.noPermission[n].Invoke();
            //Mettere qui il suono di errore e varie cose
        }
    }
}
