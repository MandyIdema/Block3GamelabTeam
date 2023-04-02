using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DomainInformation : NetworkBehaviour
{
    public int domainNumber;
    private int playersBrowsing;
    public Sprite characterModel;
    public enum DomainStatus
    {
        Free,
        Pending,
        Chosen
    }

    [SyncVar] public DomainStatus currentStatus;
     
    private void Update()
    {
        if (currentStatus != DomainStatus.Chosen)
        {
            if (playersBrowsing > 0)
            {
                currentStatus = DomainStatus.Pending;
            }
            else
            {
                currentStatus = DomainStatus.Free;
            }
        }
        else
        {
            playersBrowsing = 0;
        }
    }

    // Will still show pending even if somebody has already chosen their domain and just floats in the area
    // So dont use it to start the game
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.CompareTag("Player"))
        {
            playersBrowsing++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playersBrowsing--;
        }
    }
}
