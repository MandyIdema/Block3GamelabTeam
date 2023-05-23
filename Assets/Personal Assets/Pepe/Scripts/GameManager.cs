using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace GM
{
    public class GameManager : NetworkBehaviour
    {
        [Header("Game Info")]
        [SerializeField] public List<GameObject> players = new List<GameObject>(); 
        // public Dictionary<GameObject, int> starsCollected = new Dictionary<GameObject, int>();
        [HideInInspector] [SyncVar] public bool allPlayersAppeared = false;
        public enum GameStatus
        {
            Pending,
            Started,
            Finished
        }
        [SyncVar] public GameStatus currentStatus = GameStatus.Pending;

        [Space]

        [Header("Domains")]
        [SerializeField] private GameObject domainInfo;
        // [SyncVar] public int starsCollectedInTotal = 0;
        // [SerializeField] private TextMeshProUGUI _starsCollected;
        [SerializeField] private GameObject barriers;

        [Space]

        [Header("Other stuff")]
        public StarManager _sm;

        private void Awake()
        {
            _sm = FindObjectOfType<StarManager>();
        }
        void Update()
        {

            if (players.Count < 4 && !allPlayersAppeared)
            {
                // Awkward code but I do not currently see a way around it
                // All player objects are returned in an array via the FindGameObjectsWithTag function
                // But players have to be in a list, since it has to be resizeable
                GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
                if (playerObjects.Length != players.Count)
                {
                    players.Clear();
                    players.AddRange(playerObjects);
                }
                // Using dictionaries is impossible because empty players remain in them and the star stats do not update
                for (int i = 0; i < players.Count - 1; i++)
                {
                    if(players[i] == null)
                    {
                        players.RemoveAt(i);
                    }
                }
            }
            if (currentStatus == GameStatus.Started)
            {

            }

            if (_sm.starsTaken >= _sm.starsNeeded)
            {
                EndGame();
            }
        }
        void LateUpdate()
        {
            if (players.Count > 1 && currentStatus == GameStatus.Pending)
            {
                CheckingDomains();
            }
        }

        public void EndGame()
        {
            currentStatus = GameStatus.Finished;
            XMLManager.instance.SaveStarScore();
            Debug.Log("Current score is " + XMLManager.instance.LoadStarScore());
            if (currentStatus == GameStatus.Finished)
            {
                SceneManager.LoadScene(2);
            }
        }
        public void CheckingDomains()
        {
            var domainCount = 0;
            var domains = domainInfo.GetComponent<DomainInfoGiver>().domains;
            for (int i = 1; i < domains.Count; i++)
            {
                if (domains[i].GetComponent<DomainInformation>().currentStatus == DomainInformation.DomainStatus.Chosen)
                {
                    domainCount++;
                }
            }
            if (players.Count == domainCount)
            {
                currentStatus = GameStatus.Started;
                barriers.SetActive(false);
            }
        }
    }
}

