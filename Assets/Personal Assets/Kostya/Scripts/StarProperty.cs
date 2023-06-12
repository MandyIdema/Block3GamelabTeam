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
        [HideInInspector] public int spawnArea;
        [HideInInspector] public GameManager _gm;
        [HideInInspector] public StarManager _sm;
        [HideInInspector] public StarBar _sb;
        private AudioManager audioManager;

        public GameObject light_Star;
        public void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            audioManager.SFX.clip = audioManager.attain;
        }

        private void Update()
        {
            if (_sm == null)
            {
                _sm = FindObjectOfType<StarManager>();
            }

            if (_sb == null)
            {
                _sb = FindObjectOfType<StarBar>();
            }

            // Resets the status of the star if its owner left the game
            if (currentStatus == StarStatus.Taken && playerOwner == null)
            {
                currentStatus = StarStatus.Free;

                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;

                if (isServer)
                {
                    _sm.CheckStars();  // !!! CRUCIAL TO UPDATING THE STATS
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerEnter(collision);
                UpdateStats(collision.gameObject);
            }
        }
        [Client]
        void PlayerEnter(Collider2D player)
        {
            // Updates player stats
            playerOwner = player.gameObject;
            playerOwner.GetComponent<PlayerBehaviour>().starsCollected++;

        }
        void UpdateStats(GameObject playerOwner)
        {
            // Updates star stats
            currentStatus = StarStatus.Taken;

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
            if (isServer)
            {
                TargetPlaySound(playerOwner.GetComponent<NetworkIdentity>().connectionToClient);
            }

            if (isServer)
            {
                _sm.CheckStars(); // !!! CRUCIAL TO UPDATING THE STATS
            }
            _sb.BarUpdate(); // Updates the Bar progress whenever the player disconnects
        }

        [TargetRpc]
        void TargetPlaySound(NetworkConnectionToClient chosenPlayer)
        {
            Debug.Log("This happens once");
            audioManager.SFX.PlayOneShot(audioManager.attain, 0.2f);
        }
    }
}
