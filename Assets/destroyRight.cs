using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class destroyRight : NetworkBehaviour
{

    public void Update()
    {

    }
    public void OnCollisionStay2D(Collision2D collision)
    {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdDestroyingObject();
            }

    }

    [Command(requiresAuthority = false)]
    public void CmdDestroyingObject()
    {
        NetworkServer.Destroy(gameObject);
    }
}
