using UnityEngine;
using System.Collections;

public class TerminalEvents : MonoBehaviour
{
    [Header("Eventi per pannello pin")]
    public FeedbackEventPin feedbackEventPin;

    [Header("Eventi per permessi bottoni")]
    public FeedbackEventPermission feedbackEventPermission;

    [Header("Eventi per upgrade permessi")]
    public FeedbackEventPod feedbackEventUpgrade;

    [Header("Eventi a tempo")]
    public Timed[] times;
    public TimedFeedbackEvent timedFeedbackEvent;

    public IEnumerator TimedFeedback(int n, float t)
    {
        yield return new WaitForSeconds(t);
        timedFeedbackEvent.timedEvent[n].Invoke();
    }

    public void TimedEvent(int n)
    {
        StartCoroutine(TimedFeedback(times[n].nEvent, times[n].timeToWait));
    }
}
