using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class StoryPanel : MonoBehaviour
{
    public TextAsset[] texts;
    public Text textBox;
    public Scrollbar scrool;
    public RectTransform content;

    public void ShowText(int n)
    {
        scrool.value = 1;
        textBox.text = texts[n].text;
        content.sizeDelta = new Vector2(11.23f,textBox.preferredHeight +10);
    }

    public void DetectScroll()
    {
        float angV = Input.GetAxis("RightV");

        scrool.value -= angV * Time.deltaTime;
    }
}
