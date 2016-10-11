using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    //private GameManager gameManager;

    public GameObject imgBgCool, pnlOptions, pnlCredits, pnlAskToQuit, btnContinueGame, btnNewGame;
    public Sprite[] backgrounds;
    public int actualBg;
    public float startWaitTime, waitBetweenBgs;
    public bool isLooping, hasSaves;

    void Start () {
        //gameManager = FindObjectOfType<GameManager>();
        actualBg = 0;
        isLooping = false;
        if (hasSaves)
        {
            GetComponentInChildren<EventSystem>().firstSelectedGameObject = btnContinueGame;
        }
        else
        {
            GetComponentInChildren<EventSystem>().firstSelectedGameObject = btnNewGame;
        }
        StartCoroutine("changeBg");
    }
	

    public void StartGame()
    {
        //gameManager.SetLevel(0);
        SceneManager.LoadScene("Prototype");
    }

    public void Options()
    {
        if (pnlOptions.activeInHierarchy)
        {
            pnlOptions.SetActive(false);
        }
        else
        {
            pnlOptions.SetActive(true);
        }
    }

    public void Credits()
    {
        if (pnlCredits.activeInHierarchy)
        {
            pnlCredits.SetActive(false);
        }
        else
        {
            pnlCredits.SetActive(true);
        }
    }

    public void Exit()
    {
        if (pnlAskToQuit.activeInHierarchy)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        else
        {
            pnlAskToQuit.SetActive(true);
        }
    }

    public void Back()
    {
        pnlOptions.SetActive(false);
        pnlCredits.SetActive(false);
        pnlAskToQuit.SetActive(false);
    }

    IEnumerator changeBg()
    {
        if (!isLooping)
        {
            yield return new WaitForSeconds(startWaitTime);
            isLooping = true;
        }
        imgBgCool.GetComponent<Image>().sprite = backgrounds[actualBg];
        actualBg++;
        if (actualBg == backgrounds.Length)
        {
            actualBg = 0;
        }
        yield return new WaitForSeconds(waitBetweenBgs);
        StartCoroutine("changeBg");
    }

    void Update()
    {
        if (isLooping)
        {
            imgBgCool.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, Mathf.PingPong((Time.time - startWaitTime) / (waitBetweenBgs / 2), 1));
        }

        if (Input.GetButton("Cancel"))
        {
            Back();
        }
    }
}