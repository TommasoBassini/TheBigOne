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
    private GameObject pickubleObj;

    public bool isInteracting = false;
    public bool isInspecting = false;
    public bool isTerminal = false;

    public int rotationSpeed;

    public Image mirino;
    public Image actionImage;

    public Sprite pickubleSprite;
    public Sprite interactSprite;
    public Sprite punto;

    public GameObject button;
    public GameObject panel;

    public GameObject inspect;
    public GameObject activeCanvas;
    public Button dummyButton;

    private Ray interactionRay;
    public float dropDistance;

    public GameObject torcia;

    void Start ()
    {
        //Setta la posizione della telecamera all'inizio del gioco
        cameraPos = Camera.main.transform.position;
	}

    void FixedUpdate()
    {
        // Ray per l'interazione con gli oggetti dal centro dello schermo
        interactionRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        // Viene chiamato il metodo per controllare l'interazione con gli oggetti interagibili
        CheckInteract();
    }

    void CheckInteract()
    {
        if (!isInteracting)
        {
            RaycastHit hit;
            if (Physics.Raycast(interactionRay, out hit, dropDistance))
            {
                GameObject interactedObject = hit.transform.gameObject;

                // SE L'OGGETTO DEL RAYCAST E' PICKUBLE
                if (hit.collider.CompareTag("Pickuble"))
                {
                    //metto l'action image giusta
                    pickubleObj = hit.collider.gameObject;

                    if (Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E))
                    {
                        GetComponent<FirstPersonController>().enabled = false;

                        isInteracting = true;
                        isInspecting = true;

                        actionImage.gameObject.SetActive(false);
                        mirino.gameObject.SetActive(false);

                        lastObjPos = hit.collider.gameObject.transform.position;
                        lastObjRot = hit.collider.gameObject.transform.rotation;
                        inspect.transform.localPosition = new Vector3 (0,0,0.2f) + new Vector3(0,0,(1 * pickubleObj.transform.gameObject.GetComponent<IspectionNear>().near)); 
                        pickubleObj.transform.position = inspect.transform.position;
                        pickubleObj.transform.SetParent(inspect.transform);
                    }
                }

                // SE L'OGGETTO DEL RAYCAST E' Terminal
                if (hit.collider.CompareTag("Terminal"))
                {
                    if (Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E))
                    {
                        isInteracting = true;
                        isTerminal = true;

                        actionImage.gameObject.SetActive(false);
                        mirino.gameObject.SetActive(false);

                        cameraRot = Camera.main.transform.rotation;
                        cameraPos = Camera.main.transform.position;

                        Camera.main.transform.position = hit.collider.transform.position + (hit.collider.transform.right * 0.2f) + (-hit.collider.transform.up * ((((hit.collider.transform.localScale.y)/(hit.collider.transform.localScale.y/3)) * 0.25f)));
                        Camera.main.transform.LookAt(hit.collider.transform.position + (hit.collider.transform.right * 0.2f));

                        GetComponent<FirstPersonController>().enabled = false;
                        activeCanvas = hit.collider.transform.FindChild("Main").gameObject;
                        activeCanvas.GetComponent<SelectCanvasButton>().firstSelected.Select();
                        torcia.SetActive(false);
                    }
                }


                // SE L'OGGETTO DEL RAYCAST E' ActionObj
                if (hit.collider.CompareTag("ActionObj"))
                {
                    if (Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E))
                    {
                        interactedObject.GetComponent<ActionObj>().DoStuff();
                    }
                }

                if (!hit.collider.CompareTag("Untagged"))
                {
                    if (interactedObject.tag == "Pickuble")
                    {
                        actionImage.sprite = pickubleSprite;
                        actionImage.gameObject.SetActive(true);
                    }

                    if (interactedObject.tag == "Terminal")
                    {
                        actionImage.sprite = interactSprite;
                        actionImage.gameObject.SetActive(true);
                    }

                    if (interactedObject.tag == "ActionObj")
                    {
                        actionImage.sprite = interactSprite;
                        actionImage.gameObject.SetActive(true);
                    }
                }
                else
                    actionImage.gameObject.SetActive(false);
            }
            else
                actionImage.gameObject.SetActive(false);
        }
        else
            actionImage.gameObject.SetActive(false);

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

                if (Input.GetKeyUp(KeyCode.Joystick1Button1) && isInspecting || Input.GetKeyDown(KeyCode.Escape))
                {
                    
                    GetComponent<FirstPersonController>().enabled = true;
                    pickubleObj.transform.position = lastObjPos;
                    pickubleObj.transform.rotation = lastObjRot;
                    pickubleObj.transform.SetParent(null);
                    pickubleObj = null;

                    isInspecting = false;
                    isInteracting = false;

                    actionImage.gameObject.SetActive(true);
                    mirino.gameObject.SetActive(true);
                }

                /*if (Input.GetKeyUp(KeyCode.Joystick1Button3) && isInspecting || Input.GetKeyDown(KeyCode.Escape))
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
                }*/
            }

            if (isTerminal)
            {
                if (Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    activeCanvas.GetComponent<SelectCanvasButton>().ResetCanvas();

                    GetComponent<FirstPersonController>().enabled = true;
                    Camera.main.transform.rotation = cameraRot;
                    Camera.main.transform.position = cameraPos;
                    dummyButton.Select();
                    torcia.SetActive(true);

                    isTerminal = false;
                    isInteracting = false;

                    actionImage.gameObject.SetActive(true);
                    mirino.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
    }
}

