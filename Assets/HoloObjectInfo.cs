using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HoloObjectInfo : MonoBehaviour
{
    public Sprite itemImage;
    public GameObject itemPrefab;

	void Start ()
    {
	
	}

    public void ShowItem()
    {

        GameObject newHolo = Instantiate(itemPrefab);

        newHolo.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 1f);
        this.transform.parent.gameObject.SetActive(false);
    }
}
