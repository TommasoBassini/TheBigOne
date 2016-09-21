using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class ObjectInteract : MonoBehaviour
{

    private Vector3 lastObjPos;
    private Quaternion lastObjRot;
    private bool isPickuble = false;
    private GameObject pickubleObj;
    public bool isInspecting = false;

    public int rotationSpeed;
    public Image mirino;

    public Sprite lente;
    public Sprite punto;

    public GameObject button;
    public GameObject panel;

    public GameObject inspect;

    void Start ()
    {

	}
	
	void Update ()
    {
        RaycastHit hit;
        if (!isInspecting)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2.5f))
            {
                if (hit.collider.tag == "Pickuble")
                {
                    mirino.sprite = lente;

                    isPickuble = true;
                    pickubleObj = hit.collider.gameObject;

                    Debug.DrawLine(Camera.main.transform.position, hit.collider.transform.position);
                    if (Input.GetKeyUp(KeyCode.Joystick1Button0))
                    {
                        isInspecting = true;
                        lastObjPos = hit.collider.gameObject.transform.position;
                        lastObjRot = hit.collider.gameObject.transform.rotation;
                        GetComponent<FirstPersonController>().enabled = false;
                        pickubleObj.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 1f);
                        pickubleObj.transform.SetParent(inspect.transform);
                    }
                    return;
                }
                else
                {
                    isPickuble = false;
                    pickubleObj = null;
                    mirino.sprite = punto;
                }

                if (hit.collider.tag == "Terminal")
                {
                    Debug.Log(" jh hn");
                }
            }
            else
            {
                isPickuble = false;
                pickubleObj = null;
                mirino.sprite = punto;
            }


        }

        if (isInspecting)
        {
            float angH = Input.GetAxis("RightH");
            float angV = Input.GetAxis("RightV");

            if (Input.GetAxis("RightH") > 0.25f || Input.GetAxis("RightH") < -0.25f)
            {
                pickubleObj.transform.eulerAngles += new Vector3 (0,angH * rotationSpeed, 0) ;
            }
            if (Input.GetAxis("RightV") > 0.25f || Input.GetAxis("RightV") < -0.25f)
            {
                inspect.transform.Rotate (new Vector3(-angV * rotationSpeed, 0, 0));
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button0) && isInspecting)
            {
                GetComponent<FirstPersonController>().enabled = true;
                pickubleObj.transform.position = lastObjPos;
                pickubleObj.transform.rotation = lastObjRot;
                pickubleObj.transform.SetParent(null);
                pickubleObj = null;
                isInspecting = false;
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button3) && isInspecting)
            {
                ObjectInfo objInfo = pickubleObj.GetComponent<ObjectInfo>();

                if (!objInfo.isScan)
                {
                    GameObject newButton = Instantiate(button);
                    HoloObjectInfo holo = newButton.GetComponent<HoloObjectInfo>();
                    holo.itemPrefab = objInfo.itemPrefab;
                    holo.itemImage = objInfo.itemImage;
                    objInfo.isScan = true;
                    newButton.transform.SetParent(panel.transform);
                }
            }
        }
    }

    public void HoloInspect(GameObject obj)
    {

    }
}
