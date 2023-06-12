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

        public GameObject Door_Closed;
        public GameObject Door_Open;
        private AudioManager audioManager;

        private void Awake()
        {
            _sm = FindObjectOfType<StarManager>();
            Door_Closed.SetActive(true);
            Door_Open.SetActive(false);
            audioManager = FindObjectOfType<AudioManager>();
        }
        void Update()
        {

            if (_Referencer == null)
            {
                _Referencer = FindObjectOfType<Referencer>();
            }

            if (players.Count < 4 && !allPlayersAppeared)
            {
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
                if (isServer)
                {
                    int PlayersReady = 0;
                    foreach (GameObject player in players)
                    {
                        if (player.GetComponent<PlayerBehaviour>().movementBlocked == false)
                        {
                            // player.GetComponent<PlayerBehaviour>().currentStatus = PlayerBehaviour.PlayerStatus.Ready;
                        }
                        if (player.GetComponent<PlayerBehaviour>().currentStatus == PlayerBehaviour.PlayerStatus.Ready)
                        {
                            PlayersReady += 1;
                        }
                    }
                    if (PlayersReady == players.Count && players.Count > 1 && currentStatus != GameStatus.Started)
                    {
                        currentStatus = GameStatus.Started;
                    }
                }


                //You don't do anything with this yet?
                if (currentStatus == GameStatus.Started)
                {
                    //Open the doors when game started
                    Door_Closed.SetActive(false);
                    Door_Open.SetActive(true);
                }

                if (_sm.starsTaken >= _sm.starsNeeded)
                {
                    if (isServer && currentStatus != GameStatus.Finished)
                    {
                        currentStatus = GameStatus.Finished;
                        audioManager.SFX.clip = audioManager.endSound;
                        audioManager.SFX.Play();
                        audioManager.StopMusic();
                        audioManager.PlayMusic();
                        RpcEndGame();
                    }
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

