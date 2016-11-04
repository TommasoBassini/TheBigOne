using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class FeedbackEventPin
{
    public UnityEvent rightPinEvent;
    public UnityEvent wrongPinEvent;
}

[System.Serializable]
public class FeedbackEventPermission
{
    public UnityEvent hasPermission;
    public UnityEvent noPermission;
}

[System.Serializable]
public class Images
{
    public Image[] imagesToChange;
    public Color[] imagesColors;
    public Sprite[] imagesSprites;
    public float[] imagesFills;
}

[System.Serializable]
public class Texts
{
    public Text[] textsToChange;
    public Color[] textsColors;
    public string[] textsStrings;
}

[System.Serializable]
public class Audios
{
    public AudioClip[] audioClips;
}

public class TerminalFeedbacks : MonoBehaviour
{
    [Header("Eventi per pannello pin")]
    public FeedbackEventPin feedbackEventPin;

    [Header("Eventi per permessi bottoni")]
    public FeedbackEventPermission feedbackEventPermission;

    [Header("Feedback")]
    public Texts texts;
    public Images images;
    public Audios audios;

    public void ChangeText(int n)
    {
        if (texts.textsStrings.Length != 0)
        {
            texts.textsToChange[n].color = texts.textsColors[n];
        }

        if (texts.textsColors.Length != 0)
        {
            texts.textsToChange[n].text = texts.textsStrings[n];
        }
    }

    public void ChangeImage(int n)
    {

        if (images.imagesSprites[n] != null && images.imagesSprites.Length != 0)
        {
            images.imagesToChange[n].sprite = images.imagesSprites[n];
        }

        if (images.imagesColors.Length != 0)
        {
            images.imagesToChange[n].color = images.imagesColors[n];
        }
        if (images.imagesFills.Length != 0)
        {
            images.imagesToChange[n].fillAmount = images.imagesFills[n];

        }
    }

    public void PlayAudio(int n)
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        audioSource.Stop();
        audioSource.PlayOneShot(audios.audioClips[n]);
    }
}
