using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class touchtest : NetworkBehaviour
{
    public GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 spawnPos = transform.position + transform.forward * 2;
        Quaternion spawnRot = transform.rotation;
        GameObject cube = Instantiate(cubePrefab, spawnPos, spawnRot);
        NetworkServer.Spawn(cube);
    }
}
