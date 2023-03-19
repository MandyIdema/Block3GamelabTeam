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

    public class MenuScript : MonoBehaviour
    {

        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
        Vector2 scrollViewPos = Vector2.zero;

        public NetworkDiscovery networkDiscovery;

        public NetworkManager networkManager;
        public GameObject menuPanel;
        public GameObject gamePanel;
        public GameObject domains;  // [KOSTYA] Injected code to activate the domain choice
        bool paused = false;

        public void Host()
        {
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);
            domains.SetActive(true);  // [KOSTYA] Injected code to activate the domain choice
            paused = false;

            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();

        }

        public void StartServer()
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartServer();
            networkDiscovery.AdvertiseServer();
        }

        public void SetIP(string ip)
        {
            networkManager.networkAddress = ip;
        }

        public void Join()
        {
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);
            paused = false;

            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
        }

        public void Stop()
        {
            if (networkManager.mode == NetworkManagerMode.Host)
            {
                NetworkManager.singleton.StopHost();
                networkDiscovery.StopDiscovery();
            }
            if (networkManager.mode == NetworkManagerMode.ClientOnly)
            {
                NetworkManager.singleton.StopClient();
                networkDiscovery.StopDiscovery();
            }
            paused = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            menuPanel.SetActive(true);
            gamePanel.SetActive(false);
            paused = false;
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
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
            }

        }
    }
}
