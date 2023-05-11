using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeleportationScript : NetworkBehaviour
{
    public GameObject teleportationDestination;
    public float destructionTime = 3.0f;
    private GameObject localPlayer;
    private bool teleported = false;

    // Start is called before the first frame update
    void Start()
    {
        localPlayer = NetworkClient.localPlayer.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestionScript.QuestionAwnsered && GetComponent<QuestionRandomizer>().enteredDoors && !teleported){
            Teleport(localPlayer);
            // QuestionScript.isEnabled = false;
        }
    }

/*     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (localPlayer)
        {
            if (QuestionScript.QuestionAwnsered)
            {
                if (collision.gameObject == localPlayer)
                {
                    localPlayer.transform.position = teleportationDestination.transform.position;
                    DestroyObject();
                }
            }
        }

    } */

    private void Teleport(GameObject _player){
        if (_player == localPlayer)
        {
            if (QuestionScript.QuestionAwnsered)
            {
                teleported = true;
                localPlayer.transform.position = teleportationDestination.transform.position;
                DestroyObject();
            }
        } 
    }

    // This allows to disable the door for everyone
    [Command(requiresAuthority = false)]
    void DestroyObject()
    {
        Destroy(gameObject, destructionTime);
    }
}
