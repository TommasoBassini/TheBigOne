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
        TerminalFeedbacks terminalFeedback = GetComponentInParent<TerminalFeedbacks>();
        if (isPodInserterted)
        {
            if (!isDone)
            {
                PlayerStatus playerStatus = FindObjectOfType<PlayerStatus>();
                isDone = true;
                terminalFeedback.feedbackEventUpgrade.upgradedPodEvent.Invoke();

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
                terminalFeedback.feedbackEventUpgrade.alreadyUpgradedEvent.Invoke();
            }
        }
        else
        {
            terminalFeedback.feedbackEventUpgrade.noPodInsertedEvent.Invoke();
        }
    }
}
