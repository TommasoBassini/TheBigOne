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

    public GameObject panelToUnlock;

    public void StampNumber (string n)
    {
        if (passText.Length < 12)
        {
            currentPin = currentPin + n;
            passText += " - ";
            pinText.text = passText;
        }
    }

    public void CheckPin()
    {
        if (currentPin == unlockPin)
        {
            panelToUnlock.SetActive(true);
            panelToUnlock.GetComponentInChildren<Button>().Select();
            currentPin = "";
            passText = "";
            pinText.text = "Inserire codice";
            GetComponentInParent<TerminalFeedbacks>().feedbackEventPin.rightPinEvent.Invoke();
            this.gameObject.SetActive(false);
        }
        else
        {
            passText = "";
            currentPin = "";
            pinText.text = "Inserire codice";
            GetComponentInParent<TerminalFeedbacks>().feedbackEventPin.wrongPinEvent.Invoke();
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
