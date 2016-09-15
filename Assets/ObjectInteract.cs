using UnityEngine;
using System.Collections;

public class ObjectInteract : MonoBehaviour
{

    private Vector3 lastObjPos;
    private bool isPickuble = false;
    private GameObject pickubleObj;
    private bool isInspecting = false;

    public int rotationSpeed;

	void Start ()
    {
	
	}
	
	void Update ()
    {
        RaycastHit hit;
        if (!isInspecting)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 52.5f))
            {

                if (hit.collider.tag == "Pickuble")
                {
                    isPickuble = true;
                    pickubleObj = hit.collider.gameObject;
                    Debug.DrawLine(Camera.main.transform.position, hit.collider.transform.position);
                }
                else
                {
                    isPickuble = false;
                    pickubleObj = null;
                }
            }

            if (Input.GetKeyUp(KeyCode.K) && isPickuble)
            {
                isInspecting = true;
                
                pickubleObj.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 1.5f);
            }            
        }

        if (isInspecting)
        {
            Debug.Log(isInspecting);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pickubleObj.transform.Rotate(pickubleObj.transform.rotation.x, pickubleObj.transform.rotation.y + rotationSpeed, pickubleObj.transform.rotation.z);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                pickubleObj.transform.Rotate(pickubleObj.transform.rotation.x, pickubleObj.transform.rotation.y - rotationSpeed, pickubleObj.transform.rotation.z);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                pickubleObj.transform.Rotate(pickubleObj.transform.rotation.x, pickubleObj.transform.rotation.y, pickubleObj.transform.rotation.z - rotationSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                pickubleObj.transform.Rotate(pickubleObj.transform.rotation.x, pickubleObj.transform.rotation.y, pickubleObj.transform.rotation.z + rotationSpeed);
            }
        }
    }
}
