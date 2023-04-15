using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;


namespace GM
{
    public class GlobalStarsUI : NetworkBehaviour
    {
        [SyncVar] public int starsRemaining;
        [SyncVar] public string displayMessage;

        private void Awake()
        {
            starsRemaining = FindObjectOfType<StarManager>().starsNeeded - FindObjectOfType<StarManager>().starsTaken;
        }
        private void Update()
        {
            StarsCalculation();
            RpcStarsUpdate();
        }

        [Server]
        void StarsCalculation()
        {
            if (starsRemaining > 0)
            {
                starsRemaining = FindObjectOfType<StarManager>().starsNeeded - FindObjectOfType<StarManager>().starsTaken;
                displayMessage = $"Stars until victory: {starsRemaining}";
            }
            else
            {
                displayMessage = "Your team has finished the task, you can leave!";
            }
        }

        [ClientRpc]
        void RpcStarsUpdate()
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = displayMessage;
        }
    }
}

