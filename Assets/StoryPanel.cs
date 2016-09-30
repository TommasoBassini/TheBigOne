using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoryPanel : MonoBehaviour
{
    public TextAsset[] texts;
    public Text textBox;


	void Update ()
    {
	
	}

    public void ShowText(int n)
    {
        textBox.text = texts[n].text;
    }
}
