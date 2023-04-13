using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace GM
{
    public enum GameStatus
    {
        Pending,
        Started,
        Finished
    }
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private GameObject[] players; // That stays
                                                       // [K] Add a script to collect the info from all the player objects regarding how many stars (int starsCollected
                                                       // in the PlayerBehaviour script) they have collected in total (basically loop through all of the objects and add
                                                       // the stars from every player object script)
        public Dictionary<GameObject, int> starsCollected = new Dictionary<GameObject, int>();
        [SerializeField] private GameObject domainInfo;
        [SyncVar] public int starsCollectedInTotal = 0;
        [SerializeField] private TextMeshProUGUI _starsCollected;
        [SerializeField] private GameObject barriers;
        [SyncVar] public bool allPlayersAppeared = false;
        private GameStatus currentStatus = GameStatus.Pending;

        void Update()
        {

            if (players.Length < 2 && !allPlayersAppeared)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                for(int i = 0; i < players.Length - 1; i++)
                {
                    starsCollected.Add(players[i], 0);
                }
            }
            if (currentStatus == GameStatus.Started)
            {
                StarsScore();
            }


            // THIS PART OF THE CODE IS REDUNDANT, CHECK THE STAR MANAGER FOR THE ACTUAL TRACKER
            starsCollectedInTotal = 0;
            foreach (int value in starsCollected.Values)
            {
                //foreach (GameObject objects in starsCollected.Keys)
               // {
                    //if (objects != null)
                    //{
                        starsCollectedInTotal += value;
                    //}
                //}

            }

        }
        void LateUpdate()
        {
            if (players.Length > 1 && currentStatus == GameStatus.Pending)
            {
                CheckingDomains();
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
            if (players.Length == domainCount)
            {
                currentStatus = GameStatus.Started;
                Destroy(barriers);
            }
        }

        private void StarsScore()
        {
            for (int i = 0; i <= players.Length + 1; i++)
            {
                _starsCollected.GetComponent<TextMeshProUGUI>().text = players[i].GetComponent<PlayerBehaviour>().playerNameText + ":" + players[i].GetComponent<PlayerBehaviour>().starsCollected;
            }
        }
    }
}

