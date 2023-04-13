using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
            if (_gm == null)
            {
                _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            }
            if (_sm == null)
            {
                _sm = GameObject.FindGameObjectWithTag("StarManager").GetComponent<StarManager>();
            }

            if (currentStatus == StarStatus.Taken && playerOwner == null)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                currentStatus = StarStatus.Free;
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
            playerOwner = player.gameObject;
            playerOwner.GetComponent<PlayerBehaviour>().starsCollected++;

            currentStatus = StarStatus.Taken;

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            _gm.starsCollected.TryGetValue(playerOwner, out int j);
            _gm.starsCollected[playerOwner] = j + 1;
            _sm.CheckStars();
        }

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
