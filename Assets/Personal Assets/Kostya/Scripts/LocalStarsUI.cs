using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalStarsUI : MonoBehaviour
{
    public PlayerBehaviour _player;
    private void Update()
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
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = _player.starsCollected.ToString();
        }

    }

}
