using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Feedbacks : MonoBehaviour
{
    [Header("Feedback")]
    public Texts[] texts;
    public Images[] images;
    public Audios[] audios;
    public Animations[] animations;
    public Lights[] lights;
    public Gameobjects[] gameobjects;

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
        if (images[n].imageToChange.fillAmount != images[n].imageFill)
        {
            if (images[n].timerToFill>0)
            {
                StartCoroutine(FillImageOverTime(n));
            }
            else
            {
                images[n].imageToChange.fillAmount = images[n].imageFill;
            }
        }
    }

    public IEnumerator FillImageOverTime(int n)
    {
        float time = images[n].timerToFill;
        float elapsedTime = 0.0f;
        float startFill = images[n].imageToChange.fillAmount;
        while (elapsedTime < time)
        {
            images[n].imageToChange.fillAmount = Mathf.Lerp(startFill, images[n].imageFill, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
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

    public void ChangePanel(GameObject panelToShow)
    {
        TerminalStatus canvas = GetComponent<TerminalStatus>();
        canvas.activePanel.SetActive(false);
        panelToShow.SetActive(true);
        canvas.activePanel = panelToShow;

        TerminalStatus ts = canvas.activePanel.GetComponentInParent<TerminalStatus>();
        foreach (var panel in ts.panels)
        {
            if (panel.panel.activeInHierarchy)
            {
                panel.firstSelectButtonInPanel.Select();
                break;
            }
        }
    }

    public void SetGameobject(int n)
    {
        bool objStatus = gameobjects[n].gameobject.activeInHierarchy;

        gameobjects[n].gameobject.SetActive(!objStatus);
    }
}
