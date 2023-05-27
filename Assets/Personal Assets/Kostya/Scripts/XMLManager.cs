using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{

    public static XMLManager instance;
    [HideInInspector] public UserGlobalStats ugStats;

    private void Awake()
    {
        instance = this;
        if (!Directory.Exists(Application.persistentDataPath + "/UserStats/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/UserStats/");
        }
        LoadStarScore();
        Debug.Log("Current score is " + XMLManager.instance.LoadStarScore());
    }

    // Updates the player's XML file
    public void SaveStarScore()
    {
        ugStats.starsCollectedInTotal += PlayerBehaviour.Local.starsCollected;
        XmlSerializer serializer = new XmlSerializer(typeof(UserGlobalStats));
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserStats/userinfo.xml", FileMode.Create);
        serializer.Serialize(stream, ugStats);
        stream.Close();
    }

    // Resets the player's XML file
    public void NullifyStarScore()
    {
        ugStats.starsCollectedInTotal = 0;
        XmlSerializer serializer = new XmlSerializer(typeof(UserGlobalStats));
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserStats/userinfo.xml", FileMode.Create);
        serializer.Serialize(stream, ugStats);
        stream.Close();
    }

    // Loads the player's XML file for any in-game use
    public int LoadStarScore()
    {
        if (File.Exists(Application.persistentDataPath + "/UserStats/userinfo.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserGlobalStats));
            FileStream stream = new FileStream(Application.persistentDataPath + "/UserStats/userinfo.xml", FileMode.Open);
            ugStats = serializer.Deserialize(stream) as UserGlobalStats;
            stream.Close();
        }
        return ugStats.starsCollectedInTotal;
    }

    [System.Serializable]
    public class UserGlobalStats
    {
        public int starsCollectedInTotal;
        public List<KeyValuePair<string,bool>> obtainedClothes;
    }
}
