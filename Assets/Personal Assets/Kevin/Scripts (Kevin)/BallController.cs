using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BallController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnBallOwnerChanged))]
    public NetworkIdentity ballOwner;

    private Rigidbody2D rb;
    private Vector3 offset; 

    public float releaseForce = 1.5f;

    public float stopping = 2.0f;

    private AudioManager audioManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        offset = new Vector3(0f, -0.5f, 0f); 

        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            NetworkIdentity playerIdentity = collision.gameObject.GetComponent<NetworkIdentity>();
            if (playerIdentity != null)
            {
                if (ballOwner == null)
                {
                    ballOwner = playerIdentity;
                    rb.isKinematic = true;
                    audioManager.KickBall();
                }
            }
        }
    }

    private void Update()
    {
        if (isServer && ballOwner != null)
        {
            
            transform.position = ballOwner.transform.position + offset;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Release the ball by clearing the ball owner
                ballOwner = null;
                rb.isKinematic = false;
                ApplyReleaseForce();
            }
        }
    }

    private void ApplyReleaseForce()
    {
        Vector2 releaseDirection = Random.insideUnitCircle.normalized;
        Vector2 force = releaseDirection * releaseForce;
        rb.AddForce(force * Time.deltaTime, ForceMode2D.Impulse);


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            NetworkIdentity playerIdentity = collision.gameObject.GetComponent<NetworkIdentity>();
            if (playerIdentity != null && playerIdentity == ballOwner)
            {
                ballOwner = null;
                rb.isKinematic = false;
            }
        }
    }

    private void OnBallOwnerChanged(NetworkIdentity oldOwner, NetworkIdentity newOwner)
    {
        if (!isServer)
        {
            if (newOwner == null)
            {
                // Enable physics for the ball when there is no owner
                rb.isKinematic = false;
            }
            else
            {
                // Disable physics for the ball when it has an owner
                rb.isKinematic = true;
            }
        }
    }
}

