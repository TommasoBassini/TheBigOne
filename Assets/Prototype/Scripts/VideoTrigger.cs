using UnityEngine;
using System.Collections;

public class VideoTrigger : MonoBehaviour {

    public GameObject monitor;

    private Renderer r;
    private MovieTexture movie;
    private AudioSource audioSource;
    private bool triggered;
    
    void Start ()
    {
        r = monitor.GetComponent<Renderer>();
        movie = (MovieTexture)r.material.mainTexture;
        movie.loop = true;
        audioSource = monitor.GetComponent<AudioSource>();
    }
	
	void OnTriggerEnter (Collider other)
    {
	    if (!triggered && other.tag == "Player")
        {
            triggered = true;
            monitor.SetActive(true);
            movie.Play();
            audioSource.Play();
        }
	}
}
