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
public struct Images
{
    public Image imageToChange;
    public Color imageColor;
    public Sprite imageSprite;
    public float imageFill;
}

[System.Serializable]
public struct Texts
{
    public Text textToChange;
    public Color textColors;
    public string textString;
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

public class TerminalFeedbacks : MonoBehaviour
{
    [Header("Eventi per pannello pin")]
    public FeedbackEventPin feedbackEventPin;

    [Header("Eventi per permessi bottoni")]
    public FeedbackEventPermission feedbackEventPermission;

    [Header("Eventi per upgrade permessi")]
    public FeedbackEventPod feedbackEventUpgrade;

    [Header("Feedback")]
    public Texts[] texts;
    public Images[] images;
    public Audios[] audios;
    public Animations[] animations;
    public Lights[] lights;

    public void ChangeText(int n)
    {
        if (texts[n].textColors != new Color(0, 0, 0, 0))
        {
            texts[n].textToChange.color = texts[n].textColors;
        }

        if (texts[n].textString.Length != 0)
        {
            texts[n].textToChange.text = texts[n].textString;
        }
    }

    public void ChangeImage(int n)
    {
        if (images[n].imageSprite != null)
        {
            images[n].imageToChange.sprite = images[n].imageSprite;
        }

        if (images[n].imageColor != new Color(0, 0, 0, 0))
        {
            images[n].imageToChange.color = images[n].imageColor;
        }

        images[n].imageToChange.fillAmount = images[n].imageFill;
    }

    public void PlayAudio(int n)
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        audioSource.Stop();
        audioSource.PlayOneShot(audios[n].audioClip);
    }

    public void PlayAnimation(int n)
    {
        animations[n].objAnimator.SetTrigger(animations[n].triggerName);
    }

    public void LightOnOff(int n)
    {
        Light[] lightList = lights[n].lightsParent.GetComponentsInChildren<Light>();

        foreach (var light in lightList)
        {
            if (light.enabled)
            {
                Debug.Log("Enabled");
                light.enabled = false;
                continue;
            }
            else if (!light.enabled)
            {
                light.enabled = true;
                continue;
            }
        }
    }
}
