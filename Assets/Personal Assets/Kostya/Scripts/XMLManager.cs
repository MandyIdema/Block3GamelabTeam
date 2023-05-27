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
        //ugStats.obtainedClothes[0] = new KeyValuePair<string, bool>("head1",true);
        //Debug.Log(ugStats.obtainedClothes[0]);
    }

    // Updates the player's XML file
    public void SaveStarScore()
    {
        ugStats.starsCollectedInTotal += PlayerBehaviour.Local.starsCollected;//faulty can add even stars that have been counted before in-game
        XmlSerializer serializer = new XmlSerializer(typeof(UserGlobalStats));
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserStats/userinfo.xml", FileMode.Create);
        serializer.Serialize(stream, ugStats);
        stream.Close();
    }

    public void SaveStarScoreShop(int starAmount){
        ugStats.starsCollectedInTotal = starAmount;
        XmlSerializer serializer = new XmlSerializer(typeof(UserGlobalStats));
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserStats/userinfo.xml", FileMode.Create);
        serializer.Serialize(stream, ugStats);
        stream.Close();
    }

    public void AddToInventory(ClothSetting.ClothingInformation cInfo){
        
        XmlSerializer serializer = new XmlSerializer(typeof(ClothSetting.ClothingInformation));
        FileStream stream = new FileStream(Application.persistentDataPath+"UserStats/userinfo.xml",FileMode.Create);
        serializer.Serialize(stream,cInfo);
        stream.Close();
/*         if (File.Exists(Application.persistentDataPath + "/UserStats/userinfo.xml"))
        {
            var _tempList = ugStats.obtainedClothes;
            
            ugStats.obtainedClothes.Clear();
            var _count = 0;
            foreach(var i in _tempList){
                Debug.Log(_count+"AAAA");
                _count++;
            }
        } */

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
/*         public List<KeyValuePair<string,bool>> obtainedClothes = new List<KeyValuePair<string, bool>>{
            new KeyValuePair<string, bool>("head1",false),
            new KeyValuePair<string, bool>("head2",false),
            new KeyValuePair<string, bool>("head3",false),
            new KeyValuePair<string, bool>("head4",false),
            new KeyValuePair<string, bool>("body1",false),
            new KeyValuePair<string, bool>("body2",false),
            new KeyValuePair<string, bool>("body3",false),
            new KeyValuePair<string, bool>("body4",false),
            new KeyValuePair<string, bool>("body5",false),
            new KeyValuePair<string, bool>("body6",false),
            new KeyValuePair<string, bool>("body7",false),
            new KeyValuePair<string, bool>("body8",false),
            new KeyValuePair<string, bool>("body9",false),
            new KeyValuePair<string, bool>("feet1",false),
            new KeyValuePair<string, bool>("feet2",false),
            new KeyValuePair<string, bool>("feet3",false)
        }; */
    }
}
