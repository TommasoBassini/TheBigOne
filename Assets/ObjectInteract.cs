using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class ObjectInteract : MonoBehaviour
{

    private Vector3 lastObjPos;
    private Quaternion lastObjRot;
    private bool isPickuble = false;
    private GameObject pickubleObj;
    private bool isInspecting = false;

    public int rotationSpeed;

	void Start ()
    {
        Cursor.SetCursor(null, new Vector2(Screen.width / 2, Screen.height / 2),CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update ()
    {
        RaycastHit hit;
        if (!isInspecting)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 52.5f))
            {
                Debug.Log(hit.collider);
                if (hit.collider.tag == "Pickuble")
                {
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
                    }
                    return;
                }
                else
                {
                    isPickuble = false;
                    pickubleObj = null;
                }

                if (hit.collider.transform.gameObject.GetComponent<Button>() != null)
                {
                    
                    hit.collider.transform.gameObject.GetComponent<Button>().Select();
                }
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
                pickubleObj.transform.eulerAngles += new Vector3(0, 0, -angV * rotationSpeed);
            }

            if (Input.GetKeyUp(KeyCode.Joystick1Button0) && isInspecting)
            {
                GetComponent<FirstPersonController>().enabled = true;
                pickubleObj.transform.position = lastObjPos;
                pickubleObj.transform.rotation = lastObjRot;
                pickubleObj = null;
                isInspecting = false;
            }
        }
    }
}
