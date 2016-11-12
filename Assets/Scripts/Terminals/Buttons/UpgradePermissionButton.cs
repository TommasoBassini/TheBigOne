using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class UpgradePermissionButton : MonoBehaviour
{
    public bool isDone;
    public SecurityType permission;
    public bool isPodInserterted = false;

    public void ChangePermission()
    {
        TerminalEvents terminalEvents = GetComponentInParent<TerminalEvents>();
        if (isPodInserterted)
        {
            if (!isDone)
            {
                PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
                isDone = true;
                terminalEvents.feedbackEventUpgrade.upgradedPodEvent.Invoke();

                switch (permission)
                {
                    case SecurityType.medic:
                        {
                            playerStatus.medicLvl++;
                            break;
                        }
                    case SecurityType.engineer:
                        {
                            playerStatus.engineerLvl++;
                            break;
                        }
                    case SecurityType.guard:
                        {
                            playerStatus.guardLvl++;
                            break;
                        }
                }
            }
            else
            {
                terminalEvents.feedbackEventUpgrade.alreadyUpgradedEvent.Invoke();
            }
        }
        else
        {
            terminalEvents.feedbackEventUpgrade.noPodInsertedEvent.Invoke();
        }
    }
}
