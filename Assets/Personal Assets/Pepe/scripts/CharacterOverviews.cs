using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOverviews : MonoBehaviour
{
    [SerializeField] private GameObject[] players; // That stays
// [K] Add a script to collect the info from all the player objects regarding how many stars (int starsCollected
// in the PlayerBehaviour script) they have collected in total (basically loop through all of the objects and add
// the stars from every player object script)
    [SerializeField] private GameObject[] puzzles; // not needed
    [SerializeField] private GameObject domainInfo;
    private bool allPlayersAppeared = false;
    [SerializeField] private Transform[] spawnPoints; // not needed
    public List<int>activatedDomains; // not needed
    private bool thatsAll = false;

    void Update()
    {
        if (players.Length<4 && !allPlayersAppeared)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
    }
    void LateUpdate()
    {
        if(players.Length>1)
        {
            CheckingDomains();
        }
    }

    public void CheckingDomains()
    {
            var domainCount = 0;
            //Debug.Log("YES");
            var domains = domainInfo.GetComponent<DomainInfoGiver>().domains;
            for(int i = 1; i< domains.Count; i++)
            {
                if(domains[i].GetComponent<DomainInformation>().currentStatus == DomainInformation.DomainStatus.Chosen)
                {
                    domainCount++;
                   // activatedDomains.Add(i);
                }
            }if(players.Length == domainCount && !thatsAll)
            {
               // ActivatePuzzles(activatedDomains);
                thatsAll = true;
            }
    }
    public void ActivatePuzzles(List<int>domainsToActivate)
    {
        var takenSpawns = 0;
        for(var i = 0; i< domainsToActivate.Count; i++)
        {
            foreach(GameObject j in puzzles)
            {

                if(j.GetComponent<Puzzles>().puzzleNumber == domainsToActivate[i] && takenSpawns<2)
                {
                    Instantiate(j,spawnPoints[i]);
                    takenSpawns++;
                    Debug.Log("SPAWNED");
                }
            }
        }
    }
}
