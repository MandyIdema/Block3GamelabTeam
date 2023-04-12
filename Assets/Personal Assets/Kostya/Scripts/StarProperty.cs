using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StarProperty : NetworkBehaviour
{

    [ClientCallback]
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Debug.Log("This is now owned by the client");
    }
    [ClientCallback]
    public override void OnStopAuthority()
    {
        base.OnStopAuthority();
        Debug.Log("This is now not owned by the client");
        // gameObject.SetActive(true);
    }
}
