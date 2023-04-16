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

        public GameObject enemy;
        public float distanceBetweenObjects = 2f;
        public List<Vector2> points = new List<Vector2>();

        [Space]

        [Header("Stars")]
        public int starsNeeded = 1; // Number of stars required to finish the game
        [SyncVar] private int starsSpawnedTotal = 0; // Total number of stars spawned
        [SyncVar] public int starsTaken; // Current number of stars possessed by all the players in total
        public List<GameObject> starObjects = new List<GameObject>(); // All of the star objects

        [Space]

        [Header("Spawn Areas")]
        public List<GameObject> spawnAreas = new List<GameObject>(); // All of the areas where the stars should spawn
        public List<int> starsWithinAnArea = new List<int>(); // Number of stars within the specific area

        private void Start()
        {
            // Templates for later implementation
            // ObjectPositioning();
            // ObjectSpawning();

            RpcPositionPoints();
            RpcSpawnStars();
        }

        //void ObjectPositioning()
        //{
        //    int count = Random.Range(2, 6);

        //    for (int i = 0; i < count;)
        //    {
        //        float x = Random.Range(-2.0f, 2.0f);
        //        float y = Random.Range(-4.0f, 4.0f);
        //        Vector2 point = new Vector2(x, y);

        //        if (points.Count == 0)
        //        {
        //            points.Add(point);
        //            i++;
        //            continue;
        //        }

        //        for (int j = 0; j < points.Count; j++)
        //        {

        //            if ((point - points[j]).sqrMagnitude > distanceBetweenObjects * distanceBetweenObjects)
        //            {
        //                if (j == points.Count - 1)
        //                {
        //                    points.Add(point);
        //                    i++;
        //                }
        //                continue;
        //            }
        //            break;
        //        }
        //    }
        //}

        //void ObjectSpawning()
        //{
        //    for (int i = 0; i < points.Count; i++)
        //    {
        //        Instantiate(enemy, points[i], Quaternion.identity);
        //    }
        //}

    private void FixedUpdate()
        {
            // Disables the door if the players have collected enough stars
            RpcDestroyDoor();
        }

        // Checks how many stars the players have collected in total
        public void CheckStars()
        {
            starsTaken = 0;
            for (int j = 0; j < starObjects.Count; j++)
            {
                if (starObjects[j].GetComponent<StarProperty>().currentStatus == StarProperty.StarStatus.Taken)
                {
                    starsTaken++;
                }
            }
        }



        [ClientRpc]
        void RpcDestroyDoor()
        {
            if (starsTaken >= starsNeeded)
            {
                exitDoor.SetActive(false);
            }
        }

        [ClientRpc]
        void RpcPositionPoints()
        {

        }
        // Has some weird properties if spawned without a Client Rpc declaration
        [ClientRpc]
        void RpcSpawnStars()
        {
            for (int i = 0; i < spawnAreas.Count; i++)
            {
                for (int j = 0; j < starsWithinAnArea[i]; j++)
                {
                    Vector2 spawnPosition = new Vector2(Random.Range(-0.6f, 0.6f), Random.Range(-0.8f, 0.8f));
                    spawnPosition = spawnAreas[i].transform.TransformPoint(spawnPosition * .5f);
                    GameObject star = Instantiate(starPrefab, spawnPosition, transform.rotation); 
                    // This has to be done BEFORE the loop because bounds.extents.x does NOT work unless the game object is instantiated already
                    // For some reason OverlapCircle works in a VERY questionable manner and I cannot understand why

                    star.name = "Star " + (starsSpawnedTotal + j + 1);

                    NetworkServer.Spawn(star);
                    starObjects.Add(star);
                }
                starsSpawnedTotal += starsWithinAnArea[i];
            }
            starsNeeded = starsSpawnedTotal - 15;
        }
    }
}

