using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DomainUI : MonoBehaviour
{ 
    public List<string> domainDescriptions = new List<string>();
    public GameObject domainMenu;
    public PlayerBehaviour _player;

    void Update()
    {
        if (!_player)
        {
            _player = PlayerBehaviour.Local;
        }
    }

    private void LateUpdate()
    {

        // This part should be separated into the server side and the client side later
        if (_player && _player.showDomainMenu)
        {
            domainMenu.GetComponentInChildren<TextMeshProUGUI>().text = domainDescriptions[_player.currentDomainNumber];
            domainMenu.SetActive(_player.onDomain);
        }

        if (_player && _player.currentStatus == PlayerBehaviour.PlayerStatus.Ready)
        {
            Destroy(gameObject);
        }
    }
}
