using UnityEngine;
using System.Collections;

public class IemController : MonoBehaviour
{
    public int secOfStun;
    public float speed;

    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter()
    {

    }
}
