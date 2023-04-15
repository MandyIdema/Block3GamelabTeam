using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

namespace GM
{
    public class StarProperty : NetworkBehaviour
    {
        public enum StarStatus
        {
            Free,
            Taken
        }

        [SyncVar] public StarStatus currentStatus;
        [SerializeField] private GameObject playerOwner;
        public GameManager _gm;
        public StarManager _sm;
        public void Start()
        {
            
        }

        private void Update()
        {

            // Searches for Game and Star Manager
            if (_gm == null)
            {
                // _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                _gm = FindObjectOfType<GameManager>();
            }
            if (_sm == null)
            {
                // _sm = GameObject.FindGameObjectWithTag("StarManager").GetComponent<StarManager>();
                _sm = FindObjectOfType<StarManager>();
            }

            // Resets the status of the star if its owner left the game
            if (currentStatus == StarStatus.Taken && playerOwner == null)
            {
                currentStatus = StarStatus.Free;

                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;

                _sm.CheckStars();  // !!! CRUCIAL TO UPDATING THE STATS
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerEnter(collision);
            }
        }

        void PlayerEnter(Collider2D player)
        {
            // Updates player stats
            playerOwner = player.gameObject;
            playerOwner.GetComponent<PlayerBehaviour>().starsCollected++;

            // Updates star stats
            currentStatus = StarStatus.Taken;

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            _sm.CheckStars(); // !!! CRUCIAL TO UPDATING THE STATS

            // _gm.starsCollected.TryGetValue(playerOwner, out int j);
            // _gm.starsCollected[playerOwner] = j + 1;
        }

        // Client authority experiments
        [ClientCallback]
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            Debug.Log("This is now owned by the client");
        }
        [ClientCallback]
        public override void OnStopAuthority()
        {
            base.OnStopAuthority();
            Debug.Log("This is now not owned by the client");
            // gameObject.SetActive(true);
        }
    }
}
