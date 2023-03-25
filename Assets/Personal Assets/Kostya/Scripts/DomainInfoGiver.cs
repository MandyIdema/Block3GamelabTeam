using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainInfoGiver : MonoBehaviour
{

    public List<GameObject> domains = new();
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>() != null)
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().domainObjects.InsertRange(0, domains);
        }
    }
}
