using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{

    public static XMLManager instance;
    [HideInInspector] public UserGlobalStats ugStats;
    [HideInInspector] public UserOutfits usOutfits;

    // [K] THIS is the class used in-game
    public List<bool> obtainedClothes = new List<bool>
    {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false
    };

    private void Awake()
    {
        instance = this;
        if (!Directory.Exists(Application.persistentDataPath + "/UserStats/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/UserStats/");
        }
        LoadStarScore();

        if (!File.Exists(Application.persistentDataPath + "/UserStats/outfitinfo.xml"))
        {
            SaveOutfits();
        }
            LoadOutfits();

    }

    #region STARS

    // Saving stars from the game
    public void SaveStarScoreGame()
    {
        ugStats.starsCollectedInTotal += PlayerBehaviour.Local.starsCollected;//faulty can add even stars that have been counted before in-game
        SaveStarScore();
    }

    // Saving stars from the shop
    public void SaveStarScoreShop(int starAmount)
    {
        ugStats.starsCollectedInTotal = starAmount;
        SaveStarScore();
    }

    // Default function for updating the XML file
    public void SaveStarScore()
    {
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

    // Resets the player's XML file
    public void NullifyStarScore()
    {
        ugStats.starsCollectedInTotal = 0;
        XmlSerializer serializer = new XmlSerializer(typeof(UserGlobalStats));
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserStats/userinfo.xml", FileMode.Create);
        serializer.Serialize(stream, ugStats);
        stream.Close();
    }

    [System.Serializable]
    public class UserGlobalStats
    {
        public int starsCollectedInTotal;
    }

    #endregion

    #region OUTFITS

    // Transfers the info from the game object to XML file
    public void SaveOutfits()
    {
        usOutfits.obtainedUserClothes = instance.obtainedClothes;
        XmlSerializer serializer = new XmlSerializer(typeof(UserOutfits));
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserStats/outfitinfo.xml", FileMode.Create);
        serializer.Serialize(stream, usOutfits);
        stream.Close();
            
    }

    // Transfers the info from the XML file to the game object
    public void LoadOutfits()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(UserOutfits));
        FileStream stream = new FileStream(Application.persistentDataPath + "/UserStats/outfitinfo.xml", FileMode.Open);
        usOutfits = serializer.Deserialize(stream) as UserOutfits;
        stream.Close();

        if (usOutfits.obtainedUserClothes != null)
        {
            instance.obtainedClothes = usOutfits.obtainedUserClothes;
        }
    }

    //public void AddToInventory(ClothSetting.ClothingInformation cInfo)
    //{
    //    XmlSerializer serializer = new XmlSerializer(typeof(ClothSetting.ClothingInformation));
    //    FileStream stream = new FileStream(Application.persistentDataPath + "UserStats/userinfo.xml", FileMode.Create);
    //    serializer.Serialize(stream, cInfo);
    //    stream.Close();

    //}


    // [K] THIS is the class that is used for the XML file
    [System.Serializable]
    public class UserOutfits
    {
        public List<bool> obtainedUserClothes;
    }
    #endregion

}
