﻿using UnityEngine;
using System.Collections;

public class VariableButton : MonoBehaviour
{
    public bool isActive = false;
    public int valueDiff;

    public bool justPressed = false;

    public void Press(int n)
    {
        TerminalStatus ts = GetComponentInParent<TerminalStatus>();
        TerminalEvents te = GetComponentInParent<TerminalEvents>();

        if (!justPressed)
        {
            if (isActive)
            {
                justPressed = true;
                Invoke("ResetPressed", ts.imagesToFill[n].timerToFill);
                isActive = false;
                ts.imagesToFill[n].terminalValue += valueDiff;
                te.feedbackButtonVariableEvent.deactive[n].Invoke(); // setti gli effetti del tasto premuto
                StartCoroutine(ts.FillImageOverTime(n, valueDiff)); // L'immagine si filla automaticamente
            }
            else
            {
                if (valueDiff <= ts.imagesToFill[n].terminalValue)
                {
                    justPressed = true;
                    isActive = true;
                    Invoke("ResetPressed", ts.imagesToFill[n].timerToFill);
                    ts.imagesToFill[n].terminalValue -= valueDiff;
                    te.feedbackButtonVariableEvent.active[n].Invoke(); // setti gli effetti del tasto premuto
                    StartCoroutine(ts.FillImageOverTime(n, -valueDiff)); // L'immagine si filla automaticamente
                }
                else
                {
                    te.feedbackButtonVariableEvent.noEnergy.Invoke();
                }
            }
        }
    }

    private void ResetPressed()
    {
        justPressed = false;
    }
}
