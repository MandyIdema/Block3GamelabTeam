using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PowerUpManager : NetworkBehaviour
{
    public List<Transform> powerUpSpawns;
    public List<GameObject> powerUpPrefabs;
    public List<GameObject> readyPowerUps;

    [Space]

    [SyncVar] public GameObject powerUpUI;
    private void Start()
    {
        if (isServer)
        {
            RpcSpawnPowerUps();
        }
    }

    [ClientRpc]
    void RpcSpawnPowerUps()
    {
        for (int i = 0; i < powerUpSpawns.Count; i++)
        {
            GameObject powerUp = Instantiate(powerUpPrefabs[(int)Random.Range(0, powerUpPrefabs.Count)], powerUpSpawns[i].transform.position, Quaternion.identity);
            powerUp.name = "Power-up #" + (i + 1);
            NetworkServer.Spawn(powerUp);
            readyPowerUps.Add(powerUp);
        }
    }
}
