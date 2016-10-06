using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObjectInteract : MonoBehaviour
{
    private Vector3 cameraPos;
    private Quaternion cameraRot;
    private Vector3 lastObjPos;
    private Quaternion lastObjRot;
    private bool isPickuble = false;
    private GameObject pickubleObj;

    public bool isInteracting = false;
    public bool isInspecting = false;
    public bool isTerminal = false;

    public int rotationSpeed;
    public Image mirino;

    public Sprite lente;
    public Sprite punto;

    public GameObject button;
    public GameObject panel;

    public GameObject inspect;

    public Button dummyButton;

    private Ray interactionRay;
    public float dropDistance;

    void Start ()
    {
        cameraPos = Camera.main.transform.position;
	}

    void FixedUpdate()
    {
        interactionRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    }

    void Update()
    {
        CheckInteract();
    }

    void CheckInteract()
    {
        if (!isInteracting)
        {
            RaycastHit hit;

            if (Physics.Raycast(interactionRay, out hit, dropDistance))
            {
                if (hit.collider.tag == "Pickuble")
                {
                    mirino.sprite = lente;
                    isPickuble = true;
                    pickubleObj = hit.collider.gameObject;

                    if (Input.GetKeyUp(KeyCode.Joystick1Button0))
                    {
                        isInteracting = true;
                        isInspecting = true;
                        lastObjPos = hit.collider.gameObject.transform.position;
                        lastObjRot = hit.collider.gameObject.transform.rotation;
                        GetComponent<FirstPersonController>().enabled = false;
                        inspect.transform.localPosition = new Vector3 (0,0,0.2f) + new Vector3(0,0,(1 * pickubleObj.transform.gameObject.GetComponent<IspectionNear>().near)); 
                        pickubleObj.transform.position = inspect.transform.position;
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
                    if (Input.GetKeyUp(KeyCode.Joystick1Button0))
                    {
                        isInteracting = true;
                        isTerminal = true;
                        mirino.enabled = false;
                        cameraRot = Camera.main.transform.rotation;
                        cameraPos = Camera.main.transform.position;

                        Camera.main.transform.position = hit.collider.transform.position + (hit.collider.transform.forward * ((hit.collider.transform.localScale.y * 0.172f)));
                        Camera.main.transform.LookAt(hit.collider.transform.position + new Vector3(0, 0.006f, 0));
                        GetComponent<FirstPersonController>().enabled = false;
                        GameObject canvasMain = hit.collider.transform.FindChild("Main").gameObject;
                        canvasMain.GetComponent<SelectCanvasButton>().firstSelected.Select();
                    }
                }

                if (hit.collider.tag == "ActionObj")
                {
                    if (Input.GetKeyUp(KeyCode.Joystick1Button0))
                    {
                        hit.collider.gameObject.GetComponent<ActionObj>().DoStuff();

                    }
                }
            }
            else
            {
                isPickuble = false;
                pickubleObj = null;
                mirino.sprite = punto;
            }
        }

        if (isInteracting)
        {
            if (isInspecting)
            {
                float angH = Input.GetAxis("RightH");
                float angV = Input.GetAxis("RightV");

                if (Input.GetAxis("RightH") > 0.25f || Input.GetAxis("RightH") < -0.25f)
                {
                    pickubleObj.transform.eulerAngles += new Vector3(0, angH * rotationSpeed, 0);
                }
                if (Input.GetAxis("RightV") > 0.25f || Input.GetAxis("RightV") < -0.25f)
                {
                    inspect.transform.Rotate(new Vector3(-angV * rotationSpeed, 0, 0));
                }

                if (Input.GetKeyUp(KeyCode.Joystick1Button1) && isInspecting)
                {
                    GetComponent<FirstPersonController>().enabled = true;
                    pickubleObj.transform.position = lastObjPos;
                    pickubleObj.transform.rotation = lastObjRot;
                    pickubleObj.transform.SetParent(null);
                    pickubleObj = null;
                    isInspecting = false;
                    isInteracting = false;
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

            if (isTerminal)
            {
                if (Input.GetKeyUp(KeyCode.Joystick1Button1))
                {
                    mirino.enabled = true;
                    GetComponent<FirstPersonController>().enabled = true;
                    Camera.main.transform.rotation = cameraRot;
                    Camera.main.transform.position = cameraPos;
                    dummyButton.Select();
                    isTerminal = false;
                    isInteracting = false;
                }
            }
        }
    }

    public void HoloInspect(GameObject obj)
    {

    }
}

