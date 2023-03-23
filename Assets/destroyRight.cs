using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyRight : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetKeyDown("space"))
        {
            Destroy(this.gameObject);
            print("Destroyed");
        }
    }
}
