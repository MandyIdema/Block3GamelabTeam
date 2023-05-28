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

    public class MainMenuScript : MonoBehaviour
    {

        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
        // Vector2 scrollViewPos = Vector2.zero;

        public NetworkDiscovery networkDiscovery;

        public NetworkManager networkManager;
        public GameObject menuPanel;
        public GameObject ExitGamePanel;
        public GameObject discoveryPanel;
        public GameObject background;
   
        // bool paused = false;

        public void Stop()
        {
            //If this button is pressed the game stops and goes back to mainmenu
           if (networkManager.mode != NetworkManagerMode.ClientOnly){
                if (networkManager.mode == NetworkManagerMode.Host)
                {
                    Destroy(ExitGamePanel);
                    NetworkManager.singleton.StopHost();
                    networkDiscovery.StopDiscovery();
                    Debug.Log("Stopped game");

                    //If this computer is a host, stop the host server
                    //Automatically stops clients connected to the server as well
                }
            }

            if (networkManager.mode != NetworkManagerMode.Host)
            {

                if (networkManager.mode == NetworkManagerMode.ClientOnly)
                {
                    Destroy(ExitGamePanel);
                    NetworkManager.singleton.StopClient();
                    networkDiscovery.StopDiscovery();
                    Debug.Log("Stopped game");

                    //If this computer is a client, stop the client connected to the host
                }
            }
            
        }
        

        public void Host()
        {
            menuPanel.SetActive(false);
            background.SetActive(false);

            // paused = false;

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
        
            // paused = false;

            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
            menuPanel.SetActive(false);
            //discoveryPanel.SetActive(false);
            // [K] I changed this to prevent the menu from persisting onto the game, disable the line above
            // And enable the line below if you want to return to the way it was before
            discoveryPanel.SetActive(true);
        }

        public void Back()
        {
            // paused = false;

            menuPanel.SetActive(true);
            discoveryPanel.SetActive(false);
            discoveredServers.Clear();

        }

       

        // Start is called before the first frame update
        void Start()
        {
            menuPanel.SetActive(true);
            background.SetActive(true);
            // [K] Disabled since I found a workaround that I put in PlayerBehaviour
            // It is by no means optimal, especially with slower devices
            // ExitGamePanel.SetActive(false);
            discoveryPanel.SetActive(false);

            ExitGamePanel = GameObject.FindGameObjectWithTag("GamePanel");

            // paused = false;
        }

        // Update is called once per frame
      
    }
}
