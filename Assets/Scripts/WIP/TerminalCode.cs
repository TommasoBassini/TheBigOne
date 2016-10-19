using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TerminalCode : MonoBehaviour
{
    public string unlockPin;
    public string currentPin;
    public string passText;
    public Text pinText;

    public GameObject panelToUnlock;

    public void StampNumber (string n)
    {
        if (passText.Length < 4)
        {
            currentPin = currentPin + n;
            passText += '*';
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
            this.gameObject.SetActive(false);
        }
        else
        {
            passText = "";
            currentPin = "";
            pinText.text = "Inserire codice";
        }
    }

    public void DeleteLast()
    {
        if (passText.Length > 0)
        {
            currentPin = currentPin.Substring(0, currentPin.Length - 1);
            passText = passText.Substring(0, passText.Length - 1);

            pinText.text = passText;
            if (passText.Length == 0)
            {
                pinText.text = "Inserire codice";
            }
        }

    }
}
