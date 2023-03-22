using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InstantiateTest : NetworkBehaviour
{

    public GameObject cubePrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKey(KeyCode.X))
        {
            CmdSpawnCube();
        }
    
    }

    [Command]
    void CmdSpawnCube()
    {
        if(cubePrefab != null)
        {
            Vector3 spawnPos = transform.position + transform.forward * 2;
            Quaternion spawnRot = transform.rotation;
            GameObject cube = Instantiate(cubePrefab, spawnPos, spawnRot);
            NetworkServer.Spawn(cube);
        }

    }
}
