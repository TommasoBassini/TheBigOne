using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Classe da mettere dentro il main panel di un terminale
public class TerminalStatus : MonoBehaviour
{
    // variabili che settano lo stato iniziale del terminale
    public Button firstSelected;
    public GameObject activePanel;
    public GameObject firstActivePanel;

    public GameObject[] panels;

    public bool resetTerminalOnExit;
    //Methodo del terminale che resetta lo stato del terminale quando si abbandona il terminale
    public void ResetCanvas()
    {
        if (resetTerminalOnExit)
        {
            foreach (var panel in panels)
            {
                panel.SetActive(false);
            }
            firstActivePanel.SetActive(true);
            activePanel = firstActivePanel;
        }
    }
}
