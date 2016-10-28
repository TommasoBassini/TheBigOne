using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScanButtonManager : MonoBehaviour
{
    public int colonna = 0;
    public int righe = 0;
    public List<GameObject[]> scans = new List<GameObject[]>();

    public GameObject buttonPrefab;
    public GameObject scansPanel;

    public GameObject ShowObjPanel;
    public Scrollbar scroll;

    public void SetNewButton(GameObject obj, string datiMedici,string datiIngegneria,string datiSicurezza, Sprite objPreview)
    {
        if (scans.Count == 0)
        {
            scans.Add(new GameObject[6]);
        }

        GameObject newButton = Instantiate(buttonPrefab) as GameObject;
        newButton.transform.SetParent(scansPanel.transform);
        RectTransform rect = newButton.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = new Vector3(1, 1, 1);

        ScanButton scanButton = newButton.GetComponent<ScanButton>();
        scanButton.objToView = obj;
        scanButton.datiMedici = datiMedici;
        scanButton.datiIngegneria = datiIngegneria;
        scanButton.datiSicurezza = datiSicurezza;
        scanButton.objPreview = objPreview;
        scanButton.gridPos = new Vector2(colonna, righe);
        scanButton.Initialize();

        scans[righe][colonna] = newButton;
        colonna++;

        if (colonna == 6)
        {
            scans.Add(new GameObject[6]);
            righe++;
            colonna = 0;
            if (righe >= 5)
            {
                scansPanel.GetComponent<RectTransform>().sizeDelta = new Vector3(0, scansPanel.GetComponent<RectTransform>().sizeDelta.y + 140, 0);
            }
        }
    }

    public Navigation SetNaviagtion(Vector2 _pos)
    {
        Navigation buttonNavigation = new Navigation();
        buttonNavigation.mode = Navigation.Mode.Explicit;
        int x = (int)_pos.x;
        int y = (int)_pos.y;

        //Prendo bottone a sinistra
        if ((x-1 >= 0))
        {
            buttonNavigation.selectOnLeft = scans[y][x - 1].GetComponent<Button>();
        }

        //Prendo bottone a destra
        if ((x + 1 < 6))
        {
            if (scans[y][x + 1] != null)
            {
                buttonNavigation.selectOnRight = scans[y][x + 1].GetComponent<Button>();
            }
        }

        //Prendo bottone a alto
        if ((y - 1 >= 0))
        {
            buttonNavigation.selectOnUp = scans[y - 1][x].GetComponent<Button>();
        }

        //Prendo bottone a basso 
        if ((y + 1 <= righe))
        {
            if (scans[y + 1][x] != null)
            {
                buttonNavigation.selectOnDown = scans[y + 1][x].GetComponent<Button>();
            }
        }

        return buttonNavigation;
    }

    public void SwitchPanel()
    {
        ShowObjPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void RefreshScroll(int n)
    {
        if (n>4)
        {
            float perc = (float)n / righe;
            scroll.value = 1.0f - perc;
        }
    }
}
