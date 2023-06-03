using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planet_interact : MonoBehaviour
{

    public static bool PlanetInteract;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlanetInteract = true;
        }
        else
        {
            PlanetInteract = false;
        }
    }
}
