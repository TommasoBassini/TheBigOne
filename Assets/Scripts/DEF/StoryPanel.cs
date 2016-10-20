using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

//Da mettre dentro i terminali di storia
public class StoryPanel : MonoBehaviour
{
    //variabili del terminale storia
    public TextAsset[] texts;
    public Text textBox;
    public Scrollbar scrool;
    public RectTransform content;

    //Metodo che viene chiamato quando un button rileva di essere stato selezionato
    public void ShowText(int n)
    {
        scrool.value = 1;
        textBox.text = texts[n].text;
        content.sizeDelta = new Vector2(11.23f,textBox.preferredHeight +10);
    }

    //Metodo che viene chiamato in UpdateSelected per rilevare lo scrool del testo
    public void DetectScroll()
    {
        float angV = Input.GetAxis("RightV");

        scrool.value -= angV * Time.deltaTime;
    }
}
