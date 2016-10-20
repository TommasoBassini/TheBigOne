using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReportButton : MonoBehaviour
{
    public TextAsset text;
    public Text textBox;
    public string Titolo;
    public Scrollbar scrool;
    public RectTransform content;

    public int indexInList;
    public bool discovered = false;

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
