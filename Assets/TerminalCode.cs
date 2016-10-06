using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TerminalCode : MonoBehaviour
{
    public string unlockPin;
    public string currentPin;
    public Text pinText;

    public void StampNumber (string n)
    {
        currentPin = currentPin + n;
        pinText.text = currentPin;
    }

    public void CheckPin()
    {
        if (currentPin == unlockPin)
        {
            Debug.Log("Sblocco Porta");
        }
        else
        {
            Debug.Log("hai sbagliato carpra di merda! beee");
            currentPin = "";
            pinText.text = currentPin;
        }
    }
}
