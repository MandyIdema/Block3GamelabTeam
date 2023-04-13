using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace GM
{
    public class StarManager : NetworkBehaviour
    {

        public GameObject starPrefab;
        public List<GameObject> stars = new List<GameObject>();
        public int starsTaken;

        private void Start()
        {
            RpcSpawnStars();
        }
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

        [ClientRpc]
        void RpcSpawnStars()
        {
            for (int i = 0; i < 10; i++)

            {
                int spawnPointX = Random.Range(-5, 5);
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

