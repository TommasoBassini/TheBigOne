using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScanButton : MonoBehaviour
{
    public GameObject objToView;
    public string datiMedici;
    public string datiIngegneria;
    public string datiSicurezza;

    public Sprite objPreview;
    public Image image;

    public Vector2 gridPos;

    public void Initialize()
    {
        image.sprite = objPreview;
    }

    public void TakeNavigation()
    {
        GetComponent<Button>().navigation = FindObjectOfType<ScanButtonManager>().SetNaviagtion(gridPos);
    }

    public void Selecterds()
    {
        Debug.Log("ygv");
    }
}
