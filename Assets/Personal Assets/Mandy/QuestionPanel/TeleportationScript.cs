using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeleportationScript : NetworkBehaviour
{
    public GameObject teleportationDestination;
    public GameObject localPlayer;

    // Start is called before the first frame update
    void Start()
    {
        localPlayer = NetworkClient.localPlayer.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
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

    }

    // This allows to disable the door for everyone
    [Command(requiresAuthority = false)]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
