using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReportButton : MonoBehaviour
{
    public Text textBox;
    public Scrollbar scrool;
    public RectTransform content;

    public int indexInList;
    public bool discovered = false;

    [Header("Per i designers")]
    [Tooltip("Mettere qui il txt del testo del report")]
    public TextAsset text;
    [Tooltip("Scrivere qui il titolo del report")]
    public string titolo;
    [Tooltip("Scrivere qui l'autore del report")]
    public string autore;
    [Tooltip("Scrivere qui il luogo del ritrovamento del report")]
    public string luogo;


    void Start()
    {
        ReportManager.reportState.Add(this);
    }

    public void ShowText()
    {
        if (!discovered)
        {
            discovered = true;
        }

        scrool.value = 1;
        textBox.text = text.text;
        content.sizeDelta = new Vector2(11.23f, textBox.preferredHeight + 10);
    }

    //Metodo che viene chiamato in UpdateSelected per rilevare lo scrool del testo
    public void DetectScroll()
    {
        float angV = Input.GetAxis("RightV");
        scrool.value -= angV * Time.deltaTime;
    }
}
