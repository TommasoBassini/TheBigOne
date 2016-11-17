﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;


public class Feedbacks : MonoBehaviour
{
    [Header("Feedback")]
    public Texts[] texts;
    public Images[] images;
    public Audios[] audios;
    public Animations[] animations;
    public Lights[] lights;
    public Gameobjects[] gameobjects;

    private AsyncOperation async;

    void Start()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].textString = texts[i].textString.Replace("<br>", "\n");
        }
    }
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
                if (!images[n].isWorking)
                {
                    StartCoroutine(FillImageOverTime(n));
                }
            }
            else
            {
                images[n].imageToChange.fillAmount = images[n].imageFill;
            }
        }
    }

    public IEnumerator FillImageOverTime(int n)
    {
        images[n].isWorking = true;
        float time = images[n].timerToFill;
        float elapsedTime = 0.0f;
        float startFill = images[n].imageToChange.fillAmount;
        while (elapsedTime < time)
        {
            images[n].imageToChange.fillAmount = Mathf.Lerp(startFill, images[n].imageFill, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        images[n].isWorking = false;
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

    public void SetGameobject(int n)
    {
        bool objStatus = gameobjects[n].gameobject.activeInHierarchy;

        gameobjects[n].gameobject.SetActive(!objStatus);
    }

    public void FlashGameobject(int n)
    {
        if (!gameobjects[n].isWorking)
        {
            StartCoroutine(FlashGameobjectCO(n));
        }
    }

    IEnumerator FlashGameobjectCO(int n)
    {
        gameobjects[n].isWorking = true;
        bool initialState = gameobjects[n].gameobject.activeInHierarchy;
        for (int i = 0; i < (gameobjects[n].nSecondiFlesh * 2); i++)
        {
            gameobjects[n].gameobject.SetActive(!gameobjects[n].gameobject.activeInHierarchy);
            yield return new WaitForSeconds(0.5f);
        }
        gameobjects[n].gameobject.SetActive(initialState);
        gameobjects[n].isWorking = false;
    }

    public void ChangeTerminalPanel(GameObject panelToShow)
    {
        TerminalStatus canvas = GetComponent<TerminalStatus>();

        if (canvas)
        {

            foreach (var panel in canvas.panels)
            {
                if (panel.panel.activeInHierarchy)
                {

                    canvas.orderOfLastPanel.Add(panel);
                    canvas.activePanel.SetActive(false);
                    break;
                }
            }

            panelToShow.SetActive(true);
            canvas.activePanel = panelToShow;

            foreach (var panel in canvas.panels)
            {
                if (panel.panel.activeInHierarchy)
                {

                    panel.firstSelectButtonInPanel.Select();
                    break;
                }
            }
        }
    }

    public void ChangeScene(int sceneIndex)
    {
        StartCoroutine(ChangeSceneCO(sceneIndex));
    }

    IEnumerator ChangeSceneCO(int sceneIndex)
    {
        async = SceneManager.LoadSceneAsync(sceneIndex);
        async.allowSceneActivation = false;
        Invoke("prova", 5);
        Debug.Log("la scena è stata caricata e verra cambiata in 5 secondi");
        yield return async;
    }
    void prova()
    {
        async.allowSceneActivation = true;
    }
    public void GeneralButton(int n)
    {
        GetComponentInParent<TerminalEvents>().generalFeedbackEvent.generalEvent[n].Invoke();
    }

    public void ExitFromTerminal()
    {
        ObjectInteract objectInteract = FindObjectOfType<ObjectInteract>();
        objectInteract.ExitFromTerminal();
    }
}
