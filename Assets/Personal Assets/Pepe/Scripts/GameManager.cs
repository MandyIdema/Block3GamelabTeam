using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace GM
{
    public class GameManager : NetworkBehaviour
    {
        [Header("Game Info")]
        [SerializeField] public List<GameObject> players = new List<GameObject>(); 
        [HideInInspector] [SyncVar] public bool allPlayersAppeared = false;
        public enum GameStatus
        {
            Pending,
            Started,
            Finished
        }
        [SyncVar] public GameStatus currentStatus = GameStatus.Pending;

        [Space]

        [Header("Other stuff")]
        public StarManager _sm;
        public Referencer _Referencer;
        public GameObject onlineSceneCanvas;

        private void Awake()
        {
            _sm = FindObjectOfType<StarManager>();
        }
        void Update()
        {

            if (_Referencer == null)
            {
                _Referencer = FindObjectOfType<Referencer>();
            }

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
                    
                    if (players[i] == null)
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
                if (isServer && currentStatus != GameStatus.Finished)
                {
                    currentStatus = GameStatus.Finished;
                    RpcEndGame();
                }
            }
        }
        [ClientRpc]
        public void RpcEndGame()
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerBehaviour>().movementBlocked = true;
            } 
            Debug.Log("This message should only show up once!");
            if (isLocalPlayer)
            {
                XMLManager.instance.SaveStarScoreGame();
                Debug.Log("Current score is " + XMLManager.instance.LoadStarScore());
            }
            _Referencer.MainMenuBackground.SetActive(true);
            _Referencer.EndGamePanelButtons.SetActive(true);
            onlineSceneCanvas.SetActive(false);
        }
    }
}

