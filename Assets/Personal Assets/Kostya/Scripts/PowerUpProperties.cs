using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PowerUpProperties : NetworkBehaviour
{
    public enum PowerUpTypes
    {
        AcceleratePlayer,
        DecceleratePlayers,
        RandomlySwapPlayers,
        DoSomethingElseWithPlayers
    }

    public PowerUpTypes powerUpType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerBehaviour>().possessesAPowerUp)
            {
                return;
            }
            collision.gameObject.GetComponent<PlayerBehaviour>().possessesAPowerUp = true;
            switch (powerUpType)
            {
                case PowerUpTypes.AcceleratePlayer:
                    collision.gameObject.GetComponent<PlayerBehaviour>().currentPowerUpType = PlayerBehaviour.PowerUpTypes.SelfAcceleration;
                    break;
                case PowerUpTypes.DecceleratePlayers:
                    collision.gameObject.GetComponent<PlayerBehaviour>().currentPowerUpType = PlayerBehaviour.PowerUpTypes.GeneralLaziness;
                    break;
                case PowerUpTypes.RandomlySwapPlayers:
                    collision.gameObject.GetComponent<PlayerBehaviour>().currentPowerUpType = PlayerBehaviour.PowerUpTypes.Swapping;
                    break;
                case PowerUpTypes.DoSomethingElseWithPlayers:
                    collision.gameObject.GetComponent<PlayerBehaviour>().currentPowerUpType = PlayerBehaviour.PowerUpTypes.Type4;
                    break;
            }

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}