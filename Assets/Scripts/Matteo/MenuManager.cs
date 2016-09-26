using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    //private GameManager gameManager;

    public GameObject pnlAskTutorial, pnlOptions, pnlBack;

    void Start () {
        //gameManager = FindObjectOfType<GameManager>();
    }
	
	public void AskTutorial()
    {
        pnlBack.SetActive(true);
        pnlAskTutorial.SetActive(true);
    }

    public void StartGame(bool goTutorial)
    {
        //gameManager.SetLevel(1, goTutorial);
    }

    public void Options()
    {
        pnlBack.SetActive(true);
        pnlOptions.SetActive(true);
    }

    public void Back()
    {
        pnlBack.SetActive(false);
        pnlAskTutorial.SetActive(false);
        pnlOptions.SetActive(false);
    }
}
