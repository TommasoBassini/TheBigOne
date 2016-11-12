using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

}
