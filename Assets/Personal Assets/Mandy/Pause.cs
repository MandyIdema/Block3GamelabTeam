using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Mirror.Discovery
{

    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Discovery HUD")]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-discovery")]
    [RequireComponent(typeof(NetworkDiscovery))]

    public class Pause : MonoBehaviour
    {


        public GameObject gamePanel;
        bool paused = false;
        public NetworkDiscovery networkDiscovery;
        public NetworkManager networkManager;


        public void Stop()
        {
            if (networkManager.mode == NetworkManagerMode.Host)
            {
                NetworkManager.singleton.StopHost();
                networkDiscovery.StopDiscovery();
                Debug.Log("Stopped game");
            }
            if (networkManager.mode == NetworkManagerMode.ClientOnly)
            {
                NetworkManager.singleton.StopClient();
                networkDiscovery.StopDiscovery();
                Debug.Log("Stopped game");
            }
            paused = false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                paused = !paused;
            }
            //are we connected
            if (NetworkServer.active || NetworkClient.active)
            {

                if (paused)
                {
                    gamePanel.SetActive(true);
                }
                else
                {
                    gamePanel.SetActive(false);
                }
            }
            else
            {
                gamePanel.SetActive(false);
            }

        }
    }
}
