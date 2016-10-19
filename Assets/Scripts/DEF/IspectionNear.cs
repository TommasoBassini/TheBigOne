using UnityEngine;
using System.Collections;

public class IspectionNear : MonoBehaviour
{
    // Set della distanza della telecamera quando si ispeziona un oggetto
    [Range(0f, 1f)]
    [Tooltip("Setta la distanza dell'oggetto ispezionato dalla camera. 0 = vicino, 1 = lontano")]
    public float near;
}
