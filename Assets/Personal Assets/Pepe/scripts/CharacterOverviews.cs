using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOverviews : MonoBehaviour
{
    [SerializeField] private GameObject[] players;
    [SerializeField] private GameObject[] puzzles;
    [SerializeField] private GameObject domainInfo;
    private bool allPlayersAppeared = false;
    [SerializeField] private Transform[] spawnPoints;
    public List<int>activatedDomains;
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
                    //Debug.Log("FSFDSGSG");
                    domainCount++;
                    activatedDomains.Add(i);
                }
            }if(players.Length == domainCount && !thatsAll)
            {
                //Debug.Log("MUHAHAHA");
                ActivatePuzzles(activatedDomains);
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
