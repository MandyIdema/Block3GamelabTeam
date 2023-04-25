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
        public GameObject pausePanel;
        public GameObject discoveryPanel;
   
        bool paused = false;

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
        

        public void Host()
        {
            menuPanel.SetActive(false);

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
   
        
            paused = false;

            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
            menuPanel.SetActive(false);
            discoveryPanel.SetActive(true);
        }

        public void Back()
        {
            paused = false;

            menuPanel.SetActive(true);
            discoveryPanel.SetActive(false);

        }

       

        // Start is called before the first frame update
        void Start()
        {
            menuPanel.SetActive(true);
            pausePanel.SetActive(false);
            discoveryPanel.SetActive(false);
          
            paused = false;
        }

        // Update is called once per frame
      
    }
}
