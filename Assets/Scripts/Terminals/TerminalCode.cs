using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class TerminalCode : MonoBehaviour
{
    public string unlockPin;
    public string currentPin;
    public string passText;
    public Text pinText;

    public void StampNumber (string n)
    {
        if (passText.Length < 12)
        {
            currentPin = currentPin + n;
            passText += " - ";
            pinText.text = passText;
        }
    }

    public void CheckPin(int n)
    {
        if (currentPin == unlockPin)
        {
            currentPin = "";
            passText = "";
            pinText.text = "Inserire codice";
            GetComponentInParent<TerminalEvents>().feedbackEventPin.rightPinEvent[n].Invoke();
        }
        else
        {
            passText = "";
            currentPin = "";
            pinText.text = "Inserire codice";
            GetComponentInParent<TerminalEvents>().feedbackEventPin.wrongPinEvent[n].Invoke();
        }
    }

    public void DeleteLast()
    {
        if (passText.Length > 0)
        {
            currentPin = currentPin.Substring(0, currentPin.Length - 1);
            passText = passText.Substring(0, passText.Length - 3);

            pinText.text = passText;
            if (passText.Length == 0)
            {
                pinText.text = "Inserire codice";
            }
        }

    }
}
