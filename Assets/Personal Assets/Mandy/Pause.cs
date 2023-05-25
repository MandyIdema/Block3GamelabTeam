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
        }

        // Start is called before the first frame update
        void Start()
        {
            gamePanel = GameObject.FindGameObjectWithTag("GamePanel");
        }

        // [K] I think that this did not work at all so I switched it in favour of using Player Behaviour
        // We can always return this part back later
        /*void Update()
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

        }*/
    }
}
