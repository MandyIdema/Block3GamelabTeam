using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace GM
{
    public class StarManager : NetworkBehaviour
    {
        [Header("Prefabs")]
        public GameObject starPrefab;
        public GameObject exitDoor;

        [Space]

        [Header("Stars")]
        [SyncVar] public int starsTaken; // Current number of stars possessed by all the players in total
        public int starsNeeded = 5; // Number of stars required to finish the game
        public int starsSpawned = 10; // Number of stars spawned
        public List<GameObject> stars = new List<GameObject>(); // All of the star objects

        private void Start()
        {
            RpcSpawnStars();
        }

        private void FixedUpdate()
        {
            // Disables the door if the players have collected enough stars
            if (starsTaken >= starsNeeded)
            {
                exitDoor.SetActive(false);
            }
        }

        // Checks how many stars the players have collected in total
        public void CheckStars()
        {
            starsTaken = 0;
            for (int j = 0; j < stars.Count; j++)
            {
                if (stars[j].GetComponent<StarProperty>().currentStatus == StarProperty.StarStatus.Taken)
                {
                    starsTaken++;
                }
            }
        }

        // Has some weird properties if spawned without a Client Rpc declaration
        [ClientRpc]
        void RpcSpawnStars()
        {
            for (int i = 0; i < starsSpawned; i++)

            {
                int spawnPointX = Random.Range(2, 12);
                int spawnPointY = Random.Range(-5, 5);
                Vector2 spawnPosition = new Vector2(spawnPointX, spawnPointY);
                GameObject star = Instantiate(starPrefab, spawnPosition, transform.rotation);
                star.name = "Star " + i;
                NetworkServer.Spawn(star);
                stars.Add(star);
            }
        }
    }
}

