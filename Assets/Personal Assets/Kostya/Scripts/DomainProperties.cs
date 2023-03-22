using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DomainProperties : NetworkBehaviour
{
    private GameObject domainClassMenu;
    public DomainInformation domainInfo;

    [Space]

    [Header ("Domain Info")]
    public int domainNumber;
    public DomainStatus currentStatus = DomainStatus.free;
    public List<GameObject> playersInDomain = new List<GameObject>();

    private int peopleBrowsing = 0;


    public enum DomainStatus
    {
        free,
        pending,
        chosen
    }

    private void Start()
    {
        domainClassMenu = domainInfo.canvas.transform.GetChild(2).gameObject;
    }
    // Update is called once per frame
    void Update()
    {

        if (currentStatus != DomainStatus.chosen)
        {
            if (peopleBrowsing == 0)
            {
                currentStatus = DomainStatus.free;
            }
            else
            {
                currentStatus = DomainStatus.pending;
            }

            foreach (GameObject player in playersInDomain)
            {
                if (player.GetComponent<Movement>().domainChosen)
                {
                    player.GetComponent<Movement>().playerChosenDomain = domainNumber;
                    currentStatus = DomainStatus.chosen;
                    InitializeDomainMenu(false);
                }
            }
        }
        else
        {
            peopleBrowsing = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement>() != null)
        {
            if (collision.gameObject.GetComponent<Movement>().domainChosen == false)
            {
                playersInDomain.Add(collision.gameObject);
                collision.gameObject.GetComponent<Movement>().currentlyOnDomain = true;
                peopleBrowsing++;
                InitializeDomainMenu(true);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement>() != null)
        {
            if (collision.gameObject.GetComponent<Movement>().domainChosen == false)
            {
                playersInDomain.Remove(collision.gameObject);
                collision.gameObject.GetComponent<Movement>().currentlyOnDomain = false;
                peopleBrowsing--;
                InitializeDomainMenu(false);
            }
        }
            

    }
    [Client]
    private void InitializeDomainMenu(bool activationStatus)
    {
        // Currently uses this structure to get the text object, might change later
        if (NetworkClient.active)
        {
            domainClassMenu.GetComponentInChildren<TextMeshProUGUI>().text = domainInfo.domainDescription[domainNumber];
            domainClassMenu.SetActive(activationStatus);
        }
    }

    // Currently not used
    /*public void CmdAssignPlayerDomain(GameObject player)
    {
        player.GetComponent<Movement>().playerChosenDomain = domainNumber;
        player.GetComponent<Movement>().currentlyOnDomain = false;
        currentStatus = DomainStatus.chosen;
        InitializeDomainMenu(false);
    }*/
}
