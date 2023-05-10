using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;

namespace GM
{
    public class GameManager : NetworkBehaviour
    {
        [Header("Players")]
        [SerializeField] private List<GameObject> players = new List<GameObject>(); 
        private int playersEntered;
        // public Dictionary<GameObject, int> starsCollected = new Dictionary<GameObject, int>();
        [SyncVar] public bool allPlayersAppeared = false;
        public enum GameStatus
        {
            Pending,
            Started,
            Finished
        }
        private GameStatus currentStatus = GameStatus.Pending;

        [Space]

        [Header("Domains")]
        [SerializeField] private GameObject domainInfo;
        // [SyncVar] public int starsCollectedInTotal = 0;
        // [SerializeField] private TextMeshProUGUI _starsCollected;
        [SerializeField] private GameObject barriers;

        void Update()
        {

            if (players.Count < 4 && !allPlayersAppeared)
            {
                // Awkward code but I do not currently see a way around it
                // All player objects are returned in an array via the FindGameObjectsWithTag function
                // But players have to be in a list, since it has to be resizeable
                GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
                if (playerObjects.Length != players.Count + 1)
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
               // StarsScore();
            }
            if(currentStatus == GameStatus.Finished){
                Debug.Log("GAME FINISHED");
                SceneManager.LoadScene(2);
            }


            //THIS PART OF THE CODE IS REDUNDANT, CHECK THE STAR MANAGER FOR THE ACTUAL TRACKER
            //starsCollectedInTotal = 0;
            //foreach (int value in starsCollected.Values)
            //{
            //    foreach (GameObject objects in starsCollected.Keys)
            //    {
            //        if (objects != null)
            //        {
            //            starsCollectedInTotal += value;
            //        }
            //    }

            //}

        }
        void LateUpdate()
        {
            if (players.Count > 1 && currentStatus == GameStatus.Pending)
            {
                CheckingDomains();
            }
        }

        void OnTriggerEnter2D(Collider2D collider2D)
        {   
            if(collider2D.tag == "Player" && currentStatus == GameStatus.Started){
                if(!collider2D.GetComponent<PlayerBehaviour>().gameFinished){
                    playersEntered++;
                    collider2D.GetComponent<PlayerBehaviour>().gameFinished = true;
                }
            }
            if(playersEntered==players.Count){
              //  currentStatus = GameStatus.Finished;
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

        //private void StarsScore()
        //{
        //    for (int i = 0; i <= players.Count + 1; i++)
        //    {
        //        _starsCollected.GetComponent<TextMeshProUGUI>().text = players[i].GetComponent<PlayerBehaviour>().playerNameText + ":" + players[i].GetComponent<PlayerBehaviour>().starsCollected;
        //    }
        //}
    }
}

