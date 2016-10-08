using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public AudioClip Audio1;
    public AudioSource Audio2;
    public bool IsAlreadyPlayed = false;
    //public AudioSource Audio2;

public void Start()
    {
    Audio2 = GetComponent<AudioSource>();
    }

public void OnTriggerEnter(Collider Coll)
    {
        if (Coll && IsAlreadyPlayed == false)
        {
            IsAlreadyPlayed = true;
            Audio2.Play();
            Invoke("Destruction", Audio2.clip.length + 0.2f);
        }

    }
        public void Destruction()
    {
        Destroy(this.gameObject);
    }
}
