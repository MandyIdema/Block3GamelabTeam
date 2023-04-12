using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StarManager : NetworkBehaviour
{

    public GameObject starPrefab;

    private void Start()
    {
        RpcSpawnStars();
    }

    [ClientRpc]
    void RpcSpawnStars()
    {
        for (int i = 0; i < 10; i++)

        {
            int spawnPointX = Random.Range(-5, 5);
            int spawnPointY = Random.Range(-5, 5);
            Vector2 spawnPosition = new Vector2(spawnPointX, spawnPointY);
            GameObject star = Instantiate(starPrefab, spawnPosition, transform.rotation);
            NetworkServer.Spawn(star);
        }
    }
}
