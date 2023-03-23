using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    public Transform followTransform;
    GameObject[] players;

     void Awake()
    {
        //players = GameObject.FindGameObjectsWithTag("Player");
    }
    // Start is called before the first frame update

    void FixedUpdate()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length>0)
        {
            
            followTransform = players[0].transform;
            this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y, followTransform.position.z-10);
        }
    }
}
