using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TerminalButtonAction : MonoBehaviour
{
    public void ChangePanel(GameObject panelToShow)
    {
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
}
