using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellRinging : MonoBehaviour
{
    public AudioSource bellSource;
    public AudioClip clinkSound;
    public GameObject questionUI;
    void Start()
    {
        bellSource.clip = clinkSound;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().currentInteractiveObject = gameObject;
            collision.gameObject.GetComponent<PlayerBehaviour>().inInteractionRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().currentInteractiveObject = null;
            collision.gameObject.GetComponent<PlayerBehaviour>().inInteractionRange = false;
        }
    }

    public void RingingBell()
    {
        Debug.Log("this is triggered");
        bellSource.Play();
    }
}
