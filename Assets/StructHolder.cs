using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;


[System.Serializable]
public class FeedbackEventPin
{
    public UnityEvent[] rightPinEvent;
    public UnityEvent[] wrongPinEvent;
}

[System.Serializable]
public class FeedbackEventPod
{
    public UnityEvent upgradedPodEvent;
    public UnityEvent alreadyUpgradedEvent;
    public UnityEvent noPodInsertedEvent;
}

[System.Serializable]
public class FeedbackEventPermission
{
    public UnityEvent[] hasPermission;
    public UnityEvent[] noPermission;
}

[System.Serializable]
public class TimedFeedbackEvent
{
    public UnityEvent[] timedEvent;
}

[System.Serializable]
public class FeedbackTriggerEvent
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
}

[System.Serializable]
public struct Images
{
    public Image imageToChange;
    public Color imageColor;
    public Sprite imageSprite;
    public float imageFill;
    public float timerToFill;
}

[System.Serializable]
public struct Texts
{
    public Text textToChange;
    public Color textColors;
    public string textString;
}

[System.Serializable]
public struct Timed
{
    public float timeToWait;
    public int nEvent;
}

[System.Serializable]
public struct Audios
{
    public AudioClip audioClip;
}

[System.Serializable]
public struct Animations
{
    public Animator objAnimator;
    public string triggerName;
}

[System.Serializable]
public struct Lights
{
    public GameObject lightsParent;
}

[System.Serializable]
public struct Gameobjects
{
    public GameObject gameobject;
}