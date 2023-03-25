using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        if (_player)
        {
            domainMenu.GetComponentInChildren<TextMeshProUGUI>().text = domainDescriptions[_player.currentDomainNumber];
            domainMenu.SetActive(_player.onDomain);
        }
    }
}
