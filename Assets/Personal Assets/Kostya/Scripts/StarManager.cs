using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine;
using Mirror;

namespace GM
{
    public class StarManager : NetworkBehaviour
    {

        public Animator anim;

        [Header("Prefabs")]
        public GameObject starPrefab;
        public GameObject exitDoor;
        public Transform parentStarObject;

        //[HideInInspector] public GameObject enemy;
        //[HideInInspector] public float distanceBetweenObjects = 2f;
        //[HideInInspector] public List<Vector2> points = new List<Vector2>();

        [Space]

        [Header("Stars")]
        [SyncVar] public int starsNeeded = 100; // Number of stars required to finish the game
        [SyncVar] private int starsSpawnedTotal = 0; // Total number of stars spawned
        [SyncVar] public int starsTaken = 0; // Current number of stars possessed by all the players in total
        public List<GameObject> starObjects = new List<GameObject>(); // All of the star objects

        [Space]

        [Header("Spawning Areas")]
        public List<GameObject> spawnAreas = new List<GameObject>(); // All of the areas where the stars should spawn
        public List<int> starsWithinAnArea = new List<int>(); // Number of stars within the specific area

        [Space]

        [Header("Other Stuff")]
        public GameManager _gm;
        [SyncVar] public bool positionUpdate = false;
        private void Awake()
        {
            _gm = FindObjectOfType<GameManager>();
        }
    private void Start()
        {
            if (isServer)
            {
                RpcSpawnStars();

                if (!positionUpdate)
                {
                    RpcClientStarSpawn();
                }
            }

            Analytics();
        }

        async void Analytics()
        {
            try
            {
                await UnityServices.InitializeAsync();
                List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            }
            catch (ConsentCheckException e)
            {
                // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
            }
        }
        // [K] A sorting mechanism to reposition all of the stars for the player
        IEnumerator UpdateStarPosition()
        {
            foreach (GameObject starObject in starObjects)
            {
                while (CheckStarOverlap(starObject))
                {
                    starObject.transform.position = DetermineSpawnPosition(starObject.GetComponent<StarProperty>().spawnArea);
                    Debug.Log($"The position of the {starObject.name} was changed");
                    CheckStarOverlap(starObject);
                    yield return null;
                }
                NetworkServer.Spawn(starObject);
                starObject.GetComponent<SpriteRenderer>().enabled = true;

            }
           
        }

        IEnumerator BeginningUpdate(float waitTime)
        {
            positionUpdate = true;
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(UpdateStarPosition());
        }

        [ClientRpc]
        void RpcClientStarSpawn()
        {
            StartCoroutine(BeginningUpdate(0.5f));
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

                    Dictionary<string, object> parameters = new Dictionary<string, object>()
{
                    { "StarsCollected", starsTaken++ }
};
                    // The ‘myEvent’ event will get queued up and sent every minute
                    AnalyticsService.Instance.CustomData("starsCollected", parameters);
                }
                
            }
        }

        // Has some weird properties if spawned without a Client Rpc declaration
        [ClientRpc]
        void RpcSpawnStars()
        {
            for (int i = 0; i < spawnAreas.Count; i++)
            {
                for (int j = 0; j < starsWithinAnArea[i]; j++)
                {
                    Vector2 spawningPosition = DetermineSpawnPosition(i);
                    GameObject star = Instantiate(starPrefab, spawningPosition, transform.rotation);
                    // NetworkServer.Spawn(star);
                    star.GetComponent<StarProperty>().spawnArea = i;
                    star.name = "Star " + (starsSpawnedTotal + j + 1);
                    star.transform.SetParent(parentStarObject);
                    starObjects.Add(star);
                }
                starsSpawnedTotal += starsWithinAnArea[i];
            }
            starsNeeded = starsSpawnedTotal - 15;
        }

        // [K] Function to determine a random positioning within the spawn area
        Vector2 DetermineSpawnPosition(int currentArea)
        {
            Vector2 spawnPosition = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            spawnPosition = spawnAreas[currentArea].transform.TransformPoint(spawnPosition * .5f);
            return spawnPosition;
        }

        // [K] Checks if the star overlaps with another star or an environmental object
         bool CheckStarOverlap(GameObject starObject)
        {
            if (Physics2D.IsTouchingLayers(starObject.GetComponent<Collider2D>(), LayerMask.GetMask("Stars"))
                || Physics2D.IsTouchingLayers(starObject.GetComponent<Collider2D>(), LayerMask.GetMask("Objects")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

