using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{

    public static XMLManager instance;
    public UserGlobalStats ugStats;
    public int currentPlayerStars;

    private void Awake()
    {
        instance = this;
        if (!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
        }
    }

    public void SaveStarScore()
    {
        currentPlayerStars += PlayerBehaviour.Local.starsCollected;
        XmlSerializer serializer = new XmlSerializer(typeof(UserGlobalStats));
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/userinfo.xml", FileMode.Create);
        serializer.Serialize(stream, ugStats);
        stream.Close();
    }

    [System.Serializable]
    public class UserGlobalStats
    {

    }
}
