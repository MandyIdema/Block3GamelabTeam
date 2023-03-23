using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;
    GameObject[] players;

     void Awake()
    {
        //players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void FixedUpdate()
    {
        if (players.Length>0)
        {
            foreach(GameObject i in players)
            {
                if (i.name=="Local")
                {
                    followTransform = i.transform;
                    Debug.Log("Found it");
                }
            }
            //followTransform = players[0].transform;
            this.transform.position = new Vector3(followTransform.position.x, followTransform.position.y, followTransform.position.z-10);
        }
    }
}
