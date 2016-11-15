using UnityEngine;
using System.Collections;

public class TriggerEvents : MonoBehaviour
{
    [Header("Eventi per trigger")]
    public FeedbackTriggerEvent feedbackEventTrigger;

    [Header("Eventi a tempo")]
    public Timed[] times;
    public TimedFeedbackEvent timedFeedbackEvent;

    private bool isDoneEnter = false;
    private bool isDoneExit = false;

    public IEnumerator TimedFeedback(int n, float t)
    {
        yield return new WaitForSeconds(t);
        timedFeedbackEvent.timedEvent[n].Invoke();
    }

    public void TimedEvent(int n)
    {
        StartCoroutine(TimedFeedback(times[n].nEvent, times[n].timeToWait));
    }

    void OnTriggerEnter()
    {
        if (!isDoneEnter)
        {
            feedbackEventTrigger.onTriggerEnter.Invoke();
            isDoneEnter = true;
        }
    }

    void OnTriggerExit()
    {
        if (!isDoneExit)
        {
            feedbackEventTrigger.onTriggerExit.Invoke();
            isDoneExit = true;
        }
    }
}
