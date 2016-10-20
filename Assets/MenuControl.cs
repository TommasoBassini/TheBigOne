using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class MenuControl : MonoBehaviour
{
    private bool isMenu = false;
    public GameObject pnlMenu;
    public GameObject[] panels;
    public Button[] buttons;
    public int nMenu = 0;
    public bool isSubMenu = false;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (!isMenu)
            {
                isMenu = true;
                FindObjectOfType<FirstPersonController>().enabled = false;
                pnlMenu.SetActive(true);
                panels[0].SetActive(true);
                buttons[0].Select();
                return;
            }
            else
            {
                isMenu = false;
                FindObjectOfType<FirstPersonController>().enabled = true;
                pnlMenu.SetActive(false);
                panels[nMenu].SetActive(false);
                nMenu = 0;
                return;
            }
        }

        if (isMenu)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                if (isSubMenu)
                {
                    buttons[nMenu].Select();
                    isSubMenu = false;
                    return;
                }
                else
                {
                    isMenu = false;
                    FindObjectOfType<FirstPersonController>().enabled = true;
                    pnlMenu.SetActive(false);
                    panels[nMenu].SetActive(false);
                    nMenu = 0;
                    return;
                }
            }
        }
    }

    public void ChangePanel(int n)
    {
        panels[nMenu].SetActive(false);
        nMenu = n;
        panels[nMenu].SetActive(true);
    }

    public void SelectPanel(Button buttonToSelect)
    {
        isSubMenu = true;
        buttonToSelect.Select();
    }
}
