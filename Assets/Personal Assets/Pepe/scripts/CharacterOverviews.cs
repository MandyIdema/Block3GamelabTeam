using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameStatus
{
    Pending,
    Started,
    Finished
}
public class CharacterOverviews : MonoBehaviour
{
    [SerializeField] private GameObject[] players; // That stays
// [K] Add a script to collect the info from all the player objects regarding how many stars (int starsCollected
// in the PlayerBehaviour script) they have collected in total (basically loop through all of the objects and add
// the stars from every player object script)
    [SerializeField] private GameObject domainInfo;
    [SerializeField] private TextMeshProUGUI _starsCollected;
    [SerializeField] private GameObject barriers;
    private bool allPlayersAppeared = false;
    private GameStatus currentStatus = GameStatus.Pending;

    void Update()
    {
        if (players.Length<4 && !allPlayersAppeared)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
        if(currentStatus==GameStatus.Started)
        {
            StarsScore();
        }

    }
    void LateUpdate()
    {
        if(players.Length>1 && currentStatus==GameStatus.Pending)
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
                }
            }if(players.Length == domainCount)
            {
                currentStatus = GameStatus.Started;
                Destroy(barriers);
            }
    }

    private void StarsScore()
    {
        for(int i=0; i<=players.Length+1; i++)
        {
            _starsCollected.GetComponent<TextMeshProUGUI>().text = players[i].GetComponent<PlayerBehaviour>().playerNameText+":"+ players[i].GetComponent<PlayerBehaviour>().starsCollected;
        }
    }
}
