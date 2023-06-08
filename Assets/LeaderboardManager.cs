using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace GM
{

    public class LeaderboardManager : MonoBehaviour
    {

        public class UserData
        {
            public int stars { set; get; }
            public string username { set; get; }
        }

        public GameObject leaderboardUI;
        public GameManager _gm;

        private List<UserData> DefaultLeaderboardData = new List<UserData>();
        public List<UserData> SortedLeaderboardData = new List<UserData>();

        private void Start()
        {
            StartCoroutine(UpdateLeaderboardOrder());
        }

        IEnumerator UpdateLeaderboardOrder()
        {
            while (true)
            {
                if (_gm.players.Count >= 1)
                {
                    DefaultLeaderboardData.Clear();
                    SortedLeaderboardData.Clear();
                    for (int i = 0; i < _gm.players.Count; i++)
                    {
                        DefaultLeaderboardData.Add(new UserData {
                            stars = _gm.players[i].GetComponent<PlayerBehaviour>().starsCollected, 
                            username = _gm.players[i].name});
                    }
                    SortedLeaderboardData = DefaultLeaderboardData.OrderByDescending((UserData Ud) => Ud.stars).ToList();
                    UpdateLeaderboardUI();
                }
                yield return new WaitForSeconds(1.5f);
            }
        }

        void UpdateLeaderboardUI()
        {
            for (int i = 0; i < _gm.players.Count; i++)
            {
                leaderboardUI.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = SortedLeaderboardData[i].username;
            }
        }
    }
}

