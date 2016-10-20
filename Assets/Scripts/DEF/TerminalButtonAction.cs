using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TerminalButtonAction : MonoBehaviour
{

    public void UnlockDoor (DoorBaseScript door)
    {
        Debug.Log("Porta Sbloccata");
        door.doorIsUnLocked = true;
        door.UnlockDoor();
    }

    public void ChangePanel(GameObject panelToShow)
    {
        SelectCanvasButton canvas = GetComponent<SelectCanvasButton>();
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
