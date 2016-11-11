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

    public ScanButtonManager scan;
    private MenuControl menu;
    private float scanTime = 0.0f;
    public Image scanPerc;

    void Start()
    {
        //Setta la posizione della telecamera all'inizio del gioco
        cameraPos = Camera.main.transform.position;
        menu = FindObjectOfType<MenuControl>();
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

                    if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E)) && !menu.isMenu)
                    {
                        GetComponent<FirstPersonController>().enabled = false;

                        isInteracting = true;
                        isInspecting = true;

                        actionImage.gameObject.SetActive(false);
                        mirino.gameObject.SetActive(false);

                        lastObjPos = hit.collider.gameObject.transform.position;
                        lastObjRot = hit.collider.gameObject.transform.rotation;
                        inspect.transform.localPosition = new Vector3(0, 0, 0.2f) + new Vector3(0, 0, (1 * pickubleObj.transform.gameObject.GetComponent<ObjInformation>().near));
                        pickubleObj.transform.position = inspect.transform.position;
                        pickubleObj.transform.SetParent(inspect.transform);
                        pickubleObj.transform.localEulerAngles = new Vector3(-90, 0, 0);
                    }
                }

                // SE L'OGGETTO DEL RAYCAST E' Terminal
                if (hit.collider.CompareTag("Terminal"))
                {

                    if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E)) && !menu.isMenu)
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
                        TerminalStatus ts = activeCanvas.GetComponent<TerminalStatus>();
                        foreach (var panel in ts.panels)
                        {
                            if (panel.panel.activeInHierarchy)
                            {
                                panel.firstSelectButtonInPanel.Select();
                                break;
                            }
                        }
                        torcia.SetActive(false);
                    }
                }


                // SE L'OGGETTO DEL RAYCAST E' ActionObj
                if (hit.collider.CompareTag("ActionObj"))
                {
                    if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E)) && !menu.isMenu)
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

                if ((Input.GetKeyUp(KeyCode.Joystick1Button1)  || Input.GetKeyDown(KeyCode.Escape)) && isInspecting && !menu.isMenu)
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

                if (Input.GetKey(KeyCode.Joystick1Button0) && isInspecting || Input.GetKeyDown(KeyCode.Escape))
                {
                    ObjInformation objInfo = pickubleObj.GetComponent<ObjInformation>();
                    if (!objInfo.isScanning)
                    {
                        scanTime += 0.5f * Time.deltaTime;
                        scanPerc.fillAmount = scanTime;
                        if (scanTime > 1)
                        {
                            scanTime = 0.0f;
                            scanPerc.fillAmount = scanTime;
                            objInfo.isScanning = true;
                            scan.SetNewButton(objInfo.objToView, objInfo.datiMedici, objInfo.datiIngegneria, objInfo.datiSicurezza, objInfo.objPreview);
                        }
                    }
                }
                if (Input.GetKeyUp(KeyCode.Joystick1Button0) && isInspecting || Input.GetKeyDown(KeyCode.Escape))
                {
                    scanTime = 0.0f;
                }
            }

            if (isTerminal)
            {
                if (Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Escape))
                {
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


    public void ViewObjectMenu(GameObject obj)
    {
        pickubleObj = Instantiate(obj);

        isInteracting = true;
        isInspecting = true;
        menu.objActive = pickubleObj;
        inspect.transform.localPosition = new Vector3(0, 0, 0.2f) + new Vector3(0, 0, (1 * pickubleObj.GetComponent<ObjInformation>().near));
        pickubleObj.transform.position = inspect.transform.position;
        pickubleObj.transform.SetParent(inspect.transform);
        pickubleObj.transform.localEulerAngles = new Vector3(-90, 0, 0);
    }
}

