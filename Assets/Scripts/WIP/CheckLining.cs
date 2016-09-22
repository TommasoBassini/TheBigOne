using UnityEngine;
using System.Collections;

public class CheckLining : MonoBehaviour {

	void OnTriggerEnter ( Collider col )
    {
        if (col.gameObject.tag == "muro")
        {
            Destroy(col.gameObject);
        }
    }

}
