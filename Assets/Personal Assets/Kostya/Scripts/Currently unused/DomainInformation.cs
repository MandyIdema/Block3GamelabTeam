using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainInformation : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBehaviour>() != null)
        {
            collision.GetComponent<PlayerBehaviour>().onDomain = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBehaviour>() != null)
        {
            collision.GetComponent<PlayerBehaviour>().onDomain = false;
        }
    }
}
