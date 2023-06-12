using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace GM
{
    public class PowerUpProperties : NetworkBehaviour
    {
        public enum PowerUpTypes
        {
            AcceleratePlayer,
            DecceleratePlayers,
            SwapPlayerControls
        }

        private AudioManager audioManager;

        public PowerUpTypes powerUpType;
        private void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            audioManager.SFX.clip = audioManager.attain;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject.GetComponent<PlayerBehaviour>().possessesAPowerUp)
                {
                    return;
                }else{
                    audioManager.SFX.Play();
                }
                switch (powerUpType)
                {
                    case PowerUpTypes.AcceleratePlayer:
                        collision.gameObject.GetComponent<PlayerBehaviour>().currentPowerUpType = PlayerBehaviour.PowerUpTypes.SelfAcceleration;
                        break;
                    case PowerUpTypes.DecceleratePlayers:
                        collision.gameObject.GetComponent<PlayerBehaviour>().currentPowerUpType = PlayerBehaviour.PowerUpTypes.GeneralLaziness;
                        break;
                    case PowerUpTypes.SwapPlayerControls:
                        collision.gameObject.GetComponent<PlayerBehaviour>().currentPowerUpType = PlayerBehaviour.PowerUpTypes.SwappingControls;
                        break;
                }
                collision.gameObject.GetComponent<PlayerBehaviour>().possessesAPowerUp = true;
                ObjectDisabled();
            }
        }

        void ObjectDisabled()
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (isServer)
            {
                NetworkServer.UnSpawn(gameObject);
            }
        }
    }
}


