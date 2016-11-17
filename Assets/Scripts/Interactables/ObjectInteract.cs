using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum RaycastTarget
{
    nothing,
    pickable,
    terminal,
    action,
    absorb
}

[System.Serializable]
public struct ObjectInputFeedback
{
    public Sprite button;
    public string textToView;
    [Tooltip ("L'icona che è sotto il mirino")]
    public Sprite actionSprite;
}

[System.Serializable]
public struct TerminalInputFeedback
{
    public Sprite button;
    public string textToView;
    [Tooltip("L'icona che è sotto il mirino")]
    public Sprite actionSprite;
}

[System.Serializable]
public struct ActionInputFeedback
{
    public Sprite button;
    public string textToView;
    [Tooltip("L'icona che è sotto il mirino")]
    public Sprite actionSprite;
}

[System.Serializable]
public struct AbsorbInputFeedback
{
    public Sprite button;
    public string textToView;
    [Tooltip("L'icona che è sotto il mirino")]
    public Sprite actionSprite;
}

public class ObjectInteract : MonoBehaviour
{
    #region Variabili
    //Roba che serve per inspezionare gli oggetti
    private Vector3 cameraPos;
    private Vector3 cameraLookPos;
    private Quaternion cameraRot;
    private Vector3 lastObjPos;
    private Quaternion lastObjRot;
    private GameObject pickubleObj;

    public bool isInteracting = false;
    public bool isInspecting = false;
    public bool isTerminal = false;

    public GameObject inspect;
    private GameObject activeCanvas;
    public Text feedbacktext;
    public Image buttonFeedbackImage;
    public Button dummyButton;
    private Ray interactionRay;
    public GameObject torcia;
    public ScanButtonManager scan;
    private MenuControl menu;
    private float scanTime = 0.0f;
    public GameObject body;
    private bool pauseAbsorb = false;
    private RaycastTarget raycastTarget;

    [Header("Per i designer")]
    [Tooltip("Setta la distanza di interazione con gli oggetti")]
    public float dropDistance;

    [Tooltip("Setta la velocità di rotazione durante l'inspezione")]
    public int rotationSpeed;

    [Tooltip("Mettere Immagine del mirino")]
    public Image viewFinder;
    [Tooltip("Mettere Immagine delle azioni")]
    public Image actionImage;

    public Sprite pickubleSprite;
    public Sprite interactSprite;
    public Sprite absorbSprite;
    public Image scanPerc;

    [Header("Input Feedback manager")]
    public ObjectInputFeedback objectInputFeedback;
    public TerminalInputFeedback terminalInputFeedback;
    public ActionInputFeedback actionInputFeedback;
    public AbsorbInputFeedback absorbInputFeedback;

    #endregion

    void Start()
    {
        //Setta la posizione della telecamera all'inizio del gioco
        cameraPos = Camera.main.transform.position;
        menu = FindObjectOfType<MenuControl>();
    }

    void Update()
    {
        if (Input.GetAxis("Trigger") < -0.9f && raycastTarget != RaycastTarget.absorb)
        {
            FullOxygen();
        }
    }
    void FixedUpdate()
    {
        // Ray per l'interazione con gli oggetti dal centro dello schermo
        interactionRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.red);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.right, Color.green);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.up, Color.blue);

        // Viene chiamato il metodo per controllare l'interazione con gli oggetti interagibili
        CheckInteract();
    }

    void CheckInteract()
    {
        #region !isInteracting
        if (!isInteracting)
        {
            RaycastHit hit;
            if (Physics.Raycast(interactionRay, out hit, dropDistance))
            {
                GameObject interactedObject = hit.transform.gameObject;

                #region pickable
                // SE L'OGGETTO DEL RAYCAST E' PICKUBLE
                if (hit.collider.CompareTag("Pickuble"))
                {
                    raycastTarget = RaycastTarget.pickable;
                    InputUIFeedback();

                    //metto l'action image giusta
                    pickubleObj = hit.collider.gameObject;

                    if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E)) && (!menu || !menu.isMenu))
                    {
                        GetComponent<FirstPersonController>().enabled = false;

                        isInteracting = true;
                        isInspecting = true;

                        actionImage.gameObject.SetActive(false);
                        viewFinder.gameObject.SetActive(false);

                        lastObjPos = hit.collider.gameObject.transform.position;
                        lastObjRot = hit.collider.gameObject.transform.rotation;
                        inspect.transform.localPosition = new Vector3(0, 0, 0.2f) + new Vector3(0, 0, (1 * pickubleObj.transform.gameObject.GetComponent<ObjInformation>().near));
                        pickubleObj.transform.position = inspect.transform.position;
                        pickubleObj.transform.SetParent(inspect.transform);
                        pickubleObj.transform.localEulerAngles = new Vector3(-90, 0, 0);
                    }
                }
                #endregion

                #region terminal
                // SE L'OGGETTO DEL RAYCAST E' Terminal
                if (hit.collider.CompareTag("Terminal"))
                {
                    raycastTarget = RaycastTarget.terminal;
                    InputUIFeedback();

                    if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E)) && (!menu || !menu.isMenu))
                    {
                        isInteracting = true;
                        isTerminal = true;

                        actionImage.gameObject.SetActive(false);
                        viewFinder.gameObject.SetActive(false);

                        cameraRot = Camera.main.transform.rotation;
                        cameraPos = Camera.main.transform.position;
                        cameraLookPos = Camera.main.transform.position + Camera.main.transform.forward;
                        Vector3 endPos = hit.transform.Find("Main").position + (-hit.transform.Find("Main").transform.forward * 0.4f);
                        StartCoroutine(LerpCameraMovement(endPos, hit.transform.Find("Main").position));
                        body.SetActive(false);
                        //StartCoroutine(LerpLookAt(hit.transform.Find("Main").position));

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
                #endregion

                #region actionObj
                // SE L'OGGETTO DEL RAYCAST E' ActionObj
                if (hit.collider.CompareTag("ActionObj"))
                {
                    raycastTarget = RaycastTarget.action;
                    InputUIFeedback();

                    if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.E)) && (!menu || !menu.isMenu))
                    {
                        interactedObject.GetComponent<ActionObj>().DoStuff();
                    }
                }
                #endregion

                #region Absorb
                // SE L'OGGETTO DEL RAYCAST E' ActionObj
                if (hit.collider.CompareTag("Absorb"))
                {
                    raycastTarget = RaycastTarget.absorb;
                    InputUIFeedback();

                    if (Input.GetAxis("Trigger") < -0.9f)
                    {
                        GetComponent<FirstPersonController>().enabled = false;
                        PlayerStatus ps = GetComponent<PlayerStatus>();

                        if (ps.storageMaterial == AbrsorbType.nessuno && !pauseAbsorb)
                        {
                            AbsorbMaterial am = hit.collider.transform.GetComponent<AbsorbMaterial>();
                            if (am.material != AbrsorbType.nessuno)
                            {
                                scanTime += 0.5f * Time.deltaTime;
                                scanPerc.fillAmount = scanTime;
                                if (scanTime > 1)
                                {
                                    pauseAbsorb = true;
                                    ps.storageMaterial = am.material;
                                    am.material = 0;
                                    scanTime = 0.0f;
                                    scanPerc.fillAmount = scanTime;
                                }
                            }
                        }
                        else
                        {
                            AbsorbMaterial am = hit.collider.transform.GetComponent<AbsorbMaterial>();
                            if (am.material == AbrsorbType.nessuno && !pauseAbsorb)
                            {
                                scanTime += 0.5f * Time.deltaTime;
                                scanPerc.fillAmount = scanTime;
                                if (scanTime > 1)
                                {
                                    pauseAbsorb = true;
                                    am.material = ps.storageMaterial;
                                    ps.storageMaterial = 0;
                                    scanTime = 0.0f;
                                    scanPerc.fillAmount = scanTime;
                                }
                            }
                        }
                    }
                    else
                    {
                        pauseAbsorb = false;
                        GetComponent<FirstPersonController>().enabled = true;
                        scanTime = 0.0f;
                        scanPerc.fillAmount = scanTime;
                    }
                }
                #endregion
                if (hit.collider.CompareTag("Untagged"))
                {
                    raycastTarget = RaycastTarget.nothing;
                    InputUIFeedback();
                }
            }
            else
            {
                raycastTarget = RaycastTarget.nothing;
                InputUIFeedback();

            }
        }
        #endregion

        #region isinteracting
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

                if ((Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Escape)) && isInspecting && (!menu || !menu.isMenu))
                {
                    GetComponent<FirstPersonController>().enabled = true;
                    pickubleObj.transform.position = lastObjPos;
                    pickubleObj.transform.rotation = lastObjRot;
                    pickubleObj.transform.SetParent(null);
                    pickubleObj = null;

                    isInspecting = false;
                    isInteracting = false;

                    actionImage.gameObject.SetActive(true);
                    viewFinder.gameObject.SetActive(true);
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
                    //Camera.main.transform.rotation = cameraRot;
                    //Camera.main.transform.position = cameraPos;
                    StartCoroutine(LerpCameraMovement(cameraPos, cameraLookPos));
                    dummyButton.Select();
                    body.SetActive(true);
                    torcia.SetActive(true);

                    isTerminal = false;
                    isInteracting = false;

                    actionImage.gameObject.SetActive(true);
                    viewFinder.gameObject.SetActive(true);
                }
            }
        }
        #endregion
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

    IEnumerator LerpCameraMovement(Vector3 pos, Vector3 lookAt)
    {
        float elapsedTime = 0.0f;
        Vector3 startPos = Camera.main.transform.position;
        Vector3 startLook = Camera.main.transform.forward + Camera.main.transform.position;

        while (elapsedTime < 0.5f)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos, pos, (elapsedTime / 0.4f));
            Camera.main.transform.LookAt(Vector3.Lerp(startLook, lookAt, (elapsedTime / 0.4f)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void FullOxygen()
    {
        PlayerStatus ps = GetComponent<PlayerStatus>();
        if (ps.storageMaterial == AbrsorbType.ossigeno)
        {
            ps.storageMaterial = AbrsorbType.nessuno;
            GetComponent<OxygenScript>().FullOxygen();
        }
    }

    void InputUIFeedback()
    {
        switch (raycastTarget)
        {
            case RaycastTarget.nothing:
                {
                    actionImage.gameObject.SetActive(false);
                    buttonFeedbackImage.gameObject.SetActive(false);
                    feedbacktext.gameObject.SetActive(false);
                    break;
                }
            case RaycastTarget.pickable:
                {
                    buttonFeedbackImage.gameObject.SetActive(true);
                    feedbacktext.gameObject.SetActive(true);
                    actionImage.sprite = objectInputFeedback.actionSprite;
                    buttonFeedbackImage.sprite = objectInputFeedback.button;
                    feedbacktext.text = objectInputFeedback.textToView;
                    actionImage.gameObject.SetActive(true);
                    break;
                }
            case RaycastTarget.terminal:
                {
                    buttonFeedbackImage.gameObject.SetActive(true);
                    feedbacktext.gameObject.SetActive(true);
                    actionImage.sprite = terminalInputFeedback.actionSprite;
                    buttonFeedbackImage.sprite = terminalInputFeedback.button;
                    feedbacktext.text = terminalInputFeedback.textToView;
                    actionImage.gameObject.SetActive(true);
                    break;
                }
            case RaycastTarget.action:
                {
                    buttonFeedbackImage.gameObject.SetActive(true);
                    feedbacktext.gameObject.SetActive(true);
                    actionImage.sprite = actionInputFeedback.actionSprite;
                    buttonFeedbackImage.sprite = actionInputFeedback.button;
                    feedbacktext.text = actionInputFeedback.textToView;
                    actionImage.gameObject.SetActive(true);
                    break;
                }
            case RaycastTarget.absorb:
                {
                    buttonFeedbackImage.gameObject.SetActive(true);
                    feedbacktext.gameObject.SetActive(true);
                    actionImage.sprite = absorbInputFeedback.actionSprite;
                    buttonFeedbackImage.sprite = absorbInputFeedback.button;
                    feedbacktext.text = absorbInputFeedback.textToView;
                    actionImage.gameObject.SetActive(true);
                    break;
                }
        }
    }
}



