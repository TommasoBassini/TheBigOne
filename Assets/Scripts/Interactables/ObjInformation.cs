using UnityEngine;
using System.Collections;

public class ObjInformation : MonoBehaviour
{
    // Set della distanza della telecamera quando si ispeziona un oggetto
    [Range(0f, 1f)]
    [Tooltip("Setta la distanza dell'oggetto ispezionato dalla camera. 0 = vicino, 1 = lontano")]
    public float near;

    public GameObject objToView;
    public string datiMedici;
    public string datiIngegneria;
    public string datiSicurezza;
    public Sprite objPreview;

    public bool isScanning = false;
}
